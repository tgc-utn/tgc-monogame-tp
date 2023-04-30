using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Design
{
    public class ElementoBuilder{
        private Model Model;
        private Vector3 Posicion = Vector3.Zero;
        private Vector3 Rotacion = Vector3.Zero;
        private Effect Efecto = TGCGame.GameContent.E_BasicShader;
        private float Escala = 1f;

        public ElementoBuilder(){}
        public void Reset(){
            Posicion = Vector3.Zero;
            Rotacion = Vector3.Zero;
            Escala = 1f;
            Efecto = TGCGame.GameContent.E_BasicShader;
        }
        public ElementoBuilder Modelo(Model modelo3d){
            Reset();
            Model = modelo3d;
            return this;
        }
        public ElementoBuilder ConEscala(float escala){
            Escala = escala;
            return this;
        }
        public ElementoBuilder ConAltura(float altura){
            Posicion.Y = altura;
            return this;
        }
        public ElementoBuilder ConRotacion(float rotacionX, float rotacionY, float rotacionZ){
            Rotacion.X = rotacionX;
            Rotacion.Y = rotacionY;
            Rotacion.Z = rotacionZ;
            return this;
        }
        public ElementoBuilder ConPosicion(float distanciaAlOrigenEnX, float distanciaAlOrigenEnZ){
            Posicion.X = distanciaAlOrigenEnX;
            Posicion.Z = distanciaAlOrigenEnZ;
            return this;
        }
        public ElementoBuilder ConShader(Effect shader){
            Efecto = shader;
            return this;
        }
        public Elemento BuildMueble(){
            var elementoCreado = new Elemento(Model,Posicion,Rotacion,Escala);
            elementoCreado.SetEffect(Efecto);
            return elementoCreado;
        }
    }
}
