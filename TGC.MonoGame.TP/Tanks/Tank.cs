using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.References;

namespace TGC.MonoGame.TP.Tanks;

public class Tank : ICollidable
{
    private Model Model;
    private ModelReference Reference;
    private Effect Effect;
    public Matrix World { get; set; }
    private Vector3 Position;
    private Vector3 LastPosition;
    public BoundingBox Box { get; set; }

    private float _velocidad;
    private Matrix _rotacion;

    public Tank(ModelReference model, Vector3 position)
    {
        Reference = model;
        Position = position;
        
        _velocidad = 0;
        _rotacion = Matrix.Identity;
    }
    
    public void Load(ContentManager content, Effect effect)
    {
        World = Reference.Rotation * Matrix.CreateTranslation(Position) * Matrix.CreateScale(Reference.Scale);
        Model = content.Load<Model>(Reference.Path);
        Model.Root.Transform = World;
        Effect = effect;
        foreach (var modelMeshPart in Model.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }
        // Creo y ajusto la box
        Box = BoundingVolumesExtension.CreateAABBFrom(Model);
        Box = new BoundingBox(Box.Min + Position, Box.Max + Position);
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.W))
        {
            // Avanzo
            _velocidad += 0.01f;
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            // Retrocedo
            _velocidad -= 0.01f;
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            // Giro izq
            _rotacion *= Matrix.CreateRotationY(0.04f);
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            // Giro der
            _rotacion *= Matrix.CreateRotationY(-0.04f);
        }

        LastPosition = Position;
        Position += Vector3.Transform(Vector3.Forward, _rotacion) * _velocidad * gameTime.ElapsedGameTime.Milliseconds;
        var desplazamiento = (Position - LastPosition) * Reference.Scale;
        World = _rotacion * Matrix.CreateTranslation(Position) * Matrix.CreateScale(Reference.Scale);
        
        Box = new BoundingBox(Box.Min + desplazamiento, Box.Max + desplazamiento);
    }

    public void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;

        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        Effect.Parameters["View"].SetValue(view);
        Effect.Parameters["Projection"].SetValue(projection);
        Effect.Parameters["DiffuseColor"].SetValue(Reference.Color.ToVector3());

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }

    public void CollidedWithSmallProp()
    {
        Console.WriteLine("Chocaste con prop chico");
        _velocidad = 0.5f;
    }

    public void CollidedWithLargeProp()
    {
        Console.WriteLine("Chocaste con prop grande");
        _velocidad = 0f;
        // Corrigiendo la posicion del tanque y de la box
        var desplazamiento = (LastPosition - Position) * Reference.Scale;
        Position = LastPosition;
        Box = new BoundingBox(Box.Min + desplazamiento, Box.Max + desplazamiento);
    }

    public BoundingBox GetBoundingBox() { return Box; }
}