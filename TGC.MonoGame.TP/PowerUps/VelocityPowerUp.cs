using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.Samples.Collisions;

namespace TGC.MonoGame.TP.PowerUps
{
    public class VelocityPowerUp : PowerUp
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public VelocityPowerUp(Vector3 position) : base(position)
        {
            var quaternion = Quaternion.CreateFromAxisAngle(Vector3.Backward, 0) *
                         Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI / 2) *
                         Quaternion.CreateFromAxisAngle(Vector3.Right, 0);

            PowerUpWorld = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateTranslation(position);
                //* Matrix.CreateFromQuaternion(quaternion);
           
            var worldBounding = Matrix.CreateScale(1.5f, 1.5f, 1.5f) * Matrix.CreateTranslation(position);
            BoundingBox = BoundingVolumesExtensions.FromMatrix(worldBounding);
        }
        public override void LoadContent(ContentManager Content)
        {
            PowerUpEffect = Content.Load<Effect>(ContentFolderEffects + "PowerUpsShader");
            
            PowerUpModel = new GameModel(Content.Load<Model>(ContentFolder3D + "PowerUps/ModeloTurbo"), PowerUpEffect, 1.5f , Position);


        }

        public override async void Activate(CarConvexHull carConvexHull)
        {
            if (!Activated)
            {
                carConvexHull.maxSpeed *= 2;
                carConvexHull.maxTurn *= 2;
                Activated = true;
                await Task.Delay(4000);
                carConvexHull.maxSpeed /= 2;
                carConvexHull.maxTurn /= 2;
                await Task.Delay(4000);
                Activated = false;
            }
        }
    }
}
