using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    class EnemyCar : IElementoDinamico {
        
        //   Variables escondidas
        // Matrix World : Para hacerle modificaciones
        // String UbicacionModelo : Para ubicar al modelo
        private const string UbicacionAuto = "Models/CombatVehicle/Vehicle"; 
        private Vector3 Traslacion = new Vector3(0f,0f, 0f);
        private float Escala;

        public EnemyCar(string path, float escala, Vector3 posicionInicial) : base(UbicacionAuto, posicionInicial) {
            Escala = escala; //aprox 0.07 está bien
        }
        public override void Update(GameTime gameTime, KeyboardState keyboard){
            // Acá la lógica del auto
            Traslacion.X += 5f*MathF.Sin(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds)*2);
            Traslacion.Y += 5f*MathF.Sin(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds));
            Traslacion.Z += 5f*MathF.Sin(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds)/2);
            
            World = Matrix.CreateScale(Escala)
                    *Matrix.CreateTranslation(PosicionInicial)
                    *Matrix.CreateTranslation(Traslacion.X,50f,Traslacion.Z);
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