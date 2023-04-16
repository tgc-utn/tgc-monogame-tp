using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    public abstract class IElementoDinamico : IElemento, IDinamico  {
        public IElementoDinamico(string path, Vector3 posicionInicial, Vector3 rotacion) : base(path, posicionInicial, rotacion){}
        public abstract void Update(GameTime gameTime, KeyboardState keyboard);
    }
}