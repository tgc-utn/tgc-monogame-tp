using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Scenes;

class PhysicsProcessor : ISceneProcessor
{
    public void Draw(Scene scene) { }

    public void Initialize(Scene scene) { }

    public void Update(Scene scene, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        RigidBody[] rigidBodies = scene.GetEntities().FindAll(e => e.HasComponent<RigidBody>()).Select(e => e.GetComponent<RigidBody>()).ToArray();

        foreach (var r in rigidBodies)
        {
            if (r.IsFixed) continue;
            r.ApplyForces(dt);
            r.IntegrateVelocity(dt);
        }

        List<RigidBodyCollisionData> Collisions = GetCollisions(rigidBodies);

        foreach(var coll in Collisions){

            if (!coll.A.IsFixed){
            coll.A.Pos += coll.MinSeparatingAxis * coll.MinSeparation;
            }
        }

    }

    private static List<RigidBodyCollisionData> GetCollisions(RigidBody[] rigidBodies)
    {
        List<RigidBodyCollisionData> collisionDatas = new List<RigidBodyCollisionData>();

        foreach (var i in rigidBodies)
        {

            foreach (var j in rigidBodies.Except(new List<RigidBody>() { i }))
            {

                CollisionResult collResult = Collider.Collide(i.Collider, j.Collider);

                if (collResult.Collides)
                {

                    List<CollisionData> AB = new List<CollisionData>();
                    List<CollisionData> BA = new List<CollisionData>();

                    List<Vector3> IJ = i.Collider.VerticesContainedIn(j.Collider);
                    List<Vector3> JI = j.Collider.VerticesContainedIn(i.Collider);


                    foreach (var colV in IJ)
                    {

                        (Vector3, float)[] axis = j.Collider.GetFaceNormals();

                        Vector3 vel = i.GetVelocityOfPoint(colV);

                        foreach (var (ax, d) in axis)
                        {
                            float T = (Vector3.Dot(colV - j.Pos, ax) - d) / Vector3.Dot(vel, ax);
                            if (T > 0 && float.IsFinite(T))
                            {
                                Vector3 collPoint = colV - T * vel + j.Pos;
                                AB.Add(new CollisionData(ax, collPoint, vel));
                            }
                        }

                    }

                    foreach (var colV in JI)
                    {
                        (Vector3, float)[] axis = i.Collider.GetFaceNormals();
                        Vector3 vel = j.GetVelocityOfPoint(colV);

                        foreach (var (ax, d) in axis)
                        {
                            float T = (Vector3.Dot(colV - i.Pos, ax) - d) / Vector3.Dot(vel, ax);
                            if (T > 0 && float.IsFinite(T))
                            {
                                Vector3 collPoint = colV - T * vel + i.Pos;
                                BA.Add(new CollisionData(ax, collPoint, vel));
                            }
                        }
                    }

                    collisionDatas.Add(new RigidBodyCollisionData(AB.ToArray(), BA.ToArray(), i, j, collResult.MinSeparatingAxis, collResult.MinSeparation));

                }

            }
        }

        return collisionDatas;
    }
}

public struct RigidBodyCollisionData
{

    public CollisionData[] AB;
    public CollisionData[] BA;
    public RigidBody A;
    public RigidBody B;
    public Vector3 MinSeparatingAxis;
    public float MinSeparation;

    public RigidBodyCollisionData(CollisionData[] AB, CollisionData[] BA, RigidBody a, RigidBody b, Vector3 minSeparatingAxis, float minSeparation)
    {
        this.AB = AB;
        this.BA = BA;
        A = a;
        B = b;
        MinSeparatingAxis = minSeparatingAxis;
        MinSeparation = minSeparation;
    }

}

public struct CollisionData
{

    public Vector3 Normal;
    public Vector3 Point;
    public Vector3 Velocity;

    public CollisionData(Vector3 n, Vector3 p, Vector3 v)
    {
        Normal = n;
        Point = p;
        Velocity = v;
    }

}