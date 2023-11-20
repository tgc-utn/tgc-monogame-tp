using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.HUD;

public class TankHUD 
{
    internal Vector3 FollowedPosition;
    internal HealthBar HealthBar;
    internal ShootBar ShootBar;
    protected Matrix HUDView;
    private bool canShoot = true;

    public TankHUD(GraphicsDeviceManager graphicsDeviceManager)
    {
        HealthBar = new HealthBar(graphicsDeviceManager);
        ShootBar = new ShootBar(graphicsDeviceManager);
    }

    public void Load(ContentManager contentManager)
    {
        HealthBar.Load(contentManager);
        ShootBar.Load(contentManager);
    }

    public void Update(Matrix followedWorld, float health, float shootTime)
    {   
        FollowedPosition = followedWorld.Translation;
        HUDView = Matrix.CreateLookAt(FollowedPosition, FollowedPosition - Vector3.UnitZ, Vector3.UnitY);

        HealthBar.Update(FollowedPosition, health, HUDView);

        canShoot = shootTime <= 0f;
        
        ShootBar.Update(FollowedPosition, shootTime, HUDView);
    }
    public void Draw(Matrix projection)
    {
        HealthBar.Draw(projection);
        if(!canShoot) ShootBar.Draw(projection);
    }
}