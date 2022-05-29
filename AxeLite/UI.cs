using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AxeLite
{
    class UI
    {
        public bool started = false;
        public bool Playing = false;
        public static Texture2D PauseMenu;
        public Vector2 PosMenu;

        public static Texture2D WinMenu;
        public bool Win = false;

        public bool Dead = false;
        public static Texture2D DeadScreen;

        public UI(int x, int y)
        {
            //Este calculo vai por o menu no centro do ecra
            PosMenu.X = x/2;
            PosMenu.Y = y/2;
        }

        public void update(GameTime gametime) 
        {
            KeyboardState keyboardstate = Keyboard.GetState();

            if (!started)
                MenuOffset();

            start(keyboardstate);
        }
        
        
        public void MenuOffset() 
        {
            PosMenu.X -= PauseMenu.Width / 2;
            PosMenu.Y -= PauseMenu.Height / 2;
            started = true;
        }

        public void start(KeyboardState keyboardstate) 
        {
            if (keyboardstate.IsKeyDown(Keys.B))
                Playing = true;

            if (keyboardstate.IsKeyDown(Keys.Escape))
                Playing = false;
        }


        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(PauseMenu, PosMenu, Color.White);


        }


        

        public void drawLoseScreen(SpriteBatch spritebatch) 
        {
            spritebatch.Draw(DeadScreen, new Rectangle((int)PosMenu.X, (int)PosMenu.Y, PauseMenu.Width, PauseMenu.Height + 20), Color.White);
        }


        
    }
}
