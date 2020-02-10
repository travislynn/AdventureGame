﻿namespace AvatarAdventure.AvatarComponents.Moves
{
    public class RockThrow : IMove
    {
        #region Field Region

        private string name;
        private Target target;
        private MoveType moveType;
        private MoveElement moveElement;
        private Status status;
        private bool unlocked;
        private int unlockedAt;
        private int duration;
        private int attack;
        private int defense;
        private int speed;
        private int health;

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
        }

        public Target Target
        {
            get { return target; }
        }

        public MoveType MoveType
        {
            get { return moveType; }
        }

        public MoveElement MoveElement
        {
            get { return moveElement; }
        }

        public Status Status
        {
            get { return status; }
        }

        public int UnlockedAt
        {
            get { return unlockedAt; }
            set { unlockedAt = value; }
        }

        public bool Unlocked
        {
            get { return unlocked; }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Attack
        {
            get { return attack; }
        }

        public int Defense
        {
            get { return defense; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public int Health
        {
            get { return health; }
        }

        #endregion

        #region Constructor Region

        public RockThrow()
        {
            name = "Rock Throw";
            target = Target.Enemy;
            moveType = MoveType.Attack;
            moveElement = MoveElement.Earth;
            status = Status.Normal;
            duration = 1;
            unlocked = false;
            attack = Avatar.Random.Next(0, 0);
            defense = Avatar.Random.Next(0, 0);
            speed = Avatar.Random.Next(0, 0);
            health = MoveManager.Random.Next(15, 20);
        }

        #endregion

        #region Method Region

        public void Unlock()
        {
            unlocked = true;
        }

        public object Clone()
        {
            RockThrow rockThrow = new RockThrow();
            rockThrow.unlocked = this.unlocked;
            rockThrow.unlockedAt = this.unlockedAt;
            return rockThrow;
        }

        #endregion
    }
}
