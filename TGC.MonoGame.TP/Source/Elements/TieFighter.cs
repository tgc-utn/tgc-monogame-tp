using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace TGC.MonoGame.TP
{
	public class TieFighter
	{
		public static List<TieFighter> Enemies = new List<TieFighter>();

		public static float TieScale = 0.02f;

		public Vector3 Position { get; set; }
		public Vector3 FrontDirection { get; set; }
		public Matrix World { get; set; }
		public Matrix SRT { get; set; }
		float Time;
		public Model Model { get; set; }
		

		public int HP = 100;
		public float Yaw, Pitch;
		public List<Laser> fired = new List<Laser>();
		public BoundingSphere boundingSphere;
		public TieFighter(Vector3 pos, Vector3 front, Matrix w, Matrix srt)
		{
			Position = pos;
			FrontDirection = front;
			World = w;
			SRT = srt;
			boundingSphere = new BoundingSphere(Position, 10f);

			randomFireRate();
		}
		float betweenFire = 0f;
		float fireRate;

		//float angleBetweenVectors(Vector3 a, Vector3 b)
		//{
		//	var cross = Vector3.Cross(a, b);
		//	return MathF.Asin(cross.Length() / (a.Length() * b.Length()));
		//}
		
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
		void randomFireRate()
		{
			Random r = new Random();
			fireRate = (float)(0.001d + r.NextDouble() * 0.05d);
		}
		public void fireLaser()
		{
			//System.Diagnostics.Debug.WriteLine(Time + " " + betweenFire);
			if (betweenFire < 1)
				return;

			betweenFire = 0;
			randomFireRate();

			var Game = TGCGame.Instance;
            SoundManager.Play3DSoundAt(SoundManager.Effect.Laser, Position);
            //SoundManager.Play3DSound(Game.soundLaser, Position);
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
			Matrix SRT =
				Matrix.CreateScale(new Vector3(0.07f, 0.07f, 0.4f)) *
				rotation *
				Matrix.CreateTranslation(Position);
			Laser.EnemyLasers.Add(new Laser(Position, rotation, SRT, FrontDirection, new Vector3(0.8f, 0f, 0f)));
		}
		public float angleToX, angleToZ, y;
		public void updateDirectionVectors()
		{
			Yaw = MathF.Atan2(FrontDirection.X, FrontDirection.Z);
			//TODO: verify
			
			Pitch = -MathF.Asin(FrontDirection.Y);
			

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

		public static void GenerateEnemies(Xwing xwing)
		{
			Random rnd = new Random();
			int maxEnemies = 2;
			int distance = 300;
			for (int i = 0; i < maxEnemies - Enemies.Count; i++)
			{
				Vector3 random = new Vector3(rnd.Next(-distance, distance), 0f, rnd.Next(-distance, distance));
				Vector3 pos = xwing.Position + random;
				Vector3 dir = Vector3.Normalize(xwing.Position - pos);


				Matrix SRT = Matrix.CreateScale(TieScale) * Matrix.CreateTranslation(pos);
				Enemies.Add(new TieFighter(pos, dir, Matrix.Identity, SRT));
			}
		}
		public static void UpdateEnemies(float time, Xwing xwing)
		{
			foreach (var enemy in Enemies)
			{
				enemy.Update(xwing, time);
				enemy.VerifyCollisions(Laser.AlliedLasers);
				enemy.fireLaser();
			}

			Enemies.FindAll(enemy => enemy.HP <= 0).
				ForEach(enemy => EnemyDeath(enemy.Position, xwing))  ;

			Enemies.RemoveAll(enemy => enemy.HP <= 0);
		}
		public static void AddAllRequiredToDraw(ref List<TieFighter> drawList, BoundingFrustum frustum)
        {
			foreach(var enemy in Enemies)
            {
				if (frustum.Intersects(enemy.boundingSphere))
					drawList.Add(enemy);
            }
        }
		static void EnemyDeath(Vector3 pos, Xwing xwing)
        {
			SoundManager.Play3DSoundAt(SoundManager.Effect.TieExplosion, pos);
			//explosion effect
			xwing.Score += 10;
		}
	}
}