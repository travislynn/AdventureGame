using AvatarAdventure.AvatarComponents;

namespace AvatarAdventure.GameStates
{
    public interface IBattleState
    {
        void SetAvatars(Avatar player, Avatar enemy);
        void StartBattle();
    }
}