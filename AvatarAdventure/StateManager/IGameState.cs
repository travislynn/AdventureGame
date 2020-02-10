using Microsoft.Xna.Framework;

namespace AvatarAdventure.StateManager
{
    public interface IGameState
    {
        GameState Tag { get; }
        PlayerIndex? PlayerIndexInControl { get; set; }
    }
}
