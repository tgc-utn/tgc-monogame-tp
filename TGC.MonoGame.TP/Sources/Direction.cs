namespace TGC.MonoGame.TP
{
    internal class Direction
    {
        internal void Set((int, int) vector, Direction backward, Direction left, Direction right)
        {
            this.vector = vector;
            forward = this;
            this.backward = backward;
            this.left = left;
            this.right = right;
        }

        internal Direction forward, backward, left, right;
        private (int, int) vector;

        internal (int, int) AdvanceFrom((int, int) position) => (position.Item1 + vector.Item1, position.Item2 + vector.Item2);
    }

    internal class Directions
    {
        internal Direction Up, Down, Left, Right;
        internal Directions()
        {
            Up = new Direction();
            Down = new Direction();
            Left = new Direction();
            Right = new Direction();
            Up.Set((0, 1), Down, Left, Right);
            Down.Set((0, -1), Up, Right, Left);
            Left.Set((-1, 0), Right, Down, Up);
            Right.Set((1, 0), Left, Up, Down);
        }
    }
}