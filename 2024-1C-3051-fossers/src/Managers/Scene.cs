using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Managers;
using WarSteel.Scenes;

public enum ScenesNames
{
    MAIN
}

public class SceneManager
{
    private Dictionary<ScenesNames, Scene> scenes = new();
    private Scene currentScene;
    private ScenesNames currentSceneKey;

    public SceneManager(ScenesNames initialScene, GraphicsDeviceManager Graphics)
    {
        // here we add all the available scenes
        scenes.Add(ScenesNames.MAIN, new WarSteel.Scenes.Main.MainScene(Graphics));

        currentScene = scenes[initialScene];
        currentSceneKey = initialScene;
    }


    private static SceneManager _INSTANCE = null;
    public static void SetUpInstance(ScenesNames scene, GraphicsDeviceManager Graphics) => _INSTANCE =
        new SceneManager(scene, Graphics);
    public static SceneManager Instance() => _INSTANCE;

    //methods
    public ScenesNames CurrentSceneKey() => currentSceneKey;
    public Scene CurrentScene() => currentScene;

    public void UpdateCurrentScene(ScenesNames name)
    {
        if (scenes.ContainsKey(name))
        {
            currentScene.Unload();
            EntitiesManager.Instance().DestroyAll();
            currentScene = scenes[name];
            currentSceneKey = name;
            currentScene.Initialize();
            currentScene.LoadContent();
            currentScene.Draw();
        }
    }
}