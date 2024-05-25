#region File Description

//-----------------------------------------------------------------------------
// CubePrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.TP.Geometries
{
    /// <summary>
    ///     Geometric primitive class for drawing cubes.
    /// </summary>
    public class CubePrimitive : GeometricPrimitive
    {

        /// <summary>
        ///     Constructs a new cube primitive.
        /// </summary>
        public CubePrimitive(
                GraphicsDevice graphicsDevice,
                ContentManager content,
                Color color,
                float size = 25f,
                Vector3? coordinates = null,
                Vector3? scale = null,
                Matrix? rotation = null
            )
        {

            Color = color;

            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                // front normal
                Vector3.UnitZ,
                // back normal
                -Vector3.UnitZ,
                // right normal
                Vector3.UnitX,
                // left normal
                -Vector3.UnitX,
                // top normal
                Vector3.UnitY,
                // bottom normal
                -Vector3.UnitY
            };

            var i = 0;
            // Create each face in turn.
            foreach (var normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                // Six indices (two triangles) per face.
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // Four vertices per face.
                AddVertex((normal - side1 - side2) * size / 2, color, normal);
                AddVertex((normal - side1 + side2) * size / 2, color, normal);
                AddVertex((normal + side1 + side2) * size / 2, color, normal);
                AddVertex((normal + side1 - side2) * size / 2, color, normal);

                i++;
            }

            World = Matrix.CreateScale(scale ?? Vector3.One) * (rotation ?? Matrix.Identity) * Matrix.CreateTranslation(coordinates ?? Vector3.Zero);

            InitializePrimitive(graphicsDevice, content);
        }
    }
}