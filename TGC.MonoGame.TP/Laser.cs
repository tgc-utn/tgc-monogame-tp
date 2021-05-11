using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Laser
{
	public Matrix SRT { get; set; }
	public Vector3 Position { get; set; }
	public Vector3 FrontDirection{ get; set; }

	public Laser(Matrix srt, Vector3 fd)
	{
		SRT = srt;
		FrontDirection = fd;
	}
}
