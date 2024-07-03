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
using System;
using System.Collections.Generic;

#endregion Using Statements

namespace TGC.MonoGame.TP.Geometries
{

    public class RampPrimitive : CustomPrimitive
    {

        public List<OrientedBoundingBox> BoundingRamps { get; set; }
        private List<Triangle> Triangles { get; set; }

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

            Triangles = new List<Triangle>(); // Inicialización aquí

            BoundingRamps = new List<OrientedBoundingBox>();

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

            // Definir los triángulos usando los vértices de la lista
            for (int i = 0; i < vertexList.Length; i += 3)
            {
                Vector3 v1 = vertexList[i];
                Vector3 v2 = vertexList[i + 1];
                Vector3 v3 = vertexList[i + 2];

                this.AddTriangle(v1, v2, v3);
            }

            World = Matrix.CreateScale(scale ?? Vector3.One) * (rotation ?? Matrix.Identity) * Matrix.CreateTranslation(coordinates ?? Vector3.Zero);

            // Calcular OBB para cada triángulo y almacenarla
            foreach (var triangle in Triangles)
            {
                // Calcular el centro y los lados del triángulo
                Vector3 centroid = (triangle.V1 + triangle.V2 + triangle.V3) / 3f;
                Vector3 side1 = triangle.V2 - triangle.V1;
                Vector3 side2 = triangle.V3 - triangle.V1;

                // Calcular el sistema de coordenadas local para la OBB
                Vector3 localX = Vector3.Normalize(side1);
                Vector3 localY = Vector3.Normalize(Vector3.Cross(localX, side2));
                Vector3 localZ = Vector3.Cross(localX, localY);

                // Crear la matriz de rotación para la OBB
                Matrix rotationMatrix = Matrix.CreateWorld(centroid, localZ, localY);

                // Calcular dimensiones de la OBB (puedes ajustar esto según tus necesidades)
                Vector3 extents = new Vector3(side1.Length() / 2f, side2.Length() / 2f, 0.1f);

                // Crear la OBB y rotarla
                OrientedBoundingBox obb = new OrientedBoundingBox(centroid, extents);
                obb.Rotate(rotationMatrix);

                // Agregar la OBB a la lista de OBBs de triángulos
                BoundingRamps.Add(obb);
            }

            InitializePrimitive(graphicsDevice, content);
        }

        // Función para agregar un triángulo a la rampa
        private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Triangles.Add(new Triangle(v1, v2, v3));
        }

        // Clase interna para representar un triángulo
        private class Triangle
        {
            public Vector3 V1 { get; }
            public Vector3 V2 { get; }
            public Vector3 V3 { get; }

            public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
            {
                V1 = v1;
                V2 = v2;
                V3 = v3;
            }
        }
    }
}