using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Design
{
    /// Crea un elemento estático en la posición del vector PosicionInicial
    public class Elemento  {
        public Matrix World = Matrix.Identity;
        protected Model Model;
        public Vector3 PosicionInicial {get; set;}
        public Vector3 Rotacion;
        public float Escala;
        
        public Elemento(Model modelo, Vector3 posicionInicial, Vector3 rotacion, float escala = 1f){
            Model = modelo;
            Escala = escala;
            Rotacion = rotacion;
            PosicionInicial = posicionInicial;

            actualizarMundo();
        }
        private void actualizarMundo(){
            World = Matrix.CreateScale(Escala) * 
                    Matrix.CreateRotationX(Rotacion.X) * Matrix.CreateRotationY(Rotacion.Y) * Matrix.CreateRotationZ(Rotacion.Z) * 
                    Matrix.CreateTranslation(PosicionInicial);
        }

        public void SetPosicionInicial(Vector3 nuevaPosicion){
            PosicionInicial = nuevaPosicion;
            actualizarMundo();
        }
        
        public virtual void Draw (){
            foreach(var mesh in Model.Meshes){
                foreach(var meshPart in mesh.MeshParts){
                    meshPart.Effect.Parameters["World"]?.SetValue(World);
                }
            mesh.Draw();
            }
        }
    }
}