using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Design
{
    public class ElementoBuilder{
        private Model Model;
        private Vector3 Posicion = Vector3.Zero;
        private Vector3 Rotacion = Vector3.Zero;
        private float Escala = 1f;

        public ElementoBuilder(){}
        public void Reset(){
            // Modelo = null;
            Posicion = Vector3.Zero;
            Rotacion = Vector3.Zero;
            Escala = 1f;
        }
        public void Modelo(Model modelo3d){
            Model = modelo3d;
        }
        public void ConEscala(float escala){
            Escala = escala;
        }
        public void ConAltura(float altura){
            Posicion.Y = altura;
        }
        public void ConRotacion(float rotacionX, float rotacionY, float rotacionZ){
            Rotacion.X = rotacionX;
            Rotacion.Y = rotacionY;
            Rotacion.Z = rotacionZ;
        }
        public void ConPosicion(float DistanciaAlOrigenX, float DistanciaAlOrigenZ){
            Posicion.X = DistanciaAlOrigenX;
            Posicion.Z = DistanciaAlOrigenZ;
        }
        public Elemento BuildMueble(){
            return new Elemento(Model,Posicion,Rotacion,Escala);
        }
    }
}
