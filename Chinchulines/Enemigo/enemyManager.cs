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
        private List<Enemy> EnemigosVigilantes = new List<Enemy>();

        public void LoadContent(ContentManager content)
        {
            foreach (Enemy enemigo in Enemies)
            {
                enemigo.LoadContent(content);
            }

            foreach (Enemy enemigo in EnemigosVigilantes)
            {
                enemigo.LoadContentEnemigoVigilante(content);
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

        //TODO: mover metodos a otra clase de otro tipo de enemigo
        public void CrearEnemigoVigilante(Vector3 playerPosition)
        {
            Enemy newEnemyWatcher = new Enemy(new Vector3(playerPosition.X + 20, playerPosition.Y, playerPosition.Z));
            EnemigosVigilantes.Add(newEnemyWatcher);
        }

        public void UpdateEnemigoVigilante(Vector3 playerPosition)
        {
            foreach (Enemy enemigo in EnemigosVigilantes)
            {
                enemigo.Update(playerPosition);
            }
        }

        public void DrawEnemigoVigilante(Matrix View, Matrix Projection, Vector3 playerpos, Quaternion spaceshipRotation)
        {
            foreach (Enemy enemigo in EnemigosVigilantes)
            {
                enemigo.DrawEnemy2(View, Projection, playerpos, spaceshipRotation);
            }
        }
    }
}
