using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Collisions;


namespace TGC.MonoGame.TP
{    
    public class Object
    {
        
        private Model Model { get; set; }
        public Matrix World { get; set; }

        public bool esVictima = false;
        public bool esEliminable = false;
        
        private Texture2D Texture { get; set; }
        public Vector3 Position{ get; set; }
        private float Rotation{ get; set; }
        private Effect Effect { get; set; }

        public OrientedBoundingBox Box { get; set; }

        public Object(Vector3 Position, Model modelo, Effect efecto, Texture2D textura, bool esConstante){
            this.Position = Position;

            World = Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
            
            Model = modelo;

            var AABB = BoundingVolumesExtensions.CreateAABBFrom(Model);
            Box = OrientedBoundingBox.FromAABB(AABB);
            Box.Center = Position;
            Box.Orientation = Matrix.Identity;
            
            //Box = new OrientedBoundingBox(Position, new Vector3(25,0,25));

            Effect = efecto;

            Texture = textura;

            esEliminable = esConstante;
        }

        public void LoadContent(){

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.

            //Effect.Parameters["ModelTexture"].SetValue(Texture);

            // Al mesh le asigno el Effect (solo textura por ahora)
            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            // Tanto la vista como la proyección vienen de la cámara por parámetro
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["ModelTexture"].SetValue(Texture);

            var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);

            foreach (var mesh in Model.Meshes)
            {
                var meshWorld = modelMeshesBaseTransforms[mesh.ParentBone.Index];
                Effect.Parameters["World"].SetValue(meshWorld*World);
                mesh.Draw();
            }
        }
    }

}