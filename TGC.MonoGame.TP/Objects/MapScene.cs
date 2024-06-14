﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThunderingTanks.Objects
{
    /// <summary>
    /// A City Scene to be drawn
    /// </summary>
    class MapScene
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";

        public const float DistanceBetweenParcels = 1000f;
        public const int NumInstances = 500; // Cantidad de instancias del terreno
        public const float ScaleFactor = 200f; // Factor de escala para agrandar cada parcela

        private Model Model { get; set; }

        private Texture2D TexturaTerreno { get; set; }
        private List<Matrix> WorldMatrices { get; set; }
        private Effect Effect { get; set; }

        /// <summary>
        /// Creates a City Scene with a content manager to load resources.
        /// </summary>
        /// <param name="content">The Content Manager to load resources</param>

        public MapScene(ContentManager content)
        {
            Model = content.Load<Model>(ContentFolder3D + "Grid/ground");

            TexturaTerreno = content.Load<Texture2D>(ContentFolder3D + "Grid/terreno");

            // Load an effect that will be used to draw the scene
            Effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

            // Get the first texture we find
            // The city model only contains a single texture
            //var effect = Model.Meshes.FirstOrDefault().Effects.FirstOrDefault() as BasicEffect;
            //var texture = effect.Texture;

            // Set the Texture to the Effect
            //Effect.Parameters["ModelTexture"].SetValue(texture);

            // Assign the mesh effect
            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            }

            WorldMatrices = new List<Matrix>();
            for (int i = 0; i < NumInstances; i++)
            {
                float x = i * DistanceBetweenParcels;
                Vector3 translation = new Vector3(x, 0, 0);
                Matrix scaleMatrix = Matrix.CreateScale(ScaleFactor);
                Matrix worldMatrix = Matrix.CreateTranslation(translation) * scaleMatrix;
                WorldMatrices.Add(worldMatrix);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            //Effect.Parameters["DiffuseColor"].SetValue(Color.Green.ToVector3());
            Effect.Parameters["ModelTexture"].SetValue(TexturaTerreno);



            var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);

            foreach (var mesh in Model.Meshes)
            {
                var meshWorld = modelMeshesBaseTransforms[mesh.ParentBone.Index];

                foreach (var worldMatrix in WorldMatrices)
                {
                    Effect.Parameters["World"].SetValue(meshWorld * worldMatrix);
                    mesh.Draw();
                }
            }

        }
    }
}