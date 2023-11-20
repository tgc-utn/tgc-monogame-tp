using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
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
        Scenary = new Scenary(Scenarios.Plane, new Vector3(0f, -0.53f, 0f));
        Props = new List<StaticProp>();
        Tanks = new List<Tank>();
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint => Tanks.Add(new Tank(AliesTank, spawnPoint, graphicsDevice, 1)));
        Player = Tanks[0];
        Player.Action = new PlayerActionTank(1, graphicsDevice);
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint => Tanks.Add(new Tank(EnemiesTank, spawnPoint, graphicsDevice, 2)));

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
    }

    public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        Scenary.Load(content);
        foreach (var prop in Props)
            prop.Load(content);
        foreach (var tank in Tanks)
            tank.Load(content);
        SkyDome.Load(graphicsDevice, content);
    }

    public override void Update(GameTime gameTime)
    {
        Props = Props.Where(prop => !prop.Destroyed).ToList();
        foreach (var prop in Props)
            Tanks.ForEach(ally => prop.Update(ally));
        foreach (var tank in Tanks)
            tank.Update(gameTime);
        SkyDome.Update(gameTime);
    }

    public override void Draw(Camera camera, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice, Camera TargetLightCamera)
    {
        // Sombras
        GraphicsDevice.SetRenderTarget(ShadowMapRenderTarget);
        GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
        foreach (var tank in Tanks)
            tank.DrawOnShadowMap(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        
        foreach (var prop in Props)
            prop.DrawOnShadowMap(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        Scenary.DrawOnShadowMap(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        GraphicsDevice.SetRenderTarget(null);
        // Escena
        foreach (var tank in Tanks)
            tank.Draw(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        foreach (var prop in Props)
            prop.Draw(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        Scenary.Draw(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        SkyDome.Draw(camera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        // Luz
        // SkyDome.LightBox.Draw(Matrix.CreateTranslation(SkyDome.LightPosition), camera.View, camera.Projection);
    }
}