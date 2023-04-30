using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Design
{
    /// Crea un elemento estático en la posición del vector PosicionInicial
    public class Elemento  {
        public Matrix World = Matrix.Identity ;
        protected Model Model {get; set;}
        private Effect Shader {get; set;} 
        private Vector3 PosicionInicial {get; set;}
        private Vector3 Rotacion {get; set;}
        private float Escala {get; set;}

        public Elemento(Model modelo, Vector3 posicionInicial, Vector3 rotacion, float escala = 1f){
            Initialize(modelo, posicionInicial, rotacion, TGCGame.GameContent.E_BasicShader, escala);
        }
        public Elemento(Model modelo, Vector3 posicionInicial, Vector3 rotacion, Effect shader, float escala = 1f){
            Initialize(modelo, posicionInicial, rotacion, shader, escala);
        }
        public void Initialize(Model modelo, Vector3 posicionInicial, Vector3 rotacion, Effect shader, float escala = 1f){
            Model = modelo;
            Escala = escala;
            Rotacion = rotacion;
            PosicionInicial = posicionInicial;
            SetEffect(Shader);
            
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
        public Vector3 GetPosicionInicial(){
            return this.PosicionInicial;
        }
        public void SetEffect(Effect Shader){
            foreach(var mesh in Model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                meshPart.Effect = Shader;
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