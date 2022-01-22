using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.Components.Spawner
{
    internal class SpawnerModel
    {
        private Effect effect;
        private Model spawnerModelMesh;

        public Effect GetEffect()
        {
            return effect;
        }

        public void SetEffect(Effect newEffect)
        {
            effect = newEffect;
        }

        public Model GetSpawnerModelMesh()
        {
            return spawnerModelMesh;
        }

        public void SetSpawnerModelMesh(Model newSpawnerModel)
        {
            spawnerModelMesh = newSpawnerModel;
        }

        public void LoadContent(string ContentFolder3D, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            spawnerModelMesh = Content.Load<Model>(ContentFolder3D + "monsterlarge/MonsterLarge");
            var modelEffectSpawner = (BasicEffect)spawnerModelMesh.Meshes[0].Effects[0];
            modelEffectSpawner.DiffuseColor = Color.White.ToVector3();
        }

        public void Update(float totalGameTime, Spawner Spawner)
        {
            Vector3 spawnerPosition = new Vector3(Spawner.GetPosition().X, 13 * MathF.Sin(totalGameTime) + 120, Spawner.GetPosition().Z);
            float bullskullScale = 0.1f;
            float diamondScale = 0.05f;

            float negativeVertical = -MathF.PI / 2;
            float rotationSkull = MathF.PI / 5;
            float rotation120 = (120 * MathF.PI / 180) + totalGameTime;
            float rotation240 = (240 * MathF.PI / 180) + totalGameTime;

            Vector3 zOffset = Vector3.UnitZ * 30f;
            Vector3 diamondOffset = zOffset * 1.35f;

            ModelMesh mesh = spawnerModelMesh.Meshes["Spine_LOD0"];
            Matrix spine1Transform = Matrix.CreateRotationX(negativeVertical) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(totalGameTime) * Matrix.CreateTranslation(spawnerPosition);
            mesh.ParentBone.Transform = spine1Transform;

            mesh = spawnerModelMesh.Meshes["Spine_LOD0__1_"];
            Matrix spine2Transform = Matrix.CreateRotationX(negativeVertical) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(rotation120) * Matrix.CreateTranslation(spawnerPosition);
            mesh.ParentBone.Transform = spine2Transform;

            mesh = spawnerModelMesh.Meshes["Spine_LOD0__2_"];
            Matrix spine3Transform = Matrix.CreateRotationX(negativeVertical) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(rotation240) * Matrix.CreateTranslation(spawnerPosition);
            mesh.ParentBone.Transform = spine3Transform;

            mesh = spawnerModelMesh.Meshes["BullSkull"];
            Matrix bullskull1Transform = Matrix.CreateScale(bullskullScale) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(totalGameTime) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 90);
            mesh.ParentBone.Transform = bullskull1Transform;

            mesh = spawnerModelMesh.Meshes["BullSkull__1_"];
            Matrix bullskull2Transform = Matrix.CreateScale(bullskullScale) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(rotation120) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 90);
            mesh.ParentBone.Transform = bullskull2Transform;

            mesh = spawnerModelMesh.Meshes["BullSkull__2_"];
            Matrix bullskull3Transform = Matrix.CreateScale(bullskullScale) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(rotation240) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 90);
            mesh.ParentBone.Transform = bullskull3Transform;

            mesh = spawnerModelMesh.Meshes["Skull_LOD0"];
            Matrix skull1Transform = Matrix.CreateRotationX(rotationSkull) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(totalGameTime) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 40);
            mesh.ParentBone.Transform = skull1Transform;

            mesh = spawnerModelMesh.Meshes["Skull_LOD0__1_"];
            Matrix skull2Transform = Matrix.CreateRotationX(rotationSkull) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(rotation120) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 40);
            mesh.ParentBone.Transform = skull2Transform;

            mesh = spawnerModelMesh.Meshes["Skull_LOD0__2_"];
            Matrix skull3Transform = Matrix.CreateRotationX(rotationSkull) * Matrix.CreateTranslation(zOffset) * Matrix.CreateRotationY(rotation240) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 40);
            mesh.ParentBone.Transform = skull3Transform;

            mesh = spawnerModelMesh.Meshes["_5_Side_Diamond"];
            Matrix diamond1Transform = Matrix.CreateScale(diamondScale) * Matrix.CreateRotationX(MathF.PI) * Matrix.CreateTranslation(diamondOffset) * Matrix.CreateRotationY(totalGameTime) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 35);
            mesh.ParentBone.Transform = diamond1Transform;

            mesh = spawnerModelMesh.Meshes["_5_Side_Diamond__1_"];
            Matrix diamond2Transform = Matrix.CreateScale(diamondScale) * Matrix.CreateRotationX(MathF.PI) * Matrix.CreateTranslation(diamondOffset) * Matrix.CreateRotationY(rotation120) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 35);
            mesh.ParentBone.Transform = diamond2Transform;

            mesh = spawnerModelMesh.Meshes["_5_Side_Diamond__2_"];
            Matrix diamond3Transform = Matrix.CreateScale(diamondScale) * Matrix.CreateRotationX(MathF.PI) * Matrix.CreateTranslation(diamondOffset) * Matrix.CreateRotationY(rotation240) * Matrix.CreateTranslation(spawnerPosition - Vector3.UnitY * 35);
            mesh.ParentBone.Transform = diamond3Transform;
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            foreach (var mesh in spawnerModelMesh.Meshes)
            {

                foreach (var part in mesh.MeshParts)
                {
                    Texture2D texturePart = null;

                    if (part.Effect.Parameters["Texture"] != null) texturePart = part.Effect.Parameters["Texture"].GetValueTexture2D();

                    part.Effect = effect;

                    // We set the main matrices for each mesh to draw
                    var worldMatrix = mesh.ParentBone.Transform;

                    if (texturePart != null) effect.Parameters["baseTexture"].SetValue(texturePart);

                    effect.Parameters["World"].SetValue(worldMatrix);
                    effect.Parameters["View"].SetValue(View);
                    effect.Parameters["Projection"].SetValue(Projection);
                    // InverseTransposeWorld is used to rotate normals
                    effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
                }
                mesh.Draw();
            }
        }
    }
}
