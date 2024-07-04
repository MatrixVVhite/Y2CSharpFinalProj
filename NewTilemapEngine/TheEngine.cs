using Rendering;
using CoreEngineHierarchy;
using Positioning;
using MovementAndInteraction;
using Commands;
using System.Reflection.Metadata.Ecma335;


namespace NewTilemapEngine
{
    public class TheEngine
    {
        public int InitialSizeX { get; set; } = 10; // default value
        public int InitialSizeY { get; set; } = 10; // default value

        char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        private static TheEngine _instance;
        public RenderingEngine _renderingEngine { get; private set; }
        public TileMap _addTiles { get; private set; }

        public List<Player> _players = new List<Player>();

        /// <summary>
        /// Template of an headline
        /// </summary>
        /// <param name="Title"></param>
        public void CreateHeadLine(string Title)
        {
            Console.WriteLine("   _   _   _   _   _    ");
            Console.WriteLine("  / \\ / \\ / \\ / \\ / \\   ");
            Console.WriteLine("     " + Title + "   ");
            Console.WriteLine("  \\_/ \\_/ \\_/ \\_/ \\_/   ");
        }


        /// <summary>
        /// singletone
        /// </summary>
        /// <returns></returns>
        private TheEngine()
        {
            _renderingEngine = new RenderingEngine(InitialSizeX, InitialSizeY);
            _addTiles = new TileMap(InitialSizeX, InitialSizeY);
        }

        public static TheEngine GetInstance()
        {
            return _instance ?? (_instance = new TheEngine());
        }

        /// <summary>
        /// Initialize of necessery parameters
        /// </summary>
        /// <returns></returns>
        public void Initialize(int sizeX, int sizeY)
        {
            HundleTurns.CurrentPlayer = 1;
            InitialSizeX = sizeX + 2;
            InitialSizeY = sizeY + 2;
            _renderingEngine = new RenderingEngine(InitialSizeX, InitialSizeY);
            _addTiles = new TileMap(InitialSizeX, InitialSizeY);
            for (int i = 0; i < HundleTurns.NumberOfPlayers; i++)
            {
                if (i % 2 == 0)
                {
                    _players.Add(new Player(i + 1, ConsoleColor.Cyan, 1, 1));
                }
                else
                {
                    _players.Add(new Player(i + 1, ConsoleColor.Red, 1, -1));
                }
            }
            SetCharList();
        }
        public void SetCharList()
        {
            CommandHandler.MyMovemetHandler.chars = new List<char>();
            for (int i = 0; i < InitialSizeX; i++)
            {
                CommandHandler.MyMovemetHandler.chars.Add(alphabet[i]);
            }
        }

        /// <summary>
        /// Set the chess size (max size is 8 by 8)
        /// </summary>
        /// <returns></returns>
        public RenderingEngine InitializeChessBoard(int sizeX, int sizeY)
        {
            if (sizeX >= 8) { sizeX = 8; }
            if (sizeY >= 8) { sizeY = 8; }
            Initialize(sizeX, sizeY);

            return _renderingEngine;
        }


        /// <summary>
        /// Update board
        /// </summary>
        /// <returns></returns>
        public void UpdateBoard()
        {
            _renderingEngine.UpdateAndRender(_addTiles);
        }

        /// <summary>
        /// Create commands
        /// </summary>
        /// <returns></returns>
        public void CreateCommand()
        {
            //CommandHandler.HandleCommands()
        }


        /// <summary>
        /// Allow the consumer an order to move the player
        /// </summary>
        /// <returns></returns>
        public void MoveTo(Position position)
        {
        }

        /// <summary>
        /// Template of checkers that the player could use if want to or put any other string
        /// </summary>
        /// <returns></returns>
        public void Template_Checkers(string gameType, string pieceColor, Position position)
        {
            char pieces = GetPieces(gameType);
            ConsoleColor pieceConsoleColor = GetPieceColor(pieceColor);



        }
        public TileObject CreateObjectForFirstPlayer(char pieces, Position pos)
        {
            TileObject to1 = new TileObject(pieces.ToString(), new List<Position> { new Position(1 * _players[0].MovesToX, 1 * _players[0].MovesToY), new Position(-1 * _players[0].MovesToX, 1 * _players[0].MovesToY) }, pos, _players[0]);
            _addTiles.InsertObjectToMap(to1, pos);
            return to1;
        }
        public TileObject CreateObjectForSecondPlayer(char pieces, Position pos)
        {
            TileObject to1 = new TileObject(pieces.ToString(), new List<Position> { new Position(1 * _players[1].MovesToX, 1 * _players[1].MovesToY), new Position(-1 * _players[1].MovesToX, 1 * _players[1].MovesToY) }, pos, _players[1]);
            _addTiles.InsertObjectToMap(to1, pos);
            return to1;
        }

        /// <summary>
        /// Piece template
        /// </summary>
        /// <returns></returns>
        public char GetPieces(string gameType)
        {
            switch (gameType.ToLower())
            {
                case "checkers":
                    return 'o';

                default:
                    throw new ArgumentException("Invalid game type", nameof(gameType));
            }
        }

        /// <summary>
        /// Color template
        /// </summary>
        /// <returns></returns>
        private ConsoleColor GetPieceColor(string pieceColor)
        {
            switch (pieceColor.ToLower())
            {
                case "black":
                    return ConsoleColor.Black;
                case "white":
                    return ConsoleColor.DarkYellow;
                default:
                    throw new ArgumentException("Invalid piece color", nameof(pieceColor));
            }
        }
    }


    /// <summary>
    /// All the methods that connected to the player
    /// </summary>
    public static class HundleTurns
    {
        public static int NumberOfmovesEachTurn;
        public static int NumberOfPlayers;
        public static int CurrentPlayer;

        static event Action PlayerActions = () => { };

        public static void AddTurns(int numberOfmovesEachTurn, int numberOfPlayers_AI)
        {
            NumberOfmovesEachTurn = numberOfmovesEachTurn;
            NumberOfPlayers = numberOfPlayers_AI;
            SubscribeEventOfTheClass();
            PlayerActions?.Invoke();
        }

        internal static void SubscribeEventOfTheClass()
        {
            PlayerActions += () => HandleTurns();
        }

        internal static void HandleTurns()
        {
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                if (i > 0)
                {
                    Console.WriteLine("Player Turn over");
                }
                for (int j = 0; j < NumberOfmovesEachTurn; j++)
                {
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Method to set the number of actors in the game
        /// </summary>
        /// <param name="numberOfActors"></param>
        /// <returns></returns>
        public static int setNumberOfPlayers(int numberOfActors)
        {
            int NumberOfActors = numberOfActors;

            return NumberOfActors;
        }
    }
}
