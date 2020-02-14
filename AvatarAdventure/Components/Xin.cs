using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AvatarAdventure.Components
{
    public class Xin : GameComponent
    {
        private static KeyboardState _currentKeyboardState = Keyboard.GetState();
        private static KeyboardState _previousKeyboardState = Keyboard.GetState();

        public static MouseState MouseState { get; private set; } = Mouse.GetState();

        public static KeyboardState KeyboardState
        {
            get { return _currentKeyboardState; }
        }
        public static KeyboardState PreviousKeyboardState
        {
            get { return _previousKeyboardState; }
        }
        public static MouseState PreviousMouseState { get; private set; } = Mouse.GetState();

        public Xin(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Xin._previousKeyboardState = Xin._currentKeyboardState;
            Xin._currentKeyboardState = Keyboard.GetState();
            Xin.PreviousMouseState = Xin.MouseState;
            Xin.MouseState = Mouse.GetState();
            base.Update(gameTime);
        }
        public static void FlushInput()
        {
            MouseState = PreviousMouseState;
            _currentKeyboardState = _previousKeyboardState;
        }
        public static bool CheckKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) &&
                   _previousKeyboardState.IsKeyDown(key);
        }
        public static bool CheckMouseReleased(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return (MouseState.LeftButton == ButtonState.Released) &&
                           (PreviousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.Right:
                    return (MouseState.RightButton == ButtonState.Released) &&
                           (PreviousMouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.Center:
                    return (MouseState.MiddleButton == ButtonState.Released) &&
                           (PreviousMouseState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }
    }

}
