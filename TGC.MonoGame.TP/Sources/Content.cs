﻿using BepuPhysics.Collidables;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP
{
    internal class Content
    {
        private readonly ContentManager contentManager;

        private const string EffectsFolder = "Effects/";
        private const string ModelsFolder = "Models/";
        private const string TexturesFolder = "Textures/";
        private const string SoundsFolder = "Sounds/";
        //private const string MusicFolder = "Music/";
        //private const string SpriteFontsFolder = "SpriteFonts/";

        internal readonly Effect E_BasicShader;
        internal readonly Model M_XWing, M_TIE, M_Trench_Plain, M_Trench_Line, M_Trench_Corner, M_Trench_T, M_Trench_Cross, M_Trench_End, M_Trench2;
        internal readonly TypedIndex Sh_Sphere20, SH_XWing, Sh_Trench_Plain;
        internal readonly Texture2D[] T_DeathStar, T_XWing, T_TIE, T_Trench, T_Trench2;
        internal readonly SoundEffect S_Explotion;

        internal Content(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            // Efects
            E_BasicShader = LoadEffect("BasicShader");

            // Models
            M_XWing = LoadModel("XWing/XWing", E_BasicShader);
            M_TIE = LoadModel("TIE/TIE", E_BasicShader);
            M_Trench_Plain = LoadModel("DeathStar/Trench_Plain", E_BasicShader);
            M_Trench_Line = LoadModel("DeathStar/Trench_Line", E_BasicShader);
            M_Trench_Corner = LoadModel("DeathStar/Trench_Corner", E_BasicShader);
            M_Trench_T = LoadModel("DeathStar/Trench_T", E_BasicShader);
            M_Trench_Cross = LoadModel("DeathStar/Trench_Cross", E_BasicShader);
            M_Trench_End = LoadModel("DeathStar/Trench_End", E_BasicShader);
            M_Trench2 = LoadModel("DeathStar/Trench2", E_BasicShader);

            // Convex Hulls

            // Shapes
            Sh_Sphere20 = LoadShape(new Sphere(20f));
            SH_XWing = LoadConvexHull("XWing/XWing");
            Sh_Trench_Plain = LoadShape(new Box(DeathStar.trenchSize, 20f, DeathStar.trenchSize));

            // Textures
            T_DeathStar = new Texture2D[] { LoadTexture("DeathStar/DeathStar") };
            T_TIE = new Texture2D[] { LoadTexture("TIE/TIE_IN_DIFF") };
            T_XWing = new Texture2D[] {
                LoadTexture("XWing/lambert6_Base_Color"),
                LoadTexture("XWing/lambert5_Base_Color")
            };
            T_Trench = new Texture2D[] { LoadTexture("DeathStar/DeathStar") };
            T_Trench2 = Enumerable.Repeat(LoadTexture("DeathStar/DeathStar"), 27).ToArray();

            // Sounds
            S_Explotion = LoadSound("Explotion");
        }

        private Effect LoadEffect(string name) => contentManager.Load<Effect>(EffectsFolder + name);
        private Model LoadModel(string name, Effect effect)
        {
            Model model = contentManager.Load<Model>(ModelsFolder + name);
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            return model;
        }
        private TypedIndex LoadShape<S>(S shape) where S : unmanaged, IShape => TGCGame.physicSimulation.LoadShape(shape);
        private TypedIndex LoadConvexHull(string name) => LoadShape(ConvexHullGenerator.Generate(contentManager.Load<Model>(ModelsFolder + name)));
        private Texture2D LoadTexture(string name) => contentManager.Load<Texture2D>(TexturesFolder + name);
        private SoundEffect LoadSound(string name) => contentManager.Load<SoundEffect>(SoundsFolder + name);
    }
}