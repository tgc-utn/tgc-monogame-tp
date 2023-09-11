using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.References;
using TGC.MonoGame.TP.Scenarys;
using TGC.MonoGame.TP.Tanks;

namespace TGC.MonoGame.TP.Maps;

public class Desert : Map
{
    protected Scenary Scenary { get; }
    protected Tank Player { get; }
    protected List<Tank> Enemies { get; } = new List<Tank>();
    protected List<Tank> Alies { get; } = new List<Tank>();

    public Desert(int numberOfTanks, ModelReference AliesTank, ModelReference EnemiesTank)
    {
        Scenary = new Scenary(Models.Scenary.Desert, Vector3.Zero);
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint => Alies.Add(new Tank(EnemiesTank, spawnPoint)));
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint => Alies.Add(new Tank(AliesTank, spawnPoint)));
    }

    public override void Load(ContentManager content, Effect effect)
    {
        Scenary.Load(content, effect);
        foreach (var enemy in Enemies)
            enemy.Load(content, effect);
        foreach (var alie in Alies)
            alie.Load(content, effect);
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Scenary.Draw(view, projection);
        foreach (var enemy in Enemies)
            enemy.Draw(view, projection);
        foreach (var alie in Alies)
            alie.Draw(view, projection);
    }
}