using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.TileMap
{
    public interface ICanUseTileMap<TTileMap>
    {
        public TTileMap TileMap { get; set; }
    }
}
