using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;

namespace WarSteel.Entities;

class RigidBody : Component
{

    private Vector3 _velocity;
    private Vector3 _angularVelocity;

    private float _mass;

    private Matrix _inertiaTensor;

    private Force[] _instForces = Array.Empty<Force>();

    private Force[] _constForces = Array.Empty<Force>();


    public RigidBody(Vector3 velocity, Vector3 angularVelocity, float mass, Matrix inertiaTensor)
    {
        _velocity = velocity;
        _angularVelocity = angularVelocity;
        _mass = mass;
        _inertiaTensor = inertiaTensor;
    }

    public void UpdateEntity(Entity self, GameTime gameTime, Dictionary<string, Entity> others)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector3 torques = calculateTorques();
        Vector3 forces = calculateForces();
        _instForces = Array.Empty<Force>();
        _angularVelocity += Vector3.Transform(torques, Matrix.Invert(_inertiaTensor)) * dt;
        _velocity += forces / _mass * dt;
        self.Transform.Translate(_velocity * dt);
        self.Transform.Orientation += new Quaternion(_angularVelocity * 0.5f * dt, 0) * self.Transform.Orientation;
        self.Transform.Orientation = Quaternion.Multiply(self.Transform.Orientation,1/self.Transform.Orientation.Length());
    }

    private Vector3 calculateTorques()
    {
        Vector3 totalTorque = new Vector3(0, 0, 0);
        foreach (Force f in _instForces)
        {
            totalTorque += Vector3.Cross(f.OriginVector, f.ForceVector);
        }
        foreach (Force f in _constForces)
        {
            totalTorque += Vector3.Cross(f.OriginVector, f.ForceVector);
        }
        return totalTorque;
    }

    private Vector3 calculateForces()
    {
        Vector3 totalForce = new Vector3(0, 0, 0);
        foreach (Force f in _instForces)
        {
            totalForce += f.OriginVector.Length() < float.Epsilon ? f.ForceVector : f.OriginVector / f.OriginVector.Length() * Vector3.Dot(f.OriginVector, f.ForceVector);
        }
        foreach (Force f in _constForces)
        {
            totalForce += f.OriginVector.Length() < float.Epsilon ? f.ForceVector : f.OriginVector / f.OriginVector.Length() * Vector3.Dot(f.OriginVector, f.ForceVector);
        }
        return totalForce;
    }

    public void ApplyConstantForce(Force force)
    {
       _constForces = _constForces.Append(force).ToArray();
    }

    public void ApplyForce(Force force)
    {
        _instForces = _instForces.Append(force).ToArray();
    }


    public string id()
    {
        return "rigidbody";
    }

}

class Force
{

    public Vector3 OriginVector { get; }
    public Vector3 ForceVector { get; }

    public Force(Vector3 origin, Vector3 force)
    {
        OriginVector = origin;
        ForceVector = force;
    }

}
