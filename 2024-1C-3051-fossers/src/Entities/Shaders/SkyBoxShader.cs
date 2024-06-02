using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

class SkyBoxShader : Shader
{

    private TextureCube _texture;


    public SkyBoxShader(TextureCube texture)
    {
        _texture = texture;
        Effect = ContentRepoManager.Instance().GetEffect("SkyBox");
    }

    public override void ApplyEffects(Transform transform, Scene scene)
    {
        Transform cameraTransform = scene.GetCamera().Transform;
        Effect.Parameters["World"].SetValue(Matrix.CreateScale(transform.Dimensions) * Matrix.CreateTranslation(cameraTransform.AbsolutePosition));
        Effect.Parameters["SkyBoxTexture"].SetValue(_texture);
        Effect.Parameters["CameraPosition"].SetValue(cameraTransform.AbsolutePosition);
    }
}