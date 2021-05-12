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
            new XWing().Instantiate(new Vector3(50f, 0f, 0f));
            new TIE().Instantiate(new Vector3(100f, 0f, 0f));
            new Trench().Instantiate(new Vector3(150f, 0f, 0f));
            new Trench2().Instantiate(new Vector3(200f, 0f, 0f));
        }

        internal void Draw() => entities.ForEach(entity => entity.Draw());
    }
}