using BepuPhysics.Collidables;
using BepuUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.Samples.Collisions;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace TGC.MonoGame.TP
{
    public class BaseModel
    {
        public Model Model { get;  set; }
        public Effect Effect { get;  set; }
        public float Scale { get; set; }
        public Vector3 Position { get; set; }
        public Matrix World { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public bool Touch { get; set; }

        public List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();

    }
}
