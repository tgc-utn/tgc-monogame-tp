#region File Description

//-----------------------------------------------------------------------------
// BoxPrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.TP.Geometries
{
    /// <summary>
    ///     Geometric primitive class for drawing cubes.
    /// </summary>
    public class BoxPrimitive : GeometricPrimitive
    {
        public Vector3 Size {get; set;}

        public BoxPrimitive(GraphicsDevice graphicsDevice) : this(graphicsDevice, new Vector3(1f), Color.White, Color.White,
            Color.White, Color.White, Color.White, Color.White)
        {
        }

        public BoxPrimitive(GraphicsDevice graphicsDevice, Vector3 size, Color color) : this(graphicsDevice, size, color,
            color, color, color, color, color)
        {
        }

        /// <summary>
        ///     Constructs a new cube primitive, with the specified size.
        /// </summary>
        public BoxPrimitive(GraphicsDevice graphicsDevice, Vector3 size, Color color1, Color color2, Color color3,
            Color color4, Color color5, Color color6)
        {
            this.Size = size;
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
                -Vector3.UnitZ
            };

            Color[] colors =
            {
                color1, color2, color3, color4, color5, color6
            };

            var i = 0;
            // Create each face in turn.
            foreach (var normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);
                var size1 = Math.Abs(Vector3.Dot(side1, size));
                var size2 = Math.Abs(Vector3.Dot(side2, size));
                var sizeNormal = Math.Abs(Vector3.Dot(normal, size));

                // Six indices (two triangles) per face.
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // Four vertices per face.
                AddVertex((normal * sizeNormal - side1 * size1 - side2 * size2), colors[i], normal);
                AddVertex((normal * sizeNormal - side1 * size1 + side2 * size2), colors[i], normal);
                AddVertex((normal * sizeNormal + side1 * size1 + side2 * size2), colors[i], normal);
                AddVertex((normal * sizeNormal + side1 * size1 - side2 * size2), colors[i], normal);
                // AddVertex((normal - side1 + side2) * localSize / 2, colors[i], normal);
                // AddVertex((normal + side1 + side2) * localSize / 2, colors[i], normal);
                // AddVertex((normal + side1 - side2) * localSize / 2, colors[i], normal);

                i++;
            }

            InitializePrimitive(graphicsDevice);
        }
    }
}