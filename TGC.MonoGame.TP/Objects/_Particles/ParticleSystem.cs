using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace ThunderingTanks.Objects;
public class ParticleSystem
{
    private List<Particle> particles;
    private Random random;
    private Texture2D pixel;

    public ParticleSystem(GraphicsDevice graphicsDevice)
    {
        particles = new List<Particle>();
        random = new Random();
        pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.Gray });
    }

    public void AddParticle(Vector2 position)
    {
        float lifetime = (float)random.NextDouble() * 1f + 9f;
        float size = (float)random.NextDouble() * 7f + 1f;
        Vector2 velocity = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(-random.NextDouble() * 2 - 15)); // Ajuste de desplazamiento hacia arriba
        Color color = Color.Gray * 10f;
        particles.Add(new Particle(position, velocity, lifetime, size, color));

    }

    public void Update(float deltaTime)
    {
        for (int i = particles.Count - 1; i >= 0; i--)
        {
            particles[i].Update(deltaTime);
            if (particles[i].Lifetime <= 0)
            {
                particles.RemoveAt(i);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in particles)
        {
            particle.Draw(spriteBatch, pixel);
        }
    }
}
