using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Entities;


namespace WarSteel.Scenes;

public class Scene
{
    private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();
    public GraphicsDeviceManager GraphicsDeviceManager;
    public SpriteBatch SpriteBatch;
    protected Camera Camera;
    private Dictionary<Type, ISceneProcessor> SceneProcessors = new Dictionary<Type, ISceneProcessor>();


    public Scene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
    {
        GraphicsDeviceManager = graphics;
        SpriteBatch = spriteBatch;
    }

    public void SetCamera(Camera camera)
    {
        entities.Add(camera.Id, camera);
        Camera = camera;
    }

    public Camera GetCamera()
    {
        return Camera;
    }

    public void AddSceneProcessor(ISceneProcessor p)
    {
        SceneProcessors.Add(p.GetType(), p);
    }

    public void AddEntityBeforeRun(Entity entity)
    {
        entities.Add(entity.Id, entity);
    }

    public void AddEntityDynamically(Entity entity)
    {
        entity.Initialize(this);
        entity.LoadContent();
        entities.Add(entity.Id, entity);
    }

    private void DeleteEntity(Entity entity)
    {
        entities.Remove(entity.Id);
        entity.OnDestroy(this);
    }

    public T GetSceneProcessor<T>() where T : class, ISceneProcessor
    {
        return SceneProcessors.TryGetValue(typeof(T), out var processor) ? processor as T : default;
    }

    public List<Entity> GetEntities()
    {
        List<Entity> list = new List<Entity>();
        foreach (var e in entities.Values)
        {
            list.Add(e);
        }
        return list;
    }


    public Entity GetEntityByName(string name)
    {
        foreach (var entity in entities.Values)
        {
            if (entity.Name == name)
                return entity;
        }
        return null;
    }


    public virtual void Initialize()
    {
        foreach (var entity in entities.Values)
        {
            entity.Initialize(this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Initialize(this);
        }
    }

    public virtual void LoadContent()
    {
        foreach (var entity in entities.Values)
        {
            entity.LoadContent();
        }

    }

    public virtual void Draw()
    {


        /*
        So apparently the spritebatch messes up with some GraphicsDevice settings that basically break 3D rendering.
        Since we're storing the SpriteBatch in the scene class I think we should just have the UIProcessor behavior in the Scene class.

        I'm probably not going to make the refactor in time for the 6/4 delivery of the project so for now this stays here

        */
        // Set the DepthStencilState to enable the depth buffer
        GraphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        // Other rendering state settings as needed
        GraphicsDeviceManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        GraphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
        GraphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

        // Clear the depth buffer
        GraphicsDeviceManager.GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);


        foreach (var entity in entities.Values)
        {
            entity.Draw(this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Draw(this);
        }
    }

    public virtual void Update(GameTime gameTime)
    {

        foreach (var entity in entities.Values)
        {
            if (entity.ToDestroy)
            {
                DeleteEntity(entity);
            }
        }

        var copyEntities = new Dictionary<string, Entity>(entities);
        foreach (var entity in copyEntities.Values)
        {
            entity.Update(gameTime, this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Update(this, gameTime);
        }

    }

    public virtual void Unload()
    {
        // the entities could have a Dispose method to free its resources (models, textures, shaders)
        entities.Clear();
        Camera = null;
        SceneProcessors.Clear();

    }

}