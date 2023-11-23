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

        int count = 0;
        Scenary.GetSpawnPoints(numberOfTanks, true)
            .ForEach(spawnPoint =>
            {
                Tanks.Add(new Tank(AliesTank, spawnPoint, graphicsDevice, false, count, this));
                count++;
            });
        Player = Tanks[0];
        Player.Action = new PlayerActionTank(false, graphicsDevice);
        count = 0;
        Scenary.GetSpawnPoints(numberOfTanks, false)
            .ForEach(spawnPoint =>
            {
                Tanks.Add(new Tank(EnemiesTank, spawnPoint, graphicsDevice, true, count, this));
                count++;
            });

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
        List<Tank> AllyTanks = Tanks.Where(tank => tank.Action.isEnemy == false).ToList();
        List<Tank> EnemiesTanks = Tanks.Where(tank => tank.Action.isEnemy).ToList();
        List<Bullet> AllyBullets = AllyTanks.SelectMany(tank => tank.Bullets)
            .Where(bullet => bullet.IsAlive)
            .ToList();
        List<Bullet> EnemiesBullets = EnemiesTanks.SelectMany(tank => tank.Bullets)
            .Where(bullet => bullet.IsAlive)
            .ToList();

        foreach (var tank in Tanks)
            tank.Update(gameTime);
        
        AllyBullets.ForEach(bullet => EnemiesTanks.ForEach(tank => tank.CheckCollisionWithBullet(bullet)));
        EnemiesBullets.ForEach(bullet => AllyTanks.ForEach(tank => tank.CheckCollisionWithBullet(bullet)));

        foreach (var prop in Props.Where(prop => !prop.Destroyed).ToList())
        {
            Tanks.ForEach(tank => prop.Update(tank));
            AllyBullets.ForEach(bullet => prop.Update(bullet));
            EnemiesBullets.ForEach(bullet => prop.Update(bullet));
        }

        SkyDome.Update(gameTime);
    }

    public override void Draw(Camera camera, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice, Camera TargetLightCamera, BoundingFrustum BoundingFrustum)
    {
        // var visibleTanks = Tanks.Where(tank => BoundingFrustum.Intersects(tank.Box)).ToList();
        var visibleProps = Props.Where(prop => BoundingFrustum.Intersects(prop.Box)).ToList();

        // Sombras 
        GraphicsDevice.SetRenderTarget(ShadowMapRenderTarget);
        GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
        foreach (var tank in Tanks)
            tank.DrawOnShadowMap(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        foreach (var prop in visibleProps)
            prop.DrawOnShadowMap(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        Scenary.DrawOnShadowMap(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        GraphicsDevice.SetRenderTarget(null);
        // Escena
        foreach (var tank in Tanks)
           tank.Draw(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        foreach (var prop in visibleProps)
            prop.Draw(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        Scenary.Draw(camera, SkyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        SkyDome.Draw(camera, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        // Luz
        // SkyDome.LightBox.Draw(Matrix.CreateTranslation(SkyDome.LightPosition), camera.View, camera.Projection);
    }
}