
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
    /// Conteins the map for the game
    /// </summary>
    class MapScene
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private Texture2D DiffuseTexture { get; set; }
        private Texture2D DiffuseTexture2 { get; set; }
        private Texture2D DisplacementTexture { get; set; }
        private Texture2D ColorMap { get; set; }

        private Effect TerrainEffect { get; set; }

        public SimpleTerrain terrain;

        /// <summary>
        /// Creates the Map For The Game
        /// </summary>
        /// <param name="content">The Content Manager to load resources</param>
        public MapScene(ContentManager content, GraphicsDevice graphicsDevice)
        {
            TerrainEffect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

            DiffuseTexture = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/Snow");
            DiffuseTexture2 = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/Snow2");
            DisplacementTexture = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/Displacement");
            ColorMap = content.Load<Texture2D>(ContentFolderTextures + "/heighmap/heightmap");

            terrain = new SimpleTerrain(graphicsDevice, DisplacementTexture, ColorMap, DiffuseTexture, DiffuseTexture2, TerrainEffect);

        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            var oldRasterizeState = graphicsDevice.RasterizerState;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            

            terrain.Draw(Matrix.Identity * Matrix.CreateTranslation(new Vector3(0f, -450f, 0f)), view, projection);


            graphicsDevice.RasterizerState = oldRasterizeState;

        }
    }
}
