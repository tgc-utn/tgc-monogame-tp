﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {



        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private Model Model { get; set; }
        private Effect Effect { get; set; }

        private Camera camera;
        private Entity entity;
        private Entity entity2;

        protected override void Initialize()
        {
            ContentRepository.SetUpInstance(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {

            camera = new Camera(new Vector3(1000, 1000, 1000), GraphicsDevice.Viewport.AspectRatio);

            entity = new Entity(
                Quaternion.Identity,
                Vector3.Zero,
                new Renderable(
                    new TextureShader(ContentRepository.GetInstance().GetTexture("TankWars/T90/textures_mod/hullA")),
                    ContentRepository.GetInstance().GetModel("TankWars/Panzer/panzer"))
                    );



            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            entity.Update();
            camera.Follow(entity);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            entity.Draw(camera);
        }

        protected override void UnloadContent()
        {
            Content.Unload();

            base.UnloadContent();
        }
    }
}