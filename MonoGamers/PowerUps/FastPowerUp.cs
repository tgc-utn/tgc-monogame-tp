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

namespace MonoGamers.PowerUps
{
    internal class FastPowerUp : PowerUp 
    {

        public FastPowerUp(Vector3 position) : base(position)
        {
            var quaternion = Quaternion.CreateFromAxisAngle(Vector3.Backward, 0) *
                         Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI/2) *
                         Quaternion.CreateFromAxisAngle(Vector3.Right, 0);

            PowerUpWorld = Matrix.CreateTranslation(position)
                * Matrix.CreateScale(4f, 4f, 4f)
                * Matrix.CreateFromQuaternion(quaternion);
            var worldBounding = Matrix.CreateScale(20f, 20f, 20f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(worldBounding);
        }
    public override void LoadContent(ContentManager Content)
        {
            PowerUpModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "agiltyup/Agility_Up_FBX");
            PowerUpEffect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "BasicShader");
            PowerUpTexture = ((BasicEffect)PowerUpModel.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;
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
