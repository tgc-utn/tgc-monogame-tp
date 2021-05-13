using Microsoft.Xna.Framework;
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
            new XWing().Instantiate(new Vector3(50f, 0f, 0f));
            new TIE().Instantiate(new Vector3(100f, 0f, 0f));
        }

        internal void Update(double elapsedTime) => entities.ForEach(entity => entity.Update(elapsedTime));

        internal void Draw() => entities.ForEach(entity => entity.Draw());
    }
}