using Microsoft.Xna.Framework;

namespace WarSteel.Scenes;

public interface ISceneProcessor {

    void Initialize(Scene scene);

    void Update(Scene scene, GameTime gameTime);

    void Draw(Scene scene);

}