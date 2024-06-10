using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericVector3 = System.Numerics.Vector3;


namespace TGC.MonoGame.TP.Utils
{

    public static class ConvexHullUtils
    {
        public static List<NumericVector3> QuickHull(List<NumericVector3> points)
        {
            if (points.Count < 3)
                return points;

            // Encuentra los puntos extremos (mínimo y máximo) en el eje X
            NumericVector3 minPoint = points[0];
            NumericVector3 maxPoint = points[0];

            foreach (var point in points)
            {
                if (point.X < minPoint.X)
                    minPoint = point;
                if (point.X > maxPoint.X)
                    maxPoint = point;
            }

            // Divide los puntos en dos grupos
            var leftPoints = new List<NumericVector3>();
            var rightPoints = new List<NumericVector3>();
            foreach (var point in points)
            {

                if (point != minPoint && point != maxPoint)
                {
                    if (point.IsLeftOfLine(minPoint, maxPoint))
                        leftPoints.Add(point);
                    else
                        rightPoints.Add(point);
                }
            }

            // Construye la envolvente convexa recursivamente
            var hull = new List<NumericVector3>();
            BuildHull(hull, leftPoints, minPoint, maxPoint);
            BuildHull(hull, rightPoints, maxPoint, minPoint);

            return hull;
        }

        private static void BuildHull(List<NumericVector3> hull, List<NumericVector3> points, NumericVector3 a, NumericVector3 b)
        {
            var furthestPoint = FindFurthestPoint(points, a, b);
            if (furthestPoint == new NumericVector3(0f,0f,0f) )
                return;
            
            hull.Add(furthestPoint);

            var leftPoints = new List<NumericVector3>();
            var rightPoints = new List<NumericVector3>();
            foreach (var point in points)
            {
                if (point != a && point != b)
                {
                    if (point.IsLeftOfLine(a, furthestPoint))
                        leftPoints.Add(point);
                    else if (point.IsLeftOfLine(furthestPoint, b))
                        rightPoints.Add(point);
                }
            }

            BuildHull(hull, leftPoints, a, furthestPoint);
            BuildHull(hull, rightPoints, furthestPoint, b);
        }

        private static NumericVector3 FindFurthestPoint(List<NumericVector3> points, NumericVector3 a, NumericVector3 b)
        {
            double maxDist = 0;
            NumericVector3 furthestPoint = new NumericVector3();
            foreach (var point in points)
            {
                double dist = point.DistanceToLine(a, b);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    furthestPoint = point;
                }
            }
            return furthestPoint;
        }

        public static bool IsLeftOfLine(this NumericVector3 point, NumericVector3 lineStart, NumericVector3 lineEnd)
        {
            NumericVector3 lineDirection = lineEnd - lineStart;
            NumericVector3 pointDirection = point - lineStart;
            NumericVector3 crossProduct = NumericVector3.Cross(lineDirection, pointDirection);
            return crossProduct.Y > 0; // Assuming Y is the vertical axis
        }

        public static float DistanceToLine(this NumericVector3 point, NumericVector3 lineStart, NumericVector3 lineEnd)
        {
            NumericVector3 lineDirection = lineEnd - lineStart;
            NumericVector3 pointDirection = point - lineStart;
            float lineLength = lineDirection.Length();
            NumericVector3 projection = NumericVector3.Dot(pointDirection, lineDirection) / (lineLength * lineLength) * lineDirection;
            NumericVector3 closestPoint = lineStart + projection;
            return NumericVector3.Distance(point, closestPoint);
        }

        // Convert MonoGame Vector3 to System.Numerics Vector3
        public static List<System.Numerics.Vector3> ConvertVerticesToNumericsVertices(List<Microsoft.Xna.Framework.Vector3> monoGameVertices)
        {
            List<System.Numerics.Vector3> numericsVertices = new List<System.Numerics.Vector3>();

            foreach (var vertex in monoGameVertices)
            {
                numericsVertices.Add(new System.Numerics.Vector3((float)Math.Truncate(vertex.X), (float)Math.Truncate(vertex.Y), (float)Math.Truncate(vertex.Z)));
            }

            return numericsVertices;
        }

        // Convert MonoGame Vector3 to System.Numerics Vector3
        public static List<Vector3> ConvertVerticesToMonoGameVertices(List<NumericVector3> numericVertices)
        {
            List<Vector3> monoGameVertices = new List<Vector3>();

            foreach (var vertex in numericVertices)
            {
                monoGameVertices.Add(new Vector3((float)Math.Truncate(vertex.X), (float)Math.Truncate(vertex.Y), (float)Math.Truncate(vertex.Z)));
            }

            return monoGameVertices;
        }


    }

}
