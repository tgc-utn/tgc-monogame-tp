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
        protected float SlowVelocity = 75f;
        protected float StandarVelocity = 120f;
        protected float FastVelocity = 300f;

        internal override void Update()
        {
            StateMachine();
        }

        protected internal void StateMachine(double )
        {
            BodyReference body = Body();

            if (CurrentState == State.SEEKING)
            {
                if (XWingInSight(body))
                {
                    GetCloseToXWingSlowly(body);

                    if (CloseToXWing(body))
                    {
                        CurrentState = State.ATTACKING;
                        ShootXWing(body);
                    }
                    else
                    {
                        if (Health < 40)
                        {
                            CurrentState = State.FLEEING;
                            Flee(body);
                        }
                        else
                        {
                            CurrentState = State.SEEKING;
                            GetCloseToXWingSlowly(body);
                        }
                    }
                }
                else
                {
                    CurrentState = State.SEEKING;
                    GetCloseToXWing(body); 
                }
            }

            else if (CurrentState == State.ATTACKING)
            {
                if (CloseToXWing(body))
                {
                    ShootXWing(body);
                }
                else
                {
                    if (Health < 40)
                    {
                        CurrentState = State.FLEEING;
                        Flee(body);
                    }
                    else
                    {
                        CurrentState = State.SEEKING;
                        GetCloseToXWingSlowly(body);
                    }
                }
            }

            else if (CurrentState == State.FLEEING)
            {
                Flee(body);

                if (FleeSuccess(body))
                {
                    CurrentState = State.SEEKING;
                }
            }
        }

        private bool FleeSuccess(BodyReference body)
        {
            return DistanceToXWing(body) > 100f;
        }

        private void Flee(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * FastVelocity).ToBEPU();
        }

        private float DistanceToXWing(BodyReference body) 
        {
            Vector3 TIEPosition = body.Pose.Position.ToVector3();
            Vector3 XWingPosition = XWing.getInstance().XWingPosition().ToVector3();

            Vector3 DistanceVector = TIEPosition - XWingPosition;
            DistanceVector.Y = 0f;

            return DistanceVector.Length();
        }

        private void GetCloseToXWing(BodyReference body) 
        {
            Vector3 XWingDirection = (XWing.getInstance().XWingPosition() - body.Pose.Position).ToVector3();
            XWingDirection.Y = 0;
            Quaternion RotationToXWing = new Quaternion(XWingDirection, 1f);
            Quaternion FinalRotation = Quaternion.Lerp(RotationToXWing, body.Pose.Orientation.ToQuaternion(), 3f);
            body.Pose.Orientation = FinalRotation.ToBEPU();

            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
        }

        private void GetCloseToXWingSlowly(BodyReference body)
        {
            Vector3 XWingDirection = (XWing.getInstance().XWingPosition() - body.Pose.Position).ToVector3();
            XWingDirection.Y = 0;
            Quaternion RotationToXWing = new Quaternion(XWingDirection, 1f);
            Quaternion FinalRotation = Quaternion.Lerp(RotationToXWing, body.Pose.Orientation.ToQuaternion(), 3f);
            body.Pose.Orientation = FinalRotation.ToBEPU();

            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * SlowVelocity).ToBEPU();
        }

        private bool CloseToXWing(BodyReference body)
        {
            return DistanceToXWing(body) < 50f;
        }

        private void ShootXWing(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * SlowVelocity).ToBEPU();
            // Post lasers
        }

        private bool XWingInSight(BodyReference body)
        {
            return DistanceToXWing(body) < 250f;
        }
    }
}