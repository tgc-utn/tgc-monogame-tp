using System;
using System.Data;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP;
using NumericVector3 = System.Numerics.Vector3;

public class Missile {
    public Matrix World;
    private float velocityThreshold = 30f;
    private float radius = 0.5f;
    private Sphere BulletShape;
    private BodyHandle Handle;
    private bool firstTime;
    private Quaternion rotation;
    private float angleRot=0;

    public bool deleteFlag = false;

    public Missile(Simulation Simulation, CarConvexHull Car){

        if (Car.MachineMissile) { 
            radius = 0.5f;
            angleRot = 90f;
        }
        else { 
            radius = 0.2f;
            angleRot = 0f;
        }
        BulletShape = new Sphere(radius);
        firstTime = true;
        Vector3 forwardLocal = new Vector3(0, 0, -1);
        var forwardWorld = Vector3.Transform(forwardLocal, Car.rotationQuaternion * Car.quaternion);

        var bodyDescription = BodyDescription.CreateConvexDynamic(Car.Pose,
            new BodyVelocity(new NumericVector3(forwardWorld.X, forwardWorld.Y, forwardWorld.Z) * -50),
            BulletShape.Radius * BulletShape.Radius * BulletShape.Radius, Simulation.Shapes, BulletShape);

        Handle = Simulation.Bodies.Add(bodyDescription);
    }

    public void update(Simulation Simulation, Quaternion carQuaternion) {
        var rotationQuaternionX = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(angleRot));
        var rotationQuaternionY = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(180));

        if (!deleteFlag) {
            var body = Simulation.Bodies.GetBodyReference(Handle);
            var pose = body.Pose;
            var position = pose.Position;
            var quaternion = pose.Orientation;

            if (body.Velocity.Linear.LengthSquared() < Math.Pow(velocityThreshold, 2)) {
                Simulation.Bodies.Remove(Handle);
                deleteFlag = true;
            }

            if (firstTime)
            {
                World = Matrix.CreateScale(radius) *
                        Matrix.CreateFromQuaternion(rotationQuaternionX) *
                        Matrix.CreateFromQuaternion(rotationQuaternionY * carQuaternion) *
                        Matrix.CreateTranslation(new Vector3(position.X, position.Y, position.Z));
                rotation = rotationQuaternionY * carQuaternion;

            }
            else
            {
                World = Matrix.CreateScale(radius) *
                                Matrix.CreateFromQuaternion(rotationQuaternionX) *
                                Matrix.CreateFromQuaternion(rotation) *
                                Matrix.CreateTranslation(new Vector3(position.X, position.Y, position.Z));
            }

            firstTime = false;

        } else {
            return;
        }
    }
}
