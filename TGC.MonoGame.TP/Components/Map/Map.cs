namespace TGC.MonoGame.TP.Components.Map
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the <see cref="Map" />.
    /// </summary>
    internal class Map
    {
        /// <summary>
        /// Gets or sets the Column.
        /// </summary>
        private Model Column { get; set; }
        private Floor Floor { get; set; }
        private Matrix QuadWorld { get; set; }

        /// <summary>
        /// Defines the separation.
        /// </summary>
        private const float separation = 450f;
        private const float minSpace = 50f;
        private const float size = 2750;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
        }

        /// <summary>
        /// The LoadContent.
        /// </summary>
        /// <param name="Column">The Column<see cref="Model"/>.</param>
        public void LoadContent(Model Column, Texture2D floorTexture, GraphicsDevice graphicsDevice)
        {
            this.Column = Column;
            // Obtengo su efecto para cambiarle el color y activar la luz predeterminada que tiene MonoGame.
            var modelEffect = (BasicEffect)Column.Meshes[0].Effects[0];
            modelEffect.DiffuseColor = Color.IndianRed.ToVector3();
            modelEffect.EnableDefaultLighting();

            Floor = new Floor(graphicsDevice, new Vector3(0, -110, 0), Vector3.Up, Vector3.Backward, size, size, floorTexture, 5);
            QuadWorld = Matrix.CreateTranslation(Vector3.UnitX * size / 2 + Vector3.UnitZ * size / 2);
        }

        /// <summary>
        /// The Draw.
        /// </summary>
        /// <param name="World">The World<see cref="Matrix"/>.</param>
        /// <param name="View">The View<see cref="Matrix"/>.</param>
        /// <param name="Projection">The Projection<see cref="Matrix"/>.</param>
        public void Draw(Matrix World, Matrix View, Matrix Projection)
        {
            Column.Draw(World * Matrix.CreateTranslation(minSpace, 0, minSpace), View, Projection);
            Column.Draw(World * Matrix.CreateTranslation(size - minSpace, 0, size - minSpace), View, Projection);
            Column.Draw(World * Matrix.CreateTranslation(minSpace, 0, size - minSpace), View, Projection);
            Column.Draw(World * Matrix.CreateTranslation(size - minSpace, 0, minSpace), View, Projection);

            Floor.Draw(QuadWorld, View, Projection);
        }
    }
}
