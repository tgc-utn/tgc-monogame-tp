using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Gemotries.Textures;
using TGC.MonoGame.TP.Pistas;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        
        private const float CameraFollowRadius = 100f;
        private const float CameraUpDistance = 80f;
        private const float SphereSideSpeed = 100f;
        private const float SphereJumpSpeed = 150f;
        private const float Gravity = 350f;
        private const float SphereRotatingVelocity = 0.06f;
        private const float EPSILON = 0.00001f;

        private bool godMode = false;


        // Camera to draw the scene
        private Camera.Camera Camera { get; set; }
        
        private GraphicsDeviceManager Graphics { get; }

        // Geometries
        private Model Sphere { get; set; }
        private BoxPrimitive BoxPrimitive { get; set; }
        private QuadPrimitive Quad { get; set; }

        

        // Sphere internal matrices and vectors
        private Matrix SphereScale { get; set; }
        private Matrix SphereRotation { get; set; }
        private Vector3 SpherePosition { get; set; }
        private Vector3 SphereVelocity { get; set; }
        private Vector3 SphereAcceleration { get; set; }
        private Vector3 SphereFrontDirection { get; set; }
        
        // A boolean indicating if the Sphere is on the ground
        private bool OnGround { get; set; }


        // World matrices
        private Matrix BoxWorld { get; set; }
        private Matrix[] StairsWorld { get; set; }
        private Matrix FloorWorld { get; set; }
        private Matrix SphereWorld { get; set; }
        
        // Camera
        private FollowCamera FollowCamera { get; set; }



        // Textures
        private Texture2D StonesTexture { get; set; }
        private Texture2D WoodenTexture { get; set; }
        private Texture2D CobbleTexture { get; set; }

        
        // Effects

        // Tiling Effect for the floor
        private Effect TilingEffect { get; set; }

        // Effect for the stairs and boxes
        private BasicEffect BoxesEffect { get; set; }

        
        
        // Colliders

        // Bounding Boxes representing our colliders (floor, stairs, boxes)
        private BoundingBox[] Colliders { get; set; }

        private BoundingCylinder SphereCylinder { get; set; }
        
        
        // Pistas
        Pista1 pista1 { get; set; }
        Pista2 pista2 { get; set; }
        /*
         Pista3 pista3 {get; set;}
         Pista4 pista4 {get; set;}
         Pista5 pista5 {get; set;} 
         */
        private Matrix Platform1World { get; set;} 
        
        
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }


        /// <inheritdoc />
        protected override void Initialize()
        {
            // Enciendo Back-Face culling.
            // Configuro Blend State a Opaco.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            // Configuro las dimensiones de la pantalla.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            
                
                
            
            // Creo una camara para seguir a la esfera.
            //FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 20f);
            Camera.BuildProjection(GraphicsDevice.Viewport.AspectRatio, 0.1f, 100000f, MathF.PI / 3f);
            
            // Set the ground flag to false, as the Sphere starts in the air
            OnGround = false;

            // Sphere position and matrix initialization
            SpherePosition = Vector3.UnitX * 30f;
            SphereScale = Matrix.CreateScale(0.2f);

            SphereCylinder = new BoundingCylinder(SpherePosition, 10f, 20f);
            SphereRotation = Matrix.Identity;
            SphereFrontDirection = Vector3.Backward;

            

            // Create World matrices for our stairs
            StairsWorld = new Matrix[]
            {
                Matrix.CreateScale(70f, 6f, 15f) * Matrix.CreateTranslation(0f, 3f, 125f),
                Matrix.CreateScale(70f, 6f, 15f) * Matrix.CreateTranslation(0f, 9f, 140f),
                Matrix.CreateScale(70f, 6f, 15f) * Matrix.CreateTranslation(0f, 15f, 155f),
                Matrix.CreateScale(70f, 6f, 40f) * Matrix.CreateTranslation(0f, 21f, 182.5f),
                Matrix.CreateScale(15f, 6f, 40f) * Matrix.CreateTranslation(-42.5f, 27f, 182.5f),
                Matrix.CreateScale(15f, 6f, 40f) * Matrix.CreateTranslation(-57.5f, 33f, 182.5f),
                Matrix.CreateScale(15f, 6f, 40f) * Matrix.CreateTranslation(-72.5f, 39f, 182.5f),
                Matrix.CreateScale(100f, 6f, 100f) * Matrix.CreateTranslation(-130f, 45f, 152.5f),
            };

            // Create World matrices for the Floor and Box
            FloorWorld = Matrix.CreateScale(200f, 0.001f, 200f);
            BoxWorld = Matrix.CreateScale(30f) * Matrix.CreateTranslation(85f, 15f, -15f);

            // Create Bounding Boxes for the static geometries
            // Stairs + Floor + Box
            Colliders = new BoundingBox[StairsWorld.Length + 2];

            // Instantiate Bounding Boxes for the stairs
            int index = 0;
            for (; index < StairsWorld.Length; index++)
                Colliders[index] = BoundingVolumesExtensions.FromMatrix(StairsWorld[index]);

            // Instantiate a BoundingBox for the Box
            Colliders[index] = BoundingVolumesExtensions.FromMatrix(BoxWorld);
            index++;
            // Instantiate a BoundingBox for the Floor. Note that the height is almost zero
            Colliders[index] = new BoundingBox(new Vector3(-200f, -0.001f, -200f), new Vector3(200f, 0f, 200f));

            // Set the Acceleration (which in this case won't change) to the Gravity pointing down
            SphereAcceleration = Vector3.Down * Gravity;

            // Initialize the Velocity as zero
            SphereVelocity = Vector3.Zero;
            
            pista1 = new Pista1(Content, GraphicsDevice, 100f, -3f, 450f);
            pista2 = new Pista2(Content, GraphicsDevice, 100f, 300f, 450f);
            /*
            pista3 = new Pista3(Content, GraphicsDevice, 100f, 500f, 450f);
            pista4 = new Pista4(Content, GraphicsDevice, 100f, 700f, 450f);
            pista5 = new Pista5(Content, GraphicsDevice, 100f, 1000f, 450f);
             */


            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            // Load the models
            Sphere = Content.Load<Model>(ContentFolder3D + "geometries/sphere");

            // Enable default lighting for the Sphere
            foreach (var mesh in Sphere.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();
            
            // Create a BasicEffect to draw the Box
            BoxesEffect = new BasicEffect(GraphicsDevice);
            BoxesEffect.TextureEnabled = true;

            // Load our Tiling Effect
            TilingEffect = Content.Load<Effect>(ContentFolderEffects + "TextureTiling");
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(10f, 10f));

            // Load Textures
            StonesTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
            WoodenTexture = Content.Load<Texture2D>(ContentFolderTextures + "wood/caja-madera-1");
            CobbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "floor/adoquin");

            // Create our Quad (to draw the Floor)
            Quad = new QuadPrimitive(GraphicsDevice);
            
            // Create our Box Model
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, WoodenTexture);


            // Calculate the height of the Model of the Sphere
            // Create a Bounding Box from it, then subtract the max and min Y to get the height

            // Use the height to set the Position of the robot 
            // (it is half the height, multiplied by its scale in Y -SphereScale.M22-)

            var extents = BoundingVolumesExtensions.CreateAABBFrom(Sphere);
            var height = extents.Max.Y - extents.Min.Y;

            SpherePosition += height * 0.5f * Vector3.Up * SphereScale.M22;

            // Assign the center of the Cylinder as the Sphere Position
            SphereCylinder.Center = SpherePosition;

            // Update our World Matrix to draw the Sphere
            SphereWorld = SphereScale * Matrix.CreateTranslation(SpherePosition);
            

            base.LoadContent();
        }

        

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            // The time that passed between the last loop
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            var keyboardState = Keyboard.GetState();


            // Check for key presses and rotate accordingly
            // We can stack rotations in a given axis by multiplying our past matrix
            // By a new matrix containing a new rotation to apply
            // Also, recalculate the Front Directoin
            if (keyboardState.IsKeyDown(Keys.D))
            {
                SphereRotation *= Matrix.CreateRotationY(-SphereRotatingVelocity);
                SphereFrontDirection = Vector3.Transform(Vector3.Backward, SphereRotation);
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                SphereRotation *= Matrix.CreateRotationY(SphereRotatingVelocity);
                SphereFrontDirection = Vector3.Transform(Vector3.Backward, SphereRotation);
            }

            if (keyboardState.IsKeyDown(Keys.G)) {
                if (!godMode) godMode = true;
                else godMode = false;
            }

            // Check for the Jump key press, and add velocity in Y only if the Sphere is on the ground
            if (keyboardState.IsKeyDown(Keys.Space) && OnGround && !godMode)
                SphereVelocity += Vector3.Up * SphereJumpSpeed;

            // Check for key presses and add a velocity in the Sphere's Front Direction
            if (keyboardState.IsKeyDown(Keys.W))
                SphereVelocity += SphereFrontDirection * SphereSideSpeed;
            else if (keyboardState.IsKeyDown(Keys.S))
                SphereVelocity -= SphereFrontDirection * SphereSideSpeed;

            // Add the Acceleration to our Velocity
            // Multiply by the deltaTime to have the Position affected by deltaTime * deltaTime
            // https://gafferongames.com/post/integration_basics/
            SphereVelocity += SphereAcceleration * deltaTime;

            // Scale the velocity by deltaTime
            var scaledVelocity = SphereVelocity * deltaTime;

            // Solve the Vertical Movement first (could be done in other order)
            SolveVerticalMovement(scaledVelocity);
            
            // Take only the horizontal components of the velocity
            scaledVelocity = new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            // Solve the Horizontal Movement
            SolveHorizontalMovementSliding(scaledVelocity);


            // Update the SpherePosition based on the updated Cylinder center
            SpherePosition = SphereCylinder.Center;

            // Reset the horizontal velocity, as accumulating this is not needed in this sample
            SphereVelocity = new Vector3(0f, SphereVelocity.Y, 0f);

            // Update the Sphere World Matrix
            SphereWorld = SphereScale * SphereRotation * Matrix.CreateTranslation(SpherePosition);
            
            // Actualizo la camara, enviandole la matriz de mundo de la esfera.
            //FollowCamera.Update(gameTime, SphereWorld);
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        ///     Apply horizontal movement, detecting and solving collisions
        /// </summary>
        /// <param name="scaledVelocity">The current velocity scaled by deltaTime</param>
        private void SolveVerticalMovement(Vector3 scaledVelocity)
        {
            // If the Sphere has vertical velocity
            if (scaledVelocity.Y == 0f)
                return;

            // Start by moving the Cylinder
            SphereCylinder.Center += Vector3.Up * scaledVelocity.Y;
            // Set the OnGround flag on false, update it later if we find a collision
            OnGround = false;


            // Collision detection
            var collided = false;
            var foundIndex = -1;
            for (var index = 0; index < Colliders.Length; index++)
            {
                if (!SphereCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                    continue;
                
                // If we collided with something, set our velocity in Y to zero to reset acceleration
                SphereVelocity = new Vector3(SphereVelocity.X, 0f, SphereVelocity.Z);

                // Set our index and collision flag to true
                // The index is to tell which collider the Sphere intersects with
                collided = true;
                foundIndex = index;
                break;
            }


            // We correct based on differences in Y until we don't collide anymore
            // Not usual to iterate here more than once, but could happen
            while (collided)
            {
                var collider = Colliders[foundIndex];
                var colliderY = BoundingVolumesExtensions.GetCenter(collider).Y;
                var cylinderY = SphereCylinder.Center.Y;
                var extents = BoundingVolumesExtensions.GetExtents(collider);

                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (cylinderY > colliderY)
                {
                    penetration = colliderY + extents.Y - cylinderY + SphereCylinder.HalfHeight;
                    OnGround = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -cylinderY - SphereCylinder.HalfHeight + colliderY - extents.Y;

                // Move our Cylinder so we are not colliding anymore
                SphereCylinder.Center += Vector3.Up * penetration;
                collided = false;

                // Check for collisions again
                for (var index = 0; index < Colliders.Length; index++)
                {
                    if (!SphereCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                        continue;

                    // Iterate until we don't collide with anything anymore
                    collided = true;
                    foundIndex = index;
                    break;
                }
            }
            
        }

        /// <summary>
        ///     Apply horizontal movement, detecting and solving collisions with sliding
        /// </summary>
        /// <param name="scaledVelocity">The current velocity scaled by deltaTime</param>
        private void SolveHorizontalMovementSliding(Vector3 scaledVelocity)
        {
            // Has horizontal movement?
            if (Vector3.Dot(scaledVelocity, new Vector3(1f, 0f, 1f)) == 0f)
                return;
            
            // Start by moving the Cylinder horizontally
            SphereCylinder.Center += new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            // Check intersection for every collider
            for (var index = 0; index < Colliders.Length; index++)
            {
                if (!SphereCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                    continue;

                // Get the intersected collider and its center
                var collider = Colliders[index];
                var colliderCenter = BoundingVolumesExtensions.GetCenter(collider);

                // The Sphere collided with this thing
                // Is it a step? Can the Sphere climb it?
                bool stepClimbed = SolveStepCollision(collider, index);

                // If the Sphere collided with a step and climbed it, stop here
                // Else go on
                if (stepClimbed)
                    return;

                // Get the cylinder center at the same Y-level as the box
                var sameLevelCenter = SphereCylinder.Center;
                sameLevelCenter.Y = colliderCenter.Y;

                // Find the closest horizontal point from the box
                var closestPoint = BoundingVolumesExtensions.ClosestPoint(collider, sameLevelCenter);

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - closestPoint;
                var normalVectorLength = normalVector.Length();

                // Our penetration is the difference between the radius of the Cylinder and the Normal Vector
                // For precission problems, we push the cylinder with a small increment to prevent re-colliding into the geometry
                var penetration = SphereCylinder.Radius - normalVector.Length() + EPSILON;

                // Push the center out of the box
                // Normalize our Normal Vector using its length first
                SphereCylinder.Center += (normalVector / normalVectorLength * penetration);
            }
            
        }

        /// <summary>
        ///     Solves the intersection between the Sphere and a collider.
        /// </summary>
        /// <param name="collider">The collider the Sphere intersected with</param>
        /// <param name="colliderIndex">The index of the collider in the collider array the Sphere intersected with</param>
        /// <returns>True if the collider was a step and it was climbed, False otherwise</returns>
        private bool SolveStepCollision(BoundingBox collider, int colliderIndex)
        {
            // Get the collider properties to check if it's a step
            // Also, to calculate penetration
            var extents = BoundingVolumesExtensions.GetExtents(collider);
            var colliderCenter = BoundingVolumesExtensions.GetCenter(collider);

            // Is this collider a step?
            // If not, exit
            if (extents.Y >= 6f)
                return false;

            // Is the base of the cylinder close to the step top?
            // If not, exit
            var distanceToTop = MathF.Abs((SphereCylinder.Center.Y - SphereCylinder.HalfHeight) - (colliderCenter.Y + extents.Y));
            if (distanceToTop >= 12f)
                return false;

            // We want to climb the step
            // It is climbable if we can reposition our cylinder in a way that
            // it doesn't collide with anything else
            var pastPosition = SphereCylinder.Center;
            SphereCylinder.Center += Vector3.Up * distanceToTop;
            for (int index = 0; index < Colliders.Length; index++)
                if (index != colliderIndex && SphereCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                {
                    // We found a case in which the cylinder
                    // intersects with other colliders, so the climb is not possible
                    SphereCylinder.Center = pastPosition;
                    return false;
                }

            // If we got here the climb was possible
            // (And the Sphere position was already updated)
            return true;
        }



        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            // Limpio la pantalla.
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // Calculate the ViewProjection matrix
            //var viewProjection = FollowCamera.View * FollowCamera.Projection;
            var viewProjection = Camera.View * Camera.Projection;
            // Sphere drawing
            // El dibujo del auto debe ir aca.
            Sphere.Draw(SphereWorld, Camera.View, Camera.Projection);

            // Floor drawing
            
            // Set the Technique inside the TilingEffect to "BaseTiling", we want to control the tiling on the floor
            // Using its original Texture Coordinates
            TilingEffect.CurrentTechnique = TilingEffect.Techniques["BaseTiling"];
            // Set the Tiling value
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(10f, 10f));
            // Set the WorldViewProjection matrix
            TilingEffect.Parameters["WorldViewProjection"].SetValue(FloorWorld * viewProjection);
            // Set the Texture that the Floor will use
            TilingEffect.Parameters["Texture"].SetValue(StonesTexture);
            Quad.Draw(TilingEffect);


            // Steps drawing

            // Set the Technique inside the TilingEffect to "WorldTiling"
            // We want to use the world position of the steps to define how to sample the Texture
            TilingEffect.CurrentTechnique = TilingEffect.Techniques["WorldTiling"];
            // Set the Texture that the Steps will use
            TilingEffect.Parameters["Texture"].SetValue(CobbleTexture);
            // Set the Tiling value
            TilingEffect.Parameters["Tiling"].SetValue(Vector2.One * 0.05f);

            // Draw every Step
            for (int index = 0; index < StairsWorld.Length; index++)
            {
                // Get the World Matrix
                var matrix = StairsWorld[index];
                // Set the World Matrix
                TilingEffect.Parameters["World"].SetValue(matrix);
                // Set the WorldViewProjection Matrix
                TilingEffect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
                BoxPrimitive.Draw(TilingEffect);
            }


            // Draw the Box, setting every matrix and its Texture
            BoxesEffect.World = BoxWorld;
            BoxesEffect.View = Camera.View;
            BoxesEffect.Projection = Camera.Projection;

            BoxesEffect.Texture = WoodenTexture;
            BoxPrimitive.Draw(BoxesEffect);

            
            pista1.Draw(Camera.View,Camera.Projection);
            pista2.Draw(Camera.View, Camera.Projection);
            //pista3.Draw(Camera.View, Camera.Projection);
            //pista4.Draw(Camera.View, Camera.Projection);
            //pista5.Draw(Camera.View, Camera.Projection);



            base.Draw(gameTime);
        }
    }
}