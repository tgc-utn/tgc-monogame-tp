using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.ConcreteEntities;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP
{
    internal class World
    {
        int RandomNumber = 0;
        Random Random = new Random();

        private readonly List<Entity> entities = new List<Entity>();

        internal void Register(Entity entity) => entities.Add(entity);

        internal void Initialize()
        {
            new DeathStar().Create();
            XWing.getInstance().Instantiate(new Vector3(50f, 0f, 0f));
        }

        internal void Update(double elapsedTime)
        {
            RandomNumber = Random.Next(0, 750); // 1/750 Chances * update de que spawnee un Tie
            if (RandomNumber == 1)
            {
                new TIE().Instantiate(new Vector3((float)Random.Next(100, 400), 0f, 0f));
            }

            entities.ForEach(entity => entity.Update(elapsedTime));
        }
        internal void Draw() => entities.ForEach(entity => entity.Draw());
    }
}