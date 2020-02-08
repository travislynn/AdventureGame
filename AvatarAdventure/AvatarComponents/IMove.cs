namespace AvatarAdventure.AvatarComponents
{
    public interface IMove
    {
        string Name { get; }
        Target Target { get; }
        MoveType MoveType { get; }
        MoveElement MoveElement { get; }
        Status Status { get; }
        int UnlockedAt { get; set; }
        bool Unlocked { get; }
        int Duration { get; set; }
        int Attack { get; }
        int Defense { get; }
        int Speed { get; }
        int Health { get; }
        void Unlock();
        object Clone();
    }
}