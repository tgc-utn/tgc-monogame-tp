using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.Tanks;

namespace TGC.MonoGame.TP.Types;

public abstract class Map
{
    protected Scenary Scenary { get; set; }
    public Tank Player { get; set; }
    protected List<Tank> Enemies { get; set; }
    protected List<Tank> Alies { get; set; }

    public abstract void Load(ContentManager content);
    public abstract void Draw(Matrix view, Matrix projection);
    public abstract void Update(GameTime gameTime);
}