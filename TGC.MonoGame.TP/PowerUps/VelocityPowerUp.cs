using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public VelocityPowerUp(Vector3 position) : base(position)
        {
            Position = position;

            PowerUpWorld = Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateTranslation(position);

        }

        public override void LoadContent(ContentManager Content)
        {

            PowerUpSound = Content.Load<SoundEffect>(ContentFolderSoundEffects + "EncenderAuto");

            PowerUpEffect = Content.Load<Effect>(ContentFolderEffects + "PowerUpsShader");
            
            PowerUpModel = Content.Load<Model>(ContentFolder3D + "PowerUps/ModeloTurbo");

            PowerUpTexture = ((BasicEffect)PowerUpModel.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;

            BoundingSphere = BoundingVolumesExtensions.CreateSphereFrom(PowerUpModel);

            BoundingSphere.Center = Position;

            BoundingSphere.Radius = 2f;
        }

        public override async void Activate(CarConvexHull carConvexHull)
        {
            if (!Activated)
            {
                PowerUpSound.Play();
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
