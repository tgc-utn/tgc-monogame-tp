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
                    FlyAround(body); // Cambiar a que se acerque a XWing
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
                CurrentState = State.SEEKING;
            }

        }

        private void Flee(BodyReference body)
        {

            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            while (distanceToXWing(body) < 2000f)
            {
                body.Velocity.Linear = (forward * FastVelocity).ToBEPU(); // Debe ser mayor a la de XWing
            }

            _360Turn(body);

            forward = PhysicUtils.Forward(rotation);
            body.Velocity.Linear = (forward * ResetVelocity).ToBEPU(); 

            
        }

        private float distanceToXWing(BodyReference body)
        {
            return Vector3.Distance(body.Pose.Position.ToVector3(), XWing.getInstance().XWingPosition().ToVector3()); 
        }

        private void FlyAround(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * StandarVelocity).ToBEPU();
            body.Velocity.Angular = new Vector3(0f, Regulator, 0).ToBEPU();

            Regulator *= 0.998f;
        }

        private void GetInFront(BodyReference body)
        {
            while(!XWingInFront(body))
            {
                Quaternion rotation = body.Pose.Orientation.ToQuaternion();
                Vector3 forward = PhysicUtils.Forward(rotation);
                Vector3 backwards = -PhysicUtils.Forward(rotation);

                _360Turn(body);

                int timing = 0;

                while(timing < 5) // Debe pasar nave para volver a girar
                {
                    body.Velocity.Linear = (forward * FastVelocity).ToBEPU();
                }

                _360Turn(body);
            }
        }

        private void ShootXWing(BodyReference body)
        {
            // Post lasers
        }

        private bool XWingInFront(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation); // Puede estar completamente mal, despues revisar
            return (XWing.getInstance().XWingForward().X > 0  && (-forward).X < 0) || (XWing.getInstance().XWingForward().X <= 0 && (-forward).X >= 0) && Math.Abs((-forward).X) > Math.Abs(XWing.getInstance().XWingForward().X); 
        }

        private bool XWingInSight(BodyReference body)
        {
            return distanceToXWing(body) < 1000f;
        }

        private void _360Turn(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);
            Vector3 backwards = -PhysicUtils.Forward(rotation);

            while (backwards != forward)
            {
                body.Velocity.Angular = new Vector3(0f, 0.8f, 0).ToBEPU();
                backwards = -PhysicUtils.Forward(rotation);
            }

            body.Velocity.Angular = new Vector3(0f, 0f, 0).ToBEPU();
        }
    }


}