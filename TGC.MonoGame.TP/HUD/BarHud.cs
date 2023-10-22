using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Helpers.Gizmos.Geometries;

namespace TGC.MonoGame.TP.HUD;

public abstract class BarHud
{
    internal Effect Effect;
    private QuadPrimitive Quad;
    private (float Width, float Heigth) Window;
    internal static Matrix AjusteQuad() => Matrix.CreateTranslation(new Vector3(-0.5f,0f,-0.5f)) // centrar quad 
                                           * Matrix.CreateRotationX(MathHelper.PiOver2) ; // levantar quad
    internal static Matrix AjusteFinal() => Matrix.CreateTranslation(Vector3.UnitZ * -10f); // ubicación pantalla HUD

    private (float Ancho, float Alto) QuadSize() => (Window.Width*0.0015f,Window.Heigth*0.00015f);
    internal abstract (float X, float Y) Location();
    
    public BarHud(GraphicsDeviceManager graphicsDevice){
        Window.Width = graphicsDevice.PreferredBackBufferWidth;
        Window.Heigth = graphicsDevice.PreferredBackBufferHeight;
        Quad = new QuadPrimitive(graphicsDevice);
    }

    public abstract void Load(ContentManager contentManager);

    public void Draw(Matrix projection)
    {
        Effect.Parameters["Projection"].SetValue(projection);        
        Quad.Draw(Effect);
    } 

    public void Update(Vector3 followedPosition, float porcentajeBarra, Matrix HUDView){
        Matrix movimientoHorizontal = Matrix.CreateTranslation(Vector3.UnitX*Location().X);
        Matrix movimientoVertical = Matrix.CreateTranslation(Vector3.UnitY*Location().Y);

        Matrix QuadWorld = AjusteQuad() * // levanta el quad 
                           Matrix.CreateScale(QuadSize().Ancho, QuadSize().Alto, 0) * // tamaño barra
                           movimientoVertical * movimientoHorizontal * // ubicación de hud
                           Matrix.CreateTranslation(followedPosition) * 
                           AjusteFinal(); // un poquito más para atrás

        Effect.Parameters["View"].SetValue(HUDView);        
        Effect.Parameters["World"]?.SetValue(QuadWorld);
        Effect.Parameters["PorcentajeBarra"]?.SetValue(porcentajeBarra);
    }
}