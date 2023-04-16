using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    /// Crea un elemento estático en la posición del vector PosicionInicial
    public abstract class IElemento  {
        public Matrix World {get; set;}
        private Model Model;
        public Vector3 PosicionInicial;
        public Vector3 rotacion;
        //AGREGAR EFFECT
        public string Origen {get; set;} // '/Models/Carpeta/Modelo.fbx'
        
        public IElemento(string path, Vector3 posicionInicial, Vector3 rotacion){
            if(path!=null)
                Origen = path;
            Matrix Rotation = Matrix.CreateRotationX(rotacion.X) * Matrix.CreateRotationY(rotacion.Y) * Matrix.CreateRotationZ(rotacion.Z);
            World = Rotation * Matrix.CreateTranslation(posicionInicial);
            PosicionInicial = posicionInicial;
        }
        public void Load(ContentManager contentManager){
            if(Origen!=null)
            Model = contentManager.Load<Model>(Origen);
            return; 
        }
        public void Draw (Matrix view, Matrix projection){
            Model.Draw(World,view,projection);
        }
    }
}