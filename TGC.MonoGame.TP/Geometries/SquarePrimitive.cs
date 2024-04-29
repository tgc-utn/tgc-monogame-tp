#region File Description

//-----------------------------------------------------------------------------
// SquarePrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.TP.Geometries {
    /// <summary>
    ///     Geometric primitive class for drawing squares.
    /// </summary>
    public class SquarePrimitive : GeometricPrimitive {

        public SquarePrimitive(GraphicsDevice graphicsDevice) : this(graphicsDevice, 1, Color.White, Vector3.Up) {}

        /// <summary>
        ///     Constructs a new square primitive, with the specified size and color.
        /// </summary>
        public SquarePrimitive(GraphicsDevice graphicsDevice, float size, Color color, Vector3 faceNormal) {

            Vector3 normal = faceNormal;

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


            InitializePrimitive(graphicsDevice);
        }
    }
}