using System;
using System.Collections.Generic;
using AvatarAdventure.AvatarComponents.Moves;

namespace AvatarAdventure.AvatarComponents
{
    public static class MoveManager
    {
        private static readonly Dictionary<string, IMove> _allMoves = new Dictionary<string, IMove>();

        public static Random Random { get; } = new Random();

        public static void FillMoves()
        {
            // TODO:  Abstract moves to a string, load from csv/source like avatars
            AddMove(new Tackle());
            AddMove(new Block());
            AddMove(new Haste());
            AddMove(new Bless());
            AddMove(new Curse());
            AddMove(new Heal());
            AddMove(new Flare());
            AddMove(new Shock());
            AddMove(new Gust());
            AddMove(new Frostbite());
            AddMove(new Shade());
            AddMove(new Burst());
            AddMove(new RockThrow());
        }
        public static IMove GetMove(string name)
        {
            if (_allMoves.ContainsKey(name))
                return (IMove)_allMoves[name].Clone();

            return null;
        }
        public static void AddMove(IMove move)
        {
            if (!_allMoves.ContainsKey(move.Name))
                _allMoves.Add(move.Name, move);
        }
    }
}