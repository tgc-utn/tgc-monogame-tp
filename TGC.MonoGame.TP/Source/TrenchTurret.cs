using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace TGC.MonoGame.TP
{
	public class TrenchTurret
	{
		public Vector3 Position { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public Matrix SRT { get; set; }
		public Matrix S { get; set; }
		public Vector3 FrontDirection { get; set; }
		public float Time;
		public List<Laser> fired = new List<Laser>();

		public BoundingBox BoundingBox;
		public bool needsRemoval = false;
		public TrenchTurret()
		{
			randomFireRate();
		}
		float angleBetweenVectors(Vector3 a, Vector3 b)
		{
			return MathF.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
		}
		public void Update(Xwing xwing, float time)
		{
			Time = time;
			FrontDirection = Vector3.Normalize(xwing.Position - Position);
			updateDirectionVectors();
			SRT = 
				S * 
				Matrix.CreateFromYawPitchRoll(Yaw, 0f, 0f)*
				Matrix.CreateTranslation(Position);
			updateFireRate();
			if(xwing.Position.Y > 0)
				fireLaser();
			verifyColision();
        }
		void verifyColision()
        {
			Laser hitBy = Laser.AlliedLasers.Find(laser => laser.Hit(BoundingBox));
			if(hitBy != null)
            {
				SoundManager.Play3DSoundAt(SoundManager.Effect.TurretExplosion, Position);
				needsRemoval = true;
			}
		}
		public void updateDirectionVectors()
		{
		
			Yaw = MathF.Atan2(FrontDirection.X, FrontDirection.Z);
			Pitch = -MathF.Asin(FrontDirection.Y);

			//System.Diagnostics.Debug.WriteLine(yaw + " " + pitch); 
		}
		float betweenFire = 0f;
		float fireRate;
		public void updateFireRate()
		{
			betweenFire += fireRate * 30f * Time;
		}
		void randomFireRate()
        {
			Random r = new Random();
			fireRate = (float)(0.001d + r.NextDouble() * 0.015d);
		}
		public void fireLaser()
		{
			//System.Diagnostics.Debug.WriteLine(Time + " " + betweenFire);
			if (betweenFire < 1)
				return;

			randomFireRate();


			SoundManager.Play3DSoundAt(SoundManager.Effect.TurretLaser, Position);

			betweenFire = 0;
			Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
			Matrix SRT =
				Matrix.CreateScale(new Vector3(0.07f, 0.07f, 0.4f)) *
				rotation *
				Matrix.CreateTranslation(Position);
			Laser.EnemyLasers.Add(new Laser(Position, rotation, SRT, FrontDirection, new Vector3(0.8f, 0f, 0.8f)));
		}
	}
}
