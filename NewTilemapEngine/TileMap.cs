using Positioning;

namespace CoreEngineHierarchy
{
    public class TileMap
    {
        /// <summary>
        /// The 2d array that contain the tile map
        /// </summary>
        public Tile[,] TileMapMatrix { get; set; }


        /// <summary>
        /// The tile map reading
        /// </summary>
        public TileMap(int indexerX, int indexerY) //Reminder: this was internal
        {
            ConsoleColor TileColor;
            TileMapMatrix = new Tile[indexerX, indexerY];

            for (int i = 0; i < TileMapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMapMatrix.GetLength(1); j++)
                {
                    if (i == 0 || i == TileMapMatrix.GetLength(0) - 1 || j == 0 || j == TileMapMatrix.GetLength(1) - 1)
                    {
                        TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.DarkCyan, new TileObject(" ", new List<Position> { }, new Position(j, i), ConsoleColor.Black));
                        FillFrame();
                    }
                    else
                    {
                        TileColor = ConsoleColor.Black;

                        if (i % 2 == 0)
                        {
                            if (j % 2 == 0)
                            {
                                TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.White, new TileObject(" ", new List<Position> { }, new Position(j, i), TileColor));
                            }
                            else if (j % 2 != 0)
                            {
                                TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.Gray, new TileObject(" ", new List<Position> { }, new Position(j, i), TileColor));
                            }
                        }
                        else
                        {
                            if (j % 2 != 0)
                            {
                                TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.White, new TileObject(" ", new List<Position> { }, new Position(j, i), TileColor));
                            }
                            else if (j % 2 == 0)
                            {
                                TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.Gray, new TileObject(" ", new List<Position> { }, new Position(j, i), TileColor));
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Allow to add tile maps
        /// </summary>
        public void InsertObjectToMap(TileObject objectToInsert, Position whereToInsert)
        {
            TileMapMatrix[whereToInsert.X, whereToInsert.Y].CurrentTileObject = objectToInsert;
        }

        /// <summary>
        /// Deletes a TileObject from the map
        /// </summary>
        public void DeleteTileObjectAtPos(Position newPos)
        {
            //var newObject = new TileObject(" ", new Position[] { }, newPos, TileMapMatrix[newPos.X + 1, newPos.Y + 1].CurrentTileObject.Color);
            var newObject = new TileObject(" ", new List<Position> { }, newPos, ConsoleColor.Black);
            InsertObjectToMap(newObject, newPos);
        }

        /// <summary>
        /// Moves a TileObject to a location
        /// </summary>
        public void MoveTileObject(TileObject tileObject, Position newPos)
        {
            var newObject = new TileObject(tileObject.TileObjectChar, tileObject.Positions, new Position(newPos.X, newPos.Y), tileObject.Owner);
            InsertObjectToMap(newObject, newPos);
            DeleteTileObjectAtPos(tileObject.CurrentPos);
        }

        /// <summary>
        /// Add the frame numbers and letters
        /// </summary>
        private void FillFrame()
        {
            for (int i = 0; i < TileMapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TileMapMatrix.GetLength(0); j++)
                {
                    if (i == 0 || i == TileMapMatrix.GetLength(0) - 1)
                    {
                        TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.DarkCyan, new TileObject(j.ToString(), new List<Position> { }, new Position(j, i), ConsoleColor.Black));
                    }

                    else if (j == 0 || j == TileMapMatrix.GetLength(1) - 1)
                    {
                        char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
                        TileMapMatrix[i, j] = new Tile(new Position(i, j), ConsoleColor.DarkCyan, new TileObject(alphabet[i - 1].ToString(), new List<Position> { }, new Position(j, i), ConsoleColor.Black));
                    }
                }
            }
        }
    }
}
