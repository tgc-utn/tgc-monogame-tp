using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP.Maps;

public class PlaneMap : Map
{
    protected Scenary Scenary { get; }
    protected Tank Player { get; }
    protected List<Tank> Enemies { get; } = new List<Tank>();
    protected List<Tank> Alies { get; } = new List<Tank>();
    protected List<StaticProp> Props { get; } = new List<StaticProp>();

    public PlaneMap(int numberOfTanks, ModelReference AliesTank, ModelReference EnemiesTank)
    {
        Scenary = new Scenary(Scenarios.Plane, new Vector3(-0.1f, -0.1f, -0.1f));
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint => Alies.Add(new Tank(EnemiesTank, spawnPoint)));
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint => Alies.Add(new Tank(AliesTank, spawnPoint)));
        Scenary.Reference.PropsReference
            .ForEach(prop =>
            {
                switch (prop.Repetitions.FunctionRef.GetType())
                {
                    case FunctionType.Linear:
                        MathFunctions.GetLinearPoints(prop.Repetitions)
                            .ForEach(position => Props.Add(new StaticProp(prop, position)));
                        break;
                    case FunctionType.Sinusoidal:
                        MathFunctions.GetSinusoidalPoints(prop.Repetitions)
                            .ForEach(position => Props.Add(new StaticProp(prop, position)));
                        break;
                    case FunctionType.Circular:
                        MathFunctions.GetCircularPoints(prop.Repetitions)
                            .ForEach(position => Props.Add(new StaticProp(prop, position)));
                        break;
                    case FunctionType.Unique:
                        Props.Add(new StaticProp(prop));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
    }

    public override void Load(ContentManager content, Effect effect)
    {
        Scenary.Load(content, effect);
        foreach (var enemy in Enemies)
            enemy.Load(content, effect);
        foreach (var alie in Alies)
            alie.Load(content, effect);
        foreach (var prop in Props)
            prop.Load(content, effect);
    }

    public override void Update(GameTime gameTime)
    {
        return;
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Scenary.Draw(view, projection);
        foreach (var enemy in Enemies)
            enemy.Draw(view, projection);
        foreach (var alie in Alies)
            alie.Draw(view, projection);
        foreach (var prop in Props)
            prop.Draw(view, projection);
    }
}