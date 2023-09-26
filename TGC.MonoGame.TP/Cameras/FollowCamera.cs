using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Cameras;

public class FollowCamera : Camera
{
    private const float BackwardDistanceToTarget = 450f;
    private const float UpDistanceToTarget = 15f;
    private Vector3 CurrentBackwardVector { get; set; } = Vector3.Backward;
    private float BackwardVectorInterpolator { get; set; } = 0.5f;
    private Vector3 PastBackwardVector { get; set; } = Vector3.Backward;

    /// <summary>
    /// Crea una FollowCamera que sigue a una matriz de mundo
    /// </summary>
    /// <param name="aspectRatio"></param>
    public FollowCamera(float aspectRatio) : base(aspectRatio)
    {
        // Orthographic camera
        // Projection = Matrix.CreateOrthographic(screenWidth, screenHeight, 0.01f, 10000f);

        // Perspective camera
        // Uso 60° como FOV, aspect ratio, pongo las distancias a near plane y far plane en 0.1 y 100000 (mucho) respectivamente
        Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 10f, 100000f);
    }

    /// <summary>
    /// Actualiza la Camara usando una matriz de mundo actualizada para seguirla
    /// </summary>
    /// <param name="gameTime">The Game Time to calculate framerate-independent movement</param>
    /// <param name="followedWorld">The World matrix to follow</param>
    public override void Update(GameTime gameTime, Matrix followedWorld)
    {
        // Obtengo el tiempo
        var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        
        // Obtengo la posicion de la matriz de mundo que estoy siguiendo
        var followedPosition = followedWorld.Translation;

        // Obtengo el vector Atras de la matriz de mundo que estoy siguiendo
        var followedBackward = followedWorld.Backward;
        
        CurrentBackwardVector = Vector3.Lerp(CurrentBackwardVector, followedBackward, BackwardVectorInterpolator * BackwardVectorInterpolator);
  
        // Guardo el vector Atras para usar en la siguiente iteracion
        PastBackwardVector = followedBackward;
        
        // Calculo la posicion del a camara
        // tomo la posicion que estoy siguiendo, agrego un offset en los ejes Y y Atras
        var offsetedPosition = followedPosition 
            + CurrentBackwardVector * BackwardDistanceToTarget
            + Vector3.Up * UpDistanceToTarget;

        // Calculo el vector Arriba actualizado
        // Nota: No se puede usar el vector Arriba por defecto (0, 1, 0)
        // Como no es correcto, se calcula con este truco de producto vectorial

        // Calcular el vector Adelante haciendo la resta entre el destino y el origen
        // y luego normalizandolo (Esta operacion es cara!)
        // (La siguiente operacion necesita vectores normalizados)
        var forward = (followedPosition - offsetedPosition);
        forward.Normalize();

        // Obtengo el vector Adelante asumiendo que la camara tiene el vector Arriba apuntando hacia arriba
        // y no esta rotada en el eje X (Roll)
        var right = Vector3.Cross(forward, Vector3.Up);

        // Una vez que tengo la correcta direccion Adelante, obtengo la correcta direccion Arriba usando
        // otro producto vectorial
        var cameraCorrectUp = Vector3.Cross(right, forward);

        // Calculo la matriz de Vista de la camara usando la Posicion, La Posicion a donde esta mirando,
        // y su vector Arriba
        View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);
    }
}