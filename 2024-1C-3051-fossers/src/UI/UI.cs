using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

using UIAction = Action<Scene, UI>;

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

    public UI(Vector3 position, IUIRenderable renderable, List<UIAction> actions)
    {
        _renderable = renderable;
        _actions = actions;
        _position = position;
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
        return mousePosition.X <= _position.X + _width / 2
               && mousePosition.X >= _position.X - _width / 2
               && mousePosition.Y <= _position.Y + _height / 2
               && mousePosition.Y >= _position.Y - _height / 2;
    }

    public void OnClick(Scene scene)
    {
        _actions.ForEach(action => action.Invoke(scene, this));
    }

    public virtual void Update(GameTime gameTime, MouseState mouseState, Scene scene)
    {
        if (IsBeingClicked(new(mouseState.X, mouseState.Y)))
            OnClick(scene);
    }
}