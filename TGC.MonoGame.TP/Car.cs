using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    class Car {
        private Matrix CarWorld;
        private Model CarModel;
        private float Rotacion;
        private const string UbicacionModelo = "Models/RacingCar";
        
        public Car(){
            Rotacion = -10f;
            CarWorld = Matrix.Identity;
        }
        public void Load(ContentManager contentManager){
            CarModel = contentManager.Load<Model>(UbicacionModelo);
            return;
        }
        //public void saltar(){}        
        //public void doblar(){}        
        //public void usarPowerUp(){}        
        //public void usarLaser(){}        
        public void Update(GameTime gameTime, KeyboardState keyboard){
            // Acá la lógica del auto
            Rotacion -= Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            CarWorld =    Matrix.CreateScale(0.5f) 
                        * Matrix.CreateRotationY(Rotacion) 
                        * Matrix.CreateTranslation(0f, 50f, 0f);
        }
        public void Draw (Matrix view, Matrix projection){
            CarModel.Draw(CarWorld,view,projection);
        }
       
        public Matrix getWorld(){
            return CarWorld;
        }
    }
}