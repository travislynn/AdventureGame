using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using AvatarAdventure.AvatarComponents;
using AvatarAdventure.TileEngine;

namespace AvatarAdventure.CharacterComponents
{
    public interface ICharacter
    {
        string Name { get; }
        bool Battled { get; set; }
        AnimatedSprite Sprite { get; }
        Avatar BattleAvatar { get; }
        Avatar GiveAvatar { get; }
        string Conversation { get; }
        void SetConversation(string newConversation);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        bool Save(BinaryWriter writer);
    }
}
