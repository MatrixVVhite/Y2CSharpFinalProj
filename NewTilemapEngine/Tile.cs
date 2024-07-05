using Positioning;

namespace CoreEngineHierarchy
{
    public class Tile : ITileAble
    {
        /// <summary>
        /// Properties that repersent the characteristics
        /// </summary>
        public Position TilePos { get; set; }

        public ConsoleColor Color { get; set; }

        public TileObject CurrentTileObject { get; set; }

        /// <summary>
        /// Check when the tile object can move forword to the next square
        /// </summary>

        bool ITileAble.Land(Position finalDestination)
        {

            //Tells the renderer to remove the current rendering of the TileObject and RePrinting it in the chosen destination, of course changing the tiles properly
            throw new NotImplementedException();
        }

        public bool Pass(Position startingPos, Position destination, TileMap currentMap)
        {
            return (destination.X < currentMap.TileMapMatrix.GetLength(0) - 1 &&
                destination.Y < currentMap.TileMapMatrix.GetLength(1) - 1 &&
                destination.X > 0 && destination.Y > 0 &&
                (currentMap.TileMapMatrix[destination.X, destination.Y].CurrentTileObject.Owner != currentMap.TileMapMatrix[startingPos.X, startingPos.Y].CurrentTileObject.Owner
                || currentMap.TileMapMatrix[destination.X, destination.Y].CurrentTileObject.Owner == null));
        }

        public Tile(Position index, ConsoleColor tileColor, TileObject currentTileObject)
        {
            TilePos = index;
            Color = tileColor;
            CurrentTileObject = currentTileObject;
            CurrentTileObject.CurrentPos = TilePos;
        }
    }
}
