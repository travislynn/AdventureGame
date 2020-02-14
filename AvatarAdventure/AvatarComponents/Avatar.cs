using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AvatarAdventure.AvatarComponents.Moves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.AvatarComponents
{
    public class Avatar
    {
        private int _level;
        private int _costToBuy;

        public string Name { get; private set; }

        public int Level
        {
            get { return _level; }
            set { _level = (int)MathHelper.Clamp(value, 1, 100); }
        }
        public long Experience { get; private set; }

        public Texture2D Texture { get; private set; }

        public Dictionary<string, IMove> KnownMoves { get; private set; }

        public AvatarElement Element { get; private set; }

        public List<IMove> Effects { get; }

        public static Random Random { get; } = new Random();

        public int BaseAttack { get; private set; }

        public int BaseDefense { get; private set; }

        public int BaseSpeed { get; private set; }

        public int BaseHealth { get; private set; }

        public int CurrentHealth { get; private set; }

        public bool Alive
        {
            get { return (CurrentHealth > 0); }
        }

        private Avatar()
        {
            _level = 1;
            KnownMoves = new Dictionary<string, IMove>();
            Effects = new List<IMove>();
        }

        public void ResoleveMove(IMove move, Avatar target)
        {
            bool found = false;
            switch (move.Target)
            {
                case Target.Self:
                    if (move.MoveType == MoveType.Buff)
                    {
                        found = false;
                        for (int i = 0; i < Effects.Count; i++)
                        {
                            if (Effects[i].Name == move.Name)
                            {
                                Effects[i].Duration += move.Duration;
                                found = true;
                            }
                        }
                        if (!found)
                            Effects.Add((IMove)move.Clone());
                    }
                    else if (move.MoveType == MoveType.Heal)
                    {
                        CurrentHealth += move.Health;
                        if (CurrentHealth > BaseHealth)
                            CurrentHealth = BaseHealth;
                    }
                    else if (move.MoveType == MoveType.Status)
                    {
                    }
                    break;
                case Target.Enemy:
                    if (move.MoveType == MoveType.Debuff)
                    {
                        found = false;
                        for (int i = 0; i < target.Effects.Count; i++)
                        {
                            if (target.Effects[i].Name == move.Name)
                            {
                                target.Effects[i].Duration += move.Duration;
                                found = true;
                            }
                        }
                        if (!found)
                            target.Effects.Add((IMove)move.Clone());
                    }
                    else if (move.MoveType == MoveType.Attack)
                    {
                        float modifier = GetMoveModifier(move.MoveElement, target.Element);
                        float tDamage = GetAttack() + move.Health * modifier -
                                        target.GetDefense();
                        if (tDamage < 1f)
                            tDamage = 1f;
                        target.ApplyDamage((int)tDamage);
                    }
                    break;
            }
        }

        public static float GetMoveModifier(MoveElement moveElement, AvatarElement avatarElement)
        {
            float modifier = 1f;
            switch (moveElement)
            {
                case MoveElement.Dark:
                    if (avatarElement == AvatarElement.Light)
                        modifier += .25f;
                    else if (avatarElement == AvatarElement.Wind)
                        modifier -= .25f;
                    break;
                case MoveElement.Earth:
                    if (avatarElement == AvatarElement.Water)
                        modifier += .25f;
                    else if (avatarElement == AvatarElement.Wind)
                        modifier -= .25f;
                    break;
                case MoveElement.Fire:
                    if (avatarElement == AvatarElement.Wind)
                        modifier += .25f;
                    else if (avatarElement == AvatarElement.Water)
                        modifier -= .25f;
                    break;
                case MoveElement.Light:
                    if (avatarElement == AvatarElement.Dark)
                        modifier += .25f;
                    else if (avatarElement == AvatarElement.Earth)
                        modifier -= .25f;
                    break;
                case MoveElement.Water:
                    if (avatarElement == AvatarElement.Fire)
                        modifier += .25f;
                    else if (avatarElement == AvatarElement.Earth)
                        modifier -= .25f;
                    break;
                case MoveElement.Wind:
                    if (avatarElement == AvatarElement.Light)
                        modifier += .25f;
                    else if (avatarElement == AvatarElement.Earth)
                        modifier -= .25f;
                    break;
            }
            return modifier;
        }

        public void ApplyDamage(int tDamage)
        {
            CurrentHealth -= tDamage;
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Duration--;
                if (Effects[i].Duration < 1)
                {
                    Effects.RemoveAt(i);
                    i--;
                }
            }
        }

        public int GetAttack()
        {
            int attackMod = 0;
            foreach (IMove move in Effects)
            {
                if (move.MoveType == MoveType.Buff)
                    attackMod += move.Attack;
                if (move.MoveType == MoveType.Debuff)
                    attackMod -= move.Attack;
            }
            return BaseAttack + attackMod;
        }
        public int GetDefense()
        {
            int defenseMod = 0;
            foreach (IMove move in Effects)
            {
                if (move.MoveType == MoveType.Buff)
                    defenseMod += move.Defense;
                if (move.MoveType == MoveType.Debuff)
                    defenseMod -= move.Defense;
            }
            return BaseDefense + defenseMod;
        }
        public int GetSpeed()
        {
            int speedMod = 0;
            foreach (IMove move in Effects)
            {
                if (move.MoveType == MoveType.Buff)
                    speedMod += move.Speed;
                if (move.MoveType == MoveType.Debuff)
                    speedMod -= move.Speed;
            }
            return BaseSpeed + speedMod;
        }
        public int GetHealth()
        {
            int healthMod = 0;
            foreach (IMove move in Effects)
            {
                if (move.MoveType == MoveType.Buff)
                    healthMod += move.Health;
                if (move.MoveType == MoveType.Debuff)
                    healthMod += move.Health;
            }
            return BaseHealth + healthMod;
        }

        public void StartCombat()
        {
            Effects.Clear();
            CurrentHealth = BaseHealth;
        }
        public long WinBattle(Avatar target)
        {
            int levelDiff = target.Level - _level;
            long expGained = 0;
            if (levelDiff <= -10)
            {
                expGained = 10;
            }
            else if (levelDiff <= -5)
            {
                expGained = (long)(100f * (float)Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 0)
            {
                expGained = (long)(50f * (float)Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 5)
            {
                expGained = (long)(5f * (float)Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 10)
            {
                expGained = (long)(10f * (float)Math.Pow(2, levelDiff));
            }
            else
            {
                expGained = (long)(50f * (float)Math.Pow(2, target.Level));
            }
            return expGained;
        }
        public long LoseBattle(Avatar target)
        {
            return (long)((float)WinBattle(target) * .5f);
        }

        public bool CheckLevelUp()
        {
            bool leveled = false;
            if (Experience >= 50 * (1 + (long)Math.Pow((_level - 1), 2.5)))
            {
                leveled = true;
                _level++;
            }
            return leveled;
        }


        public void AssignPoint(string s, int p)
        {
            switch (s)
            {
                case "Attack":
                    BaseAttack += p;
                    break;
                case "Defense":
                    BaseDefense += p;
                    break;
                case "Speed":
                    BaseSpeed += p;
                    break;
                case "Health":
                    BaseHealth += p;
                    break;
            }
        }


        public static Avatar FromString(string description, ContentManager content)
        {
            Avatar avatar = new Avatar();
            string[] parts = description.Split(',');
            avatar.Name = parts[0];
            avatar.Texture = content.Load<Texture2D>(@"AvatarImages\" + parts[0]);
            avatar.Element = (AvatarElement)Enum.Parse(typeof(AvatarElement), parts[1]);
            avatar._costToBuy = int.Parse(parts[2]);
            avatar._level = int.Parse(parts[3]);
            avatar.BaseAttack = int.Parse(parts[4]);
            avatar.BaseDefense = int.Parse(parts[5]);
            avatar.BaseSpeed = int.Parse(parts[6]);
            avatar.BaseHealth = int.Parse(parts[7]);
            avatar.CurrentHealth = avatar.BaseHealth;
            avatar.KnownMoves = new Dictionary<string, IMove>();
            for (int i = 8; i < parts.Length; i++)
            {
                string[] moveParts = parts[i].Split(':');
                if (moveParts[0] != "None")
                {
                    IMove move = MoveManager.GetMove(moveParts[0]);
                    move.UnlockedAt = int.Parse(moveParts[1]);
                    if (move.UnlockedAt <= avatar.Level)
                        move.Unlock();
                    avatar.KnownMoves.Add(move.Name, move);
                }
            }
            return avatar;
        }

        public object Clone()
        {
            Avatar avatar = new Avatar();
            avatar.Name = this.Name;
            avatar.Texture = this.Texture;
            avatar.Element = this.Element;
            avatar._costToBuy = this._costToBuy;
            avatar._level = this._level;
            avatar.Experience = this.Experience;
            avatar.BaseAttack = this.BaseAttack;
            avatar.BaseDefense = this.BaseDefense;
            avatar.BaseSpeed = this.BaseSpeed;
            avatar.BaseHealth = this.BaseHealth;
            avatar.CurrentHealth = this.BaseHealth;
            foreach (string s in this.KnownMoves.Keys)
            {
                avatar.KnownMoves.Add(s, this.KnownMoves[s]);
            }
            return avatar;
        }

        public bool Save(BinaryWriter writer)
        {
            StringBuilder b = new StringBuilder();
            b.Append(Name);
            b.Append(",");
            b.Append(Element);
            b.Append(",");
            b.Append(Experience);
            b.Append(",");
            b.Append(_costToBuy);
            b.Append(",");
            b.Append(_level);
            b.Append(",");
            b.Append(BaseAttack);
            b.Append(",");
            b.Append(BaseDefense);
            b.Append(",");
            b.Append(BaseSpeed);
            b.Append(",");
            b.Append(BaseHealth);
            b.Append(",");
            b.Append(CurrentHealth);
            foreach (string s in KnownMoves.Keys)
            {
                b.Append(",");
                b.Append(s);
            }
            writer.Write(b.ToString());
            return true;
        }

    }
}
