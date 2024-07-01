#region File Description

//-----------------------------------------------------------------------------
// CubePrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using BepuPhysics.CollisionDetection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;

#endregion Using Statements

namespace TGC.MonoGame.TP.Geometries
{

    public class RampPrimitive : CustomPrimitive
    {

        public OrientedBoundingBox BoundingRamp { get; set; }

        public RampPrimitive(
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

            World =  Matrix.CreateScale(scale ?? Vector3.One) * (rotation ?? Matrix.Identity) * Matrix.CreateTranslation(coordinates ?? Vector3.Zero);

            if (coordinates.HasValue && scale.HasValue)
            {
                Vector3 regularCoordinates = coordinates.Value; // Convertir Vector3? a Vector3
                Vector3 regularSize = scale.Value * 25;
                // Ahora puedes usar regularVector, que es de tipo Vector3
                //BoundingRamp = new BoundingBox(regularCoordinates - regularSize / 2, regularCoordinates + regularSize / 2);
                BoundingRamp = OrientedBoundingBox.ComputeOBBFromTriangle(vertexList);
                //BoundingRamp = new OrientedBoundingBox(regularCoordinates, regularSize / 2);
            }

            

            InitializePrimitive(graphicsDevice, content);
        }
    }
}