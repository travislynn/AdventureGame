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
        private SpriteBatch _spriteBatch;
        private readonly Dictionary<AnimationKey, Animation> _playerAnimations = new Dictionary<AnimationKey, Animation>();
        private readonly CharacterManager _characterManager;
        private readonly ITitleIntroState _titleIntroState;
        private readonly IMainMenuState _startMenuState;
        private readonly IGamePlayState _gamePlayState;
        private IConversationState _conversationState;
        private readonly IBattleState _battleState;
        private readonly ILevelUpState _levelUpState;
        private readonly IBattleOverState _battleOverState;
        private readonly IDamageState _damageState;

        static Rectangle _screenRectangle;


        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public static Rectangle ScreenRectangle
        {
            get { return _screenRectangle; }
        }

        public ITitleIntroState TitleIntroState
        {
            get { return _titleIntroState; }
        }

        public IMainMenuState StartMenuState
        {
            get { return _startMenuState; }
        }

        public IGamePlayState GamePlayState
        {
            get { return _gamePlayState; }
        }

        public IBattleState BattleState
        {
            get { return _battleState; }
        }

        public ILevelUpState LevelUpState
        {
            get { return _levelUpState; }
        }

        public IBattleOverState BattleOverState
        {
            get { return _battleOverState; }
        }

        public IDamageState DamageState
        {
            get { return _damageState;  }
        }



    public Dictionary<AnimationKey, Animation> PlayerAnimations
    {
        get { return _playerAnimations; }
    }

        public CharacterManager CharacterManager
        {
            get { return _characterManager; }
        }

        public Game1()
        {
            var graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _screenRectangle = new Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;
            var gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);
            this.IsMouseVisible = true;
            _titleIntroState = new TitleIntroState(this);
            _startMenuState = new MainMenuState(this);
            _gamePlayState = new GamePlayState(this);
            _conversationState = new ConversationState(this);
            _battleState = new BattleState(this);
            _battleOverState = new BattleOverState(this);
            _damageState = new DamageState(this);
            _levelUpState = new LevelUpState(this);
            gameStateManager.ChangeState((TitleIntroState)_titleIntroState, PlayerIndex.One);
            _characterManager = CharacterManager.Instance;
        }

        protected override void Initialize()
        {
            Components.Add(new Xin(this));
            Animation animation = new Animation(3, 64, 64, 0, 0);
            _playerAnimations.Add(AnimationKey.WalkDown, animation);
            animation = new Animation(3, 64, 64, 0, 64);
            _playerAnimations.Add(AnimationKey.WalkLeft, animation);
            animation = new Animation(3, 64, 64, 0, 128);
            _playerAnimations.Add(AnimationKey.WalkRight, animation);
            animation = new Animation(3, 64, 64, 0, 192);
            _playerAnimations.Add(AnimationKey.WalkUp, animation);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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