using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        int Health = 100;

        float Regulator = 20f;

        float StandarVelocity = 100f;
        float FastVelocity = 300f;
        float ResetVelocity = 1000f;

        internal override void Update(double elapsedTime)
        {
            StateMachine();
        }

        protected internal void StateMachine() // Podria ser clase con manejo dentro de la misma
        {
            BodyReference body = Body();

            if (CurrentState == State.SEEKING)
            {
                if (XWingInSight(body))
                {
                    CurrentState = State.ATTACKING;

                    if (XWingInFront(body))
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
                            GetInFront(body);
                        }
                    }
                }
                else
                {
                    GetCloseToXWing(body); // Cambiar a que se acerque a XWing
                }
            }

            else if (CurrentState == State.ATTACKING)
            {
                if (XWingInFront(body))
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
                        GetInFront(body);
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
            return DistanceToXWing(body) > 1000f;
        }

        private void Flee(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * FastVelocity).ToBEPU();
        }

        private float DistanceToXWing(BodyReference body)
        {
            return Vector3.Distance(body.Pose.Position.ToVector3(), XWing.getInstance().XWingPosition().ToVector3());
        }

        private void GetCloseToXWing(BodyReference body) // Comportamiento extraño
        {
            Vector3 forward = XWing.getInstance().XWingPosition().ToVector3();

            body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
            body.Velocity.Angular = new Vector3(0f, Regulator, 0).ToBEPU();

            Regulator *= 0.998f;
        }

        private void GetInFront(BodyReference body)
        {
            Turn180FromXWing(body);
        }

        private void ShootXWing(BodyReference body)
        {
            // Post lasers
        }

        private bool XWingInFront(BodyReference body)
        {
            /* Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation); // Puede estar completamente mal, despues revisar
            return (XWing.getInstance().XWingForward().X > 0  && (-forward).X < 0) || (XWing.getInstance().XWingForward().X <= 0 && (-forward).X >= 0) && Math.Abs((-forward).X) > Math.Abs(XWing.getInstance().XWingForward().X); 
            */

            return false;
        }

        private bool XWingInSight(BodyReference body)
        {
            return DistanceToXWing(body) < 1000f;
        }

        private void Turn180FromXWing(BodyReference body) // No funca, ver como saber si ven a lados opuestos
        {
            Quaternion turn = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(turn);
            Vector3 backward = -PhysicUtils.Forward(turn);
            float dsdf = XWing.getInstance().XWingForward().X;

            if (XWing.getInstance().XWingForward().X != backward.X) 
            {
                Quaternion change = body.Pose.Orientation.ToQuaternion();
                forward = PhysicUtils.Forward(change);
                body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
                body.Velocity.Angular = (new Vector3(0f, 2f, 0f)).ToBEPU();
            }
            else
            {
                body.Velocity.Linear = (forward * 0).ToBEPU();
                body.Velocity.Angular = (new Vector3(0f, 0f, 0f)).ToBEPU();
            }
        }
    }
}