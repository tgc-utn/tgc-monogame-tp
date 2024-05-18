
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.Scenes;

public class Collider : IComponent
{

    private Transform Transform;

    private readonly Vector3 HalfWidths;


    public Collider(Transform transform, Vector3 halfWidths)
    {
        Transform = transform;
        HalfWidths = halfWidths;
    }

    private Vector3 _forward
    {
        get => Transform.GetWorld().Forward;
    }

    private Vector3 _right
    {
        get => Transform.GetWorld().Right;
    }

    private Vector3 _up
    {
        get => Transform.GetWorld().Up;
    }


    private List<Vector3> _vertices
    {
        get => new List<Vector3>(){
            {_forward * HalfWidths.X + _up  * HalfWidths.Y + _right * HalfWidths.Z},
            {_forward  * HalfWidths.X + _up* HalfWidths.Y - _right* HalfWidths.Z},
            {_forward * HalfWidths.X - _up* HalfWidths.Y + _right* HalfWidths.Z},
            {_forward * HalfWidths.X - _up* HalfWidths.Y - _right* HalfWidths.Z},
            {-_forward  * HalfWidths.X+ _up* HalfWidths.Y+ _right* HalfWidths.Z},
            {-_forward  * HalfWidths.X+ _up* HalfWidths.Y - _right* HalfWidths.Z},
            {-_forward * HalfWidths.X - _up* HalfWidths.Y + _right* HalfWidths.Z},
            {-_forward  * HalfWidths.X- _up* HalfWidths.Y - _right* HalfWidths.Z}
        }.Select(v => v + Transform.Pos).ToList();
    }


    public void Destroy(Entity self, Scene scene) { }

    public void Initialize(Entity self, Scene scene) { }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene){}

    public List<Vector3> VerticesContainedIn(Collider c)
    {
        List<Vector3> l = new List<Vector3>();
        
        foreach (var vert in _vertices)
        {
            Vector3 p = Vector3.Transform(vert, Matrix.Invert(c.Transform.GetWorld()));
            if (Math.Abs(p.X) <= c.HalfWidths.X && Math.Abs(p.Y) <= c.HalfWidths.Y && Math.Abs(p.Z) <= c.HalfWidths.Z)
            {
                l.Add(vert);
            }
        }

        return l;
    }

    public (Vector3, float)[] GetFaceNormals(){
        return new List<(Vector3, float)>{ (_forward, HalfWidths.X), (_up, HalfWidths.Y), (_right, HalfWidths.Z), (-_forward, HalfWidths.X), (-_up, HalfWidths.Y), (-_right, HalfWidths.Z)}.ToArray();
    }


    public static CollisionResult Collide(Collider A, Collider B)
    {

        Vector3[] aAxes = { B._forward, B._right, B._up };
        Vector3[] bAxes = { A._forward, A._right, A._up };

        Vector3[] axes = {
            B._forward, B._right, B._up,
            A._forward, A._right, A._up,
            Vector3.Cross(aAxes[0], bAxes[0]),
            Vector3.Cross(aAxes[0], bAxes[1]),
            Vector3.Cross(aAxes[0], bAxes[2]),
            Vector3.Cross(aAxes[1], bAxes[0]),
            Vector3.Cross(aAxes[1], bAxes[1]),
            Vector3.Cross(aAxes[1], bAxes[2]),
            Vector3.Cross(aAxes[2], bAxes[0]),
            Vector3.Cross(aAxes[2], bAxes[1]),
            Vector3.Cross(aAxes[2], bAxes[2])
            };

        List<Vector3> aVerts = B._vertices;
        List<Vector3> bVerts = A._vertices;

        Vector3 separatingAxis = Vector3.Zero;
        float minSeparation = float.MaxValue;

        foreach (var axis in axes)
        {
            float bProjMin = float.MaxValue;
            float bProjMax = float.MinValue;
            float aProjMin = float.MaxValue;
            float aProjMax = float.MinValue;

            if (axis.Length() == 0) continue;

            foreach (var vert in bVerts)
            {
                float val = Vector3.Dot(vert, axis);
                if (val < bProjMin)
                {
                    bProjMin = val;
                }
                if (val > bProjMax)
                {
                    bProjMax = val;
                }
            }

            foreach (var vert in aVerts)
            {
                float val = Vector3.Dot(vert, axis);
                if (val < aProjMin)
                {
                    aProjMin = val;
                }
                if (val > aProjMax)
                {
                    aProjMax = val;
                }
            }

            float overlap = FindOverlap(aProjMin, aProjMax, bProjMin, bProjMax);

            if (overlap <= 0) return new CollisionResult(false,Vector3.Zero,-1);

            if (overlap <= minSeparation){
                minSeparation = overlap;
                separatingAxis = axis;
            }
        }

        return new CollisionResult(true,separatingAxis,minSeparation);
    }

    private static float FindOverlap(float astart, float aend, float bstart, float bend)
    {
        if (astart < bstart)
        {
            if (aend < bstart)
            {
                return 0f;
            }

            return aend - bstart;
        }

        if (bend < astart)
        {
            return 0f;
        }

        return bend - astart;
    }

}

public struct CollisionResult {

    public bool Collides;
    public Vector3 MinSeparatingAxis;
    public float MinSeparation;

    public CollisionResult(bool collides, Vector3 minSeparatingAxis, float minSeparation){
        Collides = collides;
        MinSeparatingAxis = minSeparatingAxis;
        MinSeparation = minSeparation;
    }

}

