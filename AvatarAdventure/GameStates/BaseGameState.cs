using System;
using AvatarAdventure.StateManager;
using Microsoft.Xna.Framework;


namespace AvatarAdventure.GameStates
{
    public class BaseGameState : GameState
    {
        protected static Random random = new Random();
        protected Game1 GameRef;

        public BaseGameState(Game game) : base(game)
        {
            GameRef = (Game1)game;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
