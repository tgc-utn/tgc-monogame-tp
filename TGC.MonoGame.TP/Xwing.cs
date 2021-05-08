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
	public GameTime GameTime { get; set; }
	public Vector2 TurnDelta;
	public Xwing()
	{
	}
	float angle = 0f;
	public float Roll = 0;
	float rollSpeed = 100f;

	List<Vector2> deltas = new List<Vector2>();
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
		if (deltas.Count < 20)
			deltas.Add(TurnDelta);
		else
        	deltas.RemoveAt(0);
        
		if (barrelRolling)
		{
			float time = Convert.ToSingle(GameTime.ElapsedGameTime.TotalSeconds);
			if (Roll < 360)
				Roll += angle + rollSpeed * time;
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
	public void updatePosition(Vector3 pos)
    {
		Position = pos - new Vector3(0, 8, 0);
    }
}
