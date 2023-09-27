using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Props;
using TGC.MonoGame.TP.Props.PropType.StaticProps;
using TGC.MonoGame.TP.References;
using TGC.MonoGame.TP.Scenarys;
using TGC.MonoGame.TP.Tanks;

namespace TGC.MonoGame.TP.Maps;

public class Desert : Map
{
    public Desert(int numberOfTanks, ModelReference AlliesTank, ModelReference EnemiesTank, Tank player)
    {
        Props = new List<StaticProp>();
        Allies = new List<Tank>();
        Enemies = new List<Tank>();
        
        Scenary = new Scenary(Models.Scenary.Plane, new Vector3(-0.1f,-0.1f,-0.1f));
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint => Allies.Add(new Tank(EnemiesTank, spawnPoint)));
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint => Allies.Add(new Tank(AlliesTank, spawnPoint)));
        Scenary.Reference.PropsReference
            .ForEach(prop =>
            {
                if (prop.Repetitions > 1)
                    Scenary.GetCircularPoints(prop.Repetitions, prop.Position, 40f)
                        .ForEach(position => Props.Add(new SmallStaticProp(prop, position)));
                else
                    Props.Add(new SmallStaticProp(prop));
            });
        Player = player;
    }

    public override void Load(ContentManager content, Effect effect)
    {
        Scenary.Load(content, effect);
        foreach (var enemy in Enemies)
            enemy.Load(content, effect);
        foreach (var ally in Allies)
            ally.Load(content, effect);
        foreach (var prop in Props)
            prop.Load(content, effect);
        Player.Load(content, effect);
    }

    public override void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        base.Update(gameTime, keyboardState);
        
        foreach (var enemy in Enemies)
            enemy.Update(gameTime, keyboardState);
        foreach (var ally in Allies)
            ally.Update(gameTime, keyboardState);
        foreach (var prop in Props)
        {
            // foreach (var enemy in Enemies)
            // {
            //     prop.Update(enemy);
            // }
            // foreach (var ally in Allies)
            // {
            //     prop.Update(ally);
            // } DESCOMENTAR ESTO SI SE QUIERE QUE LAS COLISIONES TAMBIEN LAS PUEDAN HACER LOS TANQUES CON IA
            prop.Update(Player);
        }
        Player.Update(gameTime, keyboardState);
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Scenary.Draw(view, projection);
        foreach (var enemy in Enemies)
            enemy.Draw(view, projection);
        foreach (var ally in Allies)
            ally.Draw(view, projection);
        foreach (var prop in Props)
            prop.Draw(view, projection);
        Player.Draw(view, projection);
    }
}