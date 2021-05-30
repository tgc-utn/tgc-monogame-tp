using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;
namespace TGC.MonoGame.TP
{
	public class Xwing
	{
		public int HP { get; set; }
		public bool barrelRolling { get; set; }
		public float roll { get; set; }
		public Model Model { get; set; }
		public Texture[] Textures { get; set; }
		public Matrix World { get; set; }
		public Matrix SRT { get; set; }
		public float Scale { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 FrontDirection { get; set; }
		public Vector3 UpDirection { get; set; }
		public Vector3 RightDirection { get; set; }
		public float Time { get; set; }
		public Vector2 TurnDelta;
		public float Pitch { get; set; }
		public float Yaw { get; set; }

		public bool hit { get; set; }
		public float Roll = 0;
		float rollSpeed = 150f;

		Vector3 laserColor = new Vector3(0f, 0.8f, 0f);
		int LaserFired = 0;
		public List<Laser> fired = new List<Laser>();
		List<Vector2> deltas = new List<Vector2>();
		int maxDeltas = 22;

		public BoundingSphere boundingSphere;

		public float MapLimit;
		public int MapSize;
		public Vector2 CurrentBlock;
		public Xwing()
		{
			HP = 100;

		}
		public void Update(float elapsedTime, MyCamera camera)
		{

			Time = elapsedTime;
			// cuanto tengo que rotar (roll), dependiendo de que tanto giro la camara 
			//TurnDelta = camera.delta;
			//updateRoll();
			//actualizo todos los parametros importantes del xwing
			updateSRT(camera);
			//actualizo 
			updateFireRate();

			if (boundingSphere == null)
				boundingSphere = new BoundingSphere(Position, 50f);
			else
				boundingSphere.Center = Position;
			
			float blockSize = MapLimit / MapSize;

			CurrentBlock = new Vector2(
				(int)((Position.X / blockSize) + 0.5), (int)((Position.Z / blockSize) + 0.5));

			CurrentBlock.X = MathHelper.Clamp(CurrentBlock.X, 0, MapSize-1);
			CurrentBlock.Y = MathHelper.Clamp(CurrentBlock.Y, 0, MapSize-1);


		}
		Matrix rollQuaternion;
		float yawRad, correctedYaw;
		Vector3 pos;

		
		public Vector4 GetZone()
		{
			int viewSize = 3;
			
			//System.Diagnostics.Debug.Write("P " + Position.X + " " + Position.Z);
			var a = CurrentBlock.X - viewSize;
			var b = CurrentBlock.X + viewSize + 1;
			var c = CurrentBlock.Y - viewSize;
			var d = CurrentBlock.Y + viewSize + 1;

			a = MathHelper.Clamp(a, 0, MapSize);
			b = MathHelper.Clamp(b, 0, MapSize);
			c = MathHelper.Clamp(c, 0, MapSize);
			d = MathHelper.Clamp(d, 0, MapSize);

			return new Vector4(a, b, c, d);
		}
		public void VerifyCollisions(List<Laser> enemyLasers, Trench[,] map)
		{
			if (barrelRolling)
				return;
			var laserHit = false;
			Laser hitBy = enemyLasers.Find(laser => laser.Hit(boundingSphere));
			if (hitBy != null)
			{
				laserHit = true;
				HP -= 10;
				enemyLasers.Remove(hitBy);
			}
			// me fijo si el xwing esta por debajo del eje Y (posible colision con trench)
			// y adentro (entre las paredes) del bloque actual
			bool inTrench = true;
			if (Position.Y <= 0)
			{
				inTrench = map[(int)CurrentBlock.X, (int)CurrentBlock.Y].IsInTrench(boundingSphere);
				//Colision con pared de trench (rebote/perder/quitar hp/)
			}
            hit = !inTrench || laserHit;


        }
		void updateSRT(MyCamera camera)
		{
			// posicion delante de la camara que uso de referencia
			pos = camera.Position + camera.FrontDirection * 40;
			//yaw en radianes, y su correccion inicial
			yawRad = MathHelper.ToRadians(camera.Yaw);
			correctedYaw = -yawRad - MathHelper.PiOver2;
			//matriz de rotacion dado un quaternion, que me permite hacer la rotacion (roll)
			// y obtener de esa matriz la direccion hacia arriba del xwing, una vez que giro
			rollQuaternion = Matrix.CreateFromQuaternion(
				Quaternion.CreateFromAxisAngle(camera.FrontDirection, MathHelper.ToRadians(-Roll)));
			//actualizo los vectores direccion
			updateDirectionVectors(camera.FrontDirection, rollQuaternion.Up);
			//actualizo la posicion, pitch y yaw
			Position = pos - UpDirection * 8;
			Pitch = camera.Pitch;
			Yaw = MathHelper.ToDegrees(correctedYaw);
			//SRT contiene la matrix de escala, rotacion y traslacion a usar en Draw
			SRT =
				// correccion de escala
				Matrix.CreateScale(Scale) *
				// correccion por yaw y pitch de la camara
				Matrix.CreateFromYawPitchRoll(correctedYaw, MathHelper.ToRadians(Pitch), 0f) *
				// correccion por roll con un quaternion, para obtener de el vector direccion que apunta hacia arriba
				//(del modelo, una vez que giro)
				rollQuaternion *
				// lo muevo para abajo(del modelo) 8 unidades para que se aleje del centro 
				Matrix.CreateTranslation(Position);

		}
		Vector2 averageLastDeltas()
		{
			Vector2 temp = Vector2.Zero;
			float mul = 0.6f;
			
			TGCGame.MutexDeltas.WaitOne(); //deberia solucionar list modified exception
			foreach (var delta in deltas)
			{
				temp.X += delta.X * mul;
				temp.Y += delta.Y * mul;
				mul += 0.025f;
			}
			TGCGame.MutexDeltas.ReleaseMutex();
			temp.X /= deltas.Count;
			temp.Y /= deltas.Count;
			
			return temp;
		}
		Vector2 currentDelta;
		public void updateRoll(Vector2 turnDelta)
		{
			if (deltas.Count < maxDeltas)
				deltas.Add(turnDelta);
			else
				deltas.RemoveAt(0);
			if (barrelRolling)
			{

				//time = Convert.ToSingle(GameTime.ElapsedGameTime.TotalSeconds);
				if (Roll < 360)
					Roll += rollSpeed * Time;
				else
				{
					barrelRolling = false;
					Roll = 0;

				}
			}
			else
			{
				currentDelta = averageLastDeltas();
				//delta [-3;3] -> [-90;90]
				Roll = -currentDelta.X * 30;
			}
		}

		public void updateDirectionVectors(Vector3 front, Vector3 up)
		{
			FrontDirection = front;
			//RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
			//UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
			UpDirection = up;
			RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, UpDirection));

			//Quaternion q = Quaternion.CreateFromAxisAngle(FrontDirection, Roll);

			//UpDirection *= Matrix.CreateFromYawPitchRoll(0, 0, Roll);
		}

		float offsetVtop = 2.5f;
		float offsetVbot = 4;
		float offsetH = 11.5f;

		float betweenFire = 0f;
		float fireRate = 0.25f;
		public void updateFireRate()
		{
			betweenFire += fireRate * 30f * Time;
		}
		Matrix scale, translation;
		Matrix[] rot;
		Matrix[] t;
		Matrix[] laserSRT = new Matrix[] { Matrix.Identity, Matrix.Identity, Matrix.Identity, Matrix.Identity };
		public float yawCorrection = 5f;
		Vector3 laserFront;

		public void fireLaser()
		{
			//System.Diagnostics.Debug.WriteLine(Time + " " + betweenFire);
			if (betweenFire < 1)
				return;
			betweenFire = 0;


			scale = Matrix.CreateScale(new Vector3(0.07f, 0.07f, 0.4f));
			float[] corr = new float[]{
			MathHelper.ToRadians(Yaw + yawCorrection),
			MathHelper.ToRadians(Yaw + yawCorrection),
			MathHelper.ToRadians(Yaw - yawCorrection),
			MathHelper.ToRadians(Yaw - yawCorrection)
		};
			rot = new Matrix[] {
			Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(Yaw), MathHelper.ToRadians(Pitch), MathHelper.ToRadians(Roll)),
			Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(Yaw), MathHelper.ToRadians(Pitch), MathHelper.ToRadians(Roll)),
			Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(Yaw), MathHelper.ToRadians(Pitch), MathHelper.ToRadians(Roll)),
			Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(Yaw), MathHelper.ToRadians(Pitch), MathHelper.ToRadians(Roll))
		};
			translation = Matrix.CreateTranslation(Position + FrontDirection * 60f);
			t = new Matrix[] {
			Matrix.CreateTranslation(UpDirection * offsetVtop + RightDirection * offsetH),
			Matrix.CreateTranslation(-UpDirection * offsetVbot + RightDirection * offsetH),
			Matrix.CreateTranslation(-UpDirection * offsetVbot - RightDirection * offsetH),
			Matrix.CreateTranslation(UpDirection * offsetVtop - RightDirection * offsetH)
		};

			laserFront.X = MathF.Cos(MathHelper.ToRadians(Yaw + corr[LaserFired])) * MathF.Cos(MathHelper.ToRadians(Pitch));
			laserFront.Y = MathF.Sin(MathHelper.ToRadians(Pitch));
			laserFront.Z = MathF.Sin(MathHelper.ToRadians(Yaw + corr[LaserFired])) * MathF.Cos(MathHelper.ToRadians(Pitch));

			laserFront = Vector3.Normalize(laserFront);

			for (var i = 0; i < 4; i++)
				laserSRT[i] = scale * rot[i] * translation * t[i];


			fired.Add(new Laser(Position, rot[LaserFired], laserSRT[LaserFired], FrontDirection, laserColor));

			LaserFired++;
			LaserFired %= 4;

			if (fired.Count > 4)
				fired.RemoveAt(0);
		}


	}
}
