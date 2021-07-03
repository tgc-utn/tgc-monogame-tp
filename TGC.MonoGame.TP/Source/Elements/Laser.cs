using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
namespace TGC.MonoGame.TP
{
	public class Laser
	{
		public static List<Laser> EnemyLasers = new List<Laser>();
		public static List<Laser> AlliedLasers = new List<Laser>();
		public static int MapSize;
		public static float MapLimit;
		public static float BlockSize;

		public static Vector3[] OmniLightsPos;
		public static Vector3[] OmniLightsColor;
		public static int OmniLightsCount;

		public Vector2 CurrentBlock;
		public Matrix SRT { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 FrontDirection { get; set; }
		public Vector3 Color;
		public OrientedBoundingBox BoundingBox;
		//BoundingCylinder BoundingCylinder;
		public float Age = 0f;
		public float MaxAge = 1f;
		
		public bool NotVisible = false;

		TGCGame Game;
		public Laser(Vector3 pos, Matrix rotation, Matrix srt, Vector3 fd, Vector3 c)
		{
			Position = pos;
			SRT = srt;
			FrontDirection = fd;
			Color = c;

			Game = TGCGame.Instance;
			//BoundingCylinder = new BoundingCylinder(Position, 10f, 20f);
			//BoundingCylinder.Rotation = rotation;

			var tempAABB = BoundingVolumesExtensions.CreateAABBFrom(Game.Drawer.LaserModel);
			tempAABB = BoundingVolumesExtensions.Scale(tempAABB, 0.3f);
			BoundingBox = OrientedBoundingBox.FromAABB(tempAABB);
			BoundingBox.Center = pos;
			BoundingBox.Orientation = rotation;
		}
		public void Update(float time, Vector4 zone)
		{
            var translation = FrontDirection * 1500f * time;

            Age += 1f * time;
            //var translation = FrontDirection * 150f * time;

            Position += translation;


			//BoundingCylinder.Move(translation);

			BoundingBox.Center = Position;
			SRT *= Matrix.CreateTranslation(translation);
			UpdateVisibility(zone);
		}
		public bool Hit(BoundingSphere sphere)
		{
			//return BoundingCylinder.Intersects(sphere);
			return BoundingBox.Intersects(sphere);
		}
		public bool Hit(BoundingBox box)
		{
			return BoundingBox.Intersects(box);

			//var inter = BoundingCylinder.Intersects(box);
			//return inter.GetType().Equals(BoxCylinderIntersection.Intersecting);
		}
		public bool Hit(OrientedBoundingBox box)
        {
			return BoundingBox.Intersects(box);
        }
		void UpdateVisibility(Vector4 zone)
        {
			CurrentBlock = new Vector2(
				(int)((Position.X / BlockSize) + 0.5), (int)((Position.Z / BlockSize) + 0.5));

			CurrentBlock.X = MathHelper.Clamp(CurrentBlock.X, 0, MapSize - 1);
			CurrentBlock.Y = MathHelper.Clamp(CurrentBlock.Y, 0, MapSize - 1);


            NotVisible = 
				CurrentBlock.X < zone.X ||
				CurrentBlock.X > zone.Y ||
				CurrentBlock.Y < zone.Z ||
				CurrentBlock.Y > zone.W ;

		}
		public static void UpdateAll(float time, Xwing xwing)
		{
			Vector4 zone = xwing.GetZone();

			EnemyLasers.ForEach(laser => laser.Update(time, zone));
			AlliedLasers.ForEach(laser => laser.Update(time, zone));

			EnemyLasers.RemoveAll(laser => laser.NotVisible);
			AlliedLasers.RemoveAll(laser => laser.NotVisible);

			//Debug.WriteLine("AL " + AlliedLasers.Count + " EL " + EnemyLasers.Count);
		}
		static List<Vector3> lasersPos = new List<Vector3>();
		static List<Vector3> lasersColors = new List<Vector3>();

		static void verifyAndAddLaser(Laser laser, ref BoundingFrustum frustum, ref List<Laser> drawList)
        {
			if (laser.BoundingBox.Intersects(frustum))
			{
				drawList.Add(laser);
				lasersPos.Add(laser.Position);
				lasersColors.Add(laser.Color);
			}
		}
		public static void AddAllRequiredtoDraw(ref List<Laser> drawList, ref BoundingFrustum frustum)
        {
			lasersColors.Clear();
			lasersPos.Clear();
			foreach (var l in EnemyLasers)
				verifyAndAddLaser(l, ref frustum, ref drawList);
			foreach (var l in AlliedLasers)
				verifyAndAddLaser(l, ref frustum, ref drawList);

			OmniLightsPos = lasersPos.ToArray();
			OmniLightsColor = lasersColors.ToArray();
			OmniLightsCount = lasersPos.Count;
		}
	}
}