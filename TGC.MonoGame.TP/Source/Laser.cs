using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace TGC.MonoGame.TP
{
	public class Laser
	{
		public static List<Laser> EnemyLasers = new List<Laser>();
		public static List<Laser> AlliedLasers = new List<Laser>();

		public Matrix SRT { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 FrontDirection { get; set; }
		public Vector3 Color;
		BoundingCylinder BoundingCylinder;
		public float Age = 0f;
		public float MaxAge = 1f;
		public Laser(Vector3 pos, Matrix rotation, Matrix srt, Vector3 fd, Vector3 c)
		{
			Position = pos;
			SRT = srt;
			FrontDirection = fd;
			Color = c;

			BoundingCylinder = new BoundingCylinder(Position, 10f, 20f);
			BoundingCylinder.Rotation = rotation;
		}
		public void Update(float time)
		{
			var translation = FrontDirection * 1500f * time;
			Age += 1f * time;
			//var translation = FrontDirection * 150f * time;

			Position += translation;


			BoundingCylinder.Move(translation);
			SRT *= Matrix.CreateTranslation(translation);
		}
		public bool Hit(BoundingSphere sphere)
		{
			return BoundingCylinder.Intersects(sphere);
		}
		public static void UpdateAll(float time)
		{
			EnemyLasers.ForEach(laser => laser.Update(time));
			AlliedLasers.ForEach(laser => laser.Update(time));


		}

	}
}