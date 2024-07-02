using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderingTanks.Objects.Props
{
    internal class GermanSoldier : GameProp
    {
        public void LoadContent(ContentManager Content, Effect effect)
        {
            Model GermanSoliderModel = Content.Load<Model>(ContentFolder3D + "German_Soldier_1/German_Soldier_1");
            Texture2D GermanSoldierTexture = Content.Load<Texture2D>(ContentFolder3D + "German_Soldier_1/panzergren_low2k_diff");

            Model = GermanSoliderModel;
            Texture = GermanSoldierTexture;
            Effect = effect;
        }
    }
}
