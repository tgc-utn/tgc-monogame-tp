using BepuPhysics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using TGC.MonoGame.TP.Camaras;
using TGC.MonoGame.Samples.Collisions;

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
        public const string ContentFolderSoundEffects = "SoundEffects/";

        public SoundEffect PowerUpSound { get; set; }

        public bool Touch { get; set; }

        public bool RandomPositions { get; set; }

        public Vector3 Position { get; set; }

        public BoundingSphere BoundingSphere;

        public Matrix PowerUpWorld { get; set; }

        public Model PowerUpModel { get; set; }

        public Effect PowerUpEffect { get; set; }

        public Texture PowerUpTexture { get; set; }

        private float time { get; set; }

        private bool GoingUp { get; set; }

        private bool GoingDown { get; set; }

        public bool Activated;

        private int ArenaWidth = 200;
        private int ArenaHeight = 200;
        private Random _random = new Random();

        protected PowerUp(Vector3 position)
        {
            Activated = false;
            Position = position;
            GoingUp = true;
            GoingDown = false;
        }

        protected PowerUp()
        {
            Activated = false;
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
        
        public void Draw(FollowCamera Camera, GameTime gameTime, BoundingFrustum boundingFrustum)
        {
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (!Activated)
            {
                PowerUpEffect.Parameters["World"].SetValue(PowerUpWorld);
                PowerUpEffect.Parameters["View"].SetValue(Camera.View);
                PowerUpEffect.Parameters["Projection"].SetValue(Camera.Projection);
                PowerUpEffect.Parameters["ModelTexture"].SetValue(PowerUpTexture);
                PowerUpEffect.Parameters["Time"].SetValue(Convert.ToSingle(time));

                
                if (boundingFrustum.Intersects(BoundingSphere)) {

                        var mesh = PowerUpModel.Meshes.FirstOrDefault();

                          if (mesh != null)
                          {
                              foreach (var part in mesh.MeshParts)
                              {
                                  part.Effect = PowerUpEffect;
                              }

                              mesh.Draw();
                          }
                  }
            }
        }

        public bool IsWithinBounds(OrientedBoundingBox carBox)
        {
            return carBox.Intersects(BoundingSphere);
        }

        public void ActivateIfBounding(OrientedBoundingBox CarBox , CarConvexHull carConvexHull)
        {
            if (IsWithinBounds(CarBox)) Activate(carConvexHull);
        }

        public abstract void Activate(CarConvexHull carConvexHull);

        public List<Vector3> GenerateRandomPositions(int count)
        {
            var positions = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(-ArenaWidth, ArenaWidth);
                int z = _random.Next(-ArenaHeight, ArenaHeight);
                positions.Add(new Vector3(x, 0, z));
            }

            return positions;
        }


    }
}
