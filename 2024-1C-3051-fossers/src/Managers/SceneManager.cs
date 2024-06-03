using System.Collections.Generic;
using WarSteel.Scenes;

public enum ScenesNames
{
    MENU,
    MAIN
}

public class SceneManager
{
    private Dictionary<ScenesNames, Scene> scenes = new();
    private ScenesNames currentSceneName;


    public SceneManager(ScenesNames initialScene)
    {
        currentSceneName = initialScene;
    }

    private static SceneManager _INSTANCE = null;
    public static void SetUpInstance(ScenesNames initialScene)
    {
        _INSTANCE = new SceneManager(initialScene);

    }
    public static SceneManager Instance() => _INSTANCE;

    public void AddScene(ScenesNames name, Scene scene)
    {
        scenes.Add(name, scene);
    }

    public ScenesNames CurrentSceneKey() => currentSceneName;
    public Scene CurrentScene() => scenes[currentSceneName];

    public void SetCurrentScene(ScenesNames name)
    {
        if (scenes.ContainsKey(name))
        {
            CurrentScene().Unload();
            currentSceneName = name;
            CurrentScene().Initialize();
            CurrentScene().LoadContent();
        }
    }
}