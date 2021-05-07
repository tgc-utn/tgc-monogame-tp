using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
	public Xwing()
	{
	}
	float angle = 0f;
	public float barrelRollStep = 0;
	float rollSpeed = 100f;
	public Quaternion getAnimationQuaternion(GameTime gameTime)
	{
		if (barrelRolling)
		{
			double time = gameTime.ElapsedGameTime.TotalSeconds;
			if(barrelRollStep < 360)
				barrelRollStep += angle + rollSpeed * Convert.ToSingle(time);
			else
            {
				barrelRolling = false;
				barrelRollStep = 0;

			}
		}
		//falta algo para que funcione bien
		//return Quaternion.CreateFromAxisAngle(FrontDirection, MathHelper.ToRadians(barrelRollStep)); 
		return Quaternion.CreateFromAxisAngle(FrontDirection, 0f);
	}
}
