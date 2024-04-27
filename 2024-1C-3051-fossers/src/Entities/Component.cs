using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WarSteel.Entities;

public interface Component {

    public string id();

    void UpdateEntity(Entity self,GameTime gameTime, Dictionary<string,Entity> others);

}