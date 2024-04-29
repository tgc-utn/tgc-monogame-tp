#region File Description

//-----------------------------------------------------------------------------
// Terrain.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;

#endregion Using Statements

namespace TGC.MonoGame.TP.Environment {
    /// <summary>
    ///     Basic primitive to build flat terrain using squares
    /// </summary>
    public class Terrain {

        private GraphicsDevice GraphicsDevice;

        private Vector3 FloorNormal = Vector3.Down;
        private SquarePrimitive Square;
        private List<Matrix> SquareMatrices;

        /// <summary>
        ///     Constructs a new terrain as a matrix of squares, with the specified square size, color, terrain width & length
        /// </summary>
        
        public Terrain(GraphicsDevice graphicsDevice, float squareSize, Color color, int width, int length) {
            new Terrain(graphicsDevice, squareSize, color, width, length, Vector3.Zero);
        }
        
        public Terrain(GraphicsDevice graphicsDevice, float squareSize, Color color, int width, int length, Vector3 position) {

            GraphicsDevice = graphicsDevice;
            Square = new SquarePrimitive(GraphicsDevice, squareSize, color, FloorNormal);

            // Create a list of places where the squares will be drawn
            SquareMatrices = new List<Matrix>() {
                Matrix.CreateTranslation(position)
            };
            
            for (int w = 0; w < width; w++) {
                for (int l = 0; l < length; l++) {
                    if (width != 0 || length != 0) {
                        SquareMatrices.Add(Matrix.CreateTranslation((Vector3.Forward * l  + Vector3.Right * w) * squareSize + position));
                    }
                }   
            }
            
        }

        public void Draw(Matrix view, Matrix projection) {
            
            foreach (var squareWorld in SquareMatrices) {

                var squareEffect = Square.Effect;
                squareEffect.World = squareWorld;
                squareEffect.View = view;
                squareEffect.Projection = projection;
                squareEffect.LightingEnabled = false;
                Square.Draw(squareEffect);
            }
        }
    }
}