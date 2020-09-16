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
        protected float TextureRepeat;
        protected bool Reverse;
        public BasicEffect effect;
        public BasicEffect Effect 
        { 
            protected get { return effect; } 
            set 
            {
                if(effect is null){
                    effect = value;
                }
                else {
                    throw new Exception("The effect for this wall was already set.");
                }

            } 
        }

    
        protected Wall(Vector2 size, Vector3 center, Func<Vector3, Vector3> trans, bool reserve = false, float textureRepeat = 1)
        {
            Size = size;
            Center = center;
            Trans = trans;
            Reverse = reserve;
            TextureRepeat = textureRepeat;
        }
        /// The left parameter is only necessary if you plan on using back-culling
        /// else it doesn't make a difference
        public static Wall CreateSideWall(Vector2 size, Vector3 center, bool left = false, float textureRepeat = 1)
        {
            center = SideWallTrans(center);
            return new Wall(size, center, SideWallTrans, left, textureRepeat);
        }
        /// The back parameter is only necessary if you plan on using back-culling
        /// else it doesn't make a difference
        public static Wall CreateFrontWall(Vector2 size, Vector3 center, bool back = false, float textureRepeat = 1)
        {
            center = FrontWallTrans(center);
            return new Wall(size, center, FrontWallTrans, back, textureRepeat);
        }
        /// The ceiling parameter is only necessary if you plan on using back-culling
        /// else it doesn't make a difference
        public static Wall CreateFloor(Vector2 size, Vector3 center, bool ceiling = false, float textureRepeat = 1)
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
        private VertexBuffer CreateVertexBuffer(TGCGame game, Vector2 size, Vector3 center, Func<Vector3, Vector3> trans, float textureRepeat)
        {
            var x = size.X / 2;
            var z = size.Y / 2;

            var cubeVertices = new VertexPositionTexture[4];
            // Bottom-Left Front.
            cubeVertices[0].Position = trans(new Vector3(-x + center.X, center.Y, -z + center.Z));
            cubeVertices[0].TextureCoordinate = Vector2.Zero * textureRepeat;
            // Bottom-Left Back.
            cubeVertices[1].Position = trans(new Vector3(-x + center.X, center.Y, z + center.Z));
            cubeVertices[1].TextureCoordinate = Vector2.UnitY * textureRepeat;
            // Bottom-Right Back.
            cubeVertices[2].Position = trans(new Vector3(x + center.X, center.Y, z + center.Z));
            cubeVertices[2].TextureCoordinate = Vector2.One * textureRepeat;
            // Bottom-Right Front.
            cubeVertices[3].Position = trans(new Vector3(x + center.X, center.Y, -z + center.Z));
            cubeVertices[3].TextureCoordinate = Vector2.UnitX * textureRepeat;

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