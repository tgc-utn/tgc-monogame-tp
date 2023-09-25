using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Collisions;


namespace TGC.MonoGame.TP
{    
    public class TanqueEnemigo
    {
        // Colisión
        public OrientedBoundingBox TankBox { get; set; }
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
        


        public TanqueEnemigo(Vector3 Position, Model modelo, Effect efecto, Texture2D textura){
            this.Position = Position;

            World = Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
            
            Model = modelo;
            
            
            var AABB = BoundingVolumesExtensions.CreateAABBFrom(Model);
            TankBox = OrientedBoundingBox.FromAABB(AABB);
            TankBox.Center = Position;
            TankBox.Orientation = RotationMatrix;

            Effect = efecto;

            Texture = textura;

            Torreta = modelo.Bones["Turret"];
            TorretaMatrix = Torreta.Transform;

            Cannon = modelo.Bones["Cannon"];
            CannonMatrix = Cannon.Transform;

            TankDirection = Vector3.Forward;


            
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

        
        public Vector3 TankVelocity  { get; set; }
        private Vector3 TankDirection  { get; set; }
        
        private Boolean Moving  { get; set; }
        int Sentido;
        float Acceleration = 7f;
        float CurrentAcceleration = 0;

        public bool Intersecta(Object objeto){
            return TankBox.Intersects(objeto.Box);
        }

        public void Update(GameTime gameTime, List<Object> ambiente){
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            float moduloVelocidadXZ = new Vector3(TankVelocity.X, 0f, TankVelocity.Z).Length();
            
            Position += TankVelocity;                               // Actualizo la posición en función de la velocidad actual

            TankBox.Center = Position;
            TankBox.Orientation = RotationMatrix;
            
            if(TankVelocity.X != 0 || TankVelocity.Z != 0) {   // frenada del auto con desaceleración
                TankVelocity -= TankVelocity * deltaTime; 
                if(moduloVelocidadXZ < 0.1f){                       
                    TankVelocity = new Vector3(0f, TankVelocity.Y, 0f);
                    Sentido = 0;
                    CurrentAcceleration = 0;
                } 
            } 

            foreach (Object itemEspecifico in ambiente){
                if(Intersecta(itemEspecifico)){
                    if(itemEspecifico.esEliminable){
                        itemEspecifico.esVictima = true;
                    }                        
                    else{
                        //Console.WriteLine("Velocidad del tanque: " + TankVelocity);
                        Sentido *= -1;
                        TankVelocity = - Vector3.Normalize(TankVelocity) * 20;
                        //Console.WriteLine("Velocidad contraria: " + TankVelocity);
                        
                        //Vector3.Clamp()
                    }
                        
                }
            }

            //System.Console.WriteLine("Enemigos rotatio: " + TankBox.Orientation.Equals(RotationMatrix));

 
            //System.Console.WriteLine(Position);

            //El auto en el world
            World = RotationMatrix * Matrix.CreateTranslation(Position) ;

        }
        public void agregarVelocidad(Vector3 velocidad){
            TankVelocity += velocidad;
        }
        public bool Intersecta(BoundingBox objectoAAnalizar){
            return TankBox.Intersects(objectoAAnalizar);
        }

        public bool Intersecta(List<Object> listaDeObjetos){

            foreach (var objeto in listaDeObjetos)
            {
                if(TankBox.Intersects(objeto.Box) && objeto.esEliminable)
                    objeto.esVictima = true;                    
            }
            return true;
        }
    }
}