namespace TGC.MonoGame.TP.Components.Player
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines the <see cref="Player" />.
    /// </summary>
    internal class Player
    {
        /// <summary>
        /// Defines the Life.
        /// </summary>
        private int Life { get;set;}

        /// <summary>
        /// Defines the Position.
        /// </summary>
        private Vector3 Position { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="startPosition">The startPosition<see cref="Vector3"/>.</param>
        public Player(Vector3 startPosition)
        {
            Life = 100;
            Position = startPosition;
        }

        public void SetPosition(Vector3 newPosition)
        {
            Position = newPosition;
        }
    }
}
