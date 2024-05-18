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

    public class RupeePrimitive : CustomPrimitive {
        
        public RupeePrimitive(GraphicsDevice graphicsDevice, float size, Color color) {

            Vector3[] vertexList =
            {
                new Vector3(0f, 0.65f, 0.2f),
                new Vector3(-0.3f, 0.35f, 0.2f),
                new Vector3(-0.3f, -0.35f, 0.2f),
                new Vector3(0f, -0.65f, 0.2f),
                new Vector3(0.3f, -0.35f, 0.2f),
                new Vector3(0.3f, 0.35f, 0.2f),
                new Vector3(0f, 1f, 0f),
                new Vector3(-0.5f, 0.5f, 0f),
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3(0f, -1f, 0f),
                new Vector3(0.5f, -0.5f, 0f),
                new Vector3(0.5f, 0.5f, 0f),
                new Vector3(0f, 0.65f, -0.2f),
                new Vector3(-0.3f, 0.35f, -0.2f),
                new Vector3(-0.3f, -0.35f, -0.2f),
                new Vector3(0f, -0.65f, -0.2f),
                new Vector3(0.3f, -0.35f, -0.2f),
                new Vector3(0.3f, 0.35f, -0.2f)
            };

            // front
            AddTriangle(vertexList[0], vertexList[1], vertexList[5], size, color);
            AddTriangle(vertexList[3], vertexList[4], vertexList[2], size, color);
            AddTriangle(vertexList[4], vertexList[5], vertexList[2], size, color);
            AddTriangle(vertexList[1], vertexList[2], vertexList[5], size, color);
            // front to middle 
            AddTriangle(vertexList[6], vertexList[7], vertexList[0], size, color);
            AddTriangle(vertexList[1], vertexList[0], vertexList[7], size, color);
            AddTriangle(vertexList[7], vertexList[8], vertexList[1], size, color);
            AddTriangle(vertexList[2], vertexList[1], vertexList[8], size, color);
            AddTriangle(vertexList[8], vertexList[9], vertexList[2], size, color);
            AddTriangle(vertexList[3], vertexList[2], vertexList[9], size, color);
            AddTriangle(vertexList[9], vertexList[10], vertexList[3], size, color);
            AddTriangle(vertexList[4], vertexList[3], vertexList[10], size, color);
            AddTriangle(vertexList[10], vertexList[11], vertexList[4], size, color);
            AddTriangle(vertexList[5], vertexList[4], vertexList[11], size, color);
            AddTriangle(vertexList[11], vertexList[6], vertexList[5], size, color);
            AddTriangle(vertexList[0], vertexList[5], vertexList[6], size, color);

            // back
            AddTriangle(vertexList[12], vertexList[17], vertexList[13], size, color);
            AddTriangle(vertexList[15], vertexList[14], vertexList[16], size, color);
            AddTriangle(vertexList[16], vertexList[14], vertexList[17], size, color);
            AddTriangle(vertexList[13], vertexList[17], vertexList[14], size, color);
            // back to middle
            AddTriangle(vertexList[6], vertexList[7], vertexList[12], size, color);
            AddTriangle(vertexList[13], vertexList[12], vertexList[7], size, color);
            AddTriangle(vertexList[7], vertexList[8], vertexList[13], size, color);
            AddTriangle(vertexList[14], vertexList[13], vertexList[8], size, color);
            AddTriangle(vertexList[8], vertexList[9], vertexList[14], size, color);
            AddTriangle(vertexList[15], vertexList[14], vertexList[9], size, color);
            AddTriangle(vertexList[9], vertexList[10], vertexList[15], size, color);
            AddTriangle(vertexList[16], vertexList[15], vertexList[10], size, color);
            AddTriangle(vertexList[10], vertexList[11], vertexList[16], size, color);
            AddTriangle(vertexList[17], vertexList[16], vertexList[11], size, color);
            AddTriangle(vertexList[11], vertexList[6], vertexList[17], size, color);
            AddTriangle(vertexList[12], vertexList[17], vertexList[6], size, color);

            InitializePrimitive(graphicsDevice);
        }

    }
}