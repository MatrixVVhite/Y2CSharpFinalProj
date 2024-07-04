using CoreEngineHierarchy;
using Positioning;
using Rendering;
using NewTilemapEngine;


namespace MovementAndInteraction
{
    public class MovementHandler
    {
        public TileObject SelectedTileObject { get; set; } //ref
        public TileMap CommandtileMap { get; set; } //ref

        public List<Char> chars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j']; //used for select

        public Func<TileObject, TileObject, bool> CanEat { get; set; }

        public bool Select(Position position, TileMap tileMap, RenderingEngine renderer)
        {
            CommandtileMap = tileMap;
            Console.WriteLine("Selected Position " + position.X + " , " + position.Y);
            var selectedobject = tileMap.TileMapMatrix[position.X, position.Y].CurrentTileObject;
            if (selectedobject.Owner != null)
            {
                if (selectedobject.Owner.PlayerID == HundleTurns.CurrentPlayer)
                {
                    Console.WriteLine("Selected Tile Object " + selectedobject.TileObjectChar);
                    SelectedTileObject = selectedobject;
                    List<Position> PlacesToEat = new List<Position>();
                    foreach (var pos in selectedobject.Positions)
                    {
                        PlacesToEat.Add(new Position(0, 0));
                    }
                    foreach (var item in selectedobject.Positions)
                    {
                        Position destination = new(position.X + item.X, position.Y + item.Y);
                        var check = tileMap.TileMapMatrix[destination.X, destination.Y].Pass(position, destination, tileMap);

                        if (check)
                        {
                            if (tileMap.TileMapMatrix[destination.X, destination.Y].CurrentTileObject.TileObjectChar != " " &&
                                tileMap.TileMapMatrix[destination.X, destination.Y].CurrentTileObject.Owner.PlayerID != selectedobject.Owner.PlayerID &&
                                CanEat.Invoke(SelectedTileObject, tileMap.TileMapMatrix[destination.X, destination.Y].CurrentTileObject))
                            {
                                if (destination.X < SelectedTileObject.CurrentPos.X)
                                {
                                    tileMap.TileMapMatrix[destination.X - 1, destination.Y + SelectedTileObject.Owner.MovesToY].Color = ConsoleColor.Green;
                                }
                                else if (destination.X > SelectedTileObject.CurrentPos.X)
                                {
                                    tileMap.TileMapMatrix[destination.X + 1, destination.Y + SelectedTileObject.Owner.MovesToY].Color = ConsoleColor.Green;
                                }
                                continue;
                            }
                            tileMap.TileMapMatrix[destination.X, destination.Y].Color = ConsoleColor.Green;
                        }

                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("Wait! its not your turn");
                    Console.ReadKey();
                }
            }
            return false;
        }
        public bool TryMoveCommand(Position destinedLocation, TileMap tileMap, RenderingEngine renderer)
        {
            if (SelectedTileObject != null)
            {
                Position currentPos = new Position(SelectedTileObject.CurrentPos.X, SelectedTileObject.CurrentPos.Y);
                foreach (Position item in SelectedTileObject.Positions)
                {
                    Position moveDirection;
                    if (destinedLocation.X < currentPos.X)
                    {
                        moveDirection = new Position(-1, SelectedTileObject.Owner.MovesToY);
                    }
                    else
                    {
                        moveDirection = new Position(1, SelectedTileObject.Owner.MovesToY);
                    }
                    var check = CommandtileMap.TileMapMatrix[destinedLocation.X, destinedLocation.Y].Pass(currentPos, destinedLocation, CommandtileMap);

                    if (!check)
                    {
                        Console.WriteLine("can't move there!");
                        return false;
                    }
                    if (item.X + currentPos.X == moveDirection.X + currentPos.X && item.Y + currentPos.Y == moveDirection.Y + currentPos.Y)
                    {
                        Player targetTileOwner = tileMap.TileMapMatrix[currentPos.X + moveDirection.X, currentPos.Y + moveDirection.Y].CurrentTileObject.Owner;
                        if (targetTileOwner != SelectedTileObject.Owner && targetTileOwner != null)
                        {
                            if (CanEat.Invoke(SelectedTileObject, tileMap.TileMapMatrix[currentPos.X + moveDirection.X, currentPos.Y + moveDirection.Y].CurrentTileObject))
                            {
                                DeSelect(destinedLocation, tileMap, renderer);
                                while (SelectedTileObject.CurrentPos.X != destinedLocation.X && SelectedTileObject.CurrentPos.Y != destinedLocation.Y)
                                {
                                    CommandtileMap.MoveTileObject(SelectedTileObject, new Position(currentPos.X + moveDirection.X, currentPos.Y + moveDirection.Y));
                                    SelectedTileObject = CommandtileMap.TileMapMatrix[currentPos.X + moveDirection.X, currentPos.Y + moveDirection.Y].CurrentTileObject;
                                    currentPos = SelectedTileObject.CurrentPos;
                                }

                                Console.WriteLine(SelectedTileObject.Positions[0].X + SelectedTileObject.CurrentPos.X + ", " + SelectedTileObject.Positions[0].Y + SelectedTileObject.CurrentPos.Y);

                                if (HundleTurns.CurrentPlayer < HundleTurns.NumberOfPlayers)
                                {
                                    HundleTurns.CurrentPlayer++;
                                }
                                else
                                {
                                    HundleTurns.CurrentPlayer = 1;
                                }
                                ForgetSelected();
                            }
                        }
                        else if (targetTileOwner == null)
                        {
                            if (moveDirection.X < 0)
                            {
                                if (currentPos.X - destinedLocation.X == 1)
                                {
                                    DeSelect(destinedLocation, tileMap, renderer);
                                    CommandtileMap.MoveTileObject(SelectedTileObject, destinedLocation);
                                    Console.WriteLine(SelectedTileObject.Positions[0].X + SelectedTileObject.CurrentPos.X + ", " + SelectedTileObject.Positions[0].Y + SelectedTileObject.CurrentPos.Y);

                                    if (HundleTurns.CurrentPlayer < HundleTurns.NumberOfPlayers)
                                    {
                                        HundleTurns.CurrentPlayer++;
                                    }
                                    else
                                    {
                                        HundleTurns.CurrentPlayer = 1;
                                    }
                                    ForgetSelected();
                                }
                                else
                                {
                                    Console.WriteLine("wrong position!");
                                }
                            }
                            else if (moveDirection.X > 0)
                            {
                                if (destinedLocation.X - currentPos.X == 1)
                                {
                                    DeSelect(destinedLocation, tileMap, renderer);
                                    CommandtileMap.MoveTileObject(SelectedTileObject, destinedLocation);
                                    Console.WriteLine(SelectedTileObject.Positions[0].X + SelectedTileObject.CurrentPos.X + ", " + SelectedTileObject.Positions[0].Y + SelectedTileObject.CurrentPos.Y);

                                    if (HundleTurns.CurrentPlayer < HundleTurns.NumberOfPlayers)
                                    {
                                        if (TheEngine.GetInstance()._players[0].PiecesOwned.Count == 0)
                                        {
                                            Console.WriteLine("Player 2 wins");
                                            Console.ReadKey();
                                            Environment.Exit(0);
                                        }
                                        else if (TheEngine.GetInstance()._players[1].PiecesOwned.Count == 0)
                                        {
                                            Console.WriteLine("Player 1 wins");
                                            Console.ReadKey();
                                            Environment.Exit(0);
                                        }
                                        HundleTurns.CurrentPlayer++;
                                    }
                                    else
                                    {
                                        HundleTurns.CurrentPlayer = 1;
                                    }
                                    ForgetSelected();
                                }
                                else
                                {
                                    Console.WriteLine("wrong position!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("wrong position!");
                            }

                        }

                        renderer.UpdateAndRender(tileMap);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("wrong position!");
                    }
                }
            }
            renderer.UpdateAndRender(tileMap);
            return false;
        }
        public bool DeSelect(Position position, TileMap tileMap, RenderingEngine renderer)
        {
            Position currentPos = new Position(SelectedTileObject.CurrentPos.X, SelectedTileObject.CurrentPos.Y);
            foreach (var tile in tileMap.TileMapMatrix)
            {
                bool isBorderTile = true;
                if (tile.TilePos.X != 0 && tile.TilePos.Y != 0 && tile.TilePos.X != tileMap.TileMapMatrix.GetLength(0) - 1 && tile.TilePos.Y != tileMap.TileMapMatrix.GetLength(1) - 1)
                {
                    isBorderTile = false;
                }
                if (tile.TilePos.X % 2 == 0 && tile.TilePos.Y % 2 == 0 && !isBorderTile)
                {
                    CommandtileMap.TileMapMatrix[tile.TilePos.X, tile.TilePos.Y].Color = ConsoleColor.White;
                }
                else if (tile.TilePos.X % 2 != 0 && tile.TilePos.Y % 2 != 0 && !isBorderTile)
                {
                    CommandtileMap.TileMapMatrix[tile.TilePos.X, tile.TilePos.Y].Color = ConsoleColor.White;
                }
                else if (!isBorderTile)
                {
                    CommandtileMap.TileMapMatrix[tile.TilePos.X, tile.TilePos.Y].Color = ConsoleColor.Gray;
                }
            }
            return true;
        }
        public void ForgetSelected()
        {
            SelectedTileObject = null;
        }
    }
}
