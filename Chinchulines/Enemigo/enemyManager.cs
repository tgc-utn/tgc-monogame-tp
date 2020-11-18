using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Chinchulines.Entities;
using System.Text;

namespace Chinchulines.Enemigo
{
    class enemyManager
    {

        Random x = new Random();
        Random y = new Random();
        Random z = new Random();

        private List<Enemy> Enemies = new List<Enemy>();

        public void LoadContent(ContentManager content)
        {
            foreach (Enemy enemigo in Enemies)
            {
                enemigo.LoadContent(content);
            }
        }

        public void CrearEnemigo()
        {
            var posx = x.Next(-5, 5);
            var posy = y.Next(25, 35);
            var posz = z.Next(-150, -100);

            Enemies.Add(new Enemy(new Vector3(posx, posy, posz)));
        }

        public void Update(GameTime gameTime, Vector3 position)
        {
            foreach (Enemy enemigo in Enemies)
            {
                enemigo.Update(gameTime, position);
            }
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            foreach (Enemy enemigo in Enemies)
            {
                enemigo.Draw(View, Projection);
            }
        }
}
}
