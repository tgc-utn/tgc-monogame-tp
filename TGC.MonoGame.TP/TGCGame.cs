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
        private const float RobotSideSpeed = 100f;
        private const float RobotJumpSpeed = 150f;
        private const float Gravity = 350f;
        private const float RobotRotatingVelocity = 0.06f;
        private const float EPSILON = 0.00001f;


        // Camera to draw the scene
        private Camera.Camera Camera { get; set; }
        
        private GraphicsDeviceManager Graphics { get; }

        // Geometries
        private Model Robot { get; set; }
        private BoxPrimitive BoxPrimitive { get; set; }
        private QuadPrimitive Quad { get; set; }

        

        // Robot internal matrices and vectors
        private Matrix RobotScale { get; set; }
        private Matrix RobotRotation { get; set; }
        private Vector3 RobotPosition { get; set; }
        private Vector3 RobotVelocity { get; set; }
        private Vector3 RobotAcceleration { get; set; }
        private Vector3 RobotFrontDirection { get; set; }
        
        // A boolean indicating if the Robot is on the ground
        private bool OnGround { get; set; }


        // World matrices
        private Matrix BoxWorld { get; set; }
        private Matrix[] StairsWorld { get; set; }
        private Matrix FloorWorld { get; set; }
        private Matrix RobotWorld { get; set; }
        
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

        private BoundingCylinder RobotCylinder { get; set; }
        
        
        
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
            
            // Set the ground flag to false, as the Robot starts in the air
            OnGround = false;

            // Robot position and matrix initialization
            RobotPosition = Vector3.UnitX * 30f;
            RobotScale = Matrix.CreateScale(0.3f);

            RobotCylinder = new BoundingCylinder(RobotPosition, 10f, 20f);
            RobotRotation = Matrix.Identity;
            RobotFrontDirection = Vector3.Backward;

            

            // Create World matrices for our stairs
            StairsWorld = new Matrix[]
            {
                Matrix.CreateScale(70f, 6f, 15f) * Matrix.CreateTranslation(0f, 0f, 0f),
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
            RobotAcceleration = Vector3.Down * Gravity;

            // Initialize the Velocity as zero
            RobotVelocity = Vector3.Zero;


            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            // Load the models
            Robot = Content.Load<Model>(ContentFolder3D + "geometries/sphere");

            // Enable default lighting for the Robot
            foreach (var mesh in Robot.Meshes)
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


            // Calculate the height of the Model of the Robot
            // Create a Bounding Box from it, then subtract the max and min Y to get the height

            // Use the height to set the Position of the robot 
            // (it is half the height, multiplied by its scale in Y -RobotScale.M22-)

            var extents = BoundingVolumesExtensions.CreateAABBFrom(Robot);
            var height = extents.Max.Y - extents.Min.Y;

            RobotPosition += height * 0.5f * Vector3.Up * RobotScale.M22;

            // Assign the center of the Cylinder as the Robot Position
            RobotCylinder.Center = RobotPosition;

            // Update our World Matrix to draw the Robot
            RobotWorld = RobotScale * Matrix.CreateTranslation(RobotPosition);
            

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
                RobotRotation *= Matrix.CreateRotationY(-RobotRotatingVelocity);
                RobotFrontDirection = Vector3.Transform(Vector3.Backward, RobotRotation);
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                RobotRotation *= Matrix.CreateRotationY(RobotRotatingVelocity);
                RobotFrontDirection = Vector3.Transform(Vector3.Backward, RobotRotation);
            }

            // Check for the Jump key press, and add velocity in Y only if the Robot is on the ground
            if (keyboardState.IsKeyDown(Keys.Space) && OnGround)
                RobotVelocity += Vector3.Up * RobotJumpSpeed;

            // Check for key presses and add a velocity in the Robot's Front Direction
            if (keyboardState.IsKeyDown(Keys.W))
                RobotVelocity += RobotFrontDirection * RobotSideSpeed;
            else if (keyboardState.IsKeyDown(Keys.S))
                RobotVelocity -= RobotFrontDirection * RobotSideSpeed;

            // Add the Acceleration to our Velocity
            // Multiply by the deltaTime to have the Position affected by deltaTime * deltaTime
            // https://gafferongames.com/post/integration_basics/
            RobotVelocity += RobotAcceleration * deltaTime;

            // Scale the velocity by deltaTime
            var scaledVelocity = RobotVelocity * deltaTime;

            // Solve the Vertical Movement first (could be done in other order)
            SolveVerticalMovement(scaledVelocity);
            
            // Take only the horizontal components of the velocity
            scaledVelocity = new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            // Solve the Horizontal Movement
            SolveHorizontalMovementSliding(scaledVelocity);


            // Update the RobotPosition based on the updated Cylinder center
            RobotPosition = RobotCylinder.Center;

            // Reset the horizontal velocity, as accumulating this is not needed in this sample
            RobotVelocity = new Vector3(0f, RobotVelocity.Y, 0f);

            // Update the Robot World Matrix
            RobotWorld = RobotScale * RobotRotation * Matrix.CreateTranslation(RobotPosition);
            
            // Actualizo la camara, enviandole la matriz de mundo de la esfera.
            //FollowCamera.Update(gameTime, RobotWorld);
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        ///     Apply horizontal movement, detecting and solving collisions
        /// </summary>
        /// <param name="scaledVelocity">The current velocity scaled by deltaTime</param>
        private void SolveVerticalMovement(Vector3 scaledVelocity)
        {
            // If the Robot has vertical velocity
            if (scaledVelocity.Y == 0f)
                return;

            // Start by moving the Cylinder
            RobotCylinder.Center += Vector3.Up * scaledVelocity.Y;
            // Set the OnGround flag on false, update it later if we find a collision
            OnGround = false;


            // Collision detection
            var collided = false;
            var foundIndex = -1;
            for (var index = 0; index < Colliders.Length; index++)
            {
                if (!RobotCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                    continue;
                
                // If we collided with something, set our velocity in Y to zero to reset acceleration
                RobotVelocity = new Vector3(RobotVelocity.X, 0f, RobotVelocity.Z);

                // Set our index and collision flag to true
                // The index is to tell which collider the Robot intersects with
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
                var cylinderY = RobotCylinder.Center.Y;
                var extents = BoundingVolumesExtensions.GetExtents(collider);

                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (cylinderY > colliderY)
                {
                    penetration = colliderY + extents.Y - cylinderY + RobotCylinder.HalfHeight;
                    OnGround = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -cylinderY - RobotCylinder.HalfHeight + colliderY - extents.Y;

                // Move our Cylinder so we are not colliding anymore
                RobotCylinder.Center += Vector3.Up * penetration;
                collided = false;

                // Check for collisions again
                for (var index = 0; index < Colliders.Length; index++)
                {
                    if (!RobotCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
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
            RobotCylinder.Center += new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            // Check intersection for every collider
            for (var index = 0; index < Colliders.Length; index++)
            {
                if (!RobotCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                    continue;

                // Get the intersected collider and its center
                var collider = Colliders[index];
                var colliderCenter = BoundingVolumesExtensions.GetCenter(collider);

                // The Robot collided with this thing
                // Is it a step? Can the Robot climb it?
                bool stepClimbed = SolveStepCollision(collider, index);

                // If the Robot collided with a step and climbed it, stop here
                // Else go on
                if (stepClimbed)
                    return;

                // Get the cylinder center at the same Y-level as the box
                var sameLevelCenter = RobotCylinder.Center;
                sameLevelCenter.Y = colliderCenter.Y;

                // Find the closest horizontal point from the box
                var closestPoint = BoundingVolumesExtensions.ClosestPoint(collider, sameLevelCenter);

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - closestPoint;
                var normalVectorLength = normalVector.Length();

                // Our penetration is the difference between the radius of the Cylinder and the Normal Vector
                // For precission problems, we push the cylinder with a small increment to prevent re-colliding into the geometry
                var penetration = RobotCylinder.Radius - normalVector.Length() + EPSILON;

                // Push the center out of the box
                // Normalize our Normal Vector using its length first
                RobotCylinder.Center += (normalVector / normalVectorLength * penetration);
            }
            
        }

        /// <summary>
        ///     Solves the intersection between the Robot and a collider.
        /// </summary>
        /// <param name="collider">The collider the Robot intersected with</param>
        /// <param name="colliderIndex">The index of the collider in the collider array the Robot intersected with</param>
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
            var distanceToTop = MathF.Abs((RobotCylinder.Center.Y - RobotCylinder.HalfHeight) - (colliderCenter.Y + extents.Y));
            if (distanceToTop >= 12f)
                return false;

            // We want to climb the step
            // It is climbable if we can reposition our cylinder in a way that
            // it doesn't collide with anything else
            var pastPosition = RobotCylinder.Center;
            RobotCylinder.Center += Vector3.Up * distanceToTop;
            for (int index = 0; index < Colliders.Length; index++)
                if (index != colliderIndex && RobotCylinder.Intersects(Colliders[index]).Equals(BoxCylinderIntersection.Intersecting))
                {
                    // We found a case in which the cylinder
                    // intersects with other colliders, so the climb is not possible
                    RobotCylinder.Center = pastPosition;
                    return false;
                }

            // If we got here the climb was possible
            // (And the Robot position was already updated)
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
            // Robot drawing
            // El dibujo del auto debe ir aca.
            Robot.Draw(RobotWorld, Camera.View, Camera.Projection);

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


            

            base.Draw(gameTime);
        }
    }
}