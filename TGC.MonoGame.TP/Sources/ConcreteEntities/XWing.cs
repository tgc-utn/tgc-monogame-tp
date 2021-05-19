using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TGC.MonoGame.TP.Entities;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class XWing : DynamicEntity
    {
        protected override Model Model() => TGCGame.content.M_XWing;
        protected override Texture2D[] Textures() => TGCGame.content.T_XWing;

        protected override Vector3 Scale => Vector3.One;
        protected override TypedIndex Shape => TGCGame.content.SH_XWing;
        protected override float Mass => 100f;

        static public XWing INSTANCE = new XWing(); // Singleton

        static public XWing getInstance() // Singleton
        {
            return INSTANCE;
        }

        public System.Numerics.Vector3 XWingPosition()
        {
            BodyReference body = Body();
            return body.Pose.Position;
        }

        public Vector3 XWingForward()
        {
            BodyReference body = Body();
            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            return PhysicUtils.Forward(rotation);
        }


        public override bool HandleCollition(ICollitionHandler other)
        {
            TGCGame.content.S_Explotion.CreateInstance().Play();
            return false;
        }
    }
}