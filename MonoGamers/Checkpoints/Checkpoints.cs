using System;
using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Collisions;
using MonoGamers.Geometries;
using MonoGamers.Geometries.Textures;

namespace MonoGamers.Checkpoints
{
    public class Checkpoint 
    {
        public Vector3 Position {get;set;}
        
        private Matrix World {get;set;}
        public BoundingBox BoundingBox {get;set;}
        
        // _____ Geometries _______
        private BoxPrimitive BoxPrimitive { get; set; }
        
        private Texture2D CheckTexture { get; set; }
        
        public Effect Effect { get; set; }

        private Boolean alreadyPassed = false; 
        
        public Checkpoint (Vector3 position,Vector3 scale, ContentManager Content, GraphicsDevice graphicsDevice )
        {

            this.Position = position;
            var world = Matrix.CreateScale(scale.X,scale.Y,scale.Z) * Matrix.CreateTranslation(position);
            World = world;
            this.BoundingBox = BoundingVolumesExtensions.FromMatrix(world);

            CheckTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "common");
            
            Effect = Content.Load<Effect>(
                ConfigurationManager.AppSettings["ContentFolderEffects"] + "AlphaBlending");
            
            // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(graphicsDevice, Vector3.One, CheckTexture);
        }
        public bool IsWithinBounds(Vector3 position)
        {
            var BoundingSphere = new BoundingSphere(position, 10f);
            if (BoundingBox.Intersects(BoundingSphere))
            {
                alreadyPassed = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(Camera.Camera camera)
        {
            Effect.Parameters["Texture"].SetValue(CheckTexture);
            Effect.Parameters["AlphaFactor"].SetValue(0.4f);
            Effect.Parameters["Tint"].SetValue(alreadyPassed ? Color.Snow.ToVector3() : Color.Red.ToVector3());
            Effect.Parameters["WorldViewProjection"].SetValue(World * camera.View * camera.Projection);
            BoxPrimitive.Draw(Effect); 
        }
    }
}