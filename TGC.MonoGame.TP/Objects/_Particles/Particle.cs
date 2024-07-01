using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace ThunderingTanks.Objects;
public class Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Lifetime;
    public float Size;
    public float InitialLifetime; // Para guardar el valor inicial de Lifetime
    public Color Color;

    public Particle(Vector2 position, Vector2 velocity, float lifetime, float size, Color color)
    {
        Position = position;
        Velocity = velocity;
        Lifetime = lifetime;
        InitialLifetime = lifetime; // Guardamos el valor inicial
        Size = size;
        Color = color;
    }

    public void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
        Lifetime -= deltaTime;
        float alpha = Lifetime / InitialLifetime; // Calcula el alpha basado en la vida restante
        Color = new Color(Color.R, Color.G, Color.B, alpha);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, (int)Size, (int)Size), Color);
    }
}
