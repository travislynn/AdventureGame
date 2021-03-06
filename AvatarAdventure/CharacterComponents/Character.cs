﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AvatarAdventure.AvatarComponents;
using AvatarAdventure.DTOs;
using AvatarAdventure.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.CharacterComponents
{
    public class Character : ICharacter
    {
        public const float SpeakingRadius = 40f;

        private string TextureName;

        private static Game1 gameRef;
        private static Dictionary<AnimationKey, Animation> characterAnimations = new Dictionary<AnimationKey, Animation>();

        public string Name { get; private set; }

        public AnimatedSprite Sprite { get; private set; }

        public Avatar BattleAvatar { get; private set; }

        public Avatar GiveAvatar { get; private set; }

        public string Conversation { get; private set; }

        public bool Battled { get; set; }


        private Character()
        {
        }
        private static void BuildAnimations()
        {
        }

        public static Character FromDTO(Game game, CharacterData data)
        {
            if (gameRef == null)
                gameRef = (Game1)game;
            if (characterAnimations.Count == 0)
                BuildAnimations();

            Character character = new Character();

            character.Name = data.Name;
            character.TextureName = data.TextureName;

            Texture2D texture = game.Content.Load<Texture2D>(@"CharacterSprites\" + data.TextureName);
            character.Sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);

            //AnimationKey key = AnimationKey.WalkDown;
            //Enum.TryParse<AnimationKey>(parts[2], true, out key);
            character.Sprite.CurrentAnimation = data.CurrentAnimation;
            character.Conversation = data.Conversation;
            character.BattleAvatar = AvatarManager.GetAvatar(data.BattleAvatar);
            return character;

        }

        // todo: json
        public static Character FromString(Game game, string characterString)
        {
            if (gameRef == null)
                gameRef = (Game1)game;
            if (characterAnimations.Count == 0)
                BuildAnimations();

            Character character = new Character();
            string[] parts = characterString.Split(',');
            character.Name = parts[0];
            character.TextureName = parts[1];

            Texture2D texture = game.Content.Load<Texture2D>(@"CharacterSprites\" + parts[1]);
            character.Sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);

            AnimationKey key = AnimationKey.WalkDown;
            Enum.TryParse<AnimationKey>(parts[2], true, out key);
            character.Sprite.CurrentAnimation = key;
            character.Conversation = parts[3];
            character.BattleAvatar = AvatarManager.GetAvatar(parts[4].ToLowerInvariant());
            return character;
        }

        public static Character FromJson(Game game, string jsonString)
        {
            if (gameRef == null)
                gameRef = (Game1)game;
            if (characterAnimations.Count == 0)
                BuildAnimations();

            Character character = new Character();
            string[] parts = jsonString.Split(',');
            character.Name = parts[0];
            character.TextureName = parts[1];

            Texture2D texture = game.Content.Load<Texture2D>(@"CharacterSprites\" + parts[1]);
            character.Sprite = new AnimatedSprite(texture, gameRef.PlayerAnimations);

            AnimationKey key = AnimationKey.WalkDown;
            Enum.TryParse<AnimationKey>(parts[2], true, out key);
            character.Sprite.CurrentAnimation = key;
            character.Conversation = parts[3];
            character.BattleAvatar = AvatarManager.GetAvatar(parts[4].ToLowerInvariant());
            return character;
        }

        public bool Save(BinaryWriter writer)
        {
            StringBuilder b = new StringBuilder();
            b.Append(Name);
            b.Append(",");
            b.Append(TextureName);
            b.Append(",");
            b.Append(Sprite.CurrentAnimation);
            writer.Write(b.ToString());
            if (GiveAvatar != null)
                GiveAvatar.Save(writer);
            if (BattleAvatar != null)
                BattleAvatar.Save(writer);
            return true;
        }
        public void SetConversation(string newConversation)
        {
            this.Conversation = newConversation;
        }
        
        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch);
        }
    }
}
