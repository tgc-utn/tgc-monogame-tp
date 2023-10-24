using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Types.Tanks;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP.Maps;

public class PlaneMap : Map
{
    public PlaneMap(int numberOfTanks, TankReference AliesTank, TankReference EnemiesTank, GraphicsDeviceManager graphicsDevice)
    {
        Scenary = new Scenary(Scenarios.Plane, new Vector3(0f, -1.8f, 0f));
        Alies = new List<Tank>();
        Enemies = new List<Tank>();
        Props = new List<StaticProp>();
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint => Enemies.Add(new Tank(EnemiesTank, spawnPoint, graphicsDevice)));
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint => Alies.Add(new Tank(AliesTank, spawnPoint, graphicsDevice)));
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
        SkyDome = new SkyDome(Scenary.Scene.SkyDome);
        Player = Alies[0];
        Player.isPlayer = true;
        Alies.RemoveAt(0);
    }

    public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        Scenary.Load(content);
        Player.Load(content);
        foreach (var enemy in Enemies)
            enemy.Load(content);
        foreach (var alie in Alies)
            alie.Load(content);
        foreach (var prop in Props)
            prop.Load(content);
        SkyDome.Load(graphicsDevice, content);
    }

    public override void Update(GameTime gameTime)
    {
        Player.Update(gameTime);
        Props = Props.Where(prop => !prop.Destroyed).ToList();
        foreach (var prop in Props)
        {
            prop.Update(Player);
            foreach (var bullet in Player.Bullets)
            {
                prop.Update(bullet);
            }
        }
        
        // foreach (var enemy in Enemies)
        //     enemy.Update(gameTime);
        // foreach (var alie in Alies)
        //     alie.Update(gameTime);
        SkyDome.Update(gameTime);
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Scenary.Draw(view, projection, SkyDome.LightPosition, SkyDome.LightViewProjection);
        Player.Draw(view, projection, SkyDome.LightPosition, SkyDome.LightViewProjection);
        foreach (var enemy in Enemies)
            enemy.Draw(view, projection, SkyDome.LightPosition, SkyDome.LightViewProjection);
        foreach (var alie in Alies)
            alie.Draw(view, projection,SkyDome.LightPosition, SkyDome.LightViewProjection);
        foreach (var prop in Props)
            prop.Draw(view, projection, SkyDome.LightPosition, SkyDome.LightViewProjection);
        SkyDome.Draw(view, projection);
    }
}