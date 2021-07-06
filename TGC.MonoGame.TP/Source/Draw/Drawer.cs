using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    public class Drawer
    {
        TGCGame Game;
        GraphicsDevice GraphicsDevice;
        ContentManager Content;
        String ContentFolder3D;
        String ContentFolderEffects;
        String ContentFolderTextures;
        //TGCGame.GmState GameState;
        #region models effects textures targets
        public Model TieModel;
        public Model XwingModel;
        private Model XwingEnginesModel;

        public static Model TrenchPlatform;
        public static Model TrenchStraight;
        public static Model TrenchT;
        public static Model TrenchIntersection;
        public static Model TrenchElbow;
        public static Model TrenchTurret;
        SkyBox SkyBox;
        Model skyboxModel;
        public Model LaserModel;
        
        public Effect EffectBloom;
        public Effect EffectBlur;
        public Effect MasterMRT;

        private Texture TieTexture;
        private Texture TieNormalTex;
        //private Texture TrenchTexture;
        private Texture[] XwingTextures;
        private Texture[] XwingNormalTex;
        private TextureCube skyBoxTexture;

        private FullScreenQuad FullScreenQuad;
        private RenderTarget2D BlurHRenderTarget;
        private RenderTarget2D BlurVRenderTarget;
        private RenderTarget2D ShadowMapRenderTarget;
        private RenderTarget2D ColorTarget;
        private RenderTarget2D NormalTarget;
        private RenderTarget2D DirToCamTarget;
        private RenderTarget2D BloomFilterTarget;
        private RenderTarget2D LightTarget;
        private RenderTarget2D SceneTarget;
        #endregion

        Vector2 ShadowMapSize;

        public List<Trench> trenchesToDraw = new List<Trench>();
        public List<TieFighter> tiesToDraw = new List<TieFighter>();
        public List<Laser> lasersToDraw = new List<Laser>();
        public List<Ship> shipsToDraw = new List<Ship>();
        public bool showXwing;

        SpriteBatch SpriteBatch;
        enum DrawType
        {
            Regular,
            DepthMap
        }

        public Drawer()
        {
           
        }
        
        public void Init()
        {
            Game = TGCGame.Instance;
            GraphicsDevice = Game.GraphicsDevice;
            Content = Game.Content;

            ContentFolder3D = TGCGame.ContentFolder3D;
            ContentFolderEffects = TGCGame.ContentFolderEffects;
            ContentFolderTextures = TGCGame.ContentFolderTextures;

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            LoadContent();
        }
        void LoadContent()
        {
            TieModel = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            XwingModel = Content.Load<Model>(ContentFolder3D + "XWing/model");
            XwingEnginesModel = Content.Load<Model>(ContentFolder3D + "XWing/xwing-engines");

            TrenchPlatform = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Platform-Block");
            TrenchStraight = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Straight-Block");
            TrenchT = Content.Load<Model>(ContentFolder3D + "Trench/Trench-T-Block");
            TrenchElbow = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Elbow-Block");
            TrenchIntersection = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Intersection");
            TrenchTurret = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Turret");
            //Trench2 = Content.Load<Model>(ContentFolder3D + "Trench2/Trench");
            LaserModel = Content.Load<Model>(ContentFolder3D + "Laser/Laser");

            EffectBloom = Content.Load<Effect>(ContentFolderEffects + "Bloom");
            EffectBlur = Content.Load<Effect>(ContentFolderEffects + "GaussianBlur");

            MasterMRT = Content.Load<Effect>(ContentFolderEffects + "MasterMRT");
            XwingTextures = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };
            XwingNormalTex = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Normal_DirectX"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Normal_DirectX") };

            TieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");
            TieNormalTex = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Normal");
            //TrenchTexture = Content.Load<Texture2D>(ContentFolderTextures + "Trench/MetalSurface");

            skyboxModel = Content.Load<Model>(ContentFolder3D + "skybox/cube");

            skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skybox/space_earth_small_skybox");
            
            SkyBox = new SkyBox(skyboxModel, skyBoxTexture, MasterMRT);

            assignEffectToModels(new Model[] { TieModel, XwingModel, XwingEnginesModel, TrenchElbow,
                TrenchIntersection, TrenchPlatform, TrenchStraight, TrenchT, TrenchTurret, LaserModel, SkyBox.Model }, MasterMRT);

            manageEffectParameters();
            
            InitRTs();
           
            
        }
        public void InitRTs()
        {
            FullScreenQuad = new FullScreenQuad(GraphicsDevice);
            var width = GraphicsDevice.Viewport.Width;
            var height = GraphicsDevice.Viewport.Height;

            BlurHRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            BlurVRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            ColorTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            NormalTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, width * 2, height * 2, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            ShadowMapSize = new Vector2(width * 2, height * 2); 
            DirToCamTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            BloomFilterTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            LightTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            SceneTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }
        public void Unload()
        {
            FullScreenQuad.Dispose();
            BlurHRenderTarget.Dispose();
            BlurVRenderTarget.Dispose();
            ColorTarget.Dispose();
            NormalTarget.Dispose();
            DirToCamTarget.Dispose();
            BloomFilterTarget.Dispose();
            LightTarget.Dispose();
            SceneTarget.Dispose();
        }

        #region drawers

        public int ShowTarget = 0;
        public float SpecularPower = 2.55f;
        public float SpecularIntensity = 0.5f;

        void DrawSceneMRT(DrawType dt)
        {
            var CameraView = Game.SelectedCamera.View;
            var CameraProjection = Game.SelectedCamera.Projection;
            var CameraPosition = Game.SelectedCamera.Position;
            MasterMRT.Parameters["ApplyLightEffect"]?.SetValue(0f);
            MasterMRT.Parameters["InvertViewProjection"]?.SetValue(Matrix.Invert(CameraView * CameraProjection));
            MasterMRT.Parameters["CameraPosition"].SetValue(CameraPosition);
            MasterMRT.Parameters["LightDirection"].SetValue(Game.LightCamera.FrontDirection);
            MasterMRT.Parameters["SpecularIntensity"].SetValue(SpecularIntensity);
            MasterMRT.Parameters["SpecularPower"].SetValue(SpecularPower);

            if (dt == DrawType.Regular)
            {

                MasterMRT.CurrentTechnique = ETMRTskybox;
                SkyBox.Draw(CameraView, CameraProjection, CameraPosition);
            }
            if(dt == DrawType.DepthMap)
                MasterMRT.CurrentTechnique = ETMRTshadowmap;
            
            MasterMRT.Parameters["ApplyLightEffect"]?.SetValue(1f);
            if (Game.GameState.Equals(TGCGame.GmState.Running) ||
                Game.GameState.Equals(TGCGame.GmState.Paused) ||
                Game.GameState.Equals(TGCGame.GmState.Defeat))
            {
                
                
                if (showXwing)
                    DrawXWingMRT(Game.Xwing, dt);
                foreach (var enemy in tiesToDraw)
                    DrawTieMRT(enemy);
                foreach (var ship in shipsToDraw)
                {
                    if (ship.Allied)
                        DrawXwingMRT(ship);
                    else
                        DrawTieMRT(ship);
                }
                if (dt == DrawType.Regular)
                {
                    MasterMRT.CurrentTechnique = ETMRTbasicColor;
                    EPMRTaddToBloomFilter.SetValue(1f);
                    MasterMRT.Parameters["ApplyLightEffect"]?.SetValue(0f);
                    foreach (var laser in lasersToDraw)
                        DrawModelMRT(LaserModel, laser.SRT, laser.Color);
                    
                    EPMRTaddToBloomFilter.SetValue(0f);
                    MasterMRT.Parameters["ApplyLightEffect"]?.SetValue(1f);
                    EPMRTcolor.SetValue(new Vector3(0.5f, 0.5f, 0.5f));

                    MasterMRT.CurrentTechnique = MasterMRT.Techniques["TrenchDraw"];
                }
                
                DrawMapMRT();
            }
            
            
        }
        public float modEpsilon = 0.000041200182749889791011810302734375f;
        public float maxEpsilon = 0.02f;
        public bool debugShadowMap = false;
        public void DrawMRT()
        {
            /* Draw Shadow Map*/
            MasterMRT.CurrentTechnique = MasterMRT.Techniques["DepthPass"];
            MasterMRT.Parameters["modulatedEpsilon"].SetValue(modEpsilon);
            MasterMRT.Parameters["maxEpsilon"].SetValue(maxEpsilon);

            GraphicsDevice.SetRenderTarget(ShadowMapRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            DrawSceneMRT(DrawType.DepthMap);

            /* Draw Scene with MRT and apply shadows*/
            GraphicsDevice.SetRenderTargets(ColorTarget, NormalTarget, DirToCamTarget, BloomFilterTarget);

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.BlendState = BlendState.Opaque;

            MasterMRT.Parameters["ShadowMap"].SetValue(ShadowMapRenderTarget);
            MasterMRT.Parameters["ShadowMapSize"]?.SetValue(ShadowMapSize);
            EPMRTlightViewProjection.SetValue(Game.LightCamera.View * Game.LightCamera.Projection);

            DrawSceneMRT(DrawType.Regular);

            if (ShowTarget == 0)
            {

                /* Calculate lights */
                GraphicsDevice.SetRenderTarget(LightTarget);

                MasterMRT.CurrentTechnique = MasterMRT.Techniques["DirectionalLight"];
                MasterMRT.Parameters["ColorMap"].SetValue(ColorTarget);
                MasterMRT.Parameters["DirToCamMap"].SetValue(DirToCamTarget);
                MasterMRT.Parameters["NormalMap"].SetValue(NormalTarget);
                MasterMRT.Parameters["BloomFilter"].SetValue(BloomFilterTarget);
                MasterMRT.Parameters["LightColor"].SetValue(new Vector3(1f, 1f, 1f));
                MasterMRT.Parameters["AmbientLightColor"].SetValue(new Vector3(0.98f, 0.9f, 1f));
                MasterMRT.Parameters["AmbientLightIntensity"].SetValue(0.3f);


                FullScreenQuad.Draw(MasterMRT);

                GraphicsDevice.SetRenderTarget(SceneTarget);
                MasterMRT.CurrentTechnique = MasterMRT.Techniques["IntegrateLight"];
                MasterMRT.Parameters["LightMap"].SetValue(LightTarget);
                FullScreenQuad.Draw(MasterMRT);

                /* - Bloom - */

                EffectBlur.CurrentTechnique = EffectBlur.Techniques["MRTtech"];
                EPblurScreenSize.SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

                /* use MRT to blur horizontally and vertically at the same time*/
                GraphicsDevice.SetRenderTargets(BlurHRenderTarget, BlurVRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                EPblurTexture.SetValue(BloomFilterTarget);

                FullScreenQuad.Draw(EffectBlur);

                /* integrate */
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);

                EffectBloom.CurrentTechnique = EffectBloom.Techniques["Integrate"];
                EPbloomTexture.SetValue(SceneTarget);
                EPbloomBlurHTexture.SetValue(BlurHRenderTarget);
                EPbloomBlurVTexture.SetValue(BlurVRenderTarget);

                FullScreenQuad.Draw(EffectBloom);

                if (debugShadowMap)
                {
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(ShadowMapRenderTarget,
                               new Vector2(0, 250), null, Color.White, 0f, Vector2.Zero, 0.10f, SpriteEffects.None, 0);
                    SpriteBatch.End();
                }
            }
            else if (ShowTarget >= 2)
            {

                GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin();

                if (ShowTarget == 2)
                    SpriteBatch.Draw(ColorTarget, Vector2.Zero, Color.White);
                else if (ShowTarget == 3)
                    SpriteBatch.Draw(NormalTarget, Vector2.Zero, Color.White);
                else if (ShowTarget == 4)
                    SpriteBatch.Draw(LightTarget, Vector2.Zero, Color.White);
                else if (ShowTarget == 5)
                    SpriteBatch.Draw(BloomFilterTarget, Vector2.Zero, Color.White);
                else if (ShowTarget == 6)
                    SpriteBatch.Draw(DirToCamTarget, Vector2.Zero, Color.White);

                //else if (ShowTarget == 6)
                //    SpriteBatch.Draw(DepthTarget,
                //           Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                else if (ShowTarget == 7)
                    SpriteBatch.Draw(ShadowMapRenderTarget,
                           Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);


                SpriteBatch.End();
            }
            //DrawScene(DrawType.Regular);

        }
        
        #region MRTelements
        void DrawMapMRT()
        {
            foreach (var t in trenchesToDraw)
                DrawTrenchMRT(t);
        }
        void DrawTrenchMRT(Trench t)
        {
            
            Matrix world;
            
            foreach (var mesh in t.Model.Meshes)
            {
                world = mesh.ParentBone.Transform * t.SRT;
                EPMRTcolor.SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                EPMRTworld.SetValue(world);
                EPMRTworldViewProjection.SetValue(world * Game.SelectedCamera.View * Game.SelectedCamera.Projection);
                //EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                mesh.Draw();

            }
            foreach (var turret in t.Turrets)
            {
                foreach (var mesh in TrenchTurret.Meshes)
                {
                    world = mesh.ParentBone.Transform * turret.SRT;
                    EPMRTcolor.SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                    EPMRTworld.SetValue(world);
                    EPMRTworldViewProjection.SetValue(world * Game.SelectedCamera.View * Game.SelectedCamera.Projection);
                    //EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                    EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                    mesh.Draw();
                }
            }

        }
        
        void DrawXwingMRT(Ship xwing)
        {
            Matrix world;
            int meshCount = 0;
            foreach (var mesh in XwingModel.Meshes)
            {
                world = mesh.ParentBone.Transform * xwing.SRT;

                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                //var itw = Matrix.Transpose(Matrix.Invert(xwing.World));

                EPMRTtexture.SetValue(XwingTextures[meshCount]);
                MasterMRT.Parameters["ModelNormal"].SetValue(XwingNormalTex[meshCount]);
                EPMRTworld.SetValue(world);
                EPMRTworldViewProjection.SetValue(wvp);
                //EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                meshCount++;

                mesh.Draw();
            }
        }

        void DrawXWingMRT(Xwing xwing, DrawType dt)
        {
            
            int meshCount = 0;
            if (dt == DrawType.Regular)
                MasterMRT.CurrentTechnique = ETMRTbasicColor; // remove for light post proc.
            
            MasterMRT.Parameters["ApplyLightEffect"]?.SetValue(0f);
            EPMRTaddToBloomFilter.SetValue(1f);
            DrawModelMRT(XwingEnginesModel, xwing.EnginesSRT, xwing.EnginesColor);
            EPMRTaddToBloomFilter.SetValue(0f);
            MasterMRT.Parameters["ApplyLightEffect"]?.SetValue(1f);

            if (dt == DrawType.Regular)
                MasterMRT.CurrentTechnique = ETMRTtextured;

            foreach (var mesh in XwingModel.Meshes)
            {
                xwing.World = mesh.ParentBone.Transform * xwing.SRT;

                var wvp = xwing.World * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                //var itw = Matrix.Transpose(Matrix.Invert(xwing.World));

                EPMRTtexture.SetValue(XwingTextures[meshCount]);
                MasterMRT.Parameters["ModelNormal"].SetValue(XwingNormalTex[meshCount]);
                EPMRTworld.SetValue(xwing.World);
                EPMRTworldViewProjection.SetValue(wvp);
                //EPMRTlightViewProjection.SetValue(xwing.World * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(xwing.World)));
                meshCount++;

                mesh.Draw();
            }
        }

        void DrawTieMRT(Ship tie)
        {
            Matrix world;
            MasterMRT.Parameters["ModelNormal"].SetValue(TieNormalTex);
            foreach (var mesh in TieModel.Meshes)
            {
                world = mesh.ParentBone.Transform * tie.SRT;

                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                EPMRTworld.SetValue(world);
                EPMRTtexture.SetValue(TieTexture);
                EPMRTworldViewProjection.SetValue(wvp);
                //EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                mesh.Draw();
            }
        }
        void DrawTieMRT(TieFighter tie)
        {
            Matrix world;
            MasterMRT.Parameters["ModelNormal"].SetValue(TieNormalTex);
            foreach (var mesh in TieModel.Meshes)
            {
                world = mesh.ParentBone.Transform * tie.SRT;

                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                EPMRTworld.SetValue(world);
                EPMRTtexture.SetValue(TieTexture);
                EPMRTworldViewProjection.SetValue(wvp);
                //EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                //EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                mesh.Draw();
            }
        }
        void DrawModelMRT(Model model, Matrix SRT, Vector3 color)
        {
            
            foreach (var mesh in model.Meshes)
            {
                var world = mesh.ParentBone.Transform * SRT;
                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;

                EPMRTworld.SetValue(world);
                EPMRTcolor.SetValue(color);
                //MasterMRT.Parameters["World"].SetValue(world);
                //EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTworldViewProjection.SetValue(wvp);
                EPMRTinverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                mesh.Draw();
            }

        }

        #endregion
        #endregion

        void assignEffectToModel(Model model, Effect effect)
        {
            foreach (var mesh in model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
        }
        void assignEffectToModels(Model[] models, Effect effect)
        {
            foreach (Model model in models)
                assignEffectToModel(model, effect);
        }

        #region effectParameters
        EffectParameter EPbloomWorldViewProjection;
        EffectParameter EPbloomTexture;
        EffectParameter EPbloomBlurHTexture;
        EffectParameter EPbloomBlurVTexture;

        EffectParameter EPblurScreenSize;
        EffectParameter EPblurTexture;

        EffectParameter EPMRTworld;
        EffectParameter EPMRTworldViewProjection;
        EffectParameter EPMRTinverseTransposeWorld;
        EffectParameter EPMRTaddToBloomFilter;
        EffectParameter EPMRTcolor;
        EffectParameter EPMRTtexture;
        EffectParameter EPMRTlightViewProjection;

        EffectTechnique ETMRTtextured;
        EffectTechnique ETMRTbasicColor;
        EffectTechnique ETMRTskybox;
        EffectTechnique ETMRTshadowmap;
        void manageEffectParameters()
        {
            MasterMRT.Parameters["SkyBoxTexture"].SetValue(skyBoxTexture);
            
            EPbloomWorldViewProjection = EffectBloom.Parameters["WorldViewProjection"];
            EPbloomTexture = EffectBloom.Parameters["baseTexture"];
            EPbloomBlurHTexture = EffectBloom.Parameters["blurHTexture"];
            EPbloomBlurVTexture = EffectBloom.Parameters["blurVTexture"];

            EPblurTexture = EffectBlur.Parameters["baseTexture"];
            EPblurScreenSize = EffectBlur.Parameters["screenSize"];
           
            EPMRTworld = MasterMRT.Parameters["World"];
            EPMRTworldViewProjection = MasterMRT.Parameters["WorldViewProjection"];
            EPMRTtexture = MasterMRT.Parameters["Texture"];
            EPMRTaddToBloomFilter = MasterMRT.Parameters["AddToFilter"];
            EPMRTcolor = MasterMRT.Parameters["Color"];
            EPMRTlightViewProjection = MasterMRT.Parameters["LightViewProjection"];
            EPMRTinverseTransposeWorld = MasterMRT.Parameters["InverseTransposeWorld"];
            ETMRTbasicColor = MasterMRT.Techniques["BasicColorDraw"];
            ETMRTtextured = MasterMRT.Techniques["TexturedDraw"];
            ETMRTskybox = MasterMRT.Techniques["Skybox"];
            ETMRTshadowmap = MasterMRT.Techniques["DepthPass"];

        }
        #endregion
    }
}