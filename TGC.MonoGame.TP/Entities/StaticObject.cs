
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;

namespace TGC.MonoGame.TP;

public class StaticObject
{
    private Vector3 Position;
    private float Scale = 1f;
    private Matrix World;
    public BoundingBox BBox;
    private GameModel Model;

    public StaticObject(Vector3 pos, float scale) 
    {
        Position = pos;
        Scale = scale;
        var modelBoundingBox = BoundingVolumesExtensions.CreateAABBFrom(Model.Model);
        BBox = new BoundingBox(modelBoundingBox.Min + Position, modelBoundingBox.Max + Position);
    }

    public StaticObject(Vector3 pos)
    {
        Position = pos;
    }

    public void setModel(GameModel gameModel) {
        Model = gameModel; 
        var modelBoundingBox = BoundingVolumesExtensions.CreateAABBFrom(Model.Model);
        BBox = new BoundingBox(modelBoundingBox.Min + Position, modelBoundingBox.Max + Position);
    }

    public void Draw() {
        World = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
        Model.Draw(World);
    }

}

