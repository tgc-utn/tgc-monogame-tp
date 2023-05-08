using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP{
    public class Misil : IElementoDinamico
    {
        private Vector3 PosicionActual;
        private Vector3 RotacionActual;

        public Misil(Vector3 posicionInicial, Vector3 rotacion) 
        : base(TGCGame.GameContent.M_Misil, new Vector3(posicionInicial.X, TGCGame.S_METRO, posicionInicial.Z), rotacion, 10f)
        {
            PosicionActual = new Vector3(posicionInicial.X, TGCGame.S_METRO, posicionInicial.Z);
            RotacionActual = rotacion;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            PosicionActual += new Vector3(10f,0f,10f)* Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            World = Matrix.CreateTranslation(PosicionActual) * Matrix.CreateScale(100f);
        }

        public override void Draw()
        {
            foreach(var mesh in Model.Meshes){
                foreach(var meshPart in mesh.MeshParts){
                meshPart.Effect = TGCGame.GameContent.E_SpiralShader;}
            }

            base.Draw();
        }
    }

}