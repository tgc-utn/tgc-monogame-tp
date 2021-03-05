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

        /// <summary>
        /// Defines the separation.
        /// </summary>
        private const float separation = 450f;

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
        public void LoadContent(Model Column)
        {
            this.Column = Column;
            // Obtengo su efecto para cambiarle el color y activar la luz predeterminada que tiene MonoGame.
            var modelEffect = (BasicEffect)Column.Meshes[0].Effects[0];
            modelEffect.DiffuseColor = Color.IndianRed.ToVector3();
            modelEffect.EnableDefaultLighting();
        }

        /// <summary>
        /// The Draw.
        /// </summary>
        /// <param name="World">The World<see cref="Matrix"/>.</param>
        /// <param name="View">The View<see cref="Matrix"/>.</param>
        /// <param name="Projection">The Projection<see cref="Matrix"/>.</param>
        public void Draw(Matrix World, Matrix View, Matrix Projection)
        {
            this.Column.Draw(World, View, Projection);
            this.Column.Draw(World * Matrix.CreateTranslation(separation, 0, separation), View, Projection);
            this.Column.Draw(World * Matrix.CreateTranslation(0, 0, separation), View, Projection);
            this.Column.Draw(World * Matrix.CreateTranslation(separation, 0, 0), View, Projection);
        }
    }
}
