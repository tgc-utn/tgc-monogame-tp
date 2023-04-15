using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    class EnemyCar : IElementoDinamico {
        
        //   Variables escondidas
        // Matrix World : Para hacerle modificaciones
        // String UbicacionModelo : Para ubicar al modelo
        private const string UbicacionAuto = "Models/CombatVehicle/Vehicle"; 
        private const float Velocidad = 500f;
        private Vector3 Traslacion = new Vector3(0f,0f, 0f);
        private Vector3 Direccion = new Vector3(1f,0f,0f);
        private float Escala;

        public EnemyCar(string path, float escala, Vector3 posicionInicial) : base(UbicacionAuto, posicionInicial) {
            Escala = escala; //aprox 0.07 está bien
        }
        public override void Update(GameTime gameTime, KeyboardState keyboard){
            var traslacionRelativa = Traslacion + PosicionInicial;

            // En cuadrado alrededor del mapa
            switch(Direccion.X){
                case 1f: //bajando
                    if(traslacionRelativa.X>5000f){
                        Direccion.X = 0f;
                        Direccion.Z = 1f;
                    }
                break;
                case -1f: //subiendo
                    if(traslacionRelativa.X<0f){
                        Direccion.X = 0f;
                        Direccion.Z = -1f;
                    }
                break;
                case 0f: //movimiento lateral
                    switch(Direccion.Z){
                        case 1f: //yendo a la izquierda
                            if(traslacionRelativa.Z>5000f){
                                Direccion.Z = 0f;
                                Direccion.X = -1f;
                            }
                        break;
                        case -1f: //yendo a la derecha
                            if(traslacionRelativa.Z<0f){
                                Direccion.Z = 0f;
                                Direccion.X = 1f;
                            }
                        break;
                    }
                break;
            }

            Traslacion.X += Velocidad*Direccion.X * Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Traslacion.Z += Velocidad*Direccion.Z * Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Movimiento anterior
            //Traslacion.X += 5f*MathF.Sin(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds)*2);
            //Traslacion.Z += 5f*MathF.Sin(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds)/2);

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