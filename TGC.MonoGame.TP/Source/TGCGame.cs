using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const float S_METRO = 500f;
        internal static TGCGame Game;
        internal static Content GameContent;
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Auto Auto;
        private int IndiceHabAuto = 0;
        private Camera Camera; 
        private List<IHabitacion> Hogar = new List<IHabitacion>();
        private Song Soundtrack;

        public TGCGame()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
        
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GameContent = new Content(Content, GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;         
            //GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            Soundtrack = GameContent.S_SynthWars;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;

            GameContent.E_BasicShader.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());

            disponerHabitaciones();
            construirParedes();
            
            Auto = new Auto(Hogar[0].getCenter());
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 
            
            // Teleport
            if (Keyboard.GetState().IsKeyDown(Keys.T)){
                IndiceHabAuto++;
                if(IndiceHabAuto >= Hogar.Count) IndiceHabAuto = 0;
                Auto.Teleport(Hogar[IndiceHabAuto].getCenter());
            }
            else
            Auto.Update(gameTime, keyboardState);

            // Control de la música
            if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(Soundtrack);
            else if (keyboardState.IsKeyUp(Keys.W) && MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
            else if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else if (keyboardState.IsKeyDown(Keys.P) && MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();

            foreach(IHabitacion habitacion in Hogar)
                habitacion.Update(gameTime, keyboardState);
            
            Camera.Mover(keyboardState);
            Camera.Update(gameTime, Auto.World);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            foreach (var e in GameContent.Efectos){
                e.Parameters["View"].SetValue(Camera.View);
                e.Parameters["Projection"].SetValue(Camera.Projection);
                e.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }

            foreach(IHabitacion habitacion in Hogar)
                habitacion.Draw();

            Auto.Draw();          
        }

        // Esto debería ir en una nueva clase : Casa.
         private void disponerHabitaciones(){
            // DISPOSICIÓN RELATIVA DE HABITACIONES. 
            //      TAREA :  Sacar el S_METRO de acá y de Elemento. 
            //               Dejarlo solo que dependa de la Habitación definir las posiciones
            //               de los objetos (en AddElemento y cuandodefinimos la PosicionInicial)
            Hogar.Add( new HabitacionPrincipal(0f,0f));
            Hogar.Add( new HabitacionCocina(-HabitacionCocina.Size * S_METRO, S_METRO * HabitacionPrincipal.Size/2) );
            Hogar.Add( new HabitacionPasillo1(0f, -HabitacionPasillo1.Size * S_METRO) );
            Hogar.Add( new HabitacionToilette(-HabitacionToilette.Size * S_METRO, 0));
            Hogar.Add( new HabitacionPasillo2(0f, -Hogar[2].Ancho*2 * S_METRO));
            Hogar.Add( new HabitacionOficina(0f, -S_METRO *(Hogar[2].Ancho*2+HabitacionOficina.Size)));
            Hogar.Add( new HabitacionDormitorio1(-S_METRO * HabitacionDormitorio1.Size , -Hogar[2].Ancho*2 * S_METRO ));
            Hogar.Add( new HabitacionDormitorio2(Hogar[2].Ancho * S_METRO , -Hogar[2].Ancho*2 * S_METRO));
            Hogar.Add( new HabitacionToilette(Hogar[5].getVerticeExtremo().X,Hogar[5].getVerticeExtremo().Z - HabitacionToilette.Size*S_METRO));

            foreach(var hab in Hogar)
                Console.WriteLine("Habitacion cargada con {0:D}"+ " modelos.", hab.cantidadElementos());
        }

        private void construirParedes(){
            foreach(var hab in Hogar){
                hab.SetParedArriba   (new Pared(true).Cerrada());
                hab.SetParedAbajo    (new Pared(true).Cerrada());
                hab.SetParedIzquierda(new Pared().Cerrada());
                hab.SetParedDerecha  (new Pared().Cerrada());
            }
        }



        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}