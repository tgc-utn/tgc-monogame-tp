using BepuPhysics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.PowerUps
{
    public abstract class PowerUp
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public Vector3 Position { get; set; }

        public BoundingBox BoundingBox { get; set; }

        public Matrix PowerUpWorld { get; set; }

        public GameModel PowerUpModel { get; set; }

        public Effect PowerUpEffect { get; set; }

        public Texture PowerUpTexture { get; set; }

        private float time { get; set; }

        private bool GoingUp { get; set; }

        private bool GoingDown { get; set; }

        public bool Activated;

        public List<Matrix> list = new List<Matrix>();

        protected PowerUp(Vector3 position)
        {
            Activated = false;
            Position = position;
            GoingUp = true;
            GoingDown = false;
        }

        public abstract void LoadContent(ContentManager Content);

        public void Update()
        {
            if (GoingUp)
            {
                //PowerUpWorld *= Matrix.CreateTranslation(0, 0.2f, 0);
                if (PowerUpWorld.Translation.Y >= Position.Y + 10f)
                {
                    GoingUp = false;
                    GoingDown = true;
                }
            }
            if (GoingDown)
            {
                PowerUpWorld *= Matrix.CreateTranslation(0, -0.2f, 0);
                if (PowerUpWorld.Translation.Y <= Position.Y)
                {
                    GoingUp = true;
                    GoingDown = false;
                }
            }

        }
        
        public void Draw(FollowCamera Camera, GameTime gameTime)
        {
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (!Activated)
            {
                PowerUpEffect.Parameters["World"].SetValue(PowerUpWorld);
                PowerUpEffect.Parameters["View"].SetValue(Camera.View);
                PowerUpEffect.Parameters["Projection"].SetValue(Camera.Projection);
                PowerUpEffect.Parameters["ModelTexture"].SetValue(PowerUpTexture);
                PowerUpEffect.Parameters["Time"].SetValue(Convert.ToSingle(time));
                
                list.Add(PowerUpWorld);
                PowerUpModel.Draw(list);
            }
        }

        public bool IsWithinBounds(Vector3 position)
        {
            var BoundingSphere = new BoundingSphere(position, 5f);
            return BoundingBox.Intersects(BoundingSphere);
        }

        public void ActivateIfBounding(Simulation Simulation, CarConvexHull carConvexHull)
        {
            BodyReference CarBody = Simulation.Bodies.GetBodyReference(carConvexHull.CarHandle);
            if (IsWithinBounds(CarBody.Pose.Position)) Activate(carConvexHull);
        }
       
        public abstract void Activate(CarConvexHull carConvexHull);

    }
}
