using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AxeLite
{

    
    class MainCharacter
    {
        public static Viewport GraphicsViewport;

        //Personagem principal
        public static Texture2D Character;
        public Vector2 CharacterPosition = new Vector2(20, 20);
        public Rectangle CharHitbox;
        public bool Touching;

        //Personagem principal STATS
        public static SpriteFont VidaText;
        public int HP = 10;
        public int Damage = 2;

        

        //Ataque (Bola de fogo)
        public static Texture2D[] Projectile = new Texture2D[4];
        public int ProjCount = 1;
        public Rectangle FireballHitbox;
        public int Dir = 1;
        public Vector2 ProjPosition;
        public Vector2 ProjMovement;
        
        bool disparado = false;
        double timer = 0;
       
        



        //Preciso de um construtor
        public MainCharacter() 
        {
        }









        /**
         Esta funçao vai tratar de praticamente tudo relacionado com o personagem:
                Movimento
                Ataque
                Nao sair do mapa
         */
        public void update(GameTime gametime, BouncingEnemy Inimigo) 
        {
            KeyboardState keystate = Keyboard.GetState();


            CharHitbox = new Rectangle((int)CharacterPosition.X, (int)CharacterPosition.Y, Character.Width, Character.Height);


            //Trata de apontar 
            apontar(keystate);

            //Trata do movimento do jogador
            movimento(keystate);

            //Trata da colisoes
            colisao(Inimigo.EnemyHitBox, Inimigo.Damage, Inimigo.HP);

            //Trata do ataque do jogador
            ataque(keystate, Inimigo.EnemyHitBox);
            
            if(disparado == true)
                animate(gametime);
        }


        //Esta função trata da colisao entre o Jogador e o Inimigo que faz com que o jogador perca HP
        public void colisao(Rectangle EnemyHitBox, int Dmg, int EnemyHp) 
        {
            if (CharHitbox.Intersects(EnemyHitBox) && Touching == false && EnemyHp > 0)
            {
                Touching = true;
                
                HP -= Dmg;
                
            }

            if (!CharHitbox.Intersects(EnemyHitBox))
                Touching = false;

        }




        public void animate(GameTime gametime) 
        {
            //Animaçao da bola de fogo
            timer += gametime.ElapsedGameTime.TotalMilliseconds;


            if (timer > 1200 )
            {
                ProjCount++;
                timer = 0;

                if (ProjCount > 3)
                    ProjCount = 1;
            }
            


        }




        //So dispara um projetil de cada vez Pode disparar novamente quando o projetil acertar em algo ou quando este sair do mapa;
        public void ataque(KeyboardState keyboardstate, Rectangle EnemyHitbox) 
        {
            if (!disparado && keyboardstate.IsKeyDown(Keys.Space))
            {
                ProjPosition = CharacterPosition;


                //Determinar do vetor da movimento para o projetil

                if (Dir == 1)//seta para cima ou seja dispara para cima
                    ProjMovement = new Vector2(0, -5);

                if (Dir == 2)//seta para baixo ou seja dispara para baixo
                    ProjMovement = new Vector2(0, 5);

                if (Dir == 3)//Seta para a direita dispara para a direita
                    ProjMovement = new Vector2(5, 0);

                if (Dir == 4)//Seta para a esquerda dispara para a esquerda
                    ProjMovement = new Vector2(-5, 0);

                //----------------------------------------------

                disparado = true;
            }
            else //A partir do moment que calculamos o vetor do movimento da bola de fogo vamos move-la nessa direçao e retirar a sua hitbox
            {
                ProjPosition += ProjMovement;
                FireballHitbox = new Rectangle((int)ProjPosition.X, (int)ProjPosition.Y, Projectile[ProjCount].Width, Projectile[ProjCount].Height);
            }



            //Se a bola de fogo sair do ecra pode-se disparar de novo
            if (ProjPosition.X > GraphicsViewport.Width || ProjPosition.X < 0 || ProjPosition.Y > GraphicsViewport.Height || ProjPosition.Y < 0 || FireballHitbox.Intersects(EnemyHitbox))
                disparado = false;
        }

        public void apontar(KeyboardState keystate) 
        {
            if (keystate.IsKeyDown(Keys.D))//Aponta para a direita
                Dir = 3;
            if (keystate.IsKeyDown(Keys.A))//Aponta para a esquerda
                Dir = 4;
            if (keystate.IsKeyDown(Keys.W))//Aponta para a cima
                Dir = 1;
            if (keystate.IsKeyDown(Keys.S))//Aponta para a baixo
                Dir = 2;
        }
        public void movimento(KeyboardState keystate) 
        {
            if (keystate.IsKeyDown(Keys.Right) && CharacterPosition.X + Character.Width < GraphicsViewport.Width)//Direita
                CharacterPosition.X += 3;   
            if (keystate.IsKeyDown(Keys.Left) && CharacterPosition.X > 0)//Esquerda
                CharacterPosition.X -= 3;
            if (keystate.IsKeyDown(Keys.Up) && CharacterPosition.Y > 0)//Cima
                CharacterPosition.Y -= 3;
            if (keystate.IsKeyDown(Keys.Down) && CharacterPosition.Y + Character.Height < GraphicsViewport.Height)//baixo
                CharacterPosition.Y += 3;

        }




        public void draw(SpriteBatch spritebatch) 
        {
            
            
            if(disparado)
                spritebatch.Draw(Projectile[ProjCount], ProjPosition, Color.White);
            
            spritebatch.Draw(Character, CharacterPosition, Color.White);

            spritebatch.DrawString(VidaText, "HP - " + HP, new Vector2(2, 2), Color.White);
        }
    }
}
