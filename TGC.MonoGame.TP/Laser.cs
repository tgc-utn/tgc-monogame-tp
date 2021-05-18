using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Laser
{
	public Matrix SRT { get; set; }
	public Vector3 Position { get; set; }
	public Vector3 FrontDirection{ get; set; }
	public Vector3 Color;
	public Laser(Matrix srt, Vector3 fd, Vector3 c)
	{
		SRT = srt;
		FrontDirection = fd;
		Color = c;
	}
	public void Update(float time)
	{

		SRT *= Matrix.CreateTranslation(FrontDirection * 1500f * time);
	}
}
