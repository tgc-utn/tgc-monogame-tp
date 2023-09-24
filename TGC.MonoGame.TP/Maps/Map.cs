using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Props.PropType.StaticProps;
using TGC.MonoGame.TP.References;
using TGC.MonoGame.TP.Scenarys;
using TGC.MonoGame.TP.Tanks;

namespace TGC.MonoGame.TP.Maps;

public abstract class Map
{
    protected Scenary Scenary { get; }
    public Tank Player;
    public List<Tank> Enemies { get; set; }
    public List<Tank> Allies { get; set; }
    public List<StaticProp> Props { get; }

    public abstract void Load(ContentManager content, Effect effect);
    public abstract void Draw(Matrix view, Matrix projection);
    public abstract void Update(GameTime gameTime, KeyboardState keyboardState);
}