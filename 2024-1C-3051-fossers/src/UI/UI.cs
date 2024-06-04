using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public interface IUIRenderable
{
    public void Draw(Scene scene, UI ui);
}

public class UI
{
    public bool toDestroy = false;
    private Vector3 _position;
    private float _height;
    private float _width;

    private IUIRenderable _renderable;
    private List<UIAction> _actions;

    public UI(Vector3 position, float width, float height, IUIRenderable renderable, List<UIAction> actions)
    {
        _renderable = renderable;
        _actions = actions;
        _position = position;
        _height = height;
        _width = width;
    }


    public UI(Vector3 position, float width, float height, IUIRenderable renderable)
    {
        _renderable = renderable;
        _position = position;
        _height = height;
        _width = width;
        _actions = new List<UIAction>();
    }

    public UI(Vector3 position, IUIRenderable renderable)
    {
        _renderable = renderable;
        _actions = new List<UIAction>();
        _position = position;
        _height = 0;
        _width = 0;
    }

    public Vector3 Position => _position;
    public float Width => _width;
    public float Height => _height;

    public void Draw(Scene scene)
    {
        _renderable.Draw(scene, this);
    }

    public bool IsBeingClicked(Vector2 mousePosition)
    {
        Vector3 position = Position;

        return mousePosition.X <= position.X + _width / 2
               && mousePosition.X >= position.X - _width / 2
               && mousePosition.Y <= position.Y + _height / 2
               && mousePosition.Y >= position.Y - _height / 2;
    }

    public void OnClick(Scene scene)
    {
        _actions.ForEach(action => action.Invoke(scene, this));
    }
}