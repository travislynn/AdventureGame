using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvatarAdventure;
using AvatarAdventure.AvatarComponents;
using AvatarAdventure.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.CharacterComponents
{
    public class PCharacter : ICharacter
    {
        #region Constant
        public const float SpeakingRadius = 40f;
        public const int AvatarLimit = 6;
        #endregion
        #region Field Region
        private string name;
        private Avatar[] avatars = new Avatar[AvatarLimit];
        private int currentAvatar;
        private string textureName;

        private Avatar givingAvatar;
        private AnimatedSprite sprite;
        private string conversation;
        private static Game1 gameRef;
        private static Dictionary<AnimationKey, Animation> characterAnimations = new Dictionary<AnimationKey, Animation>();
        #endregion
        #region Property Region
        public string Name
        {
            get { return name; }
        }
        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }
        public Avatar BattleAvatar
        {
            get { return avatars[currentAvatar]; }
        }
        public Avatar GiveAvatar
        {
            get { return givingAvatar; }
        }
        public string Conversation
        {
            get { return conversation; }
        }

        public bool Battled { get; set; }


        #endregion
        #region Constructor Region
        private PCharacter()
        {
        }
        #endregion
        #region Method Region
        private static void BuildAnimations()
        {
        }
        public static PCharacter FromString(Game game, string characterString)
        {
            if (gameRef == null)
                gameRef = (Game1)game;
            if (characterAnimations.Count == 0)
                BuildAnimations();
            PCharacter character = new PCharacter();
            string[] parts = characterString.Split(',');


            //"Marissa 0 name,
            //teachertwo 1 textureName,
            //WalkDown 2 currentanimationkey,
            //tearchertwo 3 conversations,
            //0  - 4 - adding this for currentAvatar
            //wind 4 ??,
            //earth 5 ??
            character.name = parts[0];
            character.textureName = parts[1];
            Texture2D texture = game.Content.Load<Texture2D>(@"CharacterSprites\" + parts[1]);
            character.sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);
            AnimationKey key = AnimationKey.WalkDown;
            Enum.TryParse<AnimationKey>(parts[2], true, out key);
            character.sprite.CurrentAnimation = key;
            character.conversation = parts[3];
            // ERROR HERE
            character.currentAvatar = int.Parse(parts[4]);
            for (int i = 5; i < 11 && i < parts.Length; i++)
                character.avatars[i - 5] = AvatarManager.GetAvatar(parts[i].ToLowerInvariant());
            return character;
        }
        public bool Save(BinaryWriter writer)
        {
            StringBuilder b = new StringBuilder();
            b.Append(name);
            b.Append(",");
            b.Append(textureName);
            b.Append(",");
            b.Append(sprite.CurrentAnimation);
            b.Append(",");
            b.Append(conversation);
            b.Append(",");
            b.Append(currentAvatar);
            writer.Write(b.ToString());
            foreach (Avatar a in avatars)
            {
                if (a != null)
                    a.Save(writer);
            }
            return true;
        }


        public void ChangeAvatar(int index)
        {
            if (index < 0 || index >= AvatarLimit)
            {
                currentAvatar = index;
            }
        }
        public void SetConversation(string newConversation)
        {
            this.conversation = newConversation;
        }
        
        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}
