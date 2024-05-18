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

    public class CustomPrimitive : GeometricPrimitive {
      
        protected void AddTriangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, float size, Color color)
        {
            Vector3 normal = Vector3.Cross(vertex1 - vertex2, vertex1 - vertex3);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddVertex(vertex1 * size / 2, color, normal);
            AddVertex(vertex2 * size / 2, color, normal);
            AddVertex(vertex3 * size / 2, color, normal);
        }

        public void Draw(Matrix view, Matrix projection) {
            
            var newEffect = this.Effect;
            newEffect.View = view;
            newEffect.Projection = projection;

            this.Draw(newEffect);
        }
    }
}