
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities.Map;

public enum RockSize
{
    SMALL,
    MEDIUM,
    LARGE
}

public class Rock : Entity
{
    private RockSize rockSize;

    public Rock(RockSize size) : base("rock", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        rockSize = size;
    }

    public override void Initialize(Scene scene)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/" + GetRockSizeStringValue() + "Stone");
        AddComponent(new StaticBody(new Collider(new ConvexShape(model),new NoAction()),Vector3.Zero));
        base.Initialize(scene);
    }

    private string GetRockSizeStringValue()
    {
        switch (rockSize)
        {
            case RockSize.SMALL:
                return "Small";
            case RockSize.MEDIUM:
                return "Medium";
            case RockSize.LARGE:
                return "Large";
            default:
                return "Large";
        }
    }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/" + GetRockSizeStringValue() + "Stone");
        Renderable = new Renderable(model,new ColorShader(Color.DarkGray));

        base.LoadContent();
    }
}