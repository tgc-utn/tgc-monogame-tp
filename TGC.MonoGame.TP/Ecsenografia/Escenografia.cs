using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Escenografia
{
    public abstract class Escenografia3D 
    {
        protected Model modelo; 
        protected Effect efecto;

        public Vector3 posicion;
        protected float rotacionX, rotacionY, rotacionZ;
        /// <summary>
        /// Usado para obtener la matriz mundo de cada objeto
        /// </summary>
        /// <returns>La matriz "world" asociada al objeto que llamo</returns>
        abstract public Matrix getWorldMatrix();
        /// <summary>
        /// Inicializa un modelo junto a sus efectos dado una direccion de archivo para este
        /// </summary>
        /// <param name="direcionModelo"> Direccion en el sistema de archivos para el modelo</param>
        /// <param name="direccionEfecto"> Direccion en el sistema de archivos para el efecto</param>
        /// <param name="contManager"> Content Manager del juego </param>
        /// <remarks> 
        /// Este metodo es virtual, para permitir sobre escribirlo, en caso de que
        /// necesitemos que algun modelo tenga diferentes efectos por mesh
        /// </remarks>
        virtual public void loadModel(String direcionModelo,
                        String direccionEfecto, ContentManager contManager)
        {
            //asignamos el modelo deseado
            modelo = contManager.Load<Model>(direcionModelo);
            //mismo caso para el efecto
            efecto = contManager.Load<Effect>(direccionEfecto);
            //agregamos el efecto deseado a cada parte del modelo
            //por ahora cada modelo, carga una misma textura para todo el modelo
            //luego podemos re escribir esto para hacerlo de otra forma
            //podria mover esta parte a los hijos de la clase y solo dejar la carga generica
            //esto sera aplicado por cada clase hija
            /*
            foreach ( ModelMesh mesh in modelo.Meshes )
            {
                foreach ( ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
            */
        }
        /// <summary>+
        /// Funcion para dibujar los modelos
        /// </summary>
        /// <param name="view">la matriz de la camara</param>
        /// <param name="projection">la matriz que define el como se projecta sobre la camara</param>
        /// <param name="color">el color que queremos que tenga el modelo de base</param>
        virtual public void dibujar(Matrix view, Matrix projection, Color color)
        {
            efecto.Parameters["View"].SetValue(view);
            // le cargamos el como quedaria projectado en la pantalla
            efecto.Parameters["Projection"].SetValue(projection);
            // le pasamos el color ( repasar esto )
            efecto.Parameters["DiffuseColor"].SetValue(color.ToVector3());
            foreach( ModelMesh mesh in modelo.Meshes)
            {
                efecto.Parameters["World"].SetValue(mesh.ParentBone.Transform * getWorldMatrix());
                mesh.Draw();
            }
        }
    }
    class Primitiva 
    {
        private readonly GraphicsDevice device;
        private VertexBuffer vertices;
        private IndexBuffer indices;
        private Effect effect;

        public Primitiva(GraphicsDevice device, Effect effect,
        Vector3 vertice1, Vector3 vertice2, Vector3 vertice3, Vector3 vertice4)
        {
            this.effect = effect;
            this.device = device;
            Color color = Color.Black;
            int[] indicesTemp = new int[6];
            VertexPositionColor[] dataVertices = new VertexPositionColor[4];
            dataVertices[0] = new VertexPositionColor(vertice1, color);
            dataVertices[1] = new VertexPositionColor(vertice2, color);
            dataVertices[2] = new VertexPositionColor(vertice3, color);
            dataVertices[3] = new VertexPositionColor(vertice4, color);

            indicesTemp[0] = 0;
            indicesTemp[1] = 1;
            indicesTemp[1] = 2;
            indicesTemp[1] = 2;
            indicesTemp[1] = 3;
            indicesTemp[1] = 0;

            vertices = new VertexBuffer(device, typeof(VertexPositionColor), dataVertices.Length, BufferUsage.WriteOnly);
            vertices.SetData(dataVertices);
            indices = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, indicesTemp.Length, BufferUsage.WriteOnly);
            indices.SetData(indicesTemp);
        }

        public void dibujar(Matrix view, Matrix projection, Color color)
        {
            device.SetVertexBuffer(vertices);
            device.Indices = indices;
            effect.Parameters["World"].SetValue(Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateScale(1000f));
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }
        }
        public void Dispose()
        {

        } 
    }
}