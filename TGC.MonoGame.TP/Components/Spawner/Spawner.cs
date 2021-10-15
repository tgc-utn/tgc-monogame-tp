using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.Components.Enemy;

namespace TGC.MonoGame.TP.Components.Spawner
{
    class Spawner
    {
        private int enemies = 10;
        private Vector3 Position { get; set; }
        private int timer = 300;

        public void SetPosition(Vector3 Position)
        {
            this.Position = Position;
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public void Update(TGCGame tgcGame)
        {
            timer--; 
            if (enemies > 0 && timer == 0) 
            {
                tgcGame.AddEnemy(Position);
                enemies -= 1;
                timer = 300;
            }
        }
    }
}
