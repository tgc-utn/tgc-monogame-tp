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
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.TP.Geometries {

    public class RampPrimitive : CustomPrimitive {
        
        public RampPrimitive(GraphicsDevice graphicsDevice, float size, Color color) {

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

            InitializePrimitive(graphicsDevice);
        }

    }
}