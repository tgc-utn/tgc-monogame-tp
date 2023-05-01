using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Content.Models;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Clase principal del juego.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        /* * * * * AGREGADO POR MANU * * * * */
        
        private const float maxVelocity = 0.7f;
        private const float breakCapacity = 0.002f;
        private bool saltando = false;
        private float jumpStart = 0f;
        private Vector2 upNdown;
        private float carHeigth = 0f;
        private Vector3 carRotation = new Vector3(0f,0f,0f);
        private Vector2 carAceleration = Vector2.Zero;
        private Model CarModel { get; set; }
        private Vector3 direccionMovimiento;
        private Vector3 PosicionActual = Vector3.Zero;
        /* * * * * FIN AGREGADO * * * * */

        private GraphicsDeviceManager Graphics { get; }
        private CityScene City { get; set; }

        private Matrix CarWorld { get; set; }
        private FollowCamera FollowCamera { get; set; }


        ///     Constructor del juego.
        public TGCGame()
        {
            // Se encarga de la configuracion y administracion del Graphics Device.
            Graphics = new GraphicsDeviceManager(this);

            // Carpeta donde estan los recursos que vamos a usar.
            Content.RootDirectory = "Content";

            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        ///     Llamada una vez en la inicializacion de la aplicacion.
        ///     Escribir aca todo el codigo de inicializacion: Todo lo que debe estar precalculado para la aplicacion.
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

            // Creo una camara para seguir a nuestro auto.
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            // Configuro la matriz de mundo del auto.
            CarWorld = Matrix.Identity;

            /* * * * * AGREGADO POR MANU * * * * */

            direccionMovimiento.X = 0f;
            direccionMovimiento.Y = 0f;
            direccionMovimiento.Z = 0f;
            upNdown.X = 0f;
            upNdown.Y = 0f;
            saltando = false;

            /* * * * *  FIN AGREGADO     * * * * */

            base.Initialize();
        }

        ///     Llamada una sola vez durante la inicializacion de la aplicacion, luego de Initialize, y una vez que fue configurado GraphicsDevice.
        ///     Debe ser usada para cargar los recursos y otros elementos del contenido.
        protected override void LoadContent()
        {
            // Creo la escena de la ciudad.
            City = new CityScene(Content);

            // La carga de contenido debe ser realizada aca.

            /* * * * * AGREGADO POR MANU * * * * */
            CarModel = Content.Load<Model>(ContentFolder3D + "scene/car");
            /* * * * * FIN AGREGADO * * * * */

            base.LoadContent();
        }

        /* * * * * AGREGADO DE MANU   * * * */
        /* * * FUNCIONES DE MOVIMIENTO  * * */
        /// <summary> Gira la matriz de auto sobre su eje en el sentido indicado </summary>
        private void turn(Matrix posicionActual, Vector3 sentido){
            // Para girar tiene que ir para la izquierda
            CarWorld = Matrix.Identity;

            // La capacidad de giro depende directamente de la aceleración del auto
            var velocidadGiro = 0.06f*carAceleration.X;
            if(carAceleration.X<0.5f && carAceleration.X>-0.7f){
                velocidadGiro = 0.04f*carAceleration.X;
            }
            var rotacion = velocidadGiro * sentido.Y;

            // Si el sentido fuera un vector, podría reutilizarse
            CarWorld = Matrix.CreateRotationY(rotacion);
            CarWorld *= posicionActual;

            // Actualizo el ángulo que ya giró el auto
            carRotation.Y += rotacion;                 
        }
        /// <summary> Girar sobre su eje al auto a la derecha </summary>
        private void turnRight(Matrix posicionActual){
            var right = new Vector3(0f,-1f,0f);
            turn(posicionActual,right);
            return;
        }
        /// <summary> Girar sobre su eje auto a la izquierda </summary>
        private void turnLeft(Matrix posicionActual){
            var left = new Vector3(0f,1f,0f);
            turn(posicionActual,left);
            return;
        }
        /// <summary> Acelerar el auto </summary>
        private void accelerate(float relativeAmount){
            // Se le puede agregar fricción del material pero por ahora lo regulo con esto
            var materialFriction = breakCapacity;

            if(carAceleration.X<1f)
                carAceleration.X += (materialFriction * 1.75f) * relativeAmount;
            else carAceleration.X = 1f;
            return;
        }
        /// <summary> Desacelerar el auto. (Movimiento en reversa) </summary>
        private void decelerate(float relativeAmount){
            // Se le puede agregar fricción del material pero por ahora lo regulo con esto
            // no está bueno que dependa de la capacidad del frenado
            var materialFriction = breakCapacity;
            

            if(carAceleration.X>0f) carAceleration.X -= (materialFriction*1.2f)*relativeAmount;
            else if(carAceleration.X>-1f) carAceleration.X -= (materialFriction*1.4f)*relativeAmount;
            else carAceleration.X = -1f;
        }
        /// <summary> Mover el auto en la dirección que está yendo y según su aceleración </summary>
        private void move(){
            CarWorld *= Matrix.CreateTranslation(
                                            direccionMovimiento.X* carAceleration.X,
                                            direccionMovimiento.Y,
                                            direccionMovimiento.Z*carAceleration.X);
        }
        /// <summary> Mover el auto a determinada velocidad con un factor relativo </summary>
        private void moveAtSpeed(float speed, float relativeAmount){            
            // Velocidad * (f.trigonométrica) * (tiempo transcurrido para hacerlo independiente a los fps)
            direccionMovimiento.X = ( -speed * MathF.Sin(carRotation.Y) ) * relativeAmount;
            direccionMovimiento.Z = ( -speed * MathF.Cos(carRotation.Y) ) * relativeAmount;

            move();

            PosicionActual.X += carAceleration.X*( -speed * MathF.Sin(carRotation.Y) ) * relativeAmount;
            PosicionActual.Z += carAceleration.X*( -speed * MathF.Cos(carRotation.Y) ) * relativeAmount;
            
            return;
        }
        /// <summary> Seguir moviendo desde la posición actual, respetando el giro en que venía y dirección </summary>
        private void keepMovingFrom(){
            // Guardo la matriz sin la rotación 
            //var currentPosition = CarWorld;
            var currentPosition = Matrix.Identity
                                * Matrix.CreateTranslation(PosicionActual*-speed);
            //posicionAux *= Matrix.CreateRotationY(-carRotation.Y);
            //posicionAux *= actualPosition;
            
            // Llevo mi auto al centro (por el giro)
            CarWorld = Matrix.CreateRotationY(carRotation.Y);
            // Lo mantengo en la dirección en que estaba
            move();

            // Lo devuelvo a su posición
            CarWorld *= currentPosition;
            return;
        }
        /// <summary> Desacelerar por fricción </summary>
        private void frictionStop(float relativeAmount){
            // Es innecesario darle este valor pero es por si se quiere desligar la variable de freno a la de fricción
            var materialFriction = breakCapacity; 

            if(carAceleration.X>0f){
                carAceleration.X -= (materialFriction*0.75f)*relativeAmount;
            }
            else if(carAceleration.X<0f){
                carAceleration.X += materialFriction * relativeAmount; 
            }            
            if(carAceleration.X*maxVelocity>-0.002f && carAceleration.X*maxVelocity<0.002f) 
                carAceleration.X = 0f;
            return;
        }

        /// <summary> Saltar según el momento en el tiempo en que esté </summary>
        private void jumpMotion(float now)
        {
            var timeElapsed = now-jumpStart;
            var potenciaSalto = (- (1f/20f) * timeElapsed) + ( (1f/20000f) * (timeElapsed*timeElapsed) ) + 15f;
            var sentido = 1f;

            /* TRASLADAR */
            if( (timeElapsed<500f)){
                // subir
                CarWorld*=Matrix.CreateTranslation(0f,potenciaSalto,0f);
                upNdown.X+=potenciaSalto;

                PosicionActual.Y+=potenciaSalto;
            }else if (timeElapsed<1000f){
                // bajar
                if(upNdown.Y+potenciaSalto > upNdown.X){
                    potenciaSalto = upNdown.X-upNdown.Y;
                }
                CarWorld*=Matrix.CreateTranslation(0f,-potenciaSalto,0f);
                upNdown.Y+=potenciaSalto;
                
                sentido = -1f;
                PosicionActual.Y-=potenciaSalto;
            }else{ 
                // Hay que ajustar?
                if(upNdown.X!=upNdown.Y){
                    //Subió más de lo que bajó? 
                    if(upNdown.X>upNdown.Y){
                        // Calculemos esa diferencia y hagámoslo bajar
                        potenciaSalto = upNdown.X-upNdown.Y;
                        upNdown.Y+=potenciaSalto;

                        PosicionActual.Y-=potenciaSalto;
                        // El sentido es para abajo
                        potenciaSalto *= (-1);
                    }
                    else{
                    //Bajó más de lo que tenía que haber bajado
                        potenciaSalto = upNdown.Y-upNdown.X;
                        upNdown.X+=potenciaSalto;
                        PosicionActual.Y+=potenciaSalto;
                    }
                    CarWorld*=Matrix.CreateTranslation(0f,potenciaSalto,0f);
                }else{
                    saltando = false;
                }
            }

            /* GIRAR */

            return;
        }

        /* * * * FIN AGREGADO * * * */

        //     Es llamada N veces por segundo. Generalmente 60 veces pero puede ser configurado.
        //     La logica general debe ser escrita aca, junto al procesamiento de mouse/teclas.
        protected override void Update(GameTime gameTime)
        {
            // Caputo el estado del teclado.
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // Salgo del juego.
                Exit();
            }
            

            // La logica debe ir aca.
            
            /* * * * * AGREGADO POR MANU * * * * */

            /* VARIABLES AUXILIARES */
            var myRelativeFactor = Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds);
            //var currentPosition = CarWorld;            
            var currentPosition = CarWorld;            



            /* * * FRENADO AUTOMÁTICO * * */
            if(!saltando)
                frictionStop(myRelativeFactor);
            
            /* * * MOVIMIENTO SIN TECLAS APRETADAS * * */
            if(keyboardState.GetPressedKeys().Length == 0) {
                keepMovingFrom();

                /* * * SALTO * * */
                if(saltando) jumpMotion(Convert.ToSingle(gameTime.TotalGameTime.TotalMilliseconds));
            }
            /* * * MOVIMIENTO SI SE APRIETAN TECLAS * * */
            else{
                /* * * MOVIMIENTO POR TECLA * * */
                foreach(var key in keyboardState.GetPressedKeys()){
                    /* * GIRO * */
                    switch(key){ 
                        case Keys.Right:
                        case Keys.D:
                            turnRight(currentPosition);
                        break;
                        case Keys.Left: 
                        case Keys.A:
                            turnLeft(currentPosition);
                        break;
                    }
                    /* * ACELERACIÓN * */
                    switch(key){
                        // Movimiento para adelante (Al inicio son los Z negativos - A la derecha)
                        case Keys.Up:
                        case Keys.W:
                            if(!saltando)
                                accelerate(myRelativeFactor);
                        break;
                        // Movimiento para atrás (Al inicio son los Z positivos - A la izquierda)
                        case Keys.Down:
                        case Keys.S:
                            if(!saltando)
                                decelerate(myRelativeFactor);
                        break;
                    }                
                    /* * TRIGGERS * */
                    switch(key){
                        case Keys.Space:
                            if(!saltando){
                                saltando=true;
                                jumpStart = Convert.ToSingle(gameTime.TotalGameTime.TotalMilliseconds);
                            }
                        break;
                    }
                    /* * TRASLACIÓN EN PLANO * */
                    var teclasApretadas = keyboardState.GetPressedKeyCount();
                    moveAtSpeed(maxVelocity/teclasApretadas, myRelativeFactor);
                    

                    // Actualizo la posición del auto por si entra en otra iteración
                    currentPosition = CarWorld;
                }
                /* * * SALTO * * */
                if(saltando) jumpMotion(Convert.ToSingle(gameTime.TotalGameTime.TotalMilliseconds));
            }
                
            /* * * * *  FIN AGREGADO     * * * * */
            
            // Actualizo la camara, enviandole la matriz de mundo del auto.
            FollowCamera.Update(gameTime, CarWorld);

            base.Update(gameTime);
        }


        ///     Llamada para cada frame.
        ///     La logica de dibujo debe ir aca.
        protected override void Draw(GameTime gameTime)
        {
            // Limpio la pantalla.
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Dibujo la ciudad.
            City.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);
 
            // El dibujo del auto debe ir aca.
            /* * * * * AGREGADO MANU * * * * */
            foreach(var mesh in CarModel.Meshes){
                mesh.Draw(); 
            }
            CarModel.Draw(CarWorld, FollowCamera.View, FollowCamera.Projection);
            /* * * * *  FIN AGREGADO * * * * */

            base.Draw(gameTime);
        }

        ///     Libero los recursos cargados.
        protected override void UnloadContent()
        {
            // Libero los recursos cargados dessde Content Manager.
            Content.Unload();

            base.UnloadContent();
        }
    }
}