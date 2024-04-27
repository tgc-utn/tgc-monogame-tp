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
        Transform.Dim = new Vector3(1, 1, 1);
        Transform.Pos = new Vector3(0, -1, 0);
        base.Initialize(camera);
    }

    public override void LoadContent(Camera camera)
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground/Grass");
        Texture2D texture = ContentRepoManager.Instance().GetTexture("Map/Ground/t_grass");
        _renderable = new Renderable(model);
        _renderable.AddShader("texture", new TextureShader(texture));

        base.LoadContent(camera);
    }
}