# Запуск ollama
# ollama run gpt-oss:20b

# Запуск бота
# cd jackal_bot
# python -m venv venv
# venv\Scripts\activate
# python bot5.py

#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import asyncio
import aiohttp
import json
import uuid
import random
import math
import copy
import logging
import requests
import time

# ----------------------------------------------------
# Конфигурация
# ----------------------------------------------------
OLLAMA_URL = "http://localhost:11434/api/generate"  # пока не используется
MODEL_NAME = "gpt-oss:20b"

SERVER_URL = "https://jackal.team"  # <-- ваш сервер
LOGIN = "gpt_test_1"

# Параметры игры (изначально задаём один набор)
GAME_SETTINGS = {
    "mapId": 1731500548,
    "mapSize": 7,
    "tilesPackName": "classic",
    "gameMode": "FreeForAll",
    "players": [
        {"userId": None, "type": "human", "position": "Down"},
        {"userId": 0,    "type": "robot2", "position": "Up"},
    ],
}

SIMULATIONS = 500           # число симуляций MCTS
MAX_DEPTH    = 20           # глубина симуляции
EXPLORATION = 1.414          # коэффициент UCT
EXPLORE_BONUS = 0.05        # бонус за открытие неизвестной клетки

logging.basicConfig(level=logging.INFO,
                    format="%(asctime)s %(levelname)s: %(message)s")

# ----------------------------------------------------
# 1. Модель игрового состояния (GameState)
# ----------------------------------------------------
class GameState:
    """
    Псевдомодель состояния «Шакал». В реальности
    всё, что нужно для симуляции, хранится здесь.
    """
    def __init__(self, data: dict, visited=None):
        self.pirates  = data.get('pirates', [])
        self.map      = data.get('map', {})
        self.moves    = data.get('moves', [])
        self.stats    = data.get('stats', {})

        self.current_team = self.stats.get('currentTeamId', 0)
        self.turn_number  = self.stats.get('turnNumber',
                                           self.stats.get('currentTurn', 0))
        self.team_score   = self.stats.get('teamScore', [0, 0])

        # координаты, которые уже «открыты» симуляцией
        self.visited = visited if visited is not None else {
            (p['position']['x'], p['position']['y'])
            for p in self.pirates
        }

        self.exploration_bonus = 0.0   # за открытие новых клеток

    def clone(self):
        """Создаёт глубокую копию состояния."""
        new_state = copy.deepcopy(self)
        new_state.visited = set(self.visited)
        return new_state

    # ------------------------------------------------
    # Применяем ход к состоянию
    # ------------------------------------------------
    def apply_move(self, move: dict):
        """
        * move: { 'from': {...}, 'to': {...}, 'withCoin': bool, ... }
        """
        # Перемещаем пиратов
        for pid in move.get('pirateIds', []):
            for p in self.pirates:
                if p['id'] == pid:
                    p['position']['x'] = move['to']['x']
                    p['position']['y'] = move['to']['y']
                    break

        # Новый пункт – добавляем в visited
        new_pos = (move['to']['x'], move['to']['y'])
        if new_pos not in self.visited:
            self.visited.add(new_pos)

        # Если клетка была «неизвестной» – награда за открытие
        tile = self.tile_at(new_pos)
        if tile and tile.get('isUnknown', False):
            self.exploration_bonus += EXPLORE_BONUS

        # Захват монеты (если был флаг withCoin)
        if move.get('withCoin', False):
            for t in self.map.get('changes', []):
                if t['x'] == move['to']['x'] and t['y'] == move['to']['y']:
                    levels = t.get('levels', [])
                    if levels and levels[0].get('coins', 0) > 0:
                        levels[0]['coins'] -= 1
                    break
            self.team_score[self.current_team] += 1

        # Переходим к следующему игроку
        self.current_team = 1 - self.current_team
        self.turn_number += 1
        self.stats['turnNumber'] = self.turn_number
        self.stats['currentTeamId'] = self.current_team
        self.stats['teamScore'] = self.team_score

    # ------------------------------------------------
    # Вспомогательная: получить клетку по координатам
    # ------------------------------------------------
    def tile_at(self, pos):
        for t in self.map.get('changes', []):
            if t['x'] == pos[0] and t['y'] == pos[1]:
                return t
        return None

    # ------------------------------------------------
    # Оценка состояния (награда)
    # ------------------------------------------------
    def reward(self) -> float:
        """Награда = разница очков + бонус за исследование."""
        base = self.team_score[0] - self.team_score[1]
        return base + self.exploration_bonus * 5.0

    # ------------------------------------------------
    # Терминальность
    # ------------------------------------------------
    def is_terminal(self) -> bool:
        return not self.moves or len(self.moves) == 0

# ----------------------------------------------------
# 2. Узел MCTS
# ----------------------------------------------------
class MCTSNode:
    def __init__(self, state: GameState, parent=None, move_index=None):
        self.state      = state
        self.parent     = parent
        self.move_index = move_index      # какой ход привёл к этому узлу
        self.children   = {}
        self.visits     = 0
        self.q_value    = 0.0

    def is_fully_expanded(self) -> bool:
        return len(self.children) == len(self.state.moves)

# ----------------------------------------------------
# 3. Сам MCTS
# ----------------------------------------------------
class MCTS:
    def __init__(self,
                 root_state: GameState,
                 simulations: int = SIMULATIONS,
                 max_depth: int = MAX_DEPTH,
                 exploration: float = EXPLORATION):
        self.root = MCTSNode(root_state)
        self.simulations = simulations
        self.max_depth = max_depth
        self.exploration = exploration

    # ------------------------------------------------
    # Основной цикл поиска
    # ------------------------------------------------
    def search(self) -> int:
        for _ in range(self.simulations):
            node = self._tree_policy(self.root)
            reward = self._default_policy(node.state.clone(), self.max_depth)
            self._backpropagate(node, reward)

        best_child = self._best_child(self.root, exploration=self.exploration)
        return best_child.move_index

    # ------------------------------------------------
    # Полоса дерева (выбор/разворачивание)
    # ------------------------------------------------
    def _tree_policy(self, node: MCTSNode) -> MCTSNode:
        while not node.state.is_terminal():
            if not node.is_fully_expanded():
                return self._expand(node)
            node = self._best_child(node, exploration=self.exploration)
        return node

    # ------------------------------------------------
    # Расширяем узел
    # ------------------------------------------------
    def _expand(self, node: MCTSNode) -> MCTSNode:
        tried = set(node.children.keys())
        for i, move in enumerate(node.state.moves):
            if i not in tried:
                new_state = node.state.clone()
                new_state.apply_move(move)
                child = MCTSNode(new_state, parent=node, move_index=i)
                node.children[i] = child
                return child
        return node

    # ------------------------------------------------
    # Выбираем лучший ребёнок (UCT)
    # ------------------------------------------------
    def _best_child(self, node: MCTSNode,
                    exploration: float = EXPLORATION) -> MCTSNode | None:
        best_value = -float('inf')
        best_child = None
        for child in node.children.values():
            if child.visits == 0:
                uct = float('inf')
            else:
                uct = child.q_value / child.visits + exploration * math.sqrt(
                    math.log(node.visits + 1) / child.visits)
            # «шум» в случае равных значений
            uct += random.uniform(0, 1e-6)
            if uct > best_value:
                best_value = uct
                best_child = child
        return best_child

    # ------------------------------------------------
    # Симуляция (случайные ходы до глубины)
    # ------------------------------------------------
    def _default_policy(self, state: GameState,
                        depth_remaining: int) -> float:
        for _ in range(depth_remaining):
            if not state.moves:
                break
            move = random.choice(state.moves)
            state.apply_move(move)
        return state.reward()

    # ------------------------------------------------
    # Обратный путь: обновляем q и visits
    # ------------------------------------------------
    def _backpropagate(self, node: MCTSNode, reward: float):
        while node is not None:
            node.visits += 1
            node.q_value += reward
            node = node.parent

# ----------------------------------------------------
# 4. Логика бота (переиспользуем ту же MCTS)
# ----------------------------------------------------
class MCTSBotLogic:
    def __init__(self,
                 simulations: int = SIMULATIONS,
                 max_depth: int = MAX_DEPTH,
                 exploration: float = EXPLORATION):
        self.simulations = simulations
        self.max_depth = max_depth
        self.exploration = exploration

    def decide_move(self, state_dict: dict) -> str | None:
        root_state = GameState(state_dict)
        if not root_state.moves:
            return None
        mcts = MCTS(root_state,
                    simulations=self.simulations,
                    max_depth=self.max_depth,
                    exploration=self.exploration)
        best_index = mcts.search()
        return str(best_index)

# ----------------------------------------------------
# 5. Клиент (JackalBot)
# ----------------------------------------------------
class JackalBot:
    def __init__(self, server_url: str, username: str,
                 bot_logic: MCTSBotLogic):
        self.server_url = server_url
        self.username = username
        self.bot_logic = bot_logic

        self.ws_url = server_url.replace("https://", "wss://").replace("http://", "ws://")
        self.http_url = server_url

        self.token = None
        self.user_id = None
        self.game_id = None

    # ------------------------------------------------
    # Регистрация
    # ------------------------------------------------
    def register(self) -> bool:
        url = f"{self.http_url}/api/v1/auth/register"
        logging.info(f"Регистрация {self.username}…")
        try:
            resp = requests.post(url, json={"login": self.username}, verify=False)
        except Exception as e:
            logging.error(f"Ошибка регистрации: {e}")
            return False

        if resp.status_code == 200:
            data = resp.json()
            self.token = data.get("token") or data.get("accessToken")
            self.user_id = data.get("id") or 40
            logging.info("Авторизация прошла успешно")
            return True
        else:
            logging.error(f"Авторизация не удалась: {resp.status_code} {resp.text}")
            return False

    # ------------------------------------------------
    # Запуск / основной цикл
    # ------------------------------------------------
    async def run(self):
        logging.info("Запуск бота…")
        if not self.register():
            logging.error("Регистрация не выполнена. Выходим.")
            return

        # Константы для игры – берём из GAME_SETTINGS
        game_settings = GAME_SETTINGS.copy()
        game_settings["players"][0]["userId"] = int(self.user_id)

        async with aiohttp.ClientSession() as session:
            # 1. Negotiate
            negotiate_url = f"{self.http_url}/gamehub/negotiate?negotiateVersion=1"
            headers = {"Authorization": f"Bearer {self.token}"}
            async with session.post(negotiate_url, headers=headers, ssl=False) as resp:
                neg_data = await resp.json()
                logging.info(f"Negotiate: {neg_data.get('connectionId', 'no ID')}")

            # 2. Устанавливаем соединение WebSocket
            ws_url = f"{self.ws_url}/gamehub?access_token={self.token}"
            logging.info(f"Подключаемся к WS: {ws_url}")

            while True:          # <‑‑ бесконечный цикл: после GameOver создаём новое соединение
                async with session.ws_connect(ws_url,
                                              heartbeat=10,
                                              ssl=False) as ws:
                    logging.info("WS открыта. Отправляем Handshake…")
                    await ws.send_str(json.dumps({"protocol": "json", "version": 1}) + "\x1e")

                    # Ждём подтверждения Handshake
                    msg = await ws.receive()
                    logging.info(f"Handshake подтверждён: {msg.data}")

                    # 3. Запускаем игру
                    await self._start_game(ws, game_settings)

                    # 4. Принимаем сообщения
                    async for msg in ws:
                        if msg.type == aiohttp.WSMsgType.TEXT:
                            packets = msg.data.split('\x1e')
                            for packet in packets:
                                if not packet or packet == '{}':
                                    continue
                                data = json.loads(packet)
                                target = data.get("target")

                                # ----------------------------------------------------------------
                                # GameOver: проверяем поле isGameOver
                                # ----------------------------------------------------------------
                                if target == "GetMoveChanges":
                                    stats = data['arguments'][0]['stats']
                                    if stats.get('isGameOver'):
                                        logging.info("Игра закончилась. Перезапускаем…")
                                        await ws.close()
                                        # выйти из внутреннего цикла, чтобы создать новое соединение
                                        break

                                    # --------------------------------------------------------
                                    # Получаем новое состояние и делаем ход
                                    # --------------------------------------------------------
                                    state_dict = self._prepare_state_for_ai(data['arguments'][0])
                                    logging.info("Состояние получено – принимаем решение…")
                                    move_index = self.bot_logic.decide_move(state_dict)

                                    if move_index is not None:
                                        await self._make_move(ws,
                                                              self.game_id,
                                                              state_dict['moves'],
                                                              move_index)
                                    else:
                                        logging.info("Нет доступных ходов – ждём очередную смену.")
                                    continue

                                if target == "GetStartData":
                                    # Начало новой игры
                                    self.game_id = data['arguments'][0]['gameId']
                                    logging.info(f"Игра началась, id={self.game_id}")

                                    # Сразу сделаем первый ход
                                    state_dict = self._prepare_state_for_ai(data['arguments'][0])
                                    logging.info("Первый ход – анализируем карту…")
                                    move_index = self.bot_logic.decide_move(state_dict)

                                    if move_index is not None:
                                        await self._make_move(ws,
                                                              self.game_id,
                                                              state_dict['moves'],
                                                              move_index)
                                    else:
                                        logging.info("Невозможно выбрать ход.")
                                    continue

                        elif msg.type in (aiohttp.WSMsgType.CLOSED,
                                          aiohttp.WSMsgType.ERROR):
                            logging.error(f"Соединение закрыто: {msg.data}")
                            break

    # ------------------------------------------------
    # Отправляем стартовую игру
    # ------------------------------------------------
    async def _start_game(self, ws, settings):
        start_msg = {
            "arguments": [{"settings": settings}],
            "invocationId": str(uuid.uuid4()),
            "target": "start",
            "type": 1
        }
        await ws.send_str(json.dumps(start_msg) + "\x1e")
        logging.info("Команда start отправлена.")

    # ------------------------------------------------
    # Формируем состояние для MCTS (чтобы не менять оригинальный код)
    # ------------------------------------------------
    def _prepare_state_for_ai(self, data: dict) -> dict:
        """Преобразует raw‑данные в dict, который понимает MCTS."""
        return {
            'pirates': data.get('pirates', []),
            'map': data.get('map', {}),
            'moves': data.get('moves', []),
            'stats': data.get('stats', {})
        }

    # ------------------------------------------------
    # Отправляем ход на сервер
    # ------------------------------------------------
    async def _make_move(self,
                         ws: aiohttp.ClientSession,
                         game_id: int,
                         available_moves: list,
                         move_index: str):
        idx = int(move_index)
        #time.sleep(1)
        if idx < 0 or idx >= len(available_moves):
            logging.warning(f"Неверный индекс хода: {idx}")
            return
        chosen_move = available_moves[idx]
        logging.info(f"ИИ выбрал ход №{idx}: "
                     f"из {chosen_move['from']['x']},{chosen_move['from']['y']} "
                     f"в {chosen_move['to']['x']},{chosen_move['to']['y']}")

        # Формируем сообщение о ходе
        move_msg = {
            "arguments": [{
                "gameId": game_id,
                "turnNum": idx,  # можно заменить на текущий номер хода из stats
                "pirateId": chosen_move['pirateIds'][0] if chosen_move.get('pirateIds') else None,
                "withCoin": chosen_move.get('withCoin', False),
                "withRumBottle": chosen_move.get('withRumBottle', False)
            }],
            "invocationId": str(uuid.uuid4()),
            "target": "move",
            "type": 1
        }
        await ws.send_str(json.dumps(move_msg) + "\x1e")
        logging.debug(f"Отправлен ход: {move_msg}")

# ----------------------------------------------------
# 6. Точка входа
# ----------------------------------------------------
if __name__ == "__main__":
    bot_logic = MCTSBotLogic(simulations=SIMULATIONS,
                             max_depth=MAX_DEPTH,
                             exploration=EXPLORATION)
    bot = JackalBot(SERVER_URL, LOGIN, bot_logic)
    try:
        asyncio.run(bot.run())
    except KeyboardInterrupt:
        logging.info("Бот остановлен пользователем.")
