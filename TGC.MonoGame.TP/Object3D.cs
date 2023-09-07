using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{    
    public class Object
    {
        
    private Model _model { get; set; }
    public Matrix _world{ get; set; }
    private Vector3 _position{ get; set; }
    private float _rotation{ get; set; }
    private Effect _effect { get; set; }

        public Object(Vector3 Position, Model Modelo, Effect efecto){
            this._world = Matrix.CreateTranslation(Position);
            this._model = Modelo;
            this._effect = efecto;
        }

        public void Draw(Matrix Vista, Matrix Proyeccion){
            _model.Draw(_world, Vista, Proyeccion);
            /*foreach (var mesh in _model.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                //_effect.Parameters["World"].SetValue(_world);
                //_effect.Parameters["View"].SetValue(Vista);
                //_effect.Parameters["Projection"].SetValue(Proyeccion);

                mesh.Draw();
            }*/
        }
    }

}