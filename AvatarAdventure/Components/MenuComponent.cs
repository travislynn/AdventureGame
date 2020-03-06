using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AvatarAdventure.Components
{
    public class MenuComponent
    {
        SpriteFont spriteFont;
        readonly List<string> menuItems = new List<string>();
        int selectedIndex = -1;
        Texture2D texture;

        public Vector2 Postion { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(
                    value,
                    0,
                    menuItems.Count - 1);
            }
        }
        public Color NormalColor { get; set; } = Color.White;

        public Color HiliteColor { get; set; } = Color.Red;

        public bool MouseOver { get; private set; }

        public MenuComponent(SpriteFont spriteFont, Texture2D texture)
        {
            this.MouseOver = false;
            this.spriteFont = spriteFont;
            this.texture = texture;
        }

        public MenuComponent(SpriteFont spriteFont, Texture2D texture, string[] menuItems)
        : this(spriteFont, texture)
        {
            selectedIndex = 0;
            foreach (string s in menuItems)
            {
                this.menuItems.Add(s);
            }
            MeassureMenu();
        }

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            MeassureMenu();
            selectedIndex = 0;
        }

        private void MeassureMenu()
        {
            Width = texture.Width;
            Height = 0;
            foreach (string s in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(s);
                if (size.X > Width)
                    Width = (int)size.X;
                Height += texture.Height + 50;
            }
            Height -= 50;
        }

        public void Update(GameTime gameTime) //, PlayerIndex index)
        {
            Vector2 menuPosition = Postion;
            Point p = Xin.MouseState.Position;
            Rectangle buttonRect;
            MouseOver = false;
            for (int i = 0; i < menuItems.Count; i++)
            {
                buttonRect = new Rectangle((int)menuPosition.X, (int)menuPosition.Y, texture.Width, texture.Height);
                if (buttonRect.Contains(p))
                {
                    selectedIndex = i;
                    MouseOver = true;
                }
                menuPosition.Y += texture.Height + 50;
            }
            if (!MouseOver && (Xin.CheckKeyReleased(Keys.Up)))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
            }
            else if (!MouseOver && (Xin.CheckKeyReleased(Keys.Down)))
            {
                selectedIndex++;
                if (selectedIndex > menuItems.Count - 1)
                    selectedIndex = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = Postion;
            Color myColor;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                    myColor = HiliteColor;
                else
                    myColor = NormalColor;

                spriteBatch.Draw(texture, menuPosition, Color.White);
                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);
                Vector2 textPosition = menuPosition + new Vector2((int)(texture.Width - textSize.X) / 2, (int)(texture.Height - textSize.Y) / 2);
                spriteBatch.DrawString(spriteFont, menuItems[i], textPosition, myColor);
                menuPosition.Y += texture.Height + 50;
            }
        }
    }
}
