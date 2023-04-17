using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class IACar : Car
    {
        public IACar(string ubicacionAuto, Vector3 posicionInicial, Vector3 rotacion) : base(ubicacionAuto,posicionInicial, rotacion){}

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }
    }
}
