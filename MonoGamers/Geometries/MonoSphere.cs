using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGamers.Camera;
using NumericVector3 = System.Numerics.Vector3;
using MonoGamers.Audio;
using MonoGamers.Utilities;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace MonoGamers.Geometries
{
    internal class MonoSphere
    {

        //Tipo de esfera
        enum Type {
            Common,
            Stone,
            Metal,
            Gum
        }

        Type SphereType;
        //Referencia de material afuera:
        public string Material { get; set; }

        private const float StandardSideSpeed = 500f;
        private const float standardJumpSpeed = 55000f;
        private const float Friction = 0.5f;
        //private const float maxSpeed = 150f;


        public float SphereSideSpeed;
        public float SphereJumpSpeed;

        public bool rushed;

        public SpherePrimitive SpherePrimitive { get; set; }

        public Model SphereModel { get; set; }
        
        // Sphere internal matrices and vectors
        public Matrix SphereRotation { get; set; }
        public Vector3 SpherePosition { get; set; }
        public Vector3 SphereVelocity { get; set; }
        public Vector3 SphereAcceleration { get; set; }
        public Vector3 SphereFrontDirection { get; set; }
        public Vector3 SphereLateralDirection { get; set; }
        public Matrix SphereWorld { get; set; }


        // A boolean indicating if the Sphere is on the ground
        public bool OnGround { get; set; }

        // Textures
        public Texture2D SphereCommonTexture { get; set; }
        
        public Texture2D SphereCommonNormalTexture { get; set; }
        public Texture2D SphereStoneTexture { get; set; }
        public Texture2D SphereMetalTexture { get; set; }
        public Texture2D SphereGumTexture { get; set; }

        // Effect
        public Effect SphereEffect { get; set; }
        

        //handler
        public BodyHandle SphereHandle { get; set; }

        public Sphere SphereShape { get; set; }

        public float velocidadAngularYAnt;
        public float velocidadLinearYAnt;

        public float SphereSideTypeSpeed;
        public float SphereJumpTypeSpeed;
        
        public bool godMode;

        private bool GwasDown = false;
        

        public MonoSphere(Vector3 InitialPosition, float Gravity, Simulation Simulation)
        {
            OnGround = true;
            godMode = false;

            SphereWorld = new Matrix();
            SphereSideSpeed = StandardSideSpeed;
            SphereJumpSpeed = standardJumpSpeed;

            SphereType = Type.Common;
            Material = "Common";

            SpherePosition = InitialPosition;
            SphereRotation = Matrix.Identity;
            SphereFrontDirection = Vector3.Backward;
            SphereLateralDirection = Vector3.Right;
            // Set the Acceleration (which in this case won't change) to the Gravity pointing down
            SphereAcceleration = Vector3.Down * Gravity;

            SphereHandle = new BodyHandle();
            
            SphereShape = new Sphere(10f);
            var position = Utils.ToNumericVector3(SpherePosition);
            var initialVelocity = new BodyVelocity(new NumericVector3((float)0f, 0f, 0f));
            var mass = SphereShape.Radius * SphereShape.Radius * SphereShape.Radius ;
            var bodyDescription = BodyDescription.CreateConvexDynamic(position, initialVelocity, mass, Simulation.Shapes, SphereShape);
            SphereHandle = Simulation.Bodies.Add(bodyDescription);

            // Initialize the Velocity as zero
            SphereVelocity = Vector3.Zero;


        }

        public void Update(Simulation Simulation, TargetCamera Camera, KeyboardState KeyboardState)
        {
            var sphereBody = Simulation.Bodies.GetBodyReference(SphereHandle);
            sphereBody.Awake = true;
            SphereRotation = Camera.CameraRotation;
            SphereFrontDirection = Vector3.Transform(Vector3.Backward, SphereRotation);
            SphereLateralDirection = Vector3.Transform(Vector3.Right, SphereRotation);
            
            if(SphereType == Type.Common){
                SphereSideTypeSpeed = 1f;
                SphereJumpTypeSpeed = 1f;
            }

            if(SphereType == Type.Metal){
                SphereSideTypeSpeed = 2f;
                SphereJumpTypeSpeed = 1f;
            }

            if(SphereType == Type.Gum){
                SphereSideTypeSpeed = 1f;
                SphereJumpTypeSpeed = 2f;
            }

            if(SphereType == Type.Stone){
                SphereSideTypeSpeed = 0.5f;
                SphereJumpTypeSpeed = 0.75f;
            }

            //Cambio de tipo de esfera manualmente
            
            if (KeyboardState.IsKeyUp(Keys.G)&& GwasDown) {
                if (!godMode) godMode = true;
                else godMode = false;
            }

            GwasDown = KeyboardState.IsKeyDown(Keys.G);
            if (KeyboardState.IsKeyDown(Keys.T)){
                Material = "Common";
                SphereType = Type.Common;
            }
            if (KeyboardState.IsKeyDown(Keys.Y)){
                Material = "Metal";
                SphereType = Type.Metal;
            }
            if (KeyboardState.IsKeyDown(Keys.U)){
                Material = "Gum";
                SphereType = Type.Gum;
            }
            if (KeyboardState.IsKeyDown(Keys.I)){
                Material = "Stone";
                SphereType = Type.Stone;
            }
            
            if (KeyboardState.IsKeyDown(Keys.D1) && godMode) {
                sphereBody.Pose = new NumericVector3(100f, 10f, 160f);
            }            
            if (KeyboardState.IsKeyDown(Keys.D2) && godMode) {
                sphereBody.Pose = new NumericVector3(100f, 20f, 4580f);
            }
            if (KeyboardState.IsKeyDown(Keys.D3) && godMode) {
                sphereBody.Pose = new NumericVector3(2090f, 150f, 7144f);
            }
            if (KeyboardState.IsKeyDown(Keys.D4) && godMode) {
                sphereBody.Pose = new NumericVector3(3400f, 343f, 7200f);
            } 

            if (KeyboardState.IsKeyDown(Keys.D)) LateralMove( -1f);
            if (KeyboardState.IsKeyDown(Keys.A)) LateralMove(1f);
            if (KeyboardState.IsKeyDown(Keys.W)) FrontalMove( 1f);
            if (KeyboardState.IsKeyDown(Keys.S)) FrontalMove( -1f);

            if (MathHelper.Distance(sphereBody.Velocity.Linear.Y, velocidadLinearYAnt) < 0.1
                    && MathHelper.Distance(sphereBody.Velocity.Angular.Y, velocidadAngularYAnt) < 0.1)
                OnGround = true; // Se revisa que la velocidad lineal como la angular de la esfera en Y, su distancia se menor a 0,1 con respecto a la velocidad anterior

            if (KeyboardState.IsKeyDown(Keys.Space) && OnGround) Jump();

            /*if (KeyboardState.GetPressedKeys().Length > 0) ApplyImpulse(ref sphereBody, 2f);
            ApplyStop(ref sphereBody);*/
            ApplyImpulse(ref sphereBody, 2f);

            if (rushed) ApplyRush(ref sphereBody);

            velocidadAngularYAnt = sphereBody.Velocity.Angular.Y;
            velocidadLinearYAnt = sphereBody.Velocity.Linear.Y;

            var pose = Simulation.Bodies.GetBodyReference(SphereHandle).Pose;
            SpherePosition = pose.Position;
            SphereWorld = Matrix.CreateScale(SphereShape.Radius) *
                Matrix.CreateFromQuaternion(pose.Orientation) * Matrix.CreateTranslation(SpherePosition);

            SphereVelocity = Vector3.Zero;
        }
        

        private void LateralMove( float Sense)
        {
            //var speedOntoDirectionSense = (ActualSpeed * Sense * SphereLateralDirection * SphereLateralDirection / SphereLateralDirection.Length()).Length();

            if (OnGround)
            {
                SphereVelocity = Sense * SphereLateralDirection * SphereSideSpeed * SphereSideTypeSpeed;
            }
            else
            {
                SphereVelocity = Sense * SphereLateralDirection * SphereSideSpeed * SphereSideTypeSpeed *0.2f; 
            }
        }

        private void FrontalMove(float Sense)
        {
            /*var speedOntoDirection = (sphereBody.Velocity.Linear * Sense * SphereFrontDirection * SphereFrontDirection / (SphereFrontDirection.Length() * SphereFrontDirection.Length())).Length();*/

            if (OnGround)
            {
                SphereVelocity = Sense * SphereFrontDirection * SphereSideSpeed * SphereSideTypeSpeed;
            }
            else
            {
                SphereVelocity = Sense * SphereFrontDirection * SphereSideSpeed * SphereSideTypeSpeed * 0.2f; 
            }
            

            
        }

        public void Jump()
        {
            SphereVelocity += Vector3.Up * SphereJumpSpeed * SphereJumpTypeSpeed;
            AudioController.PlayJump();
            OnGround = false;
        }

        public void ApplyImpulse(ref BodyReference sphereBody, float intensity)
        {
            sphereBody.ApplyLinearImpulse(new NumericVector3(SphereVelocity.X * intensity,
                SphereVelocity.Y * intensity,
                SphereVelocity.Z * intensity));
        }
        public void ApplyStop(ref BodyReference sphereBody)
        {
            if (sphereBody.MotionState.Velocity.Linear.LengthSquared() > 0.0001f)
                sphereBody.ApplyLinearImpulse(-sphereBody.MotionState.Velocity.Linear * Friction);
        } 
        
        public void ApplyRush(ref BodyReference sphereBody)
        {
            ApplyImpulse(ref sphereBody, 10f);
            rushed = false;
        }


        public bool SphereFalling(float LimitY)
        {
            return SpherePosition.Y < LimitY;
        }

        public void Draw(Camera.Camera camera){
            /*
            SphereEffect.CurrentTechnique = SphereEffect.Techniques["BasicColorDrawing"];
            SphereEffect.Parameters["View"].SetValue(camera.View);
            SphereEffect.Parameters["Projection"].SetValue(camera.Projection);
            SphereEffect.Parameters["World"].SetValue(SphereWorld);
            */
            var viewProjection = camera.View * camera.Projection;
            SphereEffect.Parameters["eyePosition"].SetValue(camera.Position);
            SphereEffect.Parameters["World"].SetValue(SphereWorld);
            SphereEffect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(SphereWorld)));
            SphereEffect.Parameters["WorldViewProjection"].SetValue(SphereWorld * viewProjection);
            SphereEffect.Parameters["Tiling"].SetValue(Vector2.One);


            
            if(SphereType == Type.Common) {
                //SphereEffect.Parameters["ModelTexture"].SetValue(SphereCommonTexture);
                
                SphereEffect.Parameters["diffuseColor"].SetValue((Color.LightGray).ToVector3());
                SphereEffect.Parameters["specularColor"].SetValue((Color.LightGoldenrodYellow).ToVector3());
                SphereEffect.Parameters["KSpecular"].SetValue(1.0f);
                
                SphereEffect.Parameters["ModelTexture"].SetValue(SphereCommonTexture);
                SphereEffect.Parameters["NormalTexture"].SetValue(SphereCommonNormalTexture);
            }
            if(SphereType == Type.Gum) {
                SphereEffect.Parameters["ModelTexture"].SetValue(SphereGumTexture);
            }
            if(SphereType == Type.Metal) {
                SphereEffect.Parameters["ModelTexture"].SetValue(SphereMetalTexture);
            }
            if(SphereType == Type.Stone) {
                SphereEffect.Parameters["ModelTexture"].SetValue(SphereStoneTexture);
            }
            
            //SpherePrimitive.Draw(SphereEffect);

            foreach (var modelMesh in SphereModel.Meshes)
            { 
                modelMesh.Draw(); 
            }
        }
    }


}