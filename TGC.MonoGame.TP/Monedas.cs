using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Quads;
using TGC.MonoGame.TP.SkyBoxs;

namespace TGC.MonoGame.TP.MonedasItem
{
    public class Monedas
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        private Model Moneda { get; set; }
        private Texture2D CoinTexture { get; set; }
        private bool TocandoMonedas { get; set; }
        private Matrix moneda1World { get; set; }
        private BoundingSphere moneda1box { get; set; }
        public Effect TextureEffect { get; set; }
        public int monedas = 0;
        public Vector3 Moneda1Pos;

        public void Initialize()
        {
            TocandoMonedas = false;
            Moneda1Pos = new Vector3(-43.5f, 20f, 25f);
            moneda1World = Matrix.CreateTranslation(Moneda1Pos);

        } 

        public Monedas(ContentManager content)
        {
            //cargo moneda
            Moneda = content.Load<Model>(ContentFolder3D + "Marbel/Moneda/Coin");
            TextureEffect = content.Load<Effect>(ContentFolderEffects + "TextureShader");
            CoinTexture = content.Load<Texture2D>(ContentFolderTextures + "Coin");
            //mesh moneda
            foreach (var mesh in Moneda.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
        }
        
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, float totalGameTime, Matrix World)
        {
            List<Vector3> monedas = new List<Vector3>
            {
                new Vector3(-43.5f, 20f + MathF.Cos(totalGameTime * 2), 25f),
                new Vector3(10, -10 + MathF.Cos(totalGameTime * 2), 0),
                new Vector3(25, -14+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(37, -5+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(53, -10+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(63, -10+ MathF.Cos(totalGameTime * 2), 0),
                new Vector3(35f, -20f+ MathF.Cos(totalGameTime * 2), 110f),
                new Vector3(50, -13+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(55, -14+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(45, -16+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(40, -14+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(35, -14+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(27.5f, -12.5f+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(22.5f, -12.5f+ MathF.Cos(totalGameTime * 2), 110),
                new Vector3(4f, -12f+ MathF.Cos(totalGameTime * 2), 115),
                new Vector3(4f, -8f+ MathF.Cos(totalGameTime * 2), 115),
                new Vector3(4f, -5f+ MathF.Cos(totalGameTime * 2), 115),
                new Vector3(7f, -12f+ MathF.Cos(totalGameTime * 2), 107.5f),
                new Vector3(-17.5f, -12f+ MathF.Cos(totalGameTime * 2), 92.5f),
                new Vector3(-22.5f, -12f+ MathF.Cos(totalGameTime * 2), 87.5f),
                new Vector3(-27.5f, -7f+ MathF.Cos(totalGameTime * 2), 85f),
                new Vector3(-27.5f, -2f+ MathF.Cos(totalGameTime * 2), 85f),
                new Vector3(-27.5f, 1f+ MathF.Cos(totalGameTime * 2), 85f),
                new Vector3(-37.5f, -2f+ MathF.Cos(totalGameTime * 2), 82.5f),
                new Vector3(-37.5f, 2f+ MathF.Cos(totalGameTime * 2), 82.5f),
                new Vector3(-42.5f, 0f+ MathF.Cos(totalGameTime * 2), 80f),
                new Vector3(-45f, -3f+ MathF.Cos(totalGameTime * 2), 80f),
                new Vector3(-48f, -6f+ MathF.Cos(totalGameTime * 2), 78f),
                new Vector3(-47.5f, -12.5f+ MathF.Cos(totalGameTime * 2), 78f),
                new Vector3(-52.5f, -2.5f+ MathF.Cos(totalGameTime * 2), 75f),
                new Vector3(-52.5f, 0f+ MathF.Cos(totalGameTime * 2), 75f),
                new Vector3(-52.5f, 2.5f+ MathF.Cos(totalGameTime * 2), 75f),
                new Vector3(-57.5f, -2.5f+ MathF.Cos(totalGameTime * 2), 77.5f),
                new Vector3(-67.5f, 5f+ MathF.Cos(totalGameTime * 2), 70f),
                new Vector3(-67.5f, 0f+ MathF.Cos(totalGameTime * 2), 70f),
                new Vector3(-72.5f, 0f+ MathF.Cos(totalGameTime * 2), 67.5f),
                new Vector3(-77.5f, 0f+ MathF.Cos(totalGameTime * 2), 62.5f),
                new Vector3(-77.5f, 5f+ MathF.Cos(totalGameTime * 2), 62.5f),
                new Vector3(-77.5f, 7.5f+ MathF.Cos(totalGameTime * 2), 62.5f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 49f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 45f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 40f),
                new Vector3(-87.5f, 15f+ MathF.Cos(totalGameTime * 2), 35f)
            };
            foreach (Vector3 vector in monedas)
            {
                //TP.TGCGame.DrawMeshes((Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(totalGameTime) * Matrix.CreateTranslation(vector)), CoinTexture, Moneda);
                foreach (var mesh in Moneda.Meshes)
                {
                    World = mesh.ParentBone.Transform * (Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(totalGameTime) * Matrix.CreateTranslation(vector));
                    TextureEffect.Parameters["Texture"].SetValue(CoinTexture);
                    TextureEffect.Parameters["World"].SetValue(World);
                    mesh.Draw();
                }
            }
        }

    }
}