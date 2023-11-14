using BepuPhysics;
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
using System.Threading.Tasks;
using MonoGamers.Audio;

namespace MonoGamers.PowerUps
{
    internal class FastPowerUp : PowerUp 
    {

        public FastPowerUp(Vector3 position) : base(position)
        {

            PowerUpWorld =  Matrix.CreateScale(2f, 2f, 2f)
                            * Matrix.CreateFromYawPitchRoll(0.5f, 0, 0)
                            * Matrix.CreateTranslation(position + new Vector3(0,10f,0));
            SphereWorld = Matrix.CreateScale(10f,10f,10f) * Matrix.CreateTranslation(position + new Vector3(0, 10f, 0));
            var worldBounding = Matrix.CreateScale(20f, 20f, 20f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(worldBounding);
        }
        public override void LoadContent(ContentManager Content)
        {
            PowerUpModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "agiltyup/Shoe");
            PowerUpEffect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "PowerUpShader");
            FloatingSphereEffect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "FloatingSphereShader");
            PowerUpTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "agilityup/Diff");
            SphereModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3D"] + "geometries/sphere");
        }

        public override async void Activate(MonoSphere Sphere)
        {
            if (!Activated)
            {
                AudioController.PlayPowerUp();
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
