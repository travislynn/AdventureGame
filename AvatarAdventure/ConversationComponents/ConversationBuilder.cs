using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.ConversationComponents
{
    public class ConversationBuilder
    {
        private Texture2D _sceneTexture;
        private SpriteFont _sceneFont;
        private Game _gameRef;

        public ConversationBuilder(Game gameRef)
        {
            _gameRef = gameRef;
            _sceneTexture = gameRef.Content.Load<Texture2D>(@"Scenes\scenebackground");
            _sceneFont = gameRef.Content.Load<SpriteFont>(@"Fonts\scenefont");
        }
        
        public Conversation MakeMarissaDefault()
        {
            var marissaConversation = new Conversation("MarissaHello", "Hello", _sceneTexture, _sceneFont);
            marissaConversation.BackgroundName = "scenebackground";
            marissaConversation.FontName = "scenefont";

            marissaConversation.AddScene("Hello", new GameScene(
                _gameRef,
                "Hello, my name is Marissa. I'm still learning about summoning avatars.",
                new List<SceneOption>() {
                    new SceneOption("Good bye.", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            return marissaConversation;
        }
    }
}
