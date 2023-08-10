using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Sprites
{
    public interface IHasSpriteAnimation
    {
        public SpriteAnimation Animation { get; }
        bool HorizontalFlipped { get; set; }
        float DepthLayer { get; set; }
    }
}
