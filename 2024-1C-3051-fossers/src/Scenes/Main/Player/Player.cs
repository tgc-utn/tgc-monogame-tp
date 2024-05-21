
using Microsoft.Xna.Framework;
using WarSteel.Entities;

namespace WarSteel.Scenes.Main;

class Player
{
    public void Initialize(Scene scene)
    {
        Entity tank = new Tank("player");
        // here we add the specific components to customize the tank behavior according to the player rules
        tank.AddComponent(new PlayerControls());
        tank.Transform.Rotate(new Vector3(0, 180, 0));
        scene.AddEntity(tank);
    }
}