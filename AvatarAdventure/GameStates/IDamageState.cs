using AvatarAdventure.AvatarComponents;

namespace AvatarAdventure.GameStates
{
    public interface IDamageState
    {
        void SetAvatars(Avatar player, Avatar enemy);
        void SetMoves(IMove playerMove, IMove enemyMove);
        void Start();
    }
}