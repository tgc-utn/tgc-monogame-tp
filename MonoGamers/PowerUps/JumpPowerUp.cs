using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Collisions;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonoGamers.Audio;

namespace MonoGamers.PowerUps
{
    internal class JumpPowerUp : PowerUp
    {
        public JumpPowerUp(Vector3 position) : base(position)
        {
           PowerUpWorld = Matrix.CreateScale(0.3f, 0.3f, 0.3f) * Matrix.CreateTranslation(position);
            SphereWorld = Matrix.CreateScale(12f, 12f, 12f) * Matrix.CreateTranslation(position + new Vector3(0, 10f, 0));
           var wordBounding = Matrix.CreateScale(10f, 10f, 10f) * Matrix.CreateTranslation(position);
           BoundingBox = BoundingVolumesExtensions.FromMatrix(wordBounding);
        }

        public override void LoadContent(ContentManager Content)
        {
            PowerUpModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "pluma/feather");
            PowerUpEffect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "PowerUpShader");
            FloatingSphereEffect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "FloatingSphereShader");
            PowerUpTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "white");
            SphereModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3D"] + "geometries/sphere");
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                AudioController.PlayPowerUp();
                Sphere.SphereJumpSpeed *= 1.01f;
                Activated = true;
                await Task.Delay(4000);
                Sphere.SphereJumpSpeed /= 1.01f;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
