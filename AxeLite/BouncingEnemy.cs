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

        //Sprite HP
        public static Texture2D[] EnemyHP = new Texture2D[4];
        //Sprite related
        public static Texture2D Enemy;
        public Vector2 EnemyPosition;
        public Vector2 EnemyMov;
        public Rectangle EnemyHitBox;

        bool Touching = false;

        //Stats do inimigo
        public int HP = 30;
        public int Damage = 1;
        public bool SegundaFase = false;
        public bool TerceiraFase = false;



        //Construtor
        public BouncingEnemy(Viewport viewport)
        {
            EnemyPosition = new Vector2(r.Next(1, viewport.Width - 30), 1);
            EnemyMov = new Vector2(r.Next(3, 4), r.Next(4, 6));
        }

        //Update inimigo
        public void update(MainCharacter Personagem)
        {




            if (HP > 0)
            {
                EnemyHitBox = new Rectangle((int)EnemyPosition.X, (int)EnemyPosition.Y, Enemy.Width, Enemy.Height);
                colisao(Personagem.FireballHitbox, Personagem.Damage);
                EnemyMovement();
            }
            else
            {
                EnemyHitBox = new Rectangle(0,0,0,0);
                EnemyPosition = new Vector2(-50, -50);
            }
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
                
               
            }

            if (EnemyPosition.Y < 0 || EnemyPosition.Y + Enemy.Height > GraphicsViewport.Height)
            {
                EnemyMov.Y *= -1;

                
            }

            if (HP < 20 && SegundaFase == false)
            {
                EnemyMov = new Vector2(r.Next(6, 9), r.Next(7, 9));
                Damage = 2;
                SegundaFase = true;
            }

            if (HP < 10 && TerceiraFase == false)
            {
                EnemyMov = new Vector2(r.Next(8, 10), r.Next(9, 11));
                Damage = 3;
                TerceiraFase = true;
            }
        }

        //Draw inimigo
        public void draw(SpriteBatch spritebatch) 
        {
            if(HP > 0)
                spritebatch.Draw(Enemy, EnemyPosition, Color.White);

            if(HP > 20)
                spritebatch.Draw(EnemyHP[3], new Vector2((GraphicsViewport.Width / 2)- Enemy.Width, 1), Color.White);

            if(HP > 10)
                spritebatch.Draw(EnemyHP[2], new Vector2((GraphicsViewport.Width / 2) - Enemy.Width, 1), Color.White);

            if (HP > 0)
                spritebatch.Draw(EnemyHP[1], new Vector2((GraphicsViewport.Width / 2) - Enemy.Width , 1), Color.White);
        }
    }
}
