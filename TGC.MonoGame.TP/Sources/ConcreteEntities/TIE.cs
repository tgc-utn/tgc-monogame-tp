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

        float regulator = 20f;

        float standarVelocity = 100f;
        float fastVelocity = 300f;
        float resetVelocity = 1000f;

        internal override void Update(double elapsedTime)
        {
            stateMachine();
        }

        protected internal void stateMachine() // Podria ser clase con manejo dentro de la misma
        {
            BodyReference body = Body();

            if (CurrentState == State.SEEKING)
            {
                if (XWingInSight(body))
                {
                    CurrentState = State.ATTACKING;

                    if (XWingInFront(body))
                    {
                        shootXWing(body);
                    }
                    else
                    {
                        if (Health < 40)
                        {
                            CurrentState = State.FLEEING;
                            flee(body);
                        }
                        else
                        {
                            getInFront(body);
                        }
                    }
                }
                else
                {
                    flyAround(body); // Cambiar a que se acerque a XWing
                }
            }

            else if (CurrentState == State.ATTACKING)
            {
                if (XWingInFront(body))
                {
                    shootXWing(body);
                }
                else
                {
                    if (Health < 40)
                    {
                        CurrentState = State.FLEEING;
                        flee(body);
                    }
                    else
                    {
                        getInFront(body);
                    }
                }
            }

            else if (CurrentState == State.FLEEING)
            {
                flee(body);
                CurrentState = State.SEEKING;
            }

        }

        private void flee(BodyReference body)
        {

            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            while (distanceToXWing(body) < 2000f)
            {
                body.Velocity.Linear = (forward * fastVelocity).ToBEPU(); // Debe ser mayor a la de XWing
            }

            _360Turn(body);

            forward = PhysicUtils.Forward(rotation);
            body.Velocity.Linear = (forward * resetVelocity).ToBEPU(); 

            
        }

        private float distanceToXWing(BodyReference body)
        {
            return Vector3.Distance(body.Pose.Position.ToVector3(), XWing.getInstance().XWingPosition().ToVector3()); 
        }

        private void flyAround(BodyReference body)
        {
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * standarVelocity).ToBEPU();
            body.Velocity.Angular = new Vector3(0f, regulator, 0).ToBEPU();

            regulator *= 0.998f;
        }

        private void getInFront(BodyReference body)
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
                    body.Velocity.Linear = (forward * fastVelocity).ToBEPU();
                }

                _360Turn(body);
            }
        }

        private void shootXWing(BodyReference body)
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