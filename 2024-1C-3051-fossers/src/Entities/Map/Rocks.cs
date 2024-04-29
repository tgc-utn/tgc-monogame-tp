
using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;

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
    public Rock(RockSize size) : base("rock", Array.Empty<string>(), new Transform(), Array.Empty<Component>())
    {
        rockSize = size;
    }

    private String GetRockSizeStringValue()
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
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new ColorShader(Color.Gray));

        base.LoadContent();
    }
}