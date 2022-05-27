using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxeLite
{
    class BouncingEnemy
    {
        public Random r = new Random();

        public static Viewport GraphicsViewport;

        //Sprite related
        public static Texture2D Enemy;
        public Vector2 EnemyPosition;
        public Vector2 EnemyMov;
        public Rectangle EnemyHitBox;

        bool Touching = false;

        //Stats do inimigo
        public int HP = 3;
        public int Damage = 2;



        //Construtor
        public BouncingEnemy(Viewport viewport)
        {
            EnemyPosition = new Vector2(r.Next(1, viewport.Width - 15), 1);
            EnemyMov = new Vector2(r.Next(1, 4), r.Next(2, 5));
        }

        //Update inimigo
        public void update(MainCharacter Personagem)
        {

            EnemyHitBox = new Rectangle((int)EnemyPosition.X, (int)EnemyPosition.Y, Enemy.Width, Enemy.Height);

            colisao(Personagem.FireballHitbox, Personagem.Damage);

            EnemyMovement();
        }


        public void colisao(Rectangle FireballHitbox, int Dmg) 
        {
            if (FireballHitbox.Intersects(EnemyHitBox) && Touching == false)
            {
                Touching = true;

                HP -= Dmg;

            }

            if (!FireballHitbox.Intersects(EnemyHitBox))
                Touching = false;
        }

        public void EnemyMovement() 
        {
            EnemyPosition += EnemyMov;

            if (EnemyPosition.X < 0 || EnemyPosition.X + Enemy.Width > GraphicsViewport.Width)
            {
                EnemyMov.X *= -1;
                
                if (EnemyMov.X > 0)
                    EnemyMov.X += 1;
                else
                    EnemyMov.X -= 1;
            }

            if (EnemyPosition.Y < 0 || EnemyPosition.Y + Enemy.Height > GraphicsViewport.Height)
            {
                EnemyMov.Y *= -1;
                
                if (EnemyMov.Y > 0)
                    EnemyMov.Y += 1;
                else
                    EnemyMov.Y -= 1;
            }
        }

        //Draw inimigo
        public void draw(SpriteBatch spritebatch) 
        {
                spritebatch.Draw(Enemy, EnemyPosition, Color.White);
        }
    }
}
