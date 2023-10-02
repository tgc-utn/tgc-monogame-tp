using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Types.Tanks;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP.Maps;

public class PlaneMap : Map
{
    public PlaneMap(int numberOfTanks, TankReference AliesTank, TankReference EnemiesTank)
    {
        Scenary = new Scenary(Scenarios.Plane, new Vector3(0f, 0f, -16f));
        Alies = new List<Tank>();
        Enemies = new List<Tank>();
        Props = new List<StaticProp>();
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint => Enemies.Add(new Tank(EnemiesTank, spawnPoint)));
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint => Alies.Add(new Tank(AliesTank, spawnPoint)));
        Scenary.Scene.PropsReference
            .ForEach(prop =>
            {
                switch (prop.Repetitions.FunctionRef.GetType())
                {
                    case FunctionType.Linear:
                        MathFunctions.GetLinearPoints(prop.Repetitions)
                            .ForEach(position => Props.Add(PropsRepository.InitializeProp(prop, position)));
                        break;
                    case FunctionType.Sinusoidal:
                        MathFunctions.GetSinusoidalPoints(prop.Repetitions)
                            .ForEach(position => Props.Add(PropsRepository.InitializeProp(prop, position)));
                        break;
                    case FunctionType.Circular:
                        MathFunctions.GetCircularPoints(prop.Repetitions)
                            .ForEach(position => Props.Add(PropsRepository.InitializeProp(prop, position)));
                        break;
                    case FunctionType.Unique:
                        Props.Add(PropsRepository.InitializeProp(prop, prop.Position));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        Player = Alies[0];
        Alies.RemoveAt(0);
    }

    public override void Load(ContentManager content)
    {
        Scenary.Load(content);
        Player.Load(content);
        foreach (var enemy in Enemies)
            enemy.Load(content);
        foreach (var alie in Alies)
            alie.Load(content);
        foreach (var prop in Props)
            prop.Load(content);
    }

    public override void Update(GameTime gameTime)
    {
        Player.Update(gameTime);
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Scenary.Draw(view, projection);
        Player.Draw(view, projection);
        foreach (var enemy in Enemies)
            enemy.Draw(view, projection);
        foreach (var alie in Alies)
            alie.Draw(view, projection);
        foreach (var prop in Props)
            prop.Draw(view, projection);
    }
}