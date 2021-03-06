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
        public static Texture2D[] Character = new Texture2D[4];
        public Vector2 CharacterPosition;
        public Rectangle CharHitbox;
        public bool Touching;
        public int lado = 1;

        //Personagem principal STATS
        public static Texture2D[] Vidas = new Texture2D[6];
        public int HP = 5;
        public int Damage = 2;

        

        //Ataque (Bola de fogo)
        public static Texture2D[] Projectile = new Texture2D[4];
        public static float rotacao;
        public int ProjCount = 1;
        public Rectangle FireballHitbox;
        public int Dir = 1;
        public Vector2 ProjPosition;
        public Vector2 ProjMovement;
        
        bool disparado = false;
        double timer = 0;
       
        



        //Preciso de um construtor
        public MainCharacter(int x, int y) 
        {
            CharacterPosition.X = x/2;
            CharacterPosition.Y = y/2;
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


            CharHitbox = new Rectangle((int)CharacterPosition.X, (int)CharacterPosition.Y, Character[1].Width, Character[1].Height);


            if (HP > 0)
            {
                //Trata de apontar 
                apontar(keystate);

                //Trata do movimento do jogador
                movimento(keystate);

                //Trata da colisoes
                colisao(Inimigo.EnemyHitBox, Inimigo.Damage, Inimigo.HP);

                //Trata do ataque do jogador
                ataque(keystate, Inimigo.EnemyHitBox);

                if (disparado == true)
                    animate(gametime);
            }
            else 
            {
                updatedead(Inimigo);
            }
        }

        public void updatedead(BouncingEnemy inimigo)
        {
            KeyboardState keyboardstate = Keyboard.GetState();

            if (keyboardstate.IsKeyDown(Keys.R))
            {
                HP = 5;
                inimigo.HP = 30;
                inimigo.Damage = 1;
                inimigo.SegundaFase = false;
                inimigo.TerceiraFase = false;
            }


            
                
        }

        //Esta função trata da colisao entre o Jogador e o Inimigo que faz com que o jogador perca HP
        public void colisao(Rectangle EnemyHitBox, int Dmg, int EnemyHp) 
        {
            if (CharHitbox.Intersects(EnemyHitBox) && Touching == false && EnemyHp > 0)
            {
                Touching = true;
                
                if(HP > 0)
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
                {
                    ProjMovement = new Vector2(0, -15);
                    rotacao = 3 * (float)Math.PI / 2;
                    lado = 3;
                }
                
                if (Dir == 2)//seta para baixo ou seja dispara para baixo
                {
                    ProjMovement = new Vector2(0, 15);
                    rotacao = (float)Math.PI / 2;
                    
                }
                
                if (Dir == 3)//Seta para a direita dispara para a direita
                {
                    ProjMovement = new Vector2(15, 0);
                    rotacao = 0;
                    lado = 2;
                }
                
                if (Dir == 4)//Seta para a esquerda dispara para a esquerda
                {
                    ProjMovement = new Vector2(-15, 0);
                    rotacao = (float)Math.PI;
                    lado = 1;
                }
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
            if (keystate.IsKeyDown(Keys.Right) && CharacterPosition.X + Character[1].Width < GraphicsViewport.Width)//Direita
            {   CharacterPosition.X += 4;
                lado = 2; 
            }

            if (keystate.IsKeyDown(Keys.Left) && CharacterPosition.X > 0)//Esquerda
            { 
                CharacterPosition.X -= 4;
                lado = 1;
            }

            if (keystate.IsKeyDown(Keys.Up) && CharacterPosition.Y > 0)//Cima
                CharacterPosition.Y -= 4;
            if (keystate.IsKeyDown(Keys.Down) && CharacterPosition.Y + Character[1].Height < GraphicsViewport.Height)//baixo
                CharacterPosition.Y += 4;

        }




        public void draw(SpriteBatch spritebatch) 
        {
            
            
            if(disparado)
                spritebatch.Draw(Projectile[ProjCount], ProjPosition, null, Color.White, rotacao, new Vector2(0, 0), new Vector2(1,1), SpriteEffects.None, 0f); 
            
            spritebatch.Draw(Character[lado], CharHitbox, Color.White);

            if (HP > 0)
                spritebatch.Draw(Vidas[HP], new Vector2(1, 1), Color.White);
            else
                spritebatch.Draw(Vidas[1], new Vector2(1, 1), Color.Black);
        }
    }
}
