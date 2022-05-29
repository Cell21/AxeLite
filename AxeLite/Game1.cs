using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AxeLite
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        UI Menu;

        MainCharacter Personagem;

        BouncingEnemy InimigoRessalto;

        Texture2D Background;
        Rectangle SizeBackground;

        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Personagem = new MainCharacter(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
            InimigoRessalto = new BouncingEnemy(_graphics.GraphicsDevice.Viewport);
            Menu = new UI(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);

            base.Initialize();
        }








        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Carregar o menu de iniciar e pausa
            UI.PauseMenu = Content.Load<Texture2D>("MenuPixelArt");
            UI.DeadScreen = Content.Load<Texture2D>("DeathMenu");
            UI.WinMenu = Content.Load<Texture2D>("YouWin");

            //Carregar o Background
            Background = Content.Load<Texture2D>("Grass");
            SizeBackground = new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);//Posicao X, PosicaoY, TamanhoX, TamanhoY
            

            
            //------------------RELACIONADO COM O PERSONAGEM----------------------

            //Carregar a sprite do personagem principal
            MainCharacter.Character[1] = Content.Load<Texture2D>("Dozing_Dragon");
            MainCharacter.Character[2] = Content.Load<Texture2D>("Dozing_DragonRight");
            MainCharacter.Character[3] = Content.Load<Texture2D>("Dozing_DragonUp");
            

            //Declarar o viewport para a classe
            MainCharacter.GraphicsViewport = _graphics.GraphicsDevice.Viewport;
            
            //Declarar o meu projetil
            for(int i = 1; i < 4; i++)
                MainCharacter.Projectile[i] = Content.Load<Texture2D>("BolaDeFogo " + i);

            //Declarar as vidas
            for (int i = 1; i < 6; i++)
                MainCharacter.Vidas[i] = Content.Load<Texture2D>(i+"Hearts");

            //---------------------------------------------------------------------

            //-------------------RELACIONADO COM O INIMIGO-------------------------

            //Declarar o inimigo
            BouncingEnemy.Enemy = Content.Load<Texture2D>("BallBoss");
            
            //Declarar o viewport para a classe
            BouncingEnemy.GraphicsViewport = _graphics.GraphicsDevice.Viewport;

            //Dar load a sprite das vidas do inimigo
            for(int i = 1; i < 4; i++)
                BouncingEnemy.EnemyHP[i] = Content.Load<Texture2D>(i+"Skulls");

            //---------------------------------------------------------------------
        }






        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            // TODO: Add your update logic here

            if(Personagem.HP > 0)
                Menu.update(gameTime);

            if(!Menu.Playing && keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (Menu.Playing && Personagem.HP > 0)
            {
                //Update do personagem
                Personagem.update(gameTime, InimigoRessalto);


                //Update do bouncing enemy
                InimigoRessalto.update(Personagem);


            }

            if (Personagem.HP <= 0)//Se morrer
            {
                Menu.Dead = true;
                Personagem.update(gameTime, InimigoRessalto);
                
                if (keyboard.IsKeyDown(Keys.Escape))
                    Exit();
            }
            else
                Menu.Dead = false;

            if (InimigoRessalto.HP <= 0)
                Menu.Win = true;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            


            //desenhar o jogo
            _spriteBatch.Begin();

            if(Menu.Win)
                _spriteBatch.Draw(Background, SizeBackground, Color.Black);
            else
                _spriteBatch.Draw(Background, SizeBackground, Color.White);
 
            InimigoRessalto.draw(_spriteBatch);
            
            Personagem.draw(_spriteBatch);
            
            if(!Menu.Playing)
                Menu.draw(_spriteBatch);
            
            if (Menu.Dead)
                Menu.drawLoseScreen(_spriteBatch);

            if (Menu.Win)
                _spriteBatch.Draw(UI.WinMenu, new Vector2((_graphics.GraphicsDevice.Viewport.Width / 2)-(UI.WinMenu.Width / 2), (_graphics.GraphicsDevice.Viewport.Height / 2)-(UI.WinMenu.Height / 2)), Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
