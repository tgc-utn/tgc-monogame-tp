using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common.Shaders;
using WarSteel.Scenes;

namespace WarSteel.Common;

public class Renderable
{
    protected Model _model { get; }
    protected Dictionary<string, Shader> _shaders;

    public Renderable(Model model)
    {
        _model = model;
        _shaders = new Dictionary<string, Shader>();
    }

    public void AddShader(string name, Shader shader)
    {
        _shaders[name] = shader;
    }

    public virtual void Draw(Transform transform, Scene scene)
    {

        foreach (var shader in _shaders)
        {
            foreach (var pass in shader.Value.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var mesh in _model.Meshes)
                {
                    Matrix modelWorld = GetMatrix(mesh, transform);
                    shader.Value.UseCamera(scene.GetCamera());
                    shader.Value.UseWorld(modelWorld);
                    shader.Value.ApplyEffects(transform, scene);

                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = shader.Value.Effect;
                    }

                    mesh.Draw();
                }
            }
        };
    }

    public Vector3 GetModelCenter()
    {
        int count = 0;
        Vector3 center = Vector3.Zero;
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData(0, vertices, 0, meshPart.NumVertices, meshPart.VertexBuffer.VertexDeclaration.VertexStride);

                foreach (VertexPositionNormalTexture vertex in vertices)
                {
                    Vector3 v = Vector3.Transform(vertex.Position, mesh.ParentBone.Transform);
                    center += v;
                    count++;
                }
            }
        }
        return center / count;
    }

    public virtual Matrix GetMatrix(ModelMesh mesh, Transform transform)
    {
        return transform.LocalToWorldMatrix(mesh.ParentBone.Transform);
    }
}
