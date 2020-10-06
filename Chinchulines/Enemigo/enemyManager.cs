using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chinchulines.Enemigo
{
    class enemyManager
    {
        private Model mk2;

        Random x = new Random();
        Random y = new Random();
        Random z = new Random();

        private List<Enemy> Enemies = new List<Enemy>();

        public enemyManager(Model spaceShipModelMK2)
        {
            mk2 = spaceShipModelMK2;
        }

        public void CrearEnemigo()
        {
            var posx = x.Next(-5, 5);
            var posy = y.Next(-5, 5);
            var posz = z.Next(-250, -200);

            Enemies.Add(new Enemy(new Vector3(posx, posy, posz), mk2));
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
