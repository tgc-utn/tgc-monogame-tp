using Microsoft.Xna.Framework;
namespace TGC.MonoGame.InsaneGames.Entities
{
    abstract class Entity : IDrawable 
    {
        public Matrix? position { get; set; }

        protected Vector3 BottomVertex, UpVertex;
        virtual public bool CollidesWith(Vector3 bBottom, Vector3 bUp)
        {
            if(bUp.X < BottomVertex.X || UpVertex.X < bBottom.X) return false;
            if(bUp.Y < BottomVertex.Y || UpVertex.Y < bBottom.Y) return false;
            if(bUp.Z < BottomVertex.Z || UpVertex.Z < bBottom.Z) return false;
            return true;
        }
    }
}