using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.HUD;

public class CarHUD 
{
    internal Vector3 FollowedPosition;
    internal HealthBar HealthBar;
    protected Matrix HUDView;

    public CarHUD(GraphicsDeviceManager graphicsDeviceManager)
    {
        HealthBar = new HealthBar(graphicsDeviceManager);
    }

    public void Load(ContentManager contentManager)
    {
        HealthBar.Load(contentManager);
    }

    public void Update(Matrix followedWorld, float vida)
    {   
        FollowedPosition = followedWorld.Translation;
        HUDView = Matrix.CreateLookAt(FollowedPosition, FollowedPosition - Vector3.UnitZ, Vector3.UnitY);

        HealthBar.Update(FollowedPosition, vida, HUDView);
    }
    public void Draw(Matrix projection)
    {
        HealthBar.Draw(projection);
    }
}