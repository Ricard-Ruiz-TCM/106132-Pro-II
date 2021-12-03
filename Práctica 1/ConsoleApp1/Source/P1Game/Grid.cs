using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using SFML.System;

using System;

namespace TcGame
{
    public class Grid : Drawable
    {
        private LineDrawer lines;

        private const int numColumns = 4;

        private const int numRows = 3;

        private const int maxItems = numColumns * numRows;

        private List<Item> items;

        private Texture backgroundTexture;
        private Sprite backgroundSprite;

        public float SlotWidth
        {
            get { return P1Game.ScreenSize.X / (float)numColumns; }
        }

        public float SlotHeight
        {
            get { return P1Game.ScreenSize.Y / (float)numRows; }
        }

        public int MaxItems
        {
            get { return numColumns * numRows; }
        }

        public void Init()
        {
            backgroundTexture = new Texture("Data/Textures/Background.jpg");
            backgroundSprite = new Sprite(backgroundTexture);

            items = new List<Item>();

            FillGridLines();

        }

        public void DeInit()
        {
            lines.DeInit();
        }

        public void HandleKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.A)
            {
                if (HasNullSlot())
                {
                    AddItemAtIndex(NewRandomItem(), GetFirstNullSlot());
                }
                else
                {
                    AddItemAtEnd(NewRandomItem());
                }
            }
            else if (e.Code == Keyboard.Key.R)
            {
                RemoveLastItem();
            }
            else if (e.Code == Keyboard.Key.N)
            {
                NullAllCoins();
            }
            else if (e.Code == Keyboard.Key.V)
            {
                ReverseItems();
            }
            else if (e.Code == Keyboard.Key.S)
            {
                RemoveNullSlots();
            }
            else if (e.Code == Keyboard.Key.M)
            {
                RemoveAllItems();
            }
            else if (e.Code == Keyboard.Key.K)
            {
                NullAllWeapons();
            }
            else if (e.Code == Keyboard.Key.O)
            {
                OrderItems();
            }

        }

        public void HandleMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                int row = (int)(e.Y / (P1Game.ScreenSize.Y / numRows));
                int column = (int)(e.X / (P1Game.ScreenSize.X / numColumns));
                NullItemAtIndex((numRows * row) + column + row);
            }
        }

        private void FillGridLines()
        {
            lines = new LineDrawer(numColumns + numRows + 2);
            lines.Init();

            for (int i = 0; i <= numColumns; ++i)
            {
                lines.AddLine(new Vector2f(i * SlotWidth, 0.0f), new Vector2f(i * SlotWidth, P1Game.ScreenSize.Y), new Color(0, 0, 0, 150), 10.0f);
            }

            for (int i = 0; i <= numRows; ++i)
            {
                lines.AddLine(new Vector2f(0.0f, i * SlotHeight), new Vector2f(P1Game.ScreenSize.X, i * SlotHeight), new Color(0, 0, 0, 150), 2.0f);
            }
        }

        public void Update(float dt)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                int row = i / numColumns;
                int column = i % numColumns;

                Vector2f position = new Vector2f(SlotWidth / 2.0f + SlotWidth * column, SlotHeight / 2.0f + SlotHeight * row);
                Item item = items[i];
                if (item != null)
                {
                    item.Position = position;
                }
            }
        }

        public void Draw(RenderTarget rt, RenderStates rs)
        {
            rt.Draw(backgroundSprite, rs);
            rt.Draw(lines, rs);

            foreach (Item item in items)
            {
                if (item != null)
                {
                    rt.Draw(item, rs);
                }
            }
        }

        private Item NewRandomItem()
        {
            Item item;

            switch (new Random().Next((int)Item.ITEMS.TOTAL_ITEMS))
            {
                case (int)Item.ITEMS.BOMB:
                    item = new Bomb();
                    break;
                case (int)Item.ITEMS.HEART:
                    item = new Heart();
                    break;
                case (int)Item.ITEMS.SWORD:
                    item = new Sword();
                    break;
                case (int)Item.ITEMS.AXE:
                    item = new Axe();
                    break;
                case (int)Item.ITEMS.COIN:
                    item = new Coin();
                    break;
                case (int)Item.ITEMS.CLYDE:
                    item = new Clyde();
                    break;
                case (int)Item.ITEMS.BLINKY:
                    item = new Blinky();
                    break;
                default: item = null; break;
            }

            if (item != null) item.init();

            return item;
        }

        private void RemoveLastItem()
        {
            if (items.Count != 0) items.RemoveAt(items.Count - 1);
        }

        private void NullAllCoins()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    if (items[i].getType() == Item.ITEMS.COIN)
                    {
                        items[i].deInit(); items[i] = null;
                    }
                }
            }
        }

        private void RemoveNullSlots()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    items.RemoveAt(i); i--;
                }
            }
        }

        private void RemoveAllItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    items[i].deInit();
                }
            }
            items.Clear();
        }


        private void NullAllWeapons()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    if (items[i].isWeapon())
                    {
                        items[i].deInit();
                        items[i] = null;
                    }
                }
            }
        }


        private bool HasNullSlot()
        {
            bool empty = false;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    empty = true; i = items.Count;
                }
            }

            return empty;
        }

        private int GetFirstNullSlot()
        {
            int first = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    first = i; i = items.Count;
                }
            }

            return first;
        }

        private void AddItemAtIndex(Item item, int index)
        {
            items[index] = item;
        }

        private void AddItemAtEnd(Item item)
        {
            // Solo añade si hay hueco en el Grid
            if (items.Count < maxItems)
            {
                items.Add(item);
            }
        }

        // HEARTS > WEAPONS > BOMBS > COINS > REST
        private void OrderItems()
        {
            RemoveNullSlots();
            Item temp;
            for (int i = 0; i <= items.Count - 2; i++)
            {
                for (int x = 0; x <= items.Count - 2; x++)
                {
                    if (items[x].getType() > items[x + 1].getType())
                    {
                        temp = items[x + 1];
                        items[x + 1] = items[x];
                        items[x] = temp;
                    }
                }
            }

        }

        private void ReverseItems()
        {
            items.Reverse();
        }

        public void NullItemAtIndex(int index)
        {
            if ((index < items.Count) && (items.Count != 0) && (index >= 0))
            {
                if (items[index] != null)
                {
                    if (items[index].getType() == Item.ITEMS.BOMB)
                    {
                        items[index] = null;
                        Kaboom(index);
                    }
                }
                if (items[index] != null)
                {
                    items[index] = null;
                }
            }
        }

        public void Kaboom(int index)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    // Comprueba si estamos en la primera columna para saltarla
                    if (((index % numColumns) == 0) && (x == -1))
                    {
                        x++;
                    }
                    // Comprueba si estamos en la última columna para saltarla
                    else if (((index % numColumns) == 3) && (x == 1))
                    {
                        x++;
                    }
                    if (x < 2)
                    {
                        NullItemAtIndex(index + (numColumns * y) + x);
                    }
                }
            }
        }
       
    }
}
