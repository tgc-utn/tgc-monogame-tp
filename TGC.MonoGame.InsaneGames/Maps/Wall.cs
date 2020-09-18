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
        protected Vector2 Size; 
        protected Vector3 Center; 
        protected Func<Vector3, Vector3> Trans;
        public (float, float) TextureRepeat { protected get; set; }
        protected bool Reverse;
        public BasicEffect Effect { protected get; set; }

    
        protected Wall(Vector2 size, Vector3 center, Func<Vector3, Vector3> trans, bool reserve = false, (float, float)? textureRepeat = null)
        {
            Size = size;
            Center = center;
            Trans = trans;
            Reverse = reserve;
            TextureRepeat = textureRepeat.GetValueOrDefault((1,1));
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
            VertexBuffer = CreateVertexBuffer(game, Size, Center, Trans, TextureRepeat);
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
        private VertexBuffer CreateVertexBuffer(TGCGame game, Vector2 size, Vector3 center, Func<Vector3, Vector3> trans, (float, float) textureRepeat)
        {
            var x = size.X / 2;
            var z = size.Y / 2;

            var cubeVertices = new VertexPositionTexture[4];
            // Bottom-Left Front.
            cubeVertices[0].Position = trans(new Vector3(-x + center.X, center.Y, -z + center.Z));
            cubeVertices[0].TextureCoordinate = Vector2.Zero;
            // Bottom-Left Back.
            cubeVertices[1].Position = trans(new Vector3(-x + center.X, center.Y, z + center.Z));
            cubeVertices[1].TextureCoordinate = new Vector2(0, textureRepeat.Item2);
            // Bottom-Right Back.
            cubeVertices[2].Position = trans(new Vector3(x + center.X, center.Y, z + center.Z));
            cubeVertices[2].TextureCoordinate = new Vector2(textureRepeat.Item1, textureRepeat.Item2);
            // Bottom-Right Front.
            cubeVertices[3].Position = trans(new Vector3(x + center.X, center.Y, -z + center.Z));
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