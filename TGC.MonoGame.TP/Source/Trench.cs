using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.MonoGame.TP
{
	public class Trench
	{
		public Model Model { get; set; }
		public float Rotation { get; set; }
		public Matrix SRT { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 Color { get; set; }
		public TrenchType Type { get; set; }
		public List<TrenchTurret> Turrets = new List<TrenchTurret>();
		public List<BoundingBox> boundingBoxes = new List<BoundingBox>();
		public Trench(TrenchType t, Model m)
        {
			Type = t;
			Model = m;
        }
		public Trench(TrenchType t, float r)
		{
			Type = t;
			Rotation = r;
			//Model = TGCGame.GetModelFromType(Type); //always null, models not loaded, update in loadcontent
		}
		
		public bool IsInTrench(BoundingSphere element)
        {
			var hit = false;
			foreach(var box in boundingBoxes)
            	hit |= box.Intersects(element);

			return hit;

			//BoundingBox hit = boundingBoxes.Find(box => box.Intersects(element));
			//return !(hit.Min.Equals(Vector3.Zero) && (hit.Max.Equals(Vector3.Zero)));
        }
		private static Trench GetNextTrench(Trench input, float rotation)
		{
			//Si es bloque de estos y no esta alineado, entonces es un straight
			if (input.Type.Equals(TrenchType.Intersection) ||
				input.Type.Equals(TrenchType.Elbow) ||
				input.Type.Equals(TrenchType.Intersection)
				&& rotation != input.Rotation)
				return new Trench(TrenchType.Straight, rotation);
			//else
			//	return new Trench(TrenchType.Platform, rotation);
			

			Random rnd = new Random();
			int val = rnd.Next(0, 100);

			// 85% chance de que sea derecho, 15% repatirtido en CodoIzq, CodoDer, T, Interseccion.
			var straightChance = 85;
			var elbowLeftChance = 5;
			var elbowRightChance = 5;
			var tChance = 4;
			var intersectionChance = 1;
			
			if (val < straightChance)
				return new Trench(TrenchType.Straight, rotation);
			if (val < 100 - intersectionChance - tChance - elbowRightChance - elbowLeftChance)
				return new Trench(TrenchType.Elbow, LeftFromAngle(rotation));
			if (val < 100 - intersectionChance - tChance - elbowRightChance)
				return new Trench(TrenchType.Elbow, RightFromAngle(rotation));
			if (val < 100 - intersectionChance - tChance)
                return new Trench(TrenchType.T, rotation);
            if (val < 100 - intersectionChance)
                return new Trench(TrenchType.Intersection, rotation);

            return new Trench(TrenchType.Straight, 0f);
		}
		static float LeftFromAngle(float ang)
		{
			float res = ang - 90;
			if (res < 0)
				res += 360;
			return res;
        }
		static float RightFromAngle(float ang)
		{
			float res = ang + 90;
			res %= 360;
			return res;
		}
		static List<Vector3> GenLine(ref Trench[,] map, int xi, int yi, float rotation, int size)
        {
            int deltaX = 0;
			int deltaY = 0;
			int x = xi;
			int y = yi;
			int steps = 0;
			// X,Y,Rotation
			List<Vector3> pointsOfInterest = new List<Vector3>();
            // me muevo en la direccion en la que vengo, hasta el final
			switch (rotation)
            {
				case 0f:   deltaY = 1;  deltaX = 0; steps = size - yi-1;	break;
				case 90f:  deltaX = 1;  deltaY = 0; steps = size - xi-1;	break;
				case 180f: deltaY = -1; deltaX = 0; steps = yi;			break;
				case 270f: deltaX = -1; deltaY = 0; steps = xi;			break;
			}
			int prevX, prevY;
			for (int step = 0 ; step < steps; step++)
			{
				prevX = x;
				prevY = y;
				x += deltaX;
				y += deltaY;
				//Si hay algo ahi, freno (reemplazo por T? )
				if(map[x, y] != null)
                	return pointsOfInterest;
                

				//Obtengo el siguiente bloque (adelante en el mapa)
				Trench next = Trench.GetNextTrench(map[prevX, prevY], rotation);
				map[x, y] = next;
				//si es Interseccion o T, voy a tener dos direcciones en las que seguir dibujando (izq, der)
				if (next.Type.Equals(TrenchType.Intersection) || next.Type.Equals(TrenchType.T))
                {
					pointsOfInterest.Add(new Vector3(x, y, LeftFromAngle(rotation)));
					pointsOfInterest.Add(new Vector3(x, y, RightFromAngle(rotation)));
					poiCount++;
				}
				//si es un codo, voy a seguir en la direccion en la que este ubicado (izq O der)
				if(next.Type.Equals(TrenchType.Elbow))
				{
					//if (next.Rotation == rotation)
					//    pointsOfInterest.Add(new Vector3(x, y, LeftFromAngle(rotation)));
					//else
					//    pointsOfInterest.Add(new Vector3(x, y, RightFromAngle(rotation)));
					pointsOfInterest.Add(new Vector3(x, y, next.Rotation));
					poiCount++;
                }

				//Si no es una interseccion, no puedo seguir adelante y tengo que frenar la generacion en esta rama
				bool stop = !(next.Type.Equals(TrenchType.Straight) || next.Type.Equals(TrenchType.Intersection));
				if (stop)
					return pointsOfInterest;
				
			}
			return pointsOfInterest;

		}
		static void recursiveGen(List<Vector3> points, ref Trench[,] map, int size)
        {
			foreach (var point in points)
            {
				var otherPoints = GenLine(ref map, (int)point.X, (int)point.Y, (int) point.Z, size);
				recursiveGen(otherPoints, ref map, size);
			}
			//System.Diagnostics.Debug.Write(Trench.ShowMapInConsole(map, size));
		}
		static int poiCount = 0;
		public static Trench[,] GenerateMap(int size)
		{
			Trench[,] map = new Trench[size, size];

			int xi = size / 2;
			int yi = 0;

			//Cantidad de puntos de interes (codo, T, interseccion)
			var attempt = 0;
			poiCount = 0;
			var whiteSpace = 500;
			while (poiCount < 9 || whiteSpace > 300)
			{
				poiCount = 0;
				attempt++;
				ClearMap(ref map, size);
				map[xi, yi] = new Trench(TrenchType.Straight, 0f);

				var points = GenLine(ref map, xi, yi, 0f, size);

				recursiveGen(points, ref map, size);
				
				whiteSpace = SetPlatforms(ref map, size, false);
				
			}
			SetPlatforms(ref map, size, true);

			System.Diagnostics.Debug.WriteLine("attemps: "+ attempt+" ws: " + whiteSpace);
			return map;
		}
		static int SetPlatforms(ref Trench[,] map, int size, bool create)
        {
			int p = 0;
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					if (map[x, y] == null)
					{
						p++;
						if (create)
							map[x, y] = new Trench(TrenchType.Platform, 0f);
					}
			return p;
		}
		static void ClearMap(ref Trench[,] map, int size)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					map[x, y] = null;
		}
		
		public static String ShowMapInConsole(Trench[,] map, int size)
		{
			String str = "MAP\n";
			for (int y = size - 1; y >= 0; y--)
			{
				for (int x = size - 1; x >= 0; x--)
				{
					if (map[x, y] == null)
						str += "█ ";
					else
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
	
	public enum TrenchType
	{
		Platform,
		Straight,
		T,
		Intersection,
		Elbow
	}
}