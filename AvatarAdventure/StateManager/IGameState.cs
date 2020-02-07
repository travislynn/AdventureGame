using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AvatarAdventure.StateManager
{
    public interface IGameState
    {
        GameState Tag { get; }
        PlayerIndex? PlayerIndexInControl { get; set; }
    }
}
