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
        public VelocityPowerUp(Vector3 position) : base(position)
        {
            RandomPositions = false;

            //PowerUpWorld = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateTranslation(position);
           
            //var worldBounding = Matrix.CreateScale(1.5f, 1.5f, 1.5f) * Matrix.CreateTranslation(position);
            //BoundingBox.Add(BoundingVolumesExtensions.FromMatrix(worldBounding));
        }

        public VelocityPowerUp() : base()
        {
            RandomPositions = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            PowerUpEffect = Content.Load<Effect>(ContentFolderEffects + "PowerUpsShader");
            
            if(RandomPositions)
            PowerUpModel = new GameModel(Content.Load<Model>(ContentFolder3D + "PowerUps/ModeloTurbo"), PowerUpEffect, 1.5f ,GenerateRandomPositions(15));
            else
            PowerUpModel = new GameModel(Content.Load<Model>(ContentFolder3D + "PowerUps/ModeloTurbo"), PowerUpEffect, 1.5f , Position);

            PowerUpListWorld = PowerUpModel.World;

            PowerUpListWorld.ForEach(World => BoundingBox.Add(BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(1.5f, 1.5f, 1.5f) * World)));

            //PowerUpTexture = ((BasicEffect)PowerUpModel.Model.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;

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
