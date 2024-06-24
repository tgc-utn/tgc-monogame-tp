
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Metadata.Ecma335;


namespace ThunderingTanks.Objects
{

    /// <summary>
    /// A City Scene to be drawn
    /// </summary>
    class MapScene
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";


        public const float DistanceBetweenParcels = 1000f;
        public const int NumInstances = 500;
        public const float ScaleFactor = 200f;

        private Model Model { get; set; }

        private Texture2D TexturaTerreno { get; set; }
        private List<Matrix> WorldMatrices { get; set; }
        private Effect Effect { get; set; }

        //HEIGHTMAP
        private Texture2D DiffuseTexture { get; set; }
        private Texture2D DisplacementTexture { get; set; }
        private Texture2D ColorMap { get; set; }

        private Texture2D Roughness {  get; set; }
        private Effect TerrainEffect { get; set; }

        public float angle;
        public Vector3 DesiredLookAt;
        public bool hay_lookAt;
        public Vector3 LookAt;

        private Model model;
        public Vector2 pos;
        public Vector3 shipPos;
        public SimpleTerrain terrain;



        /// <summary>
        /// Creates a City Scene with a content manager to load resources.
        /// </summary>
        /// <param name="content">The Content Manager to load resources</param>
        public MapScene(ContentManager content, GraphicsDevice graphicsDevice)
        {
            /*

            //TRato de cargars texts para heightmap
            DiffuseTexture = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/Diffuse");

            DisplacementTexture = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/heightmap");

            TerrainEffect = content.Load<Effect>(ContentFolderEffects + "Terrain");

            ColorMap = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/Normal Map");

            Roughness = content.Load < Texture2D>(ContentFolderTextures + "/heighmap/roughness");

            terrain = new SimpleTerrain(graphicsDevice, DisplacementTexture, DiffuseTexture, TerrainEffect, ColorMap, Roughness);

            */
            ///////////////////////////
            
                        Model = content.Load<Model>(ContentFolder3D + "Grid/ground");
                        TexturaTerreno = content.Load<Texture2D>(ContentFolder3D + "Grid/terreno");

                        Effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

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

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            /*
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            var oldRasterizeState = graphicsDevice.RasterizerState;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
    
            terrain.Draw(Matrix.Identity * Matrix.CreateTranslation(new Vector3(0f, -500f, 0f)), view, projection);
            graphicsDevice.RasterizerState = oldRasterizeState;
            */

            
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
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
