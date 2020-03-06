using AvatarAdventure.TileEngine;

namespace AvatarAdventure.DTOs
{
    public class CharacterData
    {
        public string Name;
        public string TextureName;
        public AnimationKey CurrentAnimation;
        public string Conversation;
        public string BattleAvatar;

        public CharacterData(string name, string textureName, AnimationKey currentAnimation, string conversation, string battleAvatar)
        {
            Name = name;
            TextureName = textureName;
            CurrentAnimation = currentAnimation;
            Conversation = conversation;
            BattleAvatar = battleAvatar;
        }
    }
}
