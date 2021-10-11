using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionRuleManagement;

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
        public Effect TextureEffect { get; set; }
        public int monedas = 0;
        private List<Moneda> monedasList;

        public Monedas(ContentManager content, Space space, CollisionGroup marbleGroup)
        {
            //cargo moneda
            Moneda = content.Load<Model>(ContentFolder3D + "Marbel/Moneda/Coin");
            TextureEffect = content.Load<Effect>(ContentFolderEffects + "TextureShader");
            CoinTexture = content.Load<Texture2D>(ContentFolderTextures + "Coin");
            //mesh moneda
            foreach (var mesh in Moneda.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;

            var monedaGroup = new CollisionGroup();

            var monedaMarbleCollisionGroup = new CollisionGroupPair(monedaGroup, marbleGroup);
            var monedaMonedaCollisionGroup = new CollisionGroupPair(monedaGroup, monedaGroup);

            CollisionRules.CollisionGroupRules.Add(monedaMarbleCollisionGroup, CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(monedaMonedaCollisionGroup, CollisionRule.NoBroadPhase);

            monedasList = new List<Moneda>
            {
                new Moneda(new Vector3(10f, 10f, 0f), space, monedaGroup, this),
                new Moneda(new Vector3(25f, -14f, 0f), space, monedaGroup, this),
                new Moneda(new Vector3(37f, -5f, 0f), space, monedaGroup, this),
                new Moneda(new Vector3(53f, -10f, 0f), space, monedaGroup, this),
                new Moneda(new Vector3(63f, -10f, 0f), space, monedaGroup, this),
                new Moneda(new Vector3(50f, -13f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(55f, -14f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(45f, -16f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(40f, -14f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(35f, -14f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(27.5f, -12.5f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(22.5f, -12.5f, 110f), space, monedaGroup, this),
                new Moneda(new Vector3(4f, -12f, 115f), space, monedaGroup, this),
                new Moneda(new Vector3(4f, -8f, 115f), space, monedaGroup, this),
                new Moneda(new Vector3(4f, -5f, 115f), space, monedaGroup, this),
                new Moneda(new Vector3(7f, -12f, 107.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-17.5f, -12f, 92.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-22.5f, -12f, 87.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-27.5f, -7f, 85f), space, monedaGroup, this),
                new Moneda(new Vector3(-27.5f, -2f, 85f), space, monedaGroup, this),
                new Moneda(new Vector3(-27.5f, 1f, 85f), space, monedaGroup, this),
                new Moneda(new Vector3(-37.5f, -2f, 82.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-37.5f, 2f, 82.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-42.5f, 0f, 80f), space, monedaGroup, this),
                new Moneda(new Vector3(-45f, -3f, 80f), space, monedaGroup, this),
                new Moneda(new Vector3(-48f, -6f, 78f), space, monedaGroup, this),
                new Moneda(new Vector3(-47.5f, -12.5f, 78f), space, monedaGroup, this),
                new Moneda(new Vector3(-52.5f, -2.5f, 75f), space, monedaGroup, this),
                new Moneda(new Vector3(-52.5f, 0f, 75f), space, monedaGroup, this),
                new Moneda(new Vector3(-52.5f, 2.5f, 75f), space, monedaGroup, this),
                new Moneda(new Vector3(-57.5f, -2.5f, 77.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-67.5f, 5f, 70f), space, monedaGroup, this),
                new Moneda(new Vector3(-67.5f, 0f, 70f), space, monedaGroup, this),
                new Moneda(new Vector3(-72.5f, 0f, 67.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-77.5f, 0f, 62.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-77.5f, 5f, 62.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-77.5f, 10f, 62.5f), space, monedaGroup, this),
                new Moneda(new Vector3(-87.5f, 15f, 49f), space, monedaGroup, this),
                new Moneda(new Vector3(-87.5f, 15f, 45f), space, monedaGroup, this),
                new Moneda(new Vector3(-87.5f, 15f, 40f), space, monedaGroup, this),
                new Moneda(new Vector3(-87.5f, 15f, 35f), space, monedaGroup, this),
                new Moneda(new Vector3(-43.5f, 20f, 25f), space, monedaGroup, this)
            };
        }
        
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, float totalGameTime, Matrix World)
        {
            foreach(Moneda moneda in monedasList)
            {
                if(moneda.WasObtained())
                {
                    continue;
                }

                foreach (var mesh in Moneda.Meshes)
                {
                    World = mesh.ParentBone.Transform * (Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(totalGameTime) * Matrix.CreateTranslation(moneda.GetPosition(totalGameTime)));
                    TextureEffect.Parameters["Texture"].SetValue(CoinTexture);
                    TextureEffect.Parameters["World"].SetValue(World);
                    mesh.Draw();
                }
            }
        }

        public void MonedaObtained()
        {
            monedas++;
        }

        public int GetMonedasObtained()
        {
            return monedas;
        }
    }

    public class Moneda
    {
        public Box collider;
        private Vector3 position;
        public bool obtained = false;
        private Space space;
        private readonly Monedas monedasController;

        public Moneda(Vector3 position, Space space, CollisionGroup monedaGroup, Monedas monedasController)
        {
            this.space = space;
            this.position = position;
            this.monedasController = monedasController;

            collider = new Box(new BEPUutilities.Vector3(position.X, position.Y, position.Z), 1f, 1f, 1f);
            collider.CollisionInformation.Events.InitialCollisionDetected += HandleMonedaCollectedEvent;
            collider.CollisionInformation.CollisionRules.Group = monedaGroup;

            space.Add(collider);
        }

        private void HandleMonedaCollectedEvent(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            obtained = true;
            monedasController.MonedaObtained();
            
            space.Remove(sender.Entity);
        }

        public Vector3 GetPosition(float totalGameTime)
        {
            return new Vector3(position.X, position.Y + MathF.Cos(totalGameTime * 2), position.Z);
        }

        public bool WasObtained()
        {
            return obtained;
        }
    }
}