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

    public class LightningPrimitive : CustomPrimitive {
        
        public LightningPrimitive(GraphicsDevice graphicsDevice, float size, Color color) {

            Vector3[] vertexList =
            {
                new Vector3(0.2f, 1f, 0.2f),
                new Vector3(-0.5f, -0.1f, 0.2f),
                new Vector3(-0.02f, -0.1f, 0.2f),
                new Vector3(-0.2f, -1f, 0.2f),
                new Vector3(0.5f, 0.1f, 0.2f),
                new Vector3(0.02f, 0.1f, 0.2f),
                new Vector3(0.2f, 1f, -0.2f),
                new Vector3(-0.5f, -0.1f, -0.2f),
                new Vector3(-0.02f, -0.1f, -0.2f),
                new Vector3(-0.2f, -1f, -0.2f),
                new Vector3(0.5f, 0.1f, -0.2f),
                new Vector3(0.02f, 0.1f, -0.2f)
            };

            // top - front/back
            AddTriangle(vertexList[0], vertexList[1], vertexList[2], size, color);
            AddTriangle(vertexList[6], vertexList[8], vertexList[7], size, color);
            // top - sides
            AddTriangle(vertexList[6], vertexList[7], vertexList[0], size, color);
            AddTriangle(vertexList[1], vertexList[0], vertexList[7], size, color);
            AddTriangle(vertexList[7], vertexList[8], vertexList[1], size, color);
            AddTriangle(vertexList[2], vertexList[1], vertexList[8], size, color);
            AddTriangle(vertexList[11], vertexList[6], vertexList[5], size, color);
            AddTriangle(vertexList[0], vertexList[5], vertexList[6], size, color);

            // bottom - front/back
            AddTriangle(vertexList[3], vertexList[4], vertexList[5], size, color);
            AddTriangle(vertexList[9], vertexList[11], vertexList[10], size, color);
            // bottom - sides
            AddTriangle(vertexList[9], vertexList[10], vertexList[3], size, color);
            AddTriangle(vertexList[4], vertexList[3], vertexList[10], size, color);
            AddTriangle(vertexList[10], vertexList[11], vertexList[4], size, color);
            AddTriangle(vertexList[5], vertexList[4], vertexList[11], size, color);
            AddTriangle(vertexList[8], vertexList[9], vertexList[2], size, color);
            AddTriangle(vertexList[3], vertexList[2], vertexList[9], size, color);

            InitializePrimitive(graphicsDevice);
        }

    }
}