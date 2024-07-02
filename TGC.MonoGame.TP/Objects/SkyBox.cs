using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ThunderingTanks.Objects
{
    /// <summary>
    ///     Handles all of the aspects of working with a SkyBox.
    /// </summary>
    public class SkyBox
    {

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        /// <summary>
        ///     Creates a new SkyBox
        /// </summary>
        /// <param name="model">The geometry to use for SkyBox.</param>
        /// <param name="texture">The SkyBox texture to use.</param>
        /// <param name="effect">The SkyBox fx to use.</param>
        /// <param name="size">The SkyBox fx to use.</param>
        public SkyBox(float size)
        {
            Size = size;
        }

        /// <summary>
        ///     The size of the cube, used so that we can resize the box
        ///     for different sized environments.
        /// </summary>
        private float Size { get; set; }

        /// <summary>
        ///     The effect file that the SkyBox will use to render
        /// </summary>
        private Effect Effect { get; set; }

        /// <summary>
        ///     The actual SkyBox texture
        /// </summary>
        private TextureCube Texture { get; set; }

        /// <summary>
        ///     The SkyBox model, which will just be a cube
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        ///    Loads the SkyBox texture and effect.
        /// </summary>
        /// <param name="Content"></param>
        public void LoadContent(ContentManager Content)
        {
            var skyBox = Content.Load<Model>(ContentFolder3D + "cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/mountain_skybox_hd");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");

            Model = skyBox;
            Texture = skyBoxTexture;
            Effect = skyBoxEffect;
        }

        /// <summary>
        ///     Does the actual drawing of the SkyBox with our SkyBox effect.
        ///     There is no world matrix, because we're assuming the SkyBox won't
        ///     be moved around.  The size of the SkyBox can be changed with the size
        ///     variable.
        /// </summary>
        /// <param name="view">The view matrix for the effect</param>
        /// <param name="projection">The projection matrix for the effect</param>
        /// <param name="cameraPosition">The position of the camera</param>
        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition)
        {

            // Go through each pass in the effect, but we know there is only one...
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {              
                pass.Apply();

                // Draw all of the components of the mesh, but we know the cube really
                // only has one mesh
                foreach (var mesh in Model.Meshes)
                {
                    // Assign the appropriate values to each of the parameters
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = Effect;
                        part.Effect.Parameters["World"].SetValue(
                            Matrix.CreateScale(Size) * Matrix.CreateTranslation(cameraPosition));
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(Texture);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    }

                    // Draw the mesh with the SkyBox effect
                    mesh.Draw();
                }
            }
        }
    }
}