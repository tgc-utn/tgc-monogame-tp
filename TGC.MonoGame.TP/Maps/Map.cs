using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.References;
using TGC.MonoGame.TP.Scenarys;
using TGC.MonoGame.TP.Tanks;

namespace TGC.MonoGame.TP.Maps;

public abstract class Map
{
    protected Scenary Scenary { get; }
    protected Tank Player { get; }
    protected List<Tank> Enemies { get; set; }
    protected List<Tank> Alies { get; set; }

    public abstract void Load(ContentManager content, Effect effect);
    public abstract void Draw(Matrix view, Matrix projection);
}