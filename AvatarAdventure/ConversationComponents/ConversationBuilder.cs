using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.ConversationComponents
{
    public class ConversationBuilder
    {
        private readonly Texture2D _sceneTexture;
        private readonly SpriteFont _sceneFont;
        private readonly Game _gameRef;

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

        public Conversation MakeLanceDefault()
        {
            Conversation lanceConversation = new Conversation("LanceHello", "Hello", _sceneTexture, _sceneFont);
            lanceConversation.BackgroundName = "scenebackground";
            lanceConversation.FontName = "scenefont";

            // scene 1 'hello' for lance
            lanceConversation.AddScene("Hello", new GameScene(
                _gameRef,
                "Fire avatars are my favorites. Do you like fire type avatars too?",
                new List<SceneOption> {
                    new SceneOption("No", "IDislikeFire",
                        new SceneAction(ActionType.Talk, "none")),
                    new SceneOption("Yes", "ILikeFire",
                        new SceneAction(ActionType.Talk, "none"))
                }));

            // 'ILikeFire' scene - ends
            lanceConversation.AddScene("ILikeFire", new GameScene(
                _gameRef,
                "That's cool. I have one if you want to see!",
                new List<SceneOption>() {
                    new SceneOption("Good bye.", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            // 'IDislikeFire' scene - ends
            lanceConversation.AddScene("IDislikeFire", new GameScene(
                _gameRef,
                "Each to their own I guess.  I don't like water at all.  Do you?",
                new List<SceneOption>() {
                    new SceneOption("Yeah man water is cool!", "CoolWater",
                        new SceneAction(ActionType.Talk, "none")),
                    new SceneOption("No water for this hombre", "NoWater",
                        new SceneAction(ActionType.Talk, "none"))
                }));

            lanceConversation.AddScene("CoolWater", new GameScene(
                _gameRef,
                "That's so cool that you like water.  I guess you better go play the game now.",
                new List<SceneOption>() {
                    new SceneOption("Ok", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Umm, ok", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Screw you Lance!", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            lanceConversation.AddScene("NoWater", new GameScene(
                _gameRef,
                "Hmm, it seems you don't like many avatars.  You DO understand what this game is, right?",
                new List<SceneOption>() {
                    new SceneOption("Avatars!", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Characters!", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Sweet Maps!", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            return lanceConversation;
        }
    }
}
