using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Collisions;
using MonoGamers.Geometries;
using MonoGamers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal abstract class PowerUp
    {
        public Vector3 Position { get; set; }
        public BoundingBox BoundingBox { get; set; }

        public Matrix PowerUpWorld { get; set; }
        public Model PowerUpModel { get; set; }

        public Effect PowerUpEffect { get; set; }
        public Effect FloatingSphereEffect { get; set; }

        public Texture PowerUpTexture { get; set; } 

        public Model SphereModel { get; set; }
        public Matrix SphereWorld { get; set; }

        private float time { get; set;}

        private bool GoingUp { get; set; }
        private bool GoingDown { get; set; }

        public bool Activated;
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
            
        }
        public void Draw(Camera.Camera Camera, GameTime gameTime)
        {
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (!Activated)
            {
                PowerUpEffect.Parameters["World"].SetValue(PowerUpWorld);
                PowerUpEffect.Parameters["View"].SetValue(Camera.View);
                PowerUpEffect.Parameters["Projection"].SetValue(Camera.Projection);
                PowerUpEffect.Parameters["ModelTexture"].SetValue(PowerUpTexture);
                PowerUpEffect.Parameters["Time"]?.SetValue(time);

                FloatingSphereEffect.Parameters["World"].SetValue(SphereWorld);
                FloatingSphereEffect.Parameters["View"].SetValue(Camera.View);
                FloatingSphereEffect.Parameters["Projection"].SetValue(Camera.Projection);
                FloatingSphereEffect.Parameters["Time"]?.SetValue(time);

                var mesh = PowerUpModel.Meshes.FirstOrDefault();
                if (mesh != null)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = PowerUpEffect;
                    }

                    mesh.Draw();
                }
                if(SphereModel != null)
                {
                    var sphereMesh = SphereModel.Meshes.FirstOrDefault();
                    foreach (var part in sphereMesh.MeshParts)
                    {
                        part.Effect = FloatingSphereEffect;
                    }
                    sphereMesh.Draw();
                }
            }
        }

        public bool IsWithinBounds(Vector3 position)
        {
            var BoundingSphere = new BoundingSphere(position, 5f);
            return BoundingBox.Intersects(BoundingSphere);
        }

        public void ActivateIfBounding(Simulation Simulation, MonoSphere sphere)
        {
            BodyReference SphereBody = Simulation.Bodies.GetBodyReference(sphere.SphereHandle);
            if (IsWithinBounds(SphereBody.Pose.Position)) Activate(sphere);
        }
        public abstract void Activate(MonoSphere Sphere);

    }
}