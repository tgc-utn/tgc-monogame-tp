using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.ConcreteEntities;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP
{
    internal class World
    {
        private readonly List<Entity> entities = new List<Entity>();

        internal void Register(Entity entity) => entities.Add(entity);

        internal void Initialize()
        {
            new DeathStar().Create();
            XWing.getInstance().Instantiate(new Vector3(50f, 0f, 0f));
            //new TIE().Instantiate(new Vector3(100f, 0f, 0f));
        }

        internal void Update(double elapsedTime)
        {
            int RandomNumber;
            Random random = new Random();
            RandomNumber = random.Next(0, 300000); // 1/300000 Chances * segundoS de que spawnee un Tie
            if (RandomNumber == 1)
            {
                TIE tie = new TIE();
                tie.Instantiate(new Vector3(random.Next(-100, 500), 0f, 0f));
                // Register(tie);
            }

            entities.ForEach(entity => entity.Update(elapsedTime));
        }
        internal void Draw() => entities.ForEach(entity => entity.Draw());
    }
}