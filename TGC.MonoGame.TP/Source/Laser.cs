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

		public Vector2 CurrentBlock;
		public Matrix SRT { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 FrontDirection { get; set; }
		public Vector3 Color;
		BoundingCylinder BoundingCylinder;
		public float Age = 0f;
		public float MaxAge = 1f;
		
		public bool NotVisible = false;
		public Laser(Vector3 pos, Matrix rotation, Matrix srt, Vector3 fd, Vector3 c)
		{
			Position = pos;
			SRT = srt;
			FrontDirection = fd;
			Color = c;

			BoundingCylinder = new BoundingCylinder(Position, 10f, 20f);
			BoundingCylinder.Rotation = rotation;
		}
		public void Update(float time, Vector4 zone)
		{
			var translation = FrontDirection * 1500f * time;
			Age += 1f * time;
			//var translation = FrontDirection * 150f * time;

			Position += translation;


			BoundingCylinder.Move(translation);
			SRT *= Matrix.CreateTranslation(translation);
			UpdateVisibility(zone);
		}
		public bool Hit(BoundingSphere sphere)
		{
			return BoundingCylinder.Intersects(sphere);
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

	}
}