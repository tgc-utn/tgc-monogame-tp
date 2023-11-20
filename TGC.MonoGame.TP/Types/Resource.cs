using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types;

public abstract class Resource
{
    public Model Model;
    public Effect Effect;
    public Matrix World;
    public ModelReference Reference;
    
    public virtual void Load(ContentManager content)
    {
        Model = content.Load<Model>(Reference.Path);
        Effect = EffectsRepository.GetEffect(Reference.DrawReference, content);
        TexturesRepository.InitializeTextures(Reference.DrawReference, content);
        foreach (var modelMeshPart in Model.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }
    }

    public virtual void DrawOnShadowMap(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget,
        GraphicsDevice GraphicsDevice, Camera TargetLightCamera)
    {
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        // Set the render target as our shadow map, we are drawing the depth into this texture
        // GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

        Effect.CurrentTechnique = Effect.Techniques["DepthPass"];

        // We get the base transform for each mesh
        Model.Root.Transform = World;
        // var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
        // Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
        foreach (var modelMesh in Model.Meshes)
        {
            foreach (var part in modelMesh.MeshParts)
                part.Effect = Effect;

            // We set the main matrices for each mesh to draw
            // var worldMatrix = modelMeshesBaseTransforms[modelMesh.ParentBone.Index];
            var worldMatrix = modelMesh.ParentBone.Transform * World;

            // WorldViewProjection is used to transform from model space to clip space
            Effect.Parameters["WorldViewProjection"]
                .SetValue(worldMatrix * TargetLightCamera.View * TargetLightCamera.Projection);

            // Once we set these matrices we draw
            modelMesh.Draw();
        }
    }
    
    public virtual void Draw(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice, Camera TargetLightCamera)
    {
        // skyDome.LightBox.Draw(Matrix.CreateTranslation(skyDome.LightPosition), camera.View, camera.Projection);

        if (Reference.DrawReference is ShadowTextureReference)
        {
            #region Pass 2

            // Set the render target as null, we are drawing on the screen!
            // GraphicsDevice.SetRenderTarget(null);
            // GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);
            Model.Root.Transform = World;
            // var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
            // Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
            
            Effect.CurrentTechnique = Effect.Techniques["DrawShadowedPCF"];
            // Effect.Parameters["baseTexture"].SetValue(BasicEffect.Texture);
            Effect.Parameters["baseTexture"].SetValue((Reference.DrawReference as ShadowTextureReference)?.Texture);
            Effect.Parameters["shadowMap"].SetValue(ShadowMapRenderTarget);
            Effect.Parameters["lightPosition"].SetValue(skyDome.LightPosition);
            Effect.Parameters["shadowMapSize"].SetValue(Vector2.One * 2048);
            Effect.Parameters["LightViewProjection"].SetValue(TargetLightCamera.View * TargetLightCamera.Projection);
            foreach (var modelMesh in Model.Meshes)
            {
                EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, modelMesh.Name);
                foreach (var part in modelMesh.MeshParts)
                    part.Effect = Effect;

                // We set the main matrices for each mesh to draw
                // var worldMatrix = modelMeshesBaseTransforms[modelMesh.ParentBone.Index];
                var worldMatrix = modelMesh.ParentBone.Transform * World;

                // WorldViewProjection is used to transform from model space to clip space
                Effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
                Effect.Parameters["World"].SetValue(worldMatrix);
                Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));

                // Once we set these matrices we draw
                modelMesh.Draw();
            }
            #endregion
        }
        else
        {
            // GraphicsDevice.SetRenderTarget(ShadowMapRenderTarget);
            // GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);
            Model.Root.Transform = World;

            Effect.Parameters["View"]?.SetValue(camera.View);
            Effect.Parameters["Projection"]?.SetValue(camera.Projection);

            // Draw the model.
            foreach (var mesh in Model.Meshes)
            {
                EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, mesh.Name);
                var worldMatrix = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"].SetValue(worldMatrix);
                Effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
                Effect.Parameters["WorldViewProjection"]?.SetValue(worldMatrix * camera.View * camera.Projection);
                Effect.Parameters["lightPosition"]?.SetValue(skyDome.LightPosition);
                Effect.Parameters["eyePosition"]?.SetValue(skyDome.LightViewProjection);
                mesh.Draw();
            }
            
        }
    }
}
