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
            Menu.PauseMenu = Content.Load<Texture2D>("MenuPixelArt");


            //Carregar o Background
            Background = Content.Load<Texture2D>("ChaoTdj");
            SizeBackground = new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);//Posicao X, PosicaoY, TamanhoX, TamanhoY
            

            //Carregar a font que vou usar
            MainCharacter.VidaText = Content.Load<SpriteFont>("Font");

            //Carregar a sprite do personagem principal
            MainCharacter.Character = Content.Load<Texture2D>("Dozing_Dragon");
            
            //Declarar o viewport para a classe
            MainCharacter.GraphicsViewport = _graphics.GraphicsDevice.Viewport;
            
            //Declarar o meu projetil
            for(int i = 1; i < 4; i++)
                MainCharacter.Projectile[i] = Content.Load<Texture2D>("BolaDeFogo " + i);
            
            



            //Declarar o inimigo
            BouncingEnemy.Enemy = Content.Load<Texture2D>("ball");
            //Declarar o viewport para a classe
            BouncingEnemy.GraphicsViewport = _graphics.GraphicsDevice.Viewport;
        }






        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here


            Menu.update(gameTime);

            if (Menu.Playing)
            {
                //Update do personagem
                Personagem.update(gameTime, InimigoRessalto);


                //Update do bouncing enemy
                if (InimigoRessalto.HP > 0)
                    InimigoRessalto.update(Personagem);
                
                   
            }else

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            


            //desenhar o jogo
            _spriteBatch.Begin();

            _spriteBatch.Draw(Background, SizeBackground, Color.White);
            InimigoRessalto.draw(_spriteBatch);
            Personagem.draw(_spriteBatch);
            if(!Menu.Playing)
                Menu.draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
