using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Maps
{
    enum WallId 
    {
        Floor,
        Right,
        Left,
        Front,
        Back,
        Ceiling
    }
    class Wall : IDrawable
    {
        static Func<Vector3, Vector3> SideWallTrans = (v) => new Vector3(v.Y, v.X, v.Z);
        static Func<Vector3, Vector3> FrontWallTrans = (v) => new Vector3(v.X, v.Z, v.Y);
        static Func<Vector3, Vector3> FloorTrans = (v) => v;
        protected VertexBuffer VertexBuffer;
        protected IndexBuffer IndexBuffer; 
        protected Vector3 BottomRight, BottomLeft, UpperRight, UpperLeft; 
        public (float, float) TextureRepeat { protected get; set; }
        protected bool Reverse;
        public BasicEffect Effect { protected get; set; }
        protected Wall(Vector2 size, Vector3 center, Func<Vector3, Vector3> trans = null, bool reserve = false, (float, float)? textureRepeat = null)
        {
            Reverse = reserve;
            TextureRepeat = textureRepeat.GetValueOrDefault((1,1));
            CalculateVertex(center, size, trans ?? Wall.FloorTrans);
        }
        /// The left parameter is only necessary if you plan on using back-culling
        /// else it doesn't make a difference
        public static Wall CreateSideWall(Vector2 size, Vector3 center, bool left = false, (float, float)? textureRepeat = null)
        {
            center = SideWallTrans(center);
            return new Wall(size, center, SideWallTrans, left, textureRepeat);
        }
        /// The back parameter is only necessary if you plan on using back-culling
        /// else it doesn't make a difference
        public static Wall CreateFrontWall(Vector2 size, Vector3 center, bool back = false, (float, float)? textureRepeat = null)
        {
            center = FrontWallTrans(center);
            return new Wall(size, center, FrontWallTrans, back, textureRepeat);
        }
        /// The ceiling parameter is only necessary if you plan on using back-culling
        /// else it doesn't make a difference
        public static Wall CreateFloor(Vector2 size, Vector3 center, bool ceiling = false, (float, float)? textureRepeat = null)
        {
            return new Wall(size, center, FloorTrans, !ceiling, textureRepeat);
        }

        public override void Initialize(TGCGame game)
        {
            VertexBuffer = CreateVertexBuffer(game, TextureRepeat);
            IndexBuffer = CreateIndexBuffer(game.GraphicsDevice);
            base.Initialize(game);
        }
        public override void Draw(GameTime gameTime)
        {
            Effect.World = Matrix.Identity;
            Effect.View = Game.Camera.View;
            Effect.Projection = Game.Camera.Projection;

            Game.GraphicsDevice.SetVertexBuffer(VertexBuffer);
            Game.GraphicsDevice.Indices = IndexBuffer;
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 6 / 3);
            }
        }
        public bool Collides(Vector3 lowerPoint, Vector3 higherPoint)
        {
            if(higherPoint.X < BottomLeft.X || UpperRight.X < lowerPoint.X) return false;
            if(higherPoint.Y < BottomLeft.Y || UpperRight.Y < lowerPoint.Y) return false;
            if(higherPoint.Z < BottomLeft.Z || UpperRight.Z < lowerPoint.Z) return false;
            return true;
        }

        private void CalculateVertex(Vector3 center, Vector2 size, Func<Vector3, Vector3> trans)
        {
            var x = size.X / 2;
            var z = size.Y / 2;

            BottomLeft = trans(new Vector3(-x + center.X, center.Y, -z + center.Z));
            UpperLeft = trans(new Vector3(-x + center.X, center.Y, z + center.Z));
            UpperRight = trans(new Vector3(x + center.X, center.Y, z + center.Z));
            BottomRight = trans(new Vector3(x + center.X, center.Y, -z + center.Z));
        }
        private VertexBuffer CreateVertexBuffer(TGCGame game, (float, float) textureRepeat)
        {
            

            var cubeVertices = new VertexPositionTexture[4];
            // Bottom-Left Front.
            cubeVertices[0].Position = BottomLeft;
            cubeVertices[0].TextureCoordinate = Vector2.Zero;
            // Bottom-Left Back.
            cubeVertices[1].Position = UpperLeft;
            cubeVertices[1].TextureCoordinate = new Vector2(0, textureRepeat.Item2);
            // Bottom-Right Back.
            cubeVertices[2].Position = UpperRight;
            cubeVertices[2].TextureCoordinate = new Vector2(textureRepeat.Item1, textureRepeat.Item2);
            // Bottom-Right Front.
            cubeVertices[3].Position = BottomRight;
            cubeVertices[3].TextureCoordinate = new Vector2(textureRepeat.Item1, 0);

            VertexBuffer Vertices = new VertexBuffer(game.GraphicsDevice, VertexPositionTexture.VertexDeclaration, 4,
                BufferUsage.WriteOnly);
            Vertices.SetData(cubeVertices);
            return Vertices;
        }

        private IndexBuffer CreateIndexBuffer(GraphicsDevice device)
        {
            var cubeIndices = new ushort[6];

            cubeIndices[0] = 0;
            cubeIndices[1] = 1;
            cubeIndices[2] = 3;

            cubeIndices[3] = 1;
            cubeIndices[4] = 2;
            cubeIndices[5] = 3;

            if(Reverse)
                Array.Reverse(cubeIndices);
            IndexBuffer Indices = new IndexBuffer(device, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            Indices.SetData(cubeIndices);
            return Indices;
        }
    }
}