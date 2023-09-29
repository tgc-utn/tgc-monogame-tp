using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Collisions;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal class FastPowerUp : PowerUp 
    {

        public FastPowerUp(Vector3 position) : base(position)
        {
            PowerUpWorld = Matrix.CreateScale(4f, 4f, 4f) * Matrix.CreateTranslation(position);
            var wordBounding = Matrix.CreateScale(20f, 20f, 20f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(wordBounding);
        }
    public override void LoadContent(ContentManager Content)
        {
            PowerUpModel = Content.Load<Model>("Models/powerups/agiltyup/Agility_Up_FBX");
            foreach (var mesh in PowerUpModel.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                Sphere.SphereSideSpeed *= 2;
                Activated = true;
                await Task.Delay(4000);
                Sphere.SphereSideSpeed /= 2;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
