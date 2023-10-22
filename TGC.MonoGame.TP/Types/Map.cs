using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.Tanks;

namespace TGC.MonoGame.TP.Types;

public abstract class Map
{
    public Scenary Scenary { get; set; }
    public Tank Player { get; set; }
    public List<Tank> Enemies { get; set; }
    public List<Tank> Alies { get; set; }
    public List<StaticProp> Props { get; set; } 

    public abstract void Load(ContentManager content);
    public abstract void Draw(Matrix view, Matrix projection, Vector3 lightPosition, Vector3 lightViewProjection);
    public abstract void Update(GameTime gameTime);
}