using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types;

namespace TGC.MonoGame.TP.Utils;

public static class MathFunctions
{
    public static List<Vector3> GetLinearPoints(Repetition repetitions)
    {
        List<Vector3> points = new List<Vector3>();
        FunctionLinear fL = repetitions.FunctionRef as FunctionLinear;
        for (int i = 0; i < repetitions.Repetitions; i++)
        {
            float t = i / (float)(repetitions.Repetitions - 1);
            float x = MathHelper.Lerp(fL.StartX, fL.EndX, t);
            float z = MathHelper.Lerp(fL.StartZ, fL.EndZ, t);
            points.Add(new Vector3(x, fL.Y, z));
        }

        return points;
    }

    public static List<Vector3> GetSinusoidalPoints(Repetition repetitions)
    {
        List<Vector3> points = new List<Vector3>();
        FunctionSinusoidal fS = repetitions.FunctionRef as FunctionSinusoidal;
        for (int i = 0; i < repetitions.Repetitions; i++)
        {
            float t = i / (float)(repetitions.Repetitions - 1);

            float sen = fS.Amplitude *
                        (float)Math.Sin(2 * Math.PI * fS.Periods * t);
            if (fS.UseX)
            {
                float mov = MathHelper.Lerp(fS.StartZ, fS.EndZ, t);
                points.Add(new Vector3(sen + fS.StartX, fS.Y, mov));
            }
            else
            {
                float mov = MathHelper.Lerp(fS.StartX, fS.EndX, t);
                points.Add(new Vector3(mov, fS.Y, sen + fS.StartZ));
            }
        }

        return points;
    }

    public static List<Vector3> GetCircularPoints(Repetition repetitions)
    {
        List<Vector3> points = new List<Vector3>();
        FunctionCircular fC = repetitions.FunctionRef as FunctionCircular;
        for (int i = 0; i < repetitions.Repetitions; i++)
        {
            float angle = MathHelper.TwoPi * i / repetitions.Repetitions;
            Vector3 point = new Vector3(fC.CenterX, fC.Y, fC.CenterZ) +
                            fC.Radius * new Vector3((float)Math.Cos(angle), 0f, (float)Math.Sin(angle));
            points.Add(point);
        }

        return points;
    }
    
    public static List<Vector3> MirrorPointss(List<Vector3> Points, Vector3 axis)
    {
        List<Vector3> mirroredVectors = new List<Vector3>();

        foreach (Vector3 vector in Points)
        {
            Vector3 mirroredVector = vector - 2 * Vector3.Dot(vector, axis) * axis;
            mirroredVectors.Add(mirroredVector);
        }

        return mirroredVectors;
    }
}