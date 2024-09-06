using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Geometries
{
    /// <summary>
    ///     Pyramid in a 3D world.
    /// </summary>
    public class PyramidPrimitive : GeometricPrimitive
    {
        /// <summary>
        ///     Create a Pyramid based on the vertices and colored white.
        /// </summary>
        /// <param name="device">Used to initialize and control the presentation of
        /// the graphics device.</param> <param name="top_vertex">Vertex of the top of
        /// the Pyramid.</param> <param name="base_vertex">Vertex of the base of the
        /// Pyramid.</param>
        public PyramidPrimitive(GraphicsDevice device, Vector3 top_vertex,
                                Vector3[] base_vertex)
            : this(device, top_vertex, base_vertex, Color.White) { }

        /// <summary>
        ///     Create a Pyramid based on vertices and color.
        /// </summary>
        /// <param name="device">Used to initialize and control the presentation of
        /// the graphics device.</param> <param name="top_vertex">Vertex of the top of
        /// the Pyramid.</param> <param name="base_vertex">Vertex of the base of the
        /// Pyramid.</param> <param name="vertexColor">The color of the
        /// Pyramid.</param>
        public PyramidPrimitive(GraphicsDevice device, Vector3 top_vertex,
                                Vector3[] base_vertex, Color vertexColor)
            : this(device, top_vertex, base_vertex, vertexColor, vertexColor,
                   vertexColor)
        { }

        /// <summary>
        ///     Create a Pyramid based on the vertices and a color for each one.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the
        /// presentation of the graphics device.</param> <param
        /// name="top_vertex">Vertex of the top of the Pyramid.</param> <param
        /// name="base_vertex">Vertex of the base of the Pyramid.</param> <param
        /// name="vertexColor1">The color of the vertex.</param> <param
        /// name="vertexColor2">The color of the vertex.</param> <param
        /// name="vertexColor3">The color of the vertex.</param>
        public PyramidPrimitive(GraphicsDevice graphicsDevice, Vector3 top_vertex,
                                Vector3[] base_vertex, Color vertexColor1,
                                Color vertexColor2, Color vertexColor3)
        {
            // Base triangles
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 1);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 3);
            AddIndex(CurrentVertex + 2);

            // Walls triangles
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 4);

            AddIndex(CurrentVertex + 3);
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 4);

            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);
            AddIndex(CurrentVertex + 4);

            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 4);

            Vector3 normal_0 = Vector3.Cross(base_vertex[2] - base_vertex[0],
                                             base_vertex[1] - base_vertex[2]);
            Vector3 normal_1 = Vector3.Cross(base_vertex[1] - base_vertex[0],
                                             top_vertex - base_vertex[1]);
            Vector3 normal_2 = Vector3.Cross(base_vertex[0] - base_vertex[3],
                                             top_vertex - base_vertex[0]);
            Vector3 normal_3 = Vector3.Cross(base_vertex[3] - base_vertex[2],
                                             top_vertex - base_vertex[3]);
            Vector3 normal_top = Vector3.Cross(base_vertex[2] - base_vertex[1],
                                               top_vertex - base_vertex[2]);

            AddVertex(base_vertex[0], vertexColor1, normal_0);
            AddVertex(base_vertex[1], vertexColor1, normal_1);
            AddVertex(base_vertex[2], vertexColor1, normal_2);
            AddVertex(base_vertex[3], vertexColor1, normal_3);
            AddVertex(top_vertex, vertexColor1, normal_top);

            InitializePrimitive(graphicsDevice);
        }
    }
}
