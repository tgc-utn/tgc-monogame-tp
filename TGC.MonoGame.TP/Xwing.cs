using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
public class Xwing
{
	public bool barrelRolling { get; set; }
	public float roll { get; set; }

	public Model Model { get; set; }
	public Texture[] Textures { get; set; }
	public Matrix World { get; set; }
	public float Scale { get; set; }
	public Vector3 Position { get; set; }
	public Vector3 FrontDirection { get; set; }
	public Vector3 UpDirection { get; set; }
	public Vector3 RightDirection { get; set; }

	public GameTime GameTime { get; set; }
	public Vector2 TurnDelta;
	public Xwing()
	{
	}
	public float Pitch { get; set; }
	public float Yaw { get; set; }

	public float Roll = 0;
	float rollSpeed = 150f;
	
	public int LaserFired = 0;

	List<Vector2> deltas = new List<Vector2>();
	int maxDeltas = 23;
	Vector2 averageLastDeltas()
    {
		Vector2 current;
		Vector2 sum = Vector2.Zero;
		foreach(var delta in deltas)
        {
			sum.X += delta.X;
			sum.Y += delta.Y;
		}
		current.X = sum.X / deltas.Count;
		current.Y = sum.Y / deltas.Count;
		return current;
	}
	public void updateRoll()
	{
		if (deltas.Count < maxDeltas)
			deltas.Add(TurnDelta);
		else
        	deltas.RemoveAt(0);
        
		if (barrelRolling)
		{
			float time = Convert.ToSingle(GameTime.ElapsedGameTime.TotalSeconds);
			if (Roll < 360)
				Roll += rollSpeed * time;
			else
			{
				barrelRolling = false;
				Roll = 0;

			}
		}
		else
        {
			Vector2 currentDelta = averageLastDeltas();
			//delta [-3;3] -> [-90;90]
			Roll = -currentDelta.X * 30; 
        }
	}
	public Quaternion getAnimationQuaternion()
    {
		updateRoll();
		return Quaternion.CreateFromAxisAngle(FrontDirection, MathHelper.ToRadians(Roll));
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
}
