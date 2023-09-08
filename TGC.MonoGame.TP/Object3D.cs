using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP
{    
    public class Object
    {
        
        private Model Model { get; set; }
        public Matrix World { get; set; }

        private Texture2D Texture { get; set; }
        public Vector3 Position{ get; set; }
        private float Rotation{ get; set; }
        private Effect Effect { get; set; }

        public Object(Vector3 Position, Model modelo, Effect efecto, Texture2D textura){
            this.Position = Position;

            World = Matrix.CreateTranslation(Position);
            
            Model = modelo;

            Effect = efecto;

            Texture = textura;
        }

        public void LoadContent(){

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.

            Effect.Parameters["ModelTexture"].SetValue(Texture);

            // Al mesh le asigno el Effect (solo textura por ahora)
            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            // Tanto la vista como la proyección vienen de la cámara por parámetro
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in Model.Meshes)
            {
                var MeshWorld = mesh.ParentBone.Transform;
                Effect.Parameters["World"].SetValue(MeshWorld*World);
                mesh.Draw();
            }
        }
    }

}