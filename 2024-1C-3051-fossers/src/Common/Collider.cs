
using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.Managers.Gizmos;

public class Collision
{
    public Entity Entity;

    public Collision(Entity entity)
    {
        Entity = entity;
    }
}

public class Collider
{
    private ColliderShape _colliderShape;

    private CollisionAction _action;


    public ColliderShape ColliderShape
    {
        get => _colliderShape;
    }

    public Collider(ColliderShape colliderShape, CollisionAction action)
    {
        _colliderShape = colliderShape;
        _action = action;
    }

    public void OnCollide(Collision collision)
    {
        _action.ExecuteAction(collision);
    }
}

public interface CollisionAction
{
    public void ExecuteAction(Collision collision);
}

public class NoAction : CollisionAction
{
    public void ExecuteAction(Collision collision)
    {
    }

}


public interface ColliderShape
{
    public abstract IShape GetShape();

    public abstract BodyInertia GetInertia(DynamicBody body);

    public abstract void DrawGizmos(Vector3 position, Gizmos gizmos);
}

public class BoxShape : ColliderShape
{
    private float _height;
    private float _width;
    private float _length;

    public BoxShape(float height, float width, float length)
    {
        _height = height;
        _width = width;
        _length = length;
    }

    public BodyInertia GetInertia(DynamicBody body)
    {
        return ((Box)GetShape()).ComputeInertia(body.Mass);
    }

    public IShape GetShape()
    {
        return new Box(_width,_height,_length);
    }

    public void DrawGizmos(Vector3 position, Gizmos gizmos)
    {
        gizmos.DrawCube(position, new Vector3(_width,_height,_length));
    }



}

public class SphereShape : ColliderShape
{
    private float _radius;


    public SphereShape(float radius)
    {
        _radius = radius;
    }

    public BodyInertia GetInertia(DynamicBody body)
    {
        return ((Sphere)GetShape()).ComputeInertia(body.Mass);
    }

    public IShape GetShape()
    {
        return new Sphere(_radius);
    }

    public void DrawGizmos(Vector3 position, Gizmos gizmos)
    {
        gizmos.DrawSphere(position, new Vector3(_radius, _radius, _radius));
    }



}

public class ConvexShape : ColliderShape
{


    private ConvexHull _hull;
    private Vector3 _center;

    public ConvexShape(Model model)
    {
        List<Vector3> list = new List<Vector3>();

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData(0, vertices, 0, meshPart.NumVertices, meshPart.VertexBuffer.VertexDeclaration.VertexStride);

                foreach (VertexPositionNormalTexture vertex in vertices)
                {
                    Vector3 v = Vector3.Transform(vertex.Position, mesh.ParentBone.Transform);
                    list.Add(v);
                }
            }
        }

        int sizeOfVector3 = System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.Numerics.Vector3));
        int bufferPoolSize = NextPowerOfTwo(list.Count * sizeOfVector3);
        _hull = new ConvexHull(new Span<System.Numerics.Vector3>(list.Select(x => new System.Numerics.Vector3(x.X, x.Y, x.Z)).ToArray()), new BufferPool(bufferPoolSize), out System.Numerics.Vector3 center);
        _center = new Vector3(center.X, center.Y, center.Z);

    }

    private int NextPowerOfTwo(int n)
    {
        if (n < 1)
            return 1;
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        return n + 1;
    }


    public void DrawGizmos(Vector3 position, Gizmos gizmos)
    {
        for (int i = 0; i < _hull.Points.Length; i++)
        {
            Vector3Wide point = _hull.Points[i];
            Vector3 pos = new Vector3(point.X[0], point.Y[0], point.Z[0]);
            gizmos.DrawSphere(position + pos, new Vector3(10, 10, 10));
        }
    }

    public BodyInertia GetInertia(DynamicBody body)
    {
        return ((ConvexHull)GetShape()).ComputeInertia(body.Mass);
    }

    public IShape GetShape()
    {
        return _hull;
    }


}