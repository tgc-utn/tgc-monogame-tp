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

namespace TGC.MonoGame.TP.Geometries {

    public class RampPrimitive : CustomPrimitive {
        
        public RampPrimitive(GraphicsDevice graphicsDevice, ContentManager content, float size, Color color) {

            Color = color;

            Vector3[] vertexList =
            {
                new Vector3(-1f, 1f, -1f),
                new Vector3(1f, 1f, -1f),
                new Vector3(-1f, -1f, -1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(-1f, -1f, 1),
                new Vector3(1f, -1f, 1f)
            };

            // front
            AddTriangle(vertexList[0], vertexList[1], vertexList[2], size, color);
            AddTriangle(vertexList[3], vertexList[2], vertexList[1], size, color);

            // bottom
            AddTriangle(vertexList[2], vertexList[3], vertexList[4], size, color);
            AddTriangle(vertexList[5], vertexList[4], vertexList[3], size, color);

            // slope
            AddTriangle(vertexList[4], vertexList[1], vertexList[0], size, color);
            AddTriangle(vertexList[1], vertexList[4], vertexList[5], size, color);

            // sides
            AddTriangle(vertexList[0], vertexList[2], vertexList[4], size, color);
            AddTriangle(vertexList[5], vertexList[3], vertexList[1], size, color);

            InitializePrimitive(graphicsDevice, content);
        }

        private void AddTriangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, float size, Color color)
        {
            Vector3 normal = Vector3.Cross(vertex1 - vertex2, vertex1 - vertex3);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddVertex(vertex1 * size / 2, color, normal);
            AddVertex(vertex2 * size / 2, color, normal);
            AddVertex(vertex3 * size / 2, color, normal);
        }
    }
}