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
            var quaternion = Quaternion.CreateFromAxisAngle(Vector3.Backward, (float)Math.PI/2) *
                         Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI/2) *
                         Quaternion.CreateFromAxisAngle(Vector3.Right, (float)Math.PI/2);

            PowerUpWorld =  Matrix.CreateScale(4f, 4f, 4f)
                            * Matrix.CreateFromYawPitchRoll(0, 1.5f, 0)
                            * Matrix.CreateTranslation(position);
            var worldBounding = Matrix.CreateScale(20f, 20f, 20f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(worldBounding);
        }
    public override void LoadContent(ContentManager Content)
        {
            PowerUpModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "agiltyup/Agility_Up_FBX");
            PowerUpEffect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "BasicShader");
            PowerUpTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "agilityup/AU_Display_MAT_Base_Color");
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
