using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal class RushPowerUp : PowerUp
    {
        public RushPowerUp(Vector3 position) : base(position)
        {
        }

        public override void LoadContent(ContentManager Content)
        {
            PowerUpModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "arrowpush/tinker");
            foreach (var mesh in PowerUpModel.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                Activated = true;
                Sphere.rushed = true;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
