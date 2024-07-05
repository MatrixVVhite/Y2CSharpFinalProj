using CoreEngineHierarchy;
using Positioning;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Command : ICommandable
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool PosReq { get; private set; }
        public Func<Position, TileMap, RenderingEngine, bool> Execute { get; set; }

        public Command(string name, string description, Func<Position, TileMap, RenderingEngine, bool> action, bool posReq)
        {
            Name = name;
            Description = description;
            Execute = action;
            PosReq = posReq;
        }
    }
}
