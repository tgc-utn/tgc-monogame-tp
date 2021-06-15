using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace TGC.MonoGame.TP
{
	public class Xwing
	{
		public int HP { get; set; }
		public int Score { get; set; }
		public int Energy = 10;
		public bool prevBoostState = false;
		public bool Boosting { get; set; }
		public bool BoostLock { get; set; }
		public bool barrelRolling { get; set; }
		public float roll { get; set; }
		public Model Model { get; set; }
		public Model EnginesModel { get; set; }
		public Texture[] Textures { get; set; }
		public Matrix World { get; set; }
		public Matrix SRT { get; set; }
		public Matrix EnginesSRT;
		public Vector3 EnginesColor = new Vector3(0.8f, 0.3f, 0f);
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
		float rollSpeed = 180f;

		Vector3 laserColor = new Vector3(0f, 0.8f, 0f);
		int LaserFired = 0;
		
		List<Vector2> deltas = new List<Vector2>();
		int maxDeltas = 22;

		public BoundingSphere boundingSphere;
		public BoundingBox BB;
		public OrientedBoundingBox OBB;
		public OrientedBoundingBox OBBL;
		public OrientedBoundingBox OBBR;
		public OrientedBoundingBox OBBU;
		public OrientedBoundingBox OBBD;

		public float MapLimit;
		public int MapSize;
		public Vector2 CurrentBlock;
		public Xwing()
		{
			HP = 100;
			Score = 0;
			ScaleMatrix = Matrix.CreateScale(2.5f);
	
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

			updateEnergyRegen(elapsedTime);
            if (boundingSphere == null)
                boundingSphere = new BoundingSphere(Position, 15f);
            else
                boundingSphere.Center = Position;

			var min = Position - new Vector3(10, 5, 10);
			var max = Position + new Vector3(10, 5, 10);
			if (BB == null)
			{
				//BB = BoundingVolumesExtensions.CreateAABBFrom(Model);
				//BB = BoundingVolumesExtensions.Scale(BB, 0.026f);
				BB = new BoundingBox(min, max);
			}
			BB.Min = min;
			BB.Max = max;

			if (OBB == null)
            {
				var temporaryCubeAABB = BoundingVolumesExtensions.CreateAABBFrom(Model);
				// Scale it to match the model's transform
				temporaryCubeAABB = BoundingVolumesExtensions.Scale(temporaryCubeAABB, 0.026f);
				// Create an Oriented Bounding Box from the AABB
				OBB = OrientedBoundingBox.FromAABB(temporaryCubeAABB);

				var halfW = BoundingVolumesExtensions.Scale(temporaryCubeAABB, new Vector3(0.5f, 1f, 1f));
				var halfH = BoundingVolumesExtensions.Scale(temporaryCubeAABB, new Vector3(1f, 0.5f, 1f));

				OBBL = OrientedBoundingBox.FromAABB(halfW);
				OBBR = OrientedBoundingBox.FromAABB(halfW);
				OBBU = OrientedBoundingBox.FromAABB(halfH);
				OBBD = OrientedBoundingBox.FromAABB(halfH);
			}
			
			OBB.Center = Position;

			OBBL.Center = Position - RightDirection * 7;
			OBBR.Center = Position + RightDirection * 7;
			OBBU.Center = Position + UpDirection * 2;
			OBBD.Center = Position - UpDirection * 2;


			OBB.Orientation = YPR;

			OBBL.Orientation = YPR;
			OBBR.Orientation = YPR;
			OBBU.Orientation = YPR;
			OBBD.Orientation = YPR;

			

			float blockSize = MapLimit / MapSize;

			CurrentBlock = new Vector2(
				(int)((Position.X / blockSize) + 0.5), (int)((Position.Z / blockSize) + 0.5));

			CurrentBlock.X = MathHelper.Clamp(CurrentBlock.X, 0, MapSize-1);
			CurrentBlock.Y = MathHelper.Clamp(CurrentBlock.Y, 0, MapSize-1);

			

		}

		public Vector4 GetZone()
		{
			int viewSize = 2;
			
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
		public int GetHPIndex()
        {
			return HP / 10;
        }
		public void VerifyCollisions(List<Laser> enemyLasers, Trench[,] map)
		{
			var Game = TGCGame.Instance;
			if (barrelRolling)
				return;
			var laserHit = false;
			Laser hitBy = enemyLasers.Find(laser => laser.Hit(OBB));
			if (hitBy != null)
			{
				laserHit = true;
				//HP -= 10;
                
				if(HP <= 0)
                {
					SoundManager.Play3DSoundAt(SoundManager.Effect.TurretExplosion, Position);
					SoundManager.StopSound(soundBoost);
					Game.ChangeGameStateTo(TGCGame.GmState.Defeat);
					//Game.SelectedCamera = Game.Camera;
					//               Debug.WriteLine("Defeat");
					return; //no elimino el laser
                }
                enemyLasers.Remove(hitBy);
			}
			// me fijo si el xwing esta por debajo del eje Y (posible colision con trench)
			// y adentro (entre las paredes) del bloque actual
			bool inTrench = false;
            if (Position.Y <= 0)
            {
				if (Position.Y > -50)
					inTrench = map[(int)CurrentBlock.X, (int)CurrentBlock.Y].IsInTrench(OBB);
				else
					inTrench = true;
				//Colision con pared de trench (rebote/perder/quitar hp/)
            }

            hit = inTrench || laserHit;


        }

		Matrix YPR, T, ScaleMatrix;
		Vector3 EnginesScale = new Vector3(0.025f, 0.025f, 0.025f);
		float yawRad, correctedYaw;
		Vector3 pos;
		public float distanceToCamera = 35f;
		void updateSRT(MyCamera camera)
		{
			//Debug.WriteLine(distanceToCamera);
			// posicion delante de la camara que uso de referencia
			pos = camera.Position + camera.FrontDirection * distanceToCamera;
			//yaw en radianes, y su correccion inicial
			yawRad = MathHelper.ToRadians(camera.Yaw);
			correctedYaw = -yawRad - MathHelper.PiOver2;
			// actualizo pitch y yaw
			Pitch = camera.Pitch;
			Yaw = camera.Yaw;
			
			// armo la matriz de rotacion en base a pitch, roll, yaw
			YPR = Matrix.CreateFromYawPitchRoll(correctedYaw, MathHelper.ToRadians(Pitch), MathHelper.ToRadians(Roll));

			//actualizo los vectores direccion
			updateDirectionVectors(camera.FrontDirection, YPR.Up);
			//actualizo la posicion
			Position = pos - UpDirection * 8;
			
			//SRT contiene la matrix de escala, rotacion y traslacion a usar en Draw
			
			T = Matrix.CreateTranslation(Position);
			SRT = ScaleMatrix * YPR * T;

			//Motores (luces)
			EnginesSRT = Matrix.CreateScale(EnginesScale) * YPR * T;

        }
		
		Vector2 currentDelta;
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
		
		public bool clockwise = false;
		public float PrevRoll;
		public bool startRolling;
		float rollError = 3f;
		public void updateRoll(Vector2 turnDelta)
		{
			if (deltas.Count < maxDeltas)
				deltas.Add(turnDelta);
			else
				deltas.RemoveAt(0);


			if (barrelRolling)
			{
                if (Roll < 0 - rollError || 
					Roll > 0 + rollError || 
					startRolling)
                {
					if(Roll < 0 - rollError || Roll > 0 + rollError)
						startRolling = false;
					if (clockwise)
						Roll -= rollSpeed * Time;
					else
						Roll += rollSpeed * Time;

					if (Roll <= 0)
						Roll += 360;
					Roll %= 360;
				}
				else
                {
					barrelRolling = false;
				}
			}
			else
			{
				currentDelta = averageLastDeltas();
				//delta [-3;3] -> [-90;90]
				Roll = -currentDelta.X * 30;

				////delta [-3;3] -> [-12;12]
				//Pitch += currentDelta.Y * 4;

            }
		}

		public void updateDirectionVectors(Vector3 front, Vector3 up)
		{
			FrontDirection = front;
			UpDirection = up;
			RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, UpDirection));
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
		
		float energyTimer = 0f;
		SoundEffectInstance soundBoost;
		public float boostTime = 0f;
		public void updateEnergyRegen(float elapsedTime)
        {
			var Game = TGCGame.Instance;
			if (Boosting != prevBoostState)
            {
				boostTime = 0f;
				if (Boosting)
					soundBoost = SoundManager.PlaySound(SoundManager.Effect.Boost, 0.35f);
				else
				{
					SoundManager.StopSound(soundBoost);
					SoundManager.PlaySound(SoundManager.Effect.BoostStop, 0.25f);
				}

			}
			prevBoostState = Boosting;

			if(Boosting)
			{
				boostTime += 0.5f * elapsedTime;
				EnginesColor = new Vector3(0f, 0.6f, 0.8f);
				energyTimer += 0.08f * 60 * Time;

				if (energyTimer > 1)
				{
					energyTimer = 0;
					if (Energy > 0)
						Energy--;
				}
			}
			else
            {
				EnginesColor = new Vector3(0.7f, 0.15f, 0f);
				energyTimer += 0.10f * 60 * Time;
				
				if (energyTimer > 1)
				{
					energyTimer = 0;
					if (Energy < 10)
						Energy++;
				}
			}
			
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

			var Game = TGCGame.Instance;
			//SoundManager.Play3DSoundAt(SoundManager.Effect.XwingLaser, Position);
			SoundManager.PlaySound(SoundManager.Effect.XwingLaser, 0.2f);
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
			var forwardPos = Position + FrontDirection * 60f;
			translation = Matrix.CreateTranslation(forwardPos);
			Vector3[] newPos = new Vector3[] {
				forwardPos + UpDirection * offsetVtop + RightDirection * offsetH,
				forwardPos - UpDirection * offsetVbot + RightDirection * offsetH,
				forwardPos - UpDirection * offsetVbot - RightDirection * offsetH,
				forwardPos + UpDirection * offsetVtop - RightDirection * offsetH};
			t = new Matrix[] {
				Matrix.CreateTranslation(newPos[0]),
				Matrix.CreateTranslation(newPos[1]),
				Matrix.CreateTranslation(newPos[2]),
				Matrix.CreateTranslation(newPos[3])
			};

			laserFront.X = MathF.Cos(MathHelper.ToRadians(Yaw + corr[LaserFired])) * MathF.Cos(MathHelper.ToRadians(Pitch));
			laserFront.Y = MathF.Sin(MathHelper.ToRadians(Pitch));
			laserFront.Z = MathF.Sin(MathHelper.ToRadians(Yaw + corr[LaserFired])) * MathF.Cos(MathHelper.ToRadians(Pitch));

			laserFront = Vector3.Normalize(laserFront);

			
			for (var i = 0; i < 4; i++)
				laserSRT[i] = scale * YPR *  t[i];


			Laser.AlliedLasers.Add(new Laser(newPos[LaserFired], YPR, laserSRT[LaserFired], FrontDirection, laserColor));

			LaserFired++;
			LaserFired %= 4;

			//if (fired.Count > 4)
			//	fired.RemoveAt(0);
		}


	}
}
