using Positioning;
using NewTilemapEngine;
using System.Net.Http.Headers;
using CoreEngineHierarchy;
using Commands;
using System.Numerics;

namespace Demo
{
    class Demo
    {
        private static Demo _instance;

        /// <summary>
        /// singletone
        /// </summary>
        /// <returns></returns>
        public static Demo GetInstance()
        {
            return _instance ?? (_instance = new Demo());
        }
        public TileObject MakeQueen(TileObject toPromote)
        {
            TileObject queen = new TileObject("Q", new List<Position>(), toPromote.CurrentPos, toPromote.Owner);
            queen.Positions.Clear();
            var mapBounds = TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(1) - 1;
            int counter = 1;
            for (int i = 0; i < TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(1); j++)
                {
                    if (TheEngine.GetInstance()._addTiles.TileMapMatrix[i, j].TilePos.X < TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(0) - 1 &&
                    TheEngine.GetInstance()._addTiles.TileMapMatrix[i, j].TilePos.Y < TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(1) - 1 &&
                    TheEngine.GetInstance()._addTiles.TileMapMatrix[i, j].TilePos.X > 0 && TheEngine.GetInstance()._addTiles.TileMapMatrix[i, j].TilePos.Y > 0)
                    {
                        if (toPromote.CurrentPos.X - toPromote.CurrentPos.Y == i - j || toPromote.CurrentPos.X + toPromote.CurrentPos.Y == i + j)
                        {
                            queen.Positions.Add(new Position(i - toPromote.CurrentPos.X, j - toPromote.CurrentPos.Y));
                        }
                        //queen.Positions.Add(new Position((-i) * toPromote.Owner.MovesToX, (+j) * toPromote.Owner.MovesToY)); //upper left
                        //queen.Positions.Add(new Position((+i) * toPromote.Owner.MovesToX, (+j) * toPromote.Owner.MovesToY));//upper right
                        //queen.Positions.Add(new Position((+i) * toPromote.Owner.MovesToX, (-j) * toPromote.Owner.MovesToY));//down right
                        //queen.Positions.Add(new Position((-i) * toPromote.Owner.MovesToX, (-j) * toPromote.Owner.MovesToY));// down left
                    }
                }
            }
            //for (int j = 1; j < mapBounds; j++)
            //{

            //    queen.Positions.Add( new Position(( -counter) * toPromote.Owner.MovesToX,   (+ j) * toPromote.Owner.MovesToY)); //upper left
            //    queen.Positions.Add(new Position(( + counter) * toPromote.Owner.MovesToX,  (+ j) * toPromote.Owner.MovesToY));//upper right
            //    queen.Positions.Add(new Position(( +  counter) * toPromote.Owner.MovesToX, (- j) * toPromote.Owner.MovesToY));//down right
            //    queen.Positions.Add(new Position(( -  counter) * toPromote.Owner.MovesToX, (- j) * toPromote.Owner.MovesToY));// down left
            //    counter++;
            //}
            queen.IsQueen = true;
            return queen;
        }


        string _choice_player;

        //Make the first scene
        public void OpenScene()
        {
            Console.WriteLine();
            TheEngine.GetInstance().CreateHeadLine("Checkers");
            Console.WriteLine();

            string _new_game = "------ To start a game enter 1 ------";

            Console.Write(new string(' ', (Console.WindowWidth - _new_game.Length) / 2));
            Console.WriteLine(_new_game);

            Console.WriteLine();

            string _exit_game = "------ To exit game enter 2 ------";

            Console.Write(new string(' ', (Console.WindowWidth - _exit_game.Length) / 2));
            Console.WriteLine(_exit_game);

            _choice_player = Console.ReadLine();
        }

        public void RunGame()
        {
            OpenScene();

            if (_choice_player == "1")
            {
                Start_Game();
            }

            else if (_choice_player == "2")
            {
                Environment.Exit(0);
            }

            else
            {
                Console.WriteLine("There is no optation like this, try again.");
                _choice_player = Console.ReadLine();
            }

        }


        public void Start_Game()
        {
            //HundleTurns.setNumberOfPlayers(2);
            HundleTurns.CurrentPlayer = 1;
            TheEngine.GetInstance()._players = new List<Player>() { new Player(1, ConsoleColor.Red, 1, 1), new Player(2, ConsoleColor.Blue, 1, -1) };
            CommandHandler.MyMovemetHandler.CanEat = CheckersEatingCheck;
            HundleTurns.AddTurns(1, 2);
            List<TileObject> list1 = new List<TileObject>();
            List<TileObject> list2 = new List<TileObject>();

            for (int i = 1; i < TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                    {
                        Position pos = new Position(i, j);

                        TheEngine.GetInstance().Template_Checkers("checkers", "black", pos);
                        list1.Add(TheEngine.GetInstance().CreateObjectForFirstPlayer(TheEngine.GetInstance().GetPieces("Checkers"), pos));
                    }
                }

                for (int j = TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(1) - 4; j < TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(1) - 1; j++)
                {
                    if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                    {
                        Position pos = new Position(i, j);

                        TheEngine.GetInstance().Template_Checkers("checkers", "white", pos);
                        list2.Add(TheEngine.GetInstance().CreateObjectForSecondPlayer(TheEngine.GetInstance().GetPieces("Checkers"), pos));
                    }
                }
            }

            TheEngine.GetInstance()._players[0].AddPieces(list1);
            TheEngine.GetInstance()._players[1].AddPieces(list2);

            //var myQueen =    MakeQueen(TheEngine.GetInstance()._addTiles.TileMapMatrix[2, 3].CurrentTileObject);
            //TheEngine.GetInstance()._addTiles.DeleteTileObjectAtPos(new Position(2,3));
            //TheEngine.GetInstance()._addTiles.InsertObjectToMap(myQueen, TheEngine.GetInstance()._addTiles.TileMapMatrix[2, 3].CurrentTileObject.CurrentPos);

            TheEngine.GetInstance().UpdateBoard();
            Commands.CommandHandler.HandleCommands();

            while (true)
            {

                Console.SetCursorPosition(TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(0) + 25, 1);
                Console.WriteLine($"Player {HundleTurns.CurrentPlayer}'s Turn");
                Console.SetCursorPosition(0, TheEngine.GetInstance()._addTiles.TileMapMatrix.GetLength(1) + 2);


                _choice_player = Console.ReadLine();
                Commands.CommandHandler.DiagnoseCommand(_choice_player, TheEngine.GetInstance()._addTiles, TheEngine.GetInstance()._renderingEngine);
            }
        }

        public bool CheckersEatingCheck(TileObject hungryTile, TileObject threatenedTile)
        {
            if (threatenedTile.CurrentPos.X < hungryTile.CurrentPos.X)
            {
                if (TheEngine.GetInstance()._addTiles.TileMapMatrix[threatenedTile.CurrentPos.X - 1, threatenedTile.CurrentPos.Y + hungryTile.Owner.MovesToY].CurrentTileObject.TileObjectChar == " ")
                    return true;
            }
            else if (threatenedTile.CurrentPos.X > hungryTile.CurrentPos.X)
            {
                if (TheEngine.GetInstance()._addTiles.TileMapMatrix[threatenedTile.CurrentPos.X + 1, threatenedTile.CurrentPos.Y + hungryTile.Owner.MovesToY].CurrentTileObject.TileObjectChar == " ")
                    return true;
            }
            return false;
        }
    }
}
