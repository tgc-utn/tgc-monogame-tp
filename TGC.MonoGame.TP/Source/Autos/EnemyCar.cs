using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP{
    class EnemyCar : IElementoDinamico {
        const float ESCALA = 7f;
        private const float Velocidad = 500f;
        private Vector3 Traslacion = new Vector3(0f,0f, 0f);
        private Vector3 Direccion = new Vector3(0f,0f,0f); // Si está en cero, están inmóviles

        public EnemyCar(float posX, float posY, float posZ, Vector3 rotacion) 
        : base(TGCGame.GameContent.M_AutoEnemigo, new Vector3(posX,posY,posZ), Vector3.Zero, ESCALA)
        {
            Traslacion = new Vector3(posX, posY, posZ);
            this.SetEffect(TGCGame.GameContent.E_TextureShader);
        }

        public override void Draw()
        {
            TGCGame.GameContent.E_TextureShader
                .Parameters["Texture"].SetValue(TGCGame.GameContent.T_CombatVehicle);
            base.Draw();
        }
        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            var traslacionRelativa = Traslacion;
            //var traslacionRelativa = Traslacion + PosicionInicial;
            const float limiteInferior = 5500f;
            const float limiteSuperior = 1000f;
            const float limiteDerecho = 1000f;
            const float limiteIzquierdo = 7000f;

            // En cuadrado alrededor del mapa
            switch(Direccion.X){
                case 1f: //bajando
                    if(traslacionRelativa.X>limiteInferior){
                        Direccion.X = 0f;
                        Direccion.Z = 1f;
                    }
                break;
                case -1f: //subiendo
                    if(traslacionRelativa.X<limiteSuperior){
                        Direccion.X = 0f;
                        Direccion.Z = -1f;
                    }
                break;
                case 0f: //movimiento lateral
                    switch(Direccion.Z){
                        case 1f: //yendo a la izquierda
                            if(traslacionRelativa.Z>limiteIzquierdo){
                                Direccion.Z = 0f;
                                Direccion.X = -1f;
                            }
                        break;
                        case -1f: //yendo a la derecha
                            if(traslacionRelativa.Z<limiteDerecho){
                                Direccion.Z = 0f;
                                Direccion.X = 1f;
                            }
                        break;
                    }
                break;
            }

            Traslacion.X += Velocidad*Direccion.X * Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Traslacion.Z += Velocidad*Direccion.Z * Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            World = Matrix.CreateScale(Escala)
                    *Matrix.CreateTranslation(this.GetPosicionInicial())
                    *Matrix.CreateTranslation(Traslacion.X,50f,Traslacion.Z);
        }
    }
}