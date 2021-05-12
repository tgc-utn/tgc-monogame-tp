﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Trench2 : PhysicEntity<Sphere>
    {
        protected override Model Model() => TGCGame.modelManager.Trench2;
        protected override Texture2D[] Textures() => TGCGame.textureManager.Trench2;

        protected override Sphere Shape() => new Sphere(20f);
        protected override float Mass() => 100f;
        protected override Vector3 Scale { get; } = Vector3.One / 100f;
    }
}