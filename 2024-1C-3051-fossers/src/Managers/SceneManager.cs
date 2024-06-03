using System.Collections.Generic;
using WarSteel.Scenes;

public enum ScenesNames
{
    MAIN
}

public class SceneManager
{
    private Dictionary<ScenesNames, Scene> scenes = new();
    private ScenesNames currentSceneName;

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
            currentSceneName = name;
        }
    }
}