using CoreEngineHierarchy;
using Positioning;
using Rendering;
using MovementAndInteraction;
using System.Numerics;

namespace Commands
{
    internal class Program
    {
        static void Main(string[] args)
        {

            RenderingEngine tmr = new RenderingEngine(8, 8);
            TileMap myTileMap = new TileMap(8, 8);

            Player p1 = new Player(0, ConsoleColor.Red, 1, 1);
            Player p2 = new Player(1, ConsoleColor.Blue, 1, -1);

            TileObject to1 = new TileObject("E", new List<Position> { new Position(0, 0), new Position(0, 0) }, new Position(2, 2), p1);
            to1.Positions[0] = new Position(1 * to1.Owner.MovesToX, 1 * to1.Owner.MovesToY);
            to1.Positions[1] = new Position(-1 * to1.Owner.MovesToX, 1 * to1.Owner.MovesToY);
            TileObject to3 = to1.CloneToPos(new Position(4, 4));
            TileObject to2 = new TileObject("P", new List<Position> { new Position(0, 0), new Position(0, 0) }, new Position(2, 4), p2);
            to2.Positions[0] = new Position(1 * to2.Owner.MovesToX, 1 * to2.Owner.MovesToY);
            to2.Positions[1] = new Position(-1 * to2.Owner.MovesToX, 1 * to2.Owner.MovesToY);

            List<TileObject> p1List = new List<TileObject>() { to1, to3 };
            p1.AddPieces(p1List);
            List<TileObject> p2List = new List<TileObject>() { to2 };
            p2.AddPieces(p2List);

            foreach (TileObject obj in p1.PiecesOwned)
            {
                myTileMap.InsertObjectToMap(obj, obj.CurrentPos);
            }
            foreach (TileObject obj in p2.PiecesOwned)
            {
                myTileMap.InsertObjectToMap(obj, obj.CurrentPos);
            }

            tmr.UpdateAndRender(myTileMap);
            CommandHandler.HandleCommands();

            while (true)
            {
                Console.SetCursorPosition(0, myTileMap.TileMapMatrix.GetLength(0) + 1);
                CommandHandler.DiagnoseCommand(Console.ReadLine(), myTileMap, tmr);
                tmr.UpdateAndRender(myTileMap);
            }
        }
    }
}
