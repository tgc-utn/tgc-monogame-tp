using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Common.Shaders;
using WarSteel.Managers;


namespace WarSteel.Entities.Map;

public class Ground : Entity
{
    public Ground() : base("ground", Array.Empty<string>(), new Transform()) { }

    public override void Initialize(Camera camera)
    {
        Transform.Pos = new Vector3(0, -100f, 0);
        base.Initialize(camera);
    }

    public override void LoadContent(Camera camera)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground");
        Shader texture = new TextureShader(ContentRepoManager.Instance().GetTexture("Tanks/T90/textures_mod/hullA"));
        _renderable = new Renderable(model);
        _renderable.AddShader("color", new ColorShader(Color.Gray));
        base.LoadContent(camera);
    }
}