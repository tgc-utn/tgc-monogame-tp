using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace ThunderingTanks.Objects.Tanks
{
    public class SistemaDeParticulas
    {
        private List<Particula> particulas;
        private Texture2D texturaParticula;
        private Random random;

        public SistemaDeParticulas(Texture2D textura)
        {
            particulas = new List<Particula>();
            texturaParticula = textura;
            random = new Random();
        }

        public void EmitirParticulas(Vector2 posicion, int cantidad, float radio)
        {
            for (int i = 0; i < cantidad; i++)
            {
                // Generar una posición aleatoria dentro de un radio
                double angle = random.NextDouble() * Math.PI * 2;
                double distance = random.NextDouble() * radio;
                var posicionParticula = new Vector2(
                    posicion.X + (float)(Math.Cos(angle) * distance),
                    posicion.Y + (float)(Math.Sin(angle) * distance)
                );

                // Generar una velocidad aleatoria
                var velocidad = new Vector3(
                    (float)random.NextDouble() * 2 - 1,
                    (float)random.NextDouble() * 2 - 1,
                    (float)random.NextDouble() * 2 - 1
                );

                var particula = new Particula(posicionParticula, velocidad, 0.1f, texturaParticula, Color.Yellow, 0.1f);
                particulas.Add(particula);
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = particulas.Count - 1; i >= 0; i--)
            {
                particulas[i].Update(deltaTime);
                if (particulas[i].LifeTime <= 0)
                {
                    particulas.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            foreach (var particula in particulas)
            {
                // Transformar la posición de la partícula al espacio de pantalla
                //var screenPosition = Vector3.Transform(particula.Position, view * projection);

                // Dibujar la partícula en la posición de pantalla transformada
                // Calcular la posición en coordenadas de pantalla relativas
                Vector2 screenCoords = particula.Position;

                spriteBatch.Draw(
                    particula.Texture,
                    screenCoords,
                    null,
                    particula.Color,
                    0f,
                    Vector2.Zero,
                    particula.Size,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
