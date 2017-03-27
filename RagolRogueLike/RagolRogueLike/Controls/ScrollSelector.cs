using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RagolRogueLike.Controls
{
    public class ScrollSelector : Control
    {
        #region Event Region

        public event EventHandler SelectionChanged;

        #endregion

        #region Field Region

        List<string> items = new List<string>();

        Color selectedColor = Color.Red;

        int maxItemWidth;
        int selectedItem;

        #endregion

        #region Property Region

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public int SelectedIndex
        {
            get { return selectedItem; }
            set { selectedItem = (int)MathHelper.Clamp(value, 0f, items.Count); }
        }

        public string SelectedItem
        {
            get { return Items[selectedItem]; }
        }

        public List<string> Items
        {
            get { return items; }
        }

        #endregion

        #region Constructor Region

        public ScrollSelector()
        {
            TabStop = true;
            Color = Color.White;
        }

        #endregion

        #region Method Region

        public void SetItems(string[] items, int maxWidth)
        {
            this.items.Clear();

            foreach (string s in items)
                this.items.Add(s);

            maxItemWidth = maxWidth;
        }

        protected void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, null);
            }
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawTo = position;

            float itemWidth = spriteFont.MeasureString(items[selectedItem]).X;

            if (hasFocus)
                spriteBatch.DrawString(spriteFont, items[selectedItem], drawTo, selectedColor);

            else
                spriteBatch.DrawString(spriteFont, items[selectedItem], drawTo, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (items.Count == 0)
            {
                return;
            }

            if (InputHandler.KeyReleased(Keys.Left))
            {
                selectedItem--;

                if (selectedItem < 0)
                {
                    selectedItem = items.Count - 1;
                }

                OnSelectionChanged();
            }

            if (InputHandler.KeyReleased(Keys.Right))
            {
                selectedItem++;

                if (selectedItem >= items.Count)
                {
                    selectedItem = 0;
                }

                OnSelectionChanged();
            }
        }

        #endregion
    }
}
