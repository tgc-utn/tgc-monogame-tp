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

        float SlowVelocity = 50f;
        float StandarVelocity = 100f;
        float FastVelocity = 300f;

        internal override void Update(double elapsedTime)
        {
            StateMachine(elapsedTime);
        }

        protected internal void StateMachine(double elapsedTime) // Podria ser clase con manejo dentro de la misma
        {
            BodyReference body = Body();

            if (CurrentState == State.SEEKING)
            {
                if (XWingInSight(body, elapsedTime))
                {
                    CurrentState = State.ATTACKING;

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
                            GetInFront(body, elapsedTime);
                        }
                    }
                }
                else
                {
                    GetCloseToXWing(body, elapsedTime); // Cambiar a que se acerque a XWing
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
                        GetInFront(body, elapsedTime);
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

        private float DistanceToXWing(BodyReference body, double elapsedTime)
        {
            return Vector3.Distance(body.Pose.Position.ToVector3(), XWing.getInstance().XWingPosition().ToVector3());
        }

        private void GetCloseToXWing(BodyReference body, double elapsedTime) // Comportamiento extraño
        {
            Vector3 forward = -XWing.getInstance().XWingPosition().ToVector3();

            body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
            body.Velocity.Angular = new Vector3(0f, Regulator, 0).ToBEPU();

            Regulator *= 0.998f;
        }

        private void GetInFront(BodyReference body, double elapsedTime)
        {
            Turn180FromXWing(body, elapsedTime);
        }

        private void ShootXWing(BodyReference body, double elapsedTime)
        {
            // Post lasers
        }

        private bool XWingInFront(BodyReference body, double elapsedTime)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);
            float dotProduct = Vector3.Dot(forward, XWing.getInstance().XWingForward());

            return dotProduct == (float)(forward.Length() * XWing.getInstance().XWingForward().Length());
        }

        private bool XWingInSight(BodyReference body, double elapsedTime)
        {
            return DistanceToXWing(body, elapsedTime) < 1000f;
        }

        private void Turn180FromXWing(BodyReference body, double elapsedTime) // No funca, ver como saber si ven a lados opuestos
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            if (!XWingInFront(body, elapsedTime))
            {
                body.Velocity.Angular = new Vector3(0f, 0.3f, 0f).ToBEPU();
                // body.Velocity.Linear = (forward * SlowVelocity).ToBEPU();
            }
            else
            {
                body.Velocity.Angular = new Vector3(0f, 0f, 0f).ToBEPU();
                body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
            }

            
        }
    }
}