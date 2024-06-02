using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Managers;
using WarSteel.Managers.Gizmos;
using WarSteel.Scenes;

public class GizmosProcessor : ISceneProcessor
{

    protected Gizmos _gizmos = new Gizmos();

    public void Draw(Scene scene)
    {
        scene.GetEntities().ForEach(e =>
        {
            if (e.HasComponent<DynamicBody>())
            {
                e.GetComponent<DynamicBody>().DrawGizmos(_gizmos);
            }

        });

        scene.GetEntities().ForEach(e =>
       {

           if (e.HasComponent<StaticBody>())
           {
               e.GetComponent<StaticBody>().DrawGizmos(_gizmos);
           }

       });

        _gizmos.Draw();

    }

    public void Initialize(Scene scene)
    {
        _gizmos.LoadContent(scene.GetGraphicsDevice().GraphicsDevice, ContentRepoManager.Instance().Manager);
    }

    public void Update(Scene scene, GameTime gameTime)
    {
        _gizmos.UpdateViewProjection(scene.GetCamera().View, scene.GetCamera().Projection);
    }

}