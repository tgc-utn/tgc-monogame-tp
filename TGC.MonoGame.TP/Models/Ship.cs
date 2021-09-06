using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.Samples.Cameras;


namespace TGC.MonoGame.TP
{
    class Ship
    {

        private string folder { get; set; }
        private Matrix World { get; set; }
        private Matrix Scale { get; set; }
        private Vector3 Position { get; set; }
        private Model Model { get; set; }

        public Ship() {
            World = Matrix.Identity;
            Position = Vector3.UnitX * 30f;
            Scale = Matrix.CreateScale(0.05f);
        }

        public void LoadContent(ContentManager content, string folder)
        {
            this.folder = folder;
            Model = content.Load<Model>(this.folder);
            World = Scale* Matrix.CreateTranslation(Position);
        }


        public void LoadContent(ContentManager content, string folder, float position)
        {
            this.folder = folder;
            Model = content.Load<Model>(this.folder);
            Position = Vector3.UnitX * position;
            World = Scale * Matrix.CreateTranslation(Position);
        }
        public void LoadContent(ContentManager content, string folder, float position, float scale)
        {
            this.folder = folder;
            Model = content.Load<Model>(this.folder);
            Position = Vector3.UnitX * position;
            Scale = Matrix.CreateScale(scale);
            World = Scale* Matrix.CreateTranslation(Position);
        }

        public void LoadContent(ContentManager content, string folder, Vector3 position, float scale)
        {
            this.folder = folder;
            Model = content.Load<Model>(this.folder);
            Position = position;
            Scale = Matrix.CreateScale(scale);
            World = Scale * Matrix.CreateTranslation(Position);
        }

        public void Draw(Camera camera)
        {
            Model.Draw(World, camera.View, camera.Projection);
        }
    }
}
