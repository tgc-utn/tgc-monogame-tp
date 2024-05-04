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

    public class CustomCubePrimitive : CustomPrimitive {

        // Esta clase crea un modelo prácticamente igual de al CubePrimitive. 
        // La agrago al proyecto únicamente como referencia para el uso de CustomPrimitive.

        public CustomCubePrimitive(GraphicsDevice graphicsDevice, float size, Color color) {

            Vector3[] vertexList =
            {
                new Vector3(-1f, 1f, -1f),
                new Vector3(1f, 1f, -1f),
                new Vector3(-1f, 1f, 1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(-1f, -1f, -1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(-1f, -1f, 1),
                new Vector3(1f, -1f, 1f)
            };

            // front normal 
            AddTriangle(vertexList[2], vertexList[6], vertexList[3], size, color);
            AddTriangle(vertexList[7], vertexList[3], vertexList[6], size, color);

            // back normal
            AddTriangle(vertexList[0], vertexList[1], vertexList[4], size, color);
            AddTriangle(vertexList[5], vertexList[4], vertexList[1], size, color);

            // right normal
            AddTriangle(vertexList[1], vertexList[3], vertexList[5], size, color);
            AddTriangle(vertexList[7], vertexList[5], vertexList[3], size, color);

            // left normal
            AddTriangle(vertexList[0], vertexList[4], vertexList[2], size, color);
            AddTriangle(vertexList[6], vertexList[2], vertexList[4], size, color);

            // top normal
            AddTriangle(vertexList[0], vertexList[2], vertexList[1], size, color);
            AddTriangle(vertexList[3], vertexList[1], vertexList[2], size, color);

            // bottom normal
            AddTriangle(vertexList[4], vertexList[5], vertexList[6], size, color);
            AddTriangle(vertexList[7], vertexList[6], vertexList[5], size, color);

            InitializePrimitive(graphicsDevice);
        }

    }
}