using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public interface Component {

    public string id();

    void UpdateEntity(Entity self,GameTime gameTime, Scene scene);

}