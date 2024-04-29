using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Entities;

public class EntityGenerator
{
    public static List<Entity> Generate(Vector3 center, int maxElems, Type Entity, params object[] constructorParams)
    {
        List<Entity> elems = new List<Entity>();

        Random rand = new Random();

        for (int i = 0; i < maxElems; i++)
        {
            Entity elem = (Entity)Activator.CreateInstance(Entity, constructorParams);
            elem.Transform.Pos = center + new Vector3(rand.Next(-10000, 10000), 0, rand.Next(-10000, 10000));
            elems.Add(elem);
        }

        return elems;
    }
}