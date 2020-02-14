using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.ConversationComponents
{
    public class Conversation
    {
        private string _currentScene;

        public string Name { get; }

        public string FirstScene { get; }

        public GameScene CurrentScene => Scenes[_currentScene];

        public Dictionary<string, GameScene> Scenes { get; }

        public Texture2D Background { get; }

        public SpriteFont SpriteFont { get; }

        public string BackgroundName { get; set; }

        public string FontName { get; set; }

        public Conversation(string name, string firstScene, Texture2D background, SpriteFont font)
        {
            this.Scenes = new Dictionary<string, GameScene>();
            this.Name = name;
            this.FirstScene = firstScene;
            this.Background = background;
            this.SpriteFont = font;
        }
        public void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime, PlayerIndex.One);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(gameTime, spriteBatch, Background, SpriteFont);
        }
        public void AddScene(string sceneName, GameScene scene)
        {
            if (!Scenes.ContainsKey(sceneName))
                Scenes.Add(sceneName, scene);
        }
        public GameScene GetScene(string sceneName)
        {
            if (Scenes.ContainsKey(sceneName))
                return Scenes[sceneName];
            return null;
        }
        public void StartConversation()
        {
            _currentScene = FirstScene;
        }
        public void ChangeScene(string sceneName)
        {
            _currentScene = sceneName;
        }
    }

}