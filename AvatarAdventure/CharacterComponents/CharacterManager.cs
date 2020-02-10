using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvatarAdventure.CharacterComponents;

namespace AvatarAdventure.CharacterComponents
{
    public sealed class CharacterManager
    {
        private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

        // Static constructor
        public static CharacterManager Instance { get; } = new CharacterManager();

        private CharacterManager()
        {
        }

        public ICharacter GetCharacter(string name)
        {
            if (_characters.ContainsKey(name))
                return _characters[name];
            return null;
        }

        public void AddCharacter(string name, ICharacter character)
        {
            if (!_characters.ContainsKey(name))
            {
                _characters.Add(name, character);
            }
        }
    }
}