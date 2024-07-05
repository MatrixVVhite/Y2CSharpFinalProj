using CoreEngineHierarchy;
using Positioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    internal interface ICommandable
    {
        public Func<Positioning.Position, TileMap, Rendering.RenderingEngine, bool> Execute { get; set; }
    }
}
