﻿using System;
using System.Threading;

namespace Jackal.Core.Players
{
    public class HumanPlayer : IPlayer
    {
        public event EventHandler OnNextTurnesBtnClick;
        static EventWaitHandle _waitHandle = new AutoResetEvent(false);

        public HumanPlayer(EventWaitHandle waitHandle)
        {
            _waitHandle = waitHandle;
        }

        public void OnNewGame()
        {
        }

        public void SetHumanMove(int moveNum)
        {
            throw new NotImplementedException();
        }

        public int OnMove(GameState gameState)
        {
            _waitHandle.WaitOne();
            // здесь человек-пират должен отправить на форму доступные ходы и ждать выбора
            // нужно передать данные на форму gameState.AvailableMoves.Length
            return 0;
        }
    }
}
