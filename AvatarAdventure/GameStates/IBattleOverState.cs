using AvatarAdventure.AvatarComponents;

namespace AvatarAdventure.GameStates
{
    public interface IBattleOverState
    {
        void SetAvatars(Avatar player, Avatar enemy);
    }
}