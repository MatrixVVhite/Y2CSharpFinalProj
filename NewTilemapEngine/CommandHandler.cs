using Positioning;
using CoreEngineHierarchy;
using Rendering;
using MovementAndInteraction;
using System.Runtime.InteropServices;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace Commands
{
    public static class CommandHandler
    {

        private static List<Command> commandList = []; //List Of Commands avialable for use

        private static Action<Command> AddNameDescription; //Name and description of the new commands

        public static MovementHandler MyMovemetHandler { get; set; } = new MovementHandler();

        /// <summary>
        /// This method handles all the commands.
        /// Create a new Command (Class) and use method AddCommandNameAndDescription with (New Command instance inside)
        /// </summary>
        public static void HandleCommands()
        {
            AddNameDescription += AddToHelpList;
            Command SelectCommand = new("Select", "Select your desired pawn", MyMovemetHandler.Select, true);
            AddCommandNameAndDescription(SelectCommand);
            Command DeselectCommand = new("Deselect", "Deselect your selected pawn", MyMovemetHandler.DeSelect, false);
            AddCommandNameAndDescription(DeselectCommand);
            Command MoveCommand = new("Move", "Move your selected pawn to a marked position of your choice", MyMovemetHandler.TryMoveCommand, true);
            AddCommandNameAndDescription(MoveCommand);
            Command HelpCommand = new("Help", "Display all commands", Help, false);
            AddCommandNameAndDescription(HelpCommand);
        }

        /// <summary>
        /// This adds the commands to the help list. Invoked by AddCommandNameAndDescription
        /// </summary>
        private static void AddToHelpList(Command command)
        {
            commandList.Add(command);
        }

        /// <summary>
        /// Use this method to add your command to the command list
        /// </summary>
        private static void AddCommandNameAndDescription(Command newCommand)
        {
            AddNameDescription.Invoke(newCommand);
        }

        /// <summary>
        /// Shows all the commands that have been added in AddCommandNameAndDescription
        /// </summary>
        private static bool Help(Position position, TileMap tileMap, RenderingEngine renderer)
        {
            int index = 1;
            foreach (var item in commandList)
            {
                Console.SetCursorPosition((MyMovemetHandler.chars.Count + 2) * 3, index);
                Console.WriteLine(item.Name + " - " + item.Description);
                index++;
            }
            Console.SetCursorPosition((MyMovemetHandler.chars.Count + 2) * 3, index + 1);
            Console.WriteLine("Press Any key To continue...");
            Console.SetCursorPosition(0, (MyMovemetHandler.chars.Count + 1));
            Console.ReadKey();
            return false;
        }



        /// <summary>
        /// Checks if the input was correct
        /// </summary>
        private static (int, int) ReturnPosition()
        {

            Console.WriteLine("Choose a tile");
            var tile = Console.ReadLine();
            if (!string.IsNullOrEmpty(tile) && tile.Length == 2)
            {
                char first = tile.First();
                char last = tile.Last();
                var x = char.ToLower(first);
                var y = char.GetNumericValue(last);
                if (char.IsLetter(first) && char.IsNumber(last) && MyMovemetHandler.chars.IndexOf(x) + 1 <= MyMovemetHandler.chars.Count - 2 && y <= MyMovemetHandler.chars.Count - 2)
                {
                    Console.WriteLine("Diagnose Print " + x + y);
                    return (MyMovemetHandler.chars.IndexOf(x) + 1, (int)y);
                }
                else
                {
                    Console.WriteLine("Wrong position");
                    Console.WriteLine("Press Any key To continue...");
                    Console.ReadKey();
                }
            }
            return (0, 0);
        }

        /// <summary>
        /// Handles all the input from the user
        /// </summary>
        public static void DiagnoseCommand(string input, TileMap tileMap, Rendering.RenderingEngine renderer)
        {
            bool commandSucc = false;

            for (int i = 0; i < commandList.Count; i++)
            {
                if (input.ToLower() == commandList[i].Name.ToLower() && commandList[i].PosReq)
                {
                    commandSucc = true;
                    var xy = ReturnPosition();
                    commandList[i].Execute.Invoke(new Position(xy.Item1, xy.Item2), tileMap, renderer);
                    renderer.UpdateAndRender(tileMap);
                    break;
                }
                else if (input.ToLower() == commandList[i].Name.ToLower() && !commandList[i].PosReq)
                {
                    commandSucc = true;
                    commandList[i].Execute.Invoke(new Position(0, 0), tileMap, renderer);
                    renderer.UpdateAndRender(tileMap);
                    break;
                }
                else commandSucc = false;
            }
            if (!commandSucc)
            {
                renderer.UpdateAndRender(tileMap);
                Help(new Position(0, 0), tileMap, renderer);
                renderer.UpdateAndRender(tileMap);
                Console.SetCursorPosition(0, (MyMovemetHandler.chars.Count + 1));
                //Console.ReadKey();

                //DiagnoseCommand(Console.ReadLine(), tileMap, renderer);

            }
        }
    }
}
