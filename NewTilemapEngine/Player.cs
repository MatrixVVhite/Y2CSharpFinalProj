namespace CoreEngineHierarchy
{
    public class Player
    {
        public int PlayerID { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public List<TileObject> PiecesOwned { get; set; } = new List<TileObject>();
        public ConsoleColor PiecesColor { get; private set; }
        public int MovesToX { get; private set; } //Multiply by X movement to move in the right direction, must be either -1, 0, or 1
        public int MovesToY { get; private set; } //Multiply by Y movement to move in the right direction, must be either -1, 0, or 1

        /// <summary>
        /// Creates a player with a list of pieces and movement direction, direction must be either -1, 0, or 1
        /// </summary>
        public Player(int playerID, List<TileObject> pieces, ConsoleColor piecesColor, int movesToX, int movesToY)
        {
            PlayerID = playerID;
            PiecesOwned = pieces;
            PiecesColor = piecesColor;
            foreach (var piece in PiecesOwned)
            {
                piece.Color = PiecesColor;
            }
            if (movesToX <= -2 || movesToX >= 2)
            {
                throw new Exception("movesToX must be between -1 to 1");
            }
            if (movesToY <= -2 || movesToY >= 2)
            {
                throw new Exception("movesToY must be between -1 to 1");
            }
            MovesToX = movesToX;
            MovesToY = movesToY;
        }

        /// <summary>
        /// Creates a player with only a color and movement direction, direction must be either -1, 0, or 1
        /// </summary>
        public Player(int playerID, ConsoleColor piecesColor, int movesToX, int movesToY)
        {
            PlayerID = playerID;
            PiecesColor = piecesColor;
            if (movesToX <= -2 || movesToX >= 2)
            {
                throw new Exception("movesToX must be between -1 to 1");
            }
            if (movesToY <= -2 || movesToY >= 2)
            {
                throw new Exception("movesToY must be between -1 to 1");
            }
            MovesToX = movesToX;
            MovesToY = movesToY;
        }

        /// <summary>
        /// Replace current piece list with a new one
        /// </summary>
        public void ReplacePieces(List<TileObject> pieces)
        {
            List<TileObject> newList = new List<TileObject>();
            foreach (var piece in pieces)
            {
                piece.Color = PiecesColor;
                newList.Add(piece);
            }
            PiecesOwned.Clear();
            PiecesOwned = newList;
        }

        /// <summary>
        /// Adds a list of pieces to the existing piece list
        /// </summary>
        public void AddPieces(List<TileObject> pieces)
        {
            foreach (var piece in pieces)
            {
                piece.Color = PiecesColor;
                PiecesOwned.Add(piece);
            }
        }
    }
}
