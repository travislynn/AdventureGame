using System.Collections.Generic;
using AvatarAdventure.CharacterComponents;
using AvatarAdventure.Components;
using AvatarAdventure.GameStates;
using AvatarAdventure.StateManager;
using AvatarAdventure.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//  todo:  trigger "extremely effective/ineffective message"
// todo: trigger level up
// todo:  add characters to level 2
// todo:  refactor files.


namespace AvatarAdventure
{
    public class Game1 : Game
    {
        private IConversationState _conversationState;


        public SpriteBatch SpriteBatch { get; private set; }

        public static Rectangle ScreenRectangle { get; private set; }

        public ITitleIntroState TitleIntroState { get; }

        public IMainMenuState StartMenuState { get; }

        public IGamePlayState GamePlayState { get; }

        public IBattleState BattleState { get; }

        public ILevelUpState LevelUpState { get; }

        public IBattleOverState BattleOverState { get; }

        public IDamageState DamageState { get; }


        public Dictionary<AnimationKey, Animation> PlayerAnimations { get; } = new Dictionary<AnimationKey, Animation>();

        public CharacterManager CharacterManager { get; }

        public Game1()
        {
            var graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenRectangle = new Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;
            var gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);
            this.IsMouseVisible = true;
            TitleIntroState = new TitleIntroState(this);
            StartMenuState = new MainMenuState(this);
            GamePlayState = new GamePlayState(this);
            _conversationState = new ConversationState(this);
            BattleState = new BattleState(this);
            BattleOverState = new BattleOverState(this);
            DamageState = new DamageState(this);
            LevelUpState = new LevelUpState(this);
            gameStateManager.ChangeState((TitleIntroState)TitleIntroState, PlayerIndex.One);
            CharacterManager = CharacterManager.Instance;
        }

        protected override void Initialize()
        {
            Components.Add(new Xin(this));
            Animation animation = new Animation(3, 64, 64, 0, 0);
            PlayerAnimations.Add(AnimationKey.WalkDown, animation);
            animation = new Animation(3, 64, 64, 0, 64);
            PlayerAnimations.Add(AnimationKey.WalkLeft, animation);
            animation = new Animation(3, 64, 64, 0, 128);
            PlayerAnimations.Add(AnimationKey.WalkRight, animation);
            animation = new Animation(3, 64, 64, 0, 192);
            PlayerAnimations.Add(AnimationKey.WalkUp, animation);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            AvatarComponents.MoveManager.FillMoves();
            AvatarComponents.AvatarManager.FromFile(@".\Data\avatars.csv", Content);
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