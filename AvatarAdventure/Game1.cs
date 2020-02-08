﻿using System.Collections.Generic;
using AvatarAdventure.Components;
using AvatarAdventure.GameStates;
using AvatarAdventure.StateManager;
using AvatarAdventure.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AvatarAdventure
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<AnimationKey, Animation> playerAnimations = new Dictionary<AnimationKey, Animation>();
        GameStateManager gameStateManager;
        ITitleIntroState titleIntroState;
        IMainMenuState startMenuState;
        IGamePlayState gamePlayState;
        static Rectangle screenRectangle;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        public static Rectangle ScreenRectangle
        {
            get { return screenRectangle; }
        }
        public ITitleIntroState TitleIntroState
        {
            get { return titleIntroState; }
        }
        public IMainMenuState StartMenuState
        {
            get { return startMenuState; }
        }
        public IGamePlayState GamePlayState
        {
            get { return gamePlayState; }
        }
        public Dictionary<AnimationKey, Animation> PlayerAnimations
        {
            get { return playerAnimations; }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenRectangle = new Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;
            gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);
            this.IsMouseVisible = true;
            titleIntroState = new TitleIntroState(this);
            startMenuState = new MainMenuState(this);
            gamePlayState = new GamePlayState(this);

            gameStateManager.ChangeState((TitleIntroState)titleIntroState, PlayerIndex.One);
        }
        protected override void Initialize()
        {
            Components.Add(new Xin(this));
            Animation animation = new Animation(3, 32, 32, 0, 0);
            playerAnimations.Add(AnimationKey.WalkDown, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            playerAnimations.Add(AnimationKey.WalkLeft, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            playerAnimations.Add(AnimationKey.WalkRight, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            playerAnimations.Add(AnimationKey.WalkUp, animation);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
           Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }


}
