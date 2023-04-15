using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class IACar : Car
    {
        public IACar(string ubicacionAuto, Vector3 posicionInicial) : base(ubicacionAuto,posicionInicial){}

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }
    }
}
