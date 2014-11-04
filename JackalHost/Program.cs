﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using Jackal.Players;
using JackalHost.Monitors;

namespace JackalHost
{
	internal class Program
	{
        private static MonitorForm _form;
        private static Board board;
        private static Game game;

        private static bool isPause = true;
        private static int turnTimeOutInMS = 128;
        private static int nextTurnes = 0;

        private static void formStart()
        {
            Application.EnableVisualStyles();
            Application.Run(_form);
        }

		private static void Main(string[] args)
		{
			IPlayer[] players =
			{
				new MikePlayer(), 
				new SmartPlayer(),
				new SmartPlayer(),
				new SmartPlayer(),
			};
			int mapId = 987412+1;
            board = new Board(mapId);
            game = new Game(players, board);

            _form = new MonitorForm(game, mapId);
            _form.OnCloseBtnClick += (s, e) =>
            {
                Environment.Exit(0);
            };
            _form.OnPauseBtnClick += (s, e) =>
            {
                isPause = !isPause;
            };
            _form.OnSlowerBtnClick += (s, e) =>
            {
                turnTimeOutInMS = (int) (turnTimeOutInMS*1.5);
                turnTimeOutInMS = Math.Min(turnTimeOutInMS, 2048);
            };
            _form.OnFasterBtnClick += (s, e) =>
            {
                turnTimeOutInMS = (int)(turnTimeOutInMS / 1.5);
                turnTimeOutInMS = Math.Max(turnTimeOutInMS, 2);
            };
            _form.OnNewGameBtnClick += (s, e) => {
                mapId = new Random().Next(1000000);
                board = new Board(mapId);
                game = new Game(players, board);
                _form.InitBoardPanel(game, mapId);
                isPause = false;
            };
            _form.OnNextOneBtnClick += (s, e) =>
            {
                isPause = true;
                nextTurnes = 1;
            };
            _form.OnNextTurnesBtnClick += (s, e) =>
            {
                isPause = true;
                nextTurnes = 4;
            };

            var thread = new Thread(formStart);
            thread.Start();

            while (true)
            {
                while (!game.IsGameOver)
                {
                    while (isPause)
                    {
                        if (nextTurnes > 0)
                        {
                            nextTurnes--;
                            break;
                        }
                        Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    }

                    var prevBoardStr = JsonHelper.SerialiazeWithType(board);
                    game.Turn();
                    var prevBoard = JsonHelper.DeserialiazeWithType<Board>(prevBoardStr);
                    
                    _form.Draw(board, prevBoard);
                    _form.DrawStats(game);
                    Thread.Sleep(TimeSpan.FromMilliseconds(turnTimeOutInMS));
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
		}
	}
}