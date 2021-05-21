using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Laser
{
	public Matrix SRT { get; set; }
	public Vector3 Position { get; set; }
	public Vector3 FrontDirection{ get; set; }
	public Vector3 Color;
	BoundingCylinder BoundingCylinder;
    public Laser(Vector3 pos, Matrix rotation, Matrix srt, Vector3 fd, Vector3 c)
	{
		Position = pos;
		SRT = srt;
		FrontDirection = fd;
		Color = c;
		
		BoundingCylinder = new BoundingCylinder(Position, 10f, 20f);
		BoundingCylinder.Rotation = rotation;

	}
	public void Update(float time)
	{
        var translation = FrontDirection * 1500f * time;
        //var translation = FrontDirection * 150f * time;

        Position += translation;

		BoundingCylinder.Move(translation);
		SRT *= Matrix.CreateTranslation(translation);
	}
	public bool Hit(BoundingSphere sphere)
    {
		return BoundingCylinder.Intersects(sphere);
    }
}
