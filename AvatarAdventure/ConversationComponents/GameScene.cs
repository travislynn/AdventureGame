using System.Collections.Generic;
using System.Text;
using AvatarAdventure.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace AvatarAdventure.ConversationComponents
{
    public class GameScene
    {
        protected Game game;
        private Vector2 textPosition;

        public string Text { get; set; }

        public static Texture2D Selected { get; private set; }

        public List<SceneOption> Options { get; set; }

        [ContentSerializerIgnore]
        public SceneAction OptionAction
        {
            get { return Options[SelectedIndex].OptionAction; }
        }
        public string OptionScene
        {
            get { return Options[SelectedIndex].OptionScene; }
        }
        public string OptionText
        {
            get { return Options[SelectedIndex].OptionText; }
        }
        public int SelectedIndex { get; private set; }

        public bool IsMouseOver { get; private set; }

        [ContentSerializerIgnore]
        public Color NormalColor { get; set; }

        [ContentSerializerIgnore]
        public Color HighLightColor { get; set; }

        public Vector2 MenuPosition { get; } = new Vector2(50, 475);

        private GameScene()
        {
            NormalColor = Color.Blue;
            HighLightColor = Color.Red;
        }

        public GameScene(string text, List<SceneOption> options) : this()
        {
            this.Text = text;
            this.Options = options;
            textPosition = Vector2.Zero;
        }

        public GameScene(Game game, string text, List<SceneOption> options) : this(text, options)
        {
            this.game = game;
        }

        public void SetText(string text, SpriteFont font)
        {
            textPosition = new Vector2(450, 50);
            StringBuilder sb = new StringBuilder();
            float currentLength = 0f;

            if (font == null)
            {
                this.Text = text;
                return;
            }

            string[] parts = text.Split(' ');
            foreach (string s in parts)
            {
                Vector2 size = font.MeasureString(s);
                if (currentLength + size.X < 500f)
                {
                    sb.Append(s);
                    sb.Append(" ");
                    currentLength += size.X;
                }
                else
                {
                    sb.Append("\n\r");
                    sb.Append(s);
                    sb.Append(" ");
                    currentLength = 0;
                }
            }
            this.Text = sb.ToString();
        }
        public void Initialize()
        {
        }

        public void Update(GameTime gameTime, PlayerIndex index)
        {
            if (Xin.CheckKeyReleased(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex < 0)
                    SelectedIndex = Options.Count - 1;
            }
            else if (Xin.CheckKeyReleased(Keys.Down))
            {
                SelectedIndex++;
                if (SelectedIndex > Options.Count - 1)
                    SelectedIndex = 0;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background, SpriteFont font)
        {
            Vector2 selectedPosition = new Vector2();
            Color myColor;

            if (Selected == null)
                Selected = game.Content.Load<Texture2D>(@"Misc\selected");

            if (textPosition == Vector2.Zero)
                SetText(Text, font);

            if (background != null)
                spriteBatch.Draw(background, Vector2.Zero, Color.White);

            spriteBatch.DrawString(font,
                Text,
                textPosition,
                Color.White);

            Vector2 position = MenuPosition;
            Rectangle optionRect = new Rectangle(0, (int)position.Y, 1280, font.LineSpacing);

            IsMouseOver = false;

            for (int i = 0; i < Options.Count; i++)
            {
                if (optionRect.Contains(Xin.MouseState.Position))
                {
                    SelectedIndex = i;
                    IsMouseOver = true;
                }
                if (i == SelectedIndex)
                {
                    myColor = HighLightColor;
                    selectedPosition.X = position.X - 35;
                    selectedPosition.Y = position.Y;
                    spriteBatch.Draw(Selected, selectedPosition, Color.White);
                }
                else
                    myColor = NormalColor;

                spriteBatch.DrawString(font,
                    Options[i].OptionText,
                    position,
                    myColor);

                position.Y += font.LineSpacing + 5;
                optionRect.Y += font.LineSpacing + 5;
            }
        }
    }
}
