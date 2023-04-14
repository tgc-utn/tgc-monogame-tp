using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    class Car : IElementoDinamico {
        
        //   Variables escondidas
        // Matrix World : Para hacerle modificaciones
        // String UbicacionModelo : Para ubicar al modelo
        private const string UbicacionAutos = "Models/"; 
        private float Rotacion;

        public Car(string path, Vector3 posicionInicial) : base(path, posicionInicial) {
            Origen = UbicacionAutos + path;
            Rotacion = -10f;
        }   
        public override void Update(GameTime gameTime, KeyboardState keyboard){
            // Acá la lógica del auto
            Rotacion -= Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds)*2;
            World =    Matrix.CreateScale(0.5f) 
                        * Matrix.CreateRotationY(Rotacion) 
                        * Matrix.CreateTranslation(0f, 50f, 0f);
        }
      

        /* Acá funciones propias del auto */
        public Matrix getWorld(){
            return World;
        }
        //public void saltar(){}        
        //public void doblar(){}        
        //public void usarPowerUp(){}        
        //public void usarLaser(){}     

    }
}