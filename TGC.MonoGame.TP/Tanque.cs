using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP
{    
    public class Tanque
    {
        private float Speed { get; set; }
        
        Matrix RotationMatrix = Matrix.Identity;

        protected Model Model { get; set; }
        public Matrix World { get; set; }

        protected Texture2D Texture { get; set; }
        public Vector3 Position{ get; set; }
        protected float Rotation{ get; set; }
        protected Effect Effect { get; set; }

        private ModelBone Torreta;
        private ModelBone Cannon;
        private Matrix TorretaMatrix;
        private Matrix CannonMatrix;
        


        public Tanque(Vector3 Position, Model modelo, Effect efecto, Texture2D textura){
            this.Position = Position;

            World = Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
            
            Model = modelo;

            Effect = efecto;

            Texture = textura;

            Torreta = modelo.Bones["Turret"];
            TorretaMatrix = Torreta.Transform;

            Cannon = modelo.Bones["Cannon"];
            CannonMatrix = Cannon.Transform;

            
            TankVelocity = Vector3.Zero;
        }

        public void LoadContent(){

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.

            //Effect.Parameters["ModelTexture"].SetValue(Texture);

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
            Effect.Parameters["ModelTexture"]?.SetValue(Texture);

            
            var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
            
            // forma de girar la torreta con cañón
            // Torreta.Transform = Torreta.Transform * Matrix.CreateRotationZ(0.006f);
            // Cannon.Transform = Cannon.Transform * Matrix.CreateRotationZ(0.006f);
            
            World = RotationMatrix * Matrix.CreateTranslation(Position);
            foreach (var mesh in Model.Meshes)
            {
                var meshWorld = modelMeshesBaseTransforms[mesh.ParentBone.Index];
                Effect.Parameters["World"].SetValue(meshWorld*World);
                mesh.Draw();
            }
        }

        
        private Vector3 TankVelocity  { get; set; }
        private Vector3 TankDirection  { get; set; }
        
        private Boolean Moving  { get; set; }
        int Sentido;
        float Acceleration = 7f;
        float CurrentAcceleration = 0;
        public void Update(GameTime gameTime, KeyboardState key){
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            float moduloVelocidadXZ = new Vector3(TankVelocity.X, 0f, TankVelocity.Z).Length();
            
            if(key.IsKeyDown(Keys.W)){ //adeltante
                Moving = true;
                CurrentAcceleration = 1;
                Sentido = 1;
            }
            if(key.IsKeyDown(Keys.S)){ //reversa                
                Moving = true;
                CurrentAcceleration = -1;
                Sentido = -1;
            }
            if(key.IsKeyDown(Keys.A) && Moving){ 
                RotationMatrix *= Matrix.CreateRotationY(.03f);
                TankDirection = Vector3.Transform(Vector3.Forward, RotationMatrix); 
                TankVelocity = TankDirection * moduloVelocidadXZ * Sentido + new Vector3(0f, TankVelocity.Y, 0f); 
            }                
            if(key.IsKeyDown(Keys.D) && Moving){
                RotationMatrix *= Matrix.CreateRotationY(-.03f);
                TankDirection = Vector3.Transform(Vector3.Forward, RotationMatrix); 
                TankVelocity = TankDirection * moduloVelocidadXZ * Sentido + new Vector3(0f, TankVelocity.Y, 0f); 
            }

            Position += TankVelocity;                               // Actualizo la posición en función de la velocidad actual
            CurrentAcceleration *= Acceleration;                    // Decido la aceleración


            // **** Manejo de aceleración y renderizado del auto en el world **** //


            if(Moving && moduloVelocidadXZ < 20f){                  // aceleración sobre el plano XZ
                TankVelocity += TankDirection * CurrentAcceleration * deltaTime;
            }
            else if(TankVelocity.X != 0 || TankVelocity.Z != 0) {   // frenada del auto con desaceleración
                TankVelocity -= TankVelocity * 2f * deltaTime; 
                if(moduloVelocidadXZ < 0.1f){                       // analizo el módulo del vector velocidad del plano XZ
                    TankVelocity = new Vector3(0f, TankVelocity.Y, 0f);
                    Sentido = 0;
                    CurrentAcceleration = 0;
                } 
            } 

            Moving = false;

            System.Console.WriteLine(Position);

            //El auto en el world
            World = RotationMatrix * Matrix.CreateTranslation(Position) ;

        }
    }
}