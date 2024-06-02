using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public interface IComponent {

    void UpdateEntity(Entity self,GameTime gameTime, Scene scene);

    void LoadContent(Entity self);

    void Initialize(Entity self, Scene scene);

    void Destroy(Entity self, Scene scene);

}