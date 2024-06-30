using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DXGI;
using SharpFont.PostScript;
using System;
using System.Collections.Generic;

namespace ThunderingTanks.Objects.Tanks
{
    public class Particula
    {
        public Vector2 Position;
        public Vector3 Velocity;
        public float LifeTime;
        public Texture2D Texture;
        public Color Color;
        public float Size;

        public Particula(Vector2 position, Vector3 velocity, float lifetime, Texture2D texture, Color color, float size)
        {
            Position = position;  // Usar la posición proporcionada
            Velocity = velocity;
            LifeTime = lifetime;
            Texture = texture;
            Color = color;
            Size = size;
        }


        public void Update(float deltaTime)
        {
            //Position += Velocity * deltaTime;
            LifeTime -= deltaTime;
        }
    }

}

