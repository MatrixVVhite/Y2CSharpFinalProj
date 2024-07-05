using CoreEngineHierarchy;

namespace Rendering
{
    public class RenderingEngine
    {
        private TileMap _tiles;
        private string[,] _tilesToRender;

        public RenderingEngine(int indexerX, int indexerY)
        {
            _tiles = new TileMap(indexerX, indexerY);
            _tilesToRender = new string[_tiles.TileMapMatrix.GetLength(0), _tiles.TileMapMatrix.GetLength(1)];
            FinalizeTiles(_tiles);
        }

        /// <summary>
        ///  Replaces the current map with a new one, need to put on some consraints later
        /// </summary>
        private void ReplaceMap(TileMap tiles)
        {
            Console.Clear();
            _tiles = tiles;
            _tilesToRender = new string[_tiles.TileMapMatrix.GetLength(0), _tiles.TileMapMatrix.GetLength(1)];
            FinalizeTiles(_tiles);
        }

        /// <summary>
        ///  Encases all tiles withing the class with [ ] and saves them inside _tilesToRender
        ///  Runs upon initiaization
        /// </summary>
        public void FinalizeTiles(TileMap tileMapToAdd)
        {
            for (int y = 0; y < _tiles.TileMapMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < _tiles.TileMapMatrix.GetLength(1); x++)
                {
                    _tilesToRender[x, y] = $"[{tileMapToAdd.TileMapMatrix[x, y].CurrentTileObject.TileObjectChar}]";
                }
            }
        }

        /// <summary>
        ///  Updates the map and displays it
        /// </summary>
        public void UpdateAndRender(TileMap tma)
        {
            ReplaceMap(tma);
            DisplayAllTiles();
        }

        /// <summary>
        ///  Colors a single specified Tile (only affects background)
        /// </summary>
        //public void ColorTile(Tile tile, ConsoleColor bgColor) //TODO Fix ref later
        //{
        //    _tiles.TileMapMatrix[tile.TilePos.X, tile.TilePos.Y].Color = bgColor;
        //}

        /// <summary>
        ///  Colors a single specified Tile (only affects background)
        /// </summary>
        public void ColorTile(int x, int y, ConsoleColor bgColor)
        {
            _tiles.TileMapMatrix[x, y].Color = bgColor;
        }

        /// <summary>
        ///  Colors a single specified TileObject (only affects foreground)
        /// </summary>
        //public void ColorTileObject(TileObject tileObject, ConsoleColor fgColor) //TODO Fix ref later
        //{
        //    _tiles[tileObject.CurrentPos.X, tileObject.CurrentPos.Y].CurrentTileObject.Color = fgColor;
        //}

        /// <summary>
        ///  Colors a single specified TileObject (only affects foreground)
        /// </summary>
        public void ColorTileObject(int x, int y, ConsoleColor fgColor)
        {
            _tiles.TileMapMatrix[x, y].CurrentTileObject.Color = fgColor;
        }

        /// <summary>
        ///  Colors all Tiles with 1 uniform color (only affects background)
        /// </summary>
        public void ColorAllTiles(ConsoleColor bgcolor)
        {
            foreach (var tile in _tiles.TileMapMatrix)
            {
                tile.Color = bgcolor;
            }
        }
        /// <summary>
        ///  Colors all TileObjects with 1 uniform color (only affects foreground)
        /// </summary>
        public void ColorAllTileObjects(ConsoleColor fgColor)
        {
            foreach (var tile in _tiles.TileMapMatrix)
            {
                tile.CurrentTileObject.Color = fgColor;
            }
        }

        /// <summary>
        ///  Displays all tiles to the console
        /// </summary>
        private void DisplayAllTiles()
        {
            for (int y = 0; y < _tilesToRender.GetLength(0); y++)
            {
                for (int x = 0; x < _tilesToRender.GetLength(1); x++)
                {
                    Console.BackgroundColor = _tiles.TileMapMatrix[x, y].Color;
                    Console.ForegroundColor = _tiles.TileMapMatrix[x, y].CurrentTileObject.Color;
                    Console.Write(_tilesToRender[x, y]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        ///  returns tiles as a single multi-lined string
        /// </summary>
        public override string ToString()
        {
            string allTiles = "";
            for (int y = 0; y < _tilesToRender.GetLength(0); y++)
            {
                for (int x = 0; x < _tilesToRender.GetLength(1); x++)
                {
                    allTiles += _tilesToRender[x, y];
                }
                allTiles += "\n";
            }
            allTiles = allTiles.Substring(0, allTiles.Length - 1);
            return allTiles;
        }
    }
}