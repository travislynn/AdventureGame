using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvatarAdventure.CharacterComponents;
using AvatarAdventure.Components;
using AvatarAdventure.ConversationComponents;
using AvatarAdventure.GameStates;
using AvatarAdventure.PlayerComponents;
using Microsoft.Xna.Framework.Input;

namespace AvatarAdventure.GameStates
{
    public interface IConversationState
    {
        void SetConversation(Player player, ICharacter character);
        void StartConversation();
    }
    public class ConversationState : BaseGameState, IConversationState
    {
        #region Field Region
        private Conversation conversation;
        private SpriteFont font;
        private Texture2D background;
        private Player player;
        private ICharacter speaker;
        #endregion
        #region Property Region
        #endregion
        #region Constructor Region
        public ConversationState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IConversationState), this);
        }
        #endregion
        #region Method Region
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            font = GameRef.Content.Load<SpriteFont>(@"Fonts\scenefont");
            background = GameRef.Content.Load<Texture2D>(@"Scenes\scenebackground");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter))
            {
                switch (conversation.CurrentScene.OptionAction.Action)
                {
                    case ActionType.Buy:
                        break;
                    case ActionType.Change:
                        speaker.SetConversation(conversation.CurrentScene.OptionScene);
                        manager.PopState();
                        break;
                    case ActionType.End:
                        manager.PopState();
                        break;
                    case ActionType.GiveItems:
                        break;
                    case ActionType.GiveKey:
                        break;
                    case ActionType.Quest:
                        break;
                    case ActionType.Sell:
                        break;
                    case ActionType.Talk:
                        conversation.ChangeScene(conversation.CurrentScene.OptionScene);
                        break;
                }
            }
            conversation.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GameRef.SpriteBatch.Begin();
            conversation.Draw(gameTime, GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }
        public void SetConversation(Player player, ICharacter character)
        {
            this.player = player;
            speaker = character;
            // fails converting pcharacter to character, talking to first pchar
            // todo: better way of getting char.Converstaion from ICharacter.  Move to GetConverstaion or use base class

            // fix 1, use reflection to determine type of character.  But only 2 types and they both have Conversation.  Must be a better way
            //string charConvo = character is Character character1 ? character1.Conversation: ((PCharacter)character).Conversation;
            //if (ConversationManager.ConversationList.ContainsKey(charConvo))
            //    this.conversation =
            //        ConversationManager.ConversationList[charConvo];
            //else
            //    manager.PopState();


            // fix 2 by adding readonly conversation to ICharacter
            if (ConversationManager.ConversationList.ContainsKey(character.Conversation))
                this.conversation =
                    ConversationManager.ConversationList[character.Conversation];
            else
                manager.PopState();


            // original, fails converting pcharacter to character
            //if (ConversationManager.ConversationList.ContainsKey(((Character)character).Conversation))
            //    this.conversation =
            //        ConversationManager.ConversationList[((Character)character).Conversation];
            //else
            //    manager.PopState();
        }
        public void StartConversation()
        {
            conversation.StartConversation();
        }
        #endregion
    }
}
