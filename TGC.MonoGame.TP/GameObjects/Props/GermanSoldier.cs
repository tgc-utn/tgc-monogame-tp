using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ThunderingTanks.Animation.Models;
using ThunderingTanks.Animation.PipelineExtensions;
using MonoGame.Extended;
using System.DirectoryServices;

namespace ThunderingTanks.Objects.Props
{
    internal class GermanSoldier : GameObject
    {
        new public AnimatedModel Model { get; set; }
        public AnimatedModel Animation { get; set; }
        private List<string> AnimationNames { get; set; }
        private ContentManager ContentManager { get; set; }

        private IConfigurationRoot Configuration { get; set; }

        readonly string AnimationFolder = "Models/GermanModel";
        readonly string AnimatedModelFolder = ContentFolder3D + "GermanModel/GermanSoldier";

        public override void Initialize()
        {
            var configurationFileName = "app-settings.json";
            Configuration = new ConfigurationBuilder().AddJsonFile(configurationFileName, true, true).Build();
        }

        public override void LoadContent(ContentManager Content, Effect effect)
        {

            ContentManager = Content;

            AnimationNames = new List<string>
            {
                 AnimationFolder + "/GermanSoldier_1",
                 AnimationFolder + "/GermanSoldier_2",
            };

            var manager = CustomPipelineManager.CreateCustomPipelineManager(Configuration);

            foreach (var model in AnimationNames)
            {
                manager.BuildAnimationContent(model);
            }

            manager.BuildAnimationContent(AnimatedModelFolder);

            Model = new AnimatedModel(AnimatedModelFolder);
            Model.LoadContent(Content);

            Texture2D GermanSoldierTexture = Content.Load<Texture2D>(ContentFolder3D + "GermanModel/Wermarcht_albedo");
            Texture = GermanSoldierTexture;

            Effect = effect;
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            Model.Draw(WorldMatrix, view, projection);
        }

        public void ChangeAnimation()
        {
            Animation = new AnimatedModel(AnimationNames[0]);
            Animation.LoadContent(ContentManager);

            var player = Model.PlayClip(Animation.Clips[0]);
            player.Looping = true;
        }
    }
}
