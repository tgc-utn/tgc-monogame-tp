using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;

public class UI
{
    public bool toDestroy = false;

    private Vector3 _position;
    private float _height;
    private float _width;

    private UIRenderable _renderable;
    private List<UIAction> _actions;

    public UI(Vector3 position, UIRenderable renderable, List<UIAction> actions)
    {
        _renderable = renderable;
        _actions = actions;
        _position = position;
    }

    public void Draw(Scene scene)
    {
        _renderable.Draw(scene);
    }

    public bool IsBeingClicked(Vector2 mousePosition)
    {
        return mousePosition.X <= _position.X + _width / 2 & mousePosition.X >= _position.X - _width / 2 && mousePosition.Y <= _position.Y + _height / 2 && mousePosition.Y >= _position.Y - _height / 2;
    }

    public void OnClick(Scene scene, Vector2 mousePosition)
    {
        _actions.ForEach(action => action.Execute(scene, this));
    }

}

public interface UIAction
{

    public void Execute(Scene scene, UI ui);

}

public interface UIRenderable
{

    public void Draw(Scene scene);

}