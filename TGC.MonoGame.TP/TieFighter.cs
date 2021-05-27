using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.MonoGame.TP
{
	public class TieFighter
	{
		public bool drawn = false;
		public Vector3 Position { get; set; }
		public Vector3 FrontDirection { get; set; }
		public Matrix World { get; set; }
		public Matrix SRT { get; set; }
		float Time;
		public Model Model { get; set; }
		public float TieScale { get; set; }

		public int HP = 100;
		public float Yaw, Pitch;
		public List<Laser> fired = new List<Laser>();
		BoundingSphere boundingSphere;
		public TieFighter(Vector3 pos, Vector3 front, Matrix w, Matrix srt, float s)
		{
			Position = pos;
			FrontDirection = front;
			World = w;
			SRT = srt;
			TieScale = s;
			boundingSphere = new BoundingSphere(Position, 30f);
		}
		float betweenFire = 0f;
		float fireRate = 0.025f;

		//float angleBetweenVectors(Vector3 a, Vector3 b)
		//{
		//	var cross = Vector3.Cross(a, b);
		//	return MathF.Asin(cross.Length() / (a.Length() * b.Length()));
		//}
		float angleBetweenVectors(Vector3 a, Vector3 b)
		{
			return MathF.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
		}
		public void Update(Xwing xwing, float time)
		{

			Time = time;
			FrontDirection = Vector3.Normalize(xwing.Position - Position);
			updateDirectionVectors();
			if (Vector3.Distance(xwing.Position, Position) > 100)
				Position += FrontDirection * 50f * time;
			SRT =
				Matrix.CreateScale(TieScale) *
				Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f) *
				Matrix.CreateTranslation(Position);
			updateFireRate();
			boundingSphere.Center = Position;

		}
		public void updateFireRate()
		{
			betweenFire += fireRate * 30f * Time;
		}
		public void fireLaser()
		{
			//System.Diagnostics.Debug.WriteLine(Time + " " + betweenFire);
			if (betweenFire < 1)
				return;

			betweenFire = 0;
			Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
			Matrix SRT =
				Matrix.CreateScale(new Vector3(0.07f, 0.07f, 0.4f)) *
				rotation *
				Matrix.CreateTranslation(Position);
			fired.Add(new Laser(Position, rotation, SRT, FrontDirection, new Vector3(0.8f, 0f, 0f)));
		}
		public float angleToX, angleToZ, y;
		public void updateDirectionVectors()
		{
			//angleToX = angleBetweenVectors(FrontDirection, Vector3.Right);
			//angleToZ = angleBetweenVectors(FrontDirection, Vector3.Backward);

			//if (angleToZ < MathHelper.PiOver2)
			//	y = MathHelper.TwoPi - angleToX;
			//else
			//	y = angleToX;
			//Yaw = MathHelper.ToDegrees(y + MathHelper.PiOver2);

			//Pitch = MathHelper.ToDegrees(angleBetweenVectors(FrontDirection, Vector3.Up)) + 90;
			//Pitch = MathHelper.ToDegrees(MathF.Asin(FrontDirection.Y));

			//System.Diagnostics.Debug.WriteLine(yaw + " " + pitch); 
			Yaw = MathF.Atan2(FrontDirection.X, FrontDirection.Z);
			Pitch = MathF.Asin(FrontDirection.Y);

		}
		public void VerifyCollisions(List<Laser> playerLasers)
		{
			Laser hitBy = playerLasers.Find(laser => laser.Hit(boundingSphere));
			if (hitBy != null)
			{
				HP -= 50;
				playerLasers.Remove(hitBy);
			}
		}
	}
}