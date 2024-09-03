using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Escenografia
{
    abstract class Escenografia
    {
        protected Model modelo; 
        protected Effect efecto;
        public Vector3 posicion;
        float rotacionX, rotacionY, rotacionZ;
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
        public void loadModel(String direcionModelo,
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
            foreach ( ModelMesh mesh in modelo.Meshes )
            {
                foreach ( ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
        }
        /// <summary>
        /// Funcion para dibujar los modelos
        /// </summary>
        /// <param name="view">la matriz de la camara</param>
        /// <param name="projection">la matriz que define el como se projecta sobre la camara</param>
        /// <param name="color">el color que queremos que tenga el modelo de base</param>
        public void dibujar(Matrix view, Matrix projection, Color color)
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
}