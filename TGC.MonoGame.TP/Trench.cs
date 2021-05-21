using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC;

namespace TGC.MonoGame.TP
{
	public class Trench
	{
		Model Model;
		float Rotation;
		TrenchType Type;

		public Trench(TrenchType t, float r)
		{
			Type = t;
			Rotation = r;
			Model = TGCGame.GetModelFromType(Type);
		}
		public enum TrenchType
		{
			Platform,
			Straight,
			T,
			Intersection,
			Elbow,
			Turret
		}
		private static Trench GetNextTrench(Trench input, float rotation)
		{
			//Si es bloque de estos y no esta alineado, entonces es un straight
			if (input.Type.Equals(TrenchType.Intersection) ||
				input.Type.Equals(TrenchType.Elbow) ||
				input.Type.Equals(TrenchType.Intersection)
				&& rotation != input.Rotation)
				return new Trench(TrenchType.Straight, rotation);
			else
				return new Trench(TrenchType.Platform, rotation);
			//Si no esta alineado, implica 

			Random rnd = new Random();
			int val = rnd.Next(0, 100);

			// 70% chance de que sea derecho, 30% repatirtido en T, Codo, Interseccion.
			if (val < 70)
				return new Trench(TrenchType.Straight, rotation);
			if (val < 80)
				return new Trench(TrenchType.T, rotation);
			if (val < 90)
				return new Trench(TrenchType.Elbow, rotation);
			if (val < 100)
				return new Trench(TrenchType.Intersection, rotation);

			return new Trench(TrenchType.Straight, 0f);
		}

		public static Trench[,] generateMap(int size)
		{
			Trench[,] map = new Trench[size, size];

			int xi = size / 2;
			int yi = 0;

			map[xi, yi] = new Trench(TrenchType.Straight, 0f);

			for (int y = yi+1; y < size; y++)
            {
				map[xi, y] = Trench.GetNextTrench(map[xi, y-1], 0f);
			}
			//bool complete = false;
			//float rotation = 0;
			//while (complete)
			//{
			//	int prevX = x, prevY = y;
			//	switch (rotation)
			//	{
			//		case 0f: y++; break;
			//		case 90f: x++; break;
			//		case 180f: y--; break;
			//		case 270f: x--; break;
			//	}
			//	map[x, y] = Trench.GetNextTrench(map[prevX, prevY], rotation);
			//	if (map[x, y].Type.Equals(TrenchType.T))
			//		rotation = 90f;
			//	//map[]
			//}
			return map;
		}
		public static String ShowMapInConsole(Trench[,] map, int size)
		{
			String str = "MAP\n";
			for (int x = 0; x < size; x++)
			{
				for (int y = size-1; y >= 0; y--)
				{
					switch(map[x,y].Type)
                    {
						case TrenchType.Platform: str += "█ ";break;
						case TrenchType.Intersection: str += "╬ "; break;
						case TrenchType.Straight:
							if(map[x,y].Rotation == 0f)
								str += "▲ "; 
							else if(map[x, y].Rotation == 90f)
								str += "► ";
							else if (map[x, y].Rotation == 180f)
								str += "▼ ";
							else if (map[x, y].Rotation == 270f)
								str += "◄ ";
							break;
						case TrenchType.T:
							if (map[x, y].Rotation == 0f)
								str += "┬ ";
							else if (map[x, y].Rotation == 90f)
								str += "┤ ";
							else if (map[x, y].Rotation == 180f)
								str += "┴ ";
							else if (map[x, y].Rotation == 270f)
								str += "├ ";
							break;
						case TrenchType.Elbow:
							if (map[x, y].Rotation == 0f)
								str += "╔ ";
							else if (map[x, y].Rotation == 90f)
								str += "╗ ";
							else if (map[x, y].Rotation == 180f)
								str += "╝ ";
							else if (map[x, y].Rotation == 270f)
								str += "╚ ";
							break;
						default: str += "e ";break;
					}
				}
				str += "\n";
			}
			return str;
		}
	}
}