using System;
using System.Collections.Generic;
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
        GraphicsDevice GraphicsDevice, Camera TargetLightCamera, bool modifyRootTransform = true)
    {
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        Effect.CurrentTechnique = Effect.Techniques["DepthPass"];
        if (modifyRootTransform)
            Model.Root.Transform = World;
        foreach (var modelMesh in Model.Meshes)
        {
            foreach (var part in modelMesh.MeshParts)
                part.Effect = Effect;
            var worldMatrix = modelMesh.ParentBone.Transform * World;
            Effect.Parameters["WorldViewProjection"]
                .SetValue(worldMatrix * TargetLightCamera.View * TargetLightCamera.Projection);
            modelMesh.Draw();
        }
    }
    
    public virtual void Draw(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice, Camera TargetLightCamera,
        List<Vector3> ImpactPositions = null, List<Vector3> ImpactDirections = null, bool modifyRootTransform = true)
    {
        if (Reference.DrawReference is ShadowTextureReference)
        {
            if (modifyRootTransform)
                Model.Root.Transform = World;
            Effect.CurrentTechnique = Effect.Techniques["DrawShadowedPCF"];
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
                var worldMatrix = modelMesh.ParentBone.Transform * World;
                Effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
                Effect.Parameters["World"].SetValue(worldMatrix);
                Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
                modelMesh.Draw();
            }
        }
        else if (Reference.DrawReference is ShadowBlingPhongReference)
        {
            if (ImpactPositions == null)
                ImpactPositions = new List<Vector3>();
            if (ImpactDirections == null)
                ImpactDirections = new List<Vector3>();
            if (modifyRootTransform)
                Model.Root.Transform = World;
            Effect.CurrentTechnique = Effect.Techniques["DrawShadowedPCF"];
            Effect.Parameters["baseTexture"].SetValue((Reference.DrawReference as ShadowBlingPhongReference)?.Texture);
            Effect.Parameters["shadowMap"].SetValue(ShadowMapRenderTarget);
            Effect.Parameters["lightPosition"].SetValue(skyDome.LightPosition);
            Effect.Parameters["shadowMapSize"].SetValue(Vector2.One * 2048);
            Effect.Parameters["LightViewProjection"].SetValue(TargetLightCamera.View * TargetLightCamera.Projection);
            foreach (var modelMesh in Model.Meshes)
            {
                EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, modelMesh.Name);
                foreach (var part in modelMesh.MeshParts)
                    part.Effect = Effect;
                var worldMatrix = modelMesh.ParentBone.Transform * World;
                Effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
                Effect.Parameters["World"].SetValue(worldMatrix);
                Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
                Effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
                Effect.Parameters["lightPosition"].SetValue(skyDome.LightPosition);
                Effect.Parameters["eyePosition"].SetValue(skyDome.LightViewProjection);
                Effect.Parameters["View"]?.SetValue(camera.View);
                Effect.Parameters["Projection"]?.SetValue(camera.Projection);
                Effect.Parameters["ImpactPositions"]?.SetValue(ImpactPositions.ToArray());
                Effect.Parameters["ImpactDirections"]?.SetValue(ImpactDirections.ToArray());
                Effect.Parameters["Impacts"]?.SetValue(ImpactPositions.Count);
                // Once we set these matrices we draw
                modelMesh.Draw();
            }
        }
        else
        {
            if (modifyRootTransform)
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
