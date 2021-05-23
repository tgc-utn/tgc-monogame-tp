using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Entities;
using TGC.MonoGame.TP.Physics;

enum State { SEEKING, ATTACKING, FLEEING };

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class TIE : DynamicEntity
    {
        protected override Model Model() => TGCGame.content.M_TIE;
        protected override Texture2D[] Textures() => TGCGame.content.T_TIE;

        protected override Vector3 Scale => Vector3.One / 100f;
        protected override TypedIndex Shape => TGCGame.content.Sh_Sphere20;
        protected override float Mass => 100f;

        protected State CurrentState = State.SEEKING;

        protected int Health = 100;
        protected float Regulator = 20f;
        protected float SlowVelocity = 50f;
        protected float StandarVelocity = 100f;
        protected float FastVelocity = 300f;
        protected double PreviousTime = 0;

        internal override void Update(double elapsedTime)
        {
            StateMachine(elapsedTime);
        }

        protected internal void StateMachine(double elapsedTime)
        {
            BodyReference body = Body();

            if (CurrentState == State.SEEKING)
            {
                if (XWingInSight(body, elapsedTime))
                {
                    if (XWingInFront(body, elapsedTime))
                    {
                        CurrentState = State.ATTACKING;
                        ShootXWing(body, elapsedTime);
                    }
                    else
                    {
                        if (Health < 40)
                        {
                            CurrentState = State.FLEEING;
                            Flee(body, elapsedTime);
                        }
                        else
                        {
                            CurrentState = State.SEEKING;
                            GetFarInFront(body, elapsedTime);
                        }
                    }
                }
                else
                {
                    CurrentState = State.SEEKING;
                    GetCloseToXWing(body, elapsedTime); 
                }
            }

            else if (CurrentState == State.ATTACKING)
            {
                if (XWingInFront(body, elapsedTime))
                {
                    ShootXWing(body, elapsedTime);
                }
                else
                {
                    if (Health < 40)
                    {
                        CurrentState = State.FLEEING;
                        Flee(body, elapsedTime);
                    }
                    else
                    {
                        CurrentState = State.SEEKING;
                        GetFarInFront(body, elapsedTime);
                    }
                }
            }

            else if (CurrentState == State.FLEEING)
            {
                Flee(body, elapsedTime);

                if (FleeSuccess(body, elapsedTime))
                {
                    CurrentState = State.SEEKING;
                }
            }
        }

        private bool FleeSuccess(BodyReference body, double elapsedTime)
        {
            return DistanceToXWing(body, elapsedTime) > 1000f;
        }

        private void Flee(BodyReference body, double elapsedTime)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * FastVelocity).ToBEPU();
        }

        private float DistanceToXWing(BodyReference body, double elapsedTime) // Siempre devuelve 0
        {
            Vector3 TIEPosition = body.Pose.Position.ToVector3();
            Vector3 XWingPosition = XWing.getInstance().XWingPosition().ToVector3();
            return Vector3.Distance(TIEPosition, XWingPosition);
        }

        private void GetCloseToXWing(BodyReference body, double elapsedTime) // Comportamiento extraño
        {
            body.Pose.Orientation = -XWing.getInstance().XWingOrientation().ToBEPU();
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
        }

        private void GetFarInFront(BodyReference body, double elapsedTime)
        {
                Turn180(body, elapsedTime);
                Quaternion rotation = body.Pose.Orientation.ToQuaternion();
                Vector3 forward = PhysicUtils.Forward(rotation);

                body.Velocity.Linear = (forward * FastVelocity).ToBEPU();
        }

        private bool XWingInFront(BodyReference body, double elapsedTime)
        {
            return XWingOrientedInversely(body, elapsedTime) && DistanceToXWing(body, elapsedTime) > 300f;
        }

        private void ShootXWing(BodyReference body, double elapsedTime)
        {
            // Post lasers
        }

        private bool XWingOrientedInversely(BodyReference body, double elapsedTime)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);
            float dotProduct = Vector3.Dot(forward, XWing.getInstance().XWingForward());

            return dotProduct < 0;
        }

        private bool XWingInSight(BodyReference body, double elapsedTime)
        {
            return DistanceToXWing(body, elapsedTime) < 500f;
        }

        private void Turn180(BodyReference body, double elapsedTime) // No smooth pero funciona
        {
            Quaternion turn = new Quaternion(0, 1, 0, 0); 
            body.Pose.Orientation = turn.ToBEPU();
            Vector3 forward = PhysicUtils.Forward(turn);

            body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
        }
    }
}