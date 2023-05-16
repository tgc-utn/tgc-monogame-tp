using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP{
    class EnemyCar : IElementoDinamico {
        const float ESCALA = 7f;
        const float SIMU_BOX_SCALE = 0.01f;
        private BodyHandle Handle;
        private const float Velocidad = 500f;
        private Vector3 Traslacion = new Vector3(0f,0f, 0f);
        private Vector3 Direccion = new Vector3(0f,0f,0f); // Si está en cero, están inmóviles

        public EnemyCar(float posX, float posY, float posZ, Vector3 rotacion) 
        : base(TGCGame.GameContent.M_AutoEnemigo, new Vector3(posX,posY,posZ), Vector3.Zero, ESCALA)
        {
            Traslacion = new Vector3(posX, posY, posZ);
            this.SetEffect(TGCGame.GameContent.E_TextureShader);

            var boxSize = (Utils.ModelSize(Model))*SIMU_BOX_SCALE;
            var boxShape = new Box(boxSize.X,boxSize.Y,boxSize.Z); // a chequear
            var boxInertia = boxShape.ComputeInertia(15);
            var boxIndex = TGCGame.Simulation.Shapes.Add(boxShape);

            Handle = TGCGame.Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                            (this.GetPosicionInicial()).ToBepu(),
                            boxInertia,
                            new CollidableDescription(boxIndex, 0.1f),
                            new BodyActivityDescription(0.01f)));

        }

        public override void Update(GameTime gameTime, KeyboardState keyboard)
        {
            var simuWorld = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            var position = simuWorld.Pose.Position;
            var quaternion = simuWorld.Pose.Orientation;
            World =
                Matrix.CreateScale(Escala) *
                Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)) * 
                Matrix.CreateTranslation(new Vector3(position.X, position.Y, position.Z));
        }
        public override void Draw()
        {
            // var simuWorld = TGCGame.Simulation.Bodies.GetBodyReference(Handle);            
            // var aabb = simuWorld.BoundingBox;
            // TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.HotPink);

            TGCGame.GameContent.E_TextureShader
                .Parameters["Texture"].SetValue(TGCGame.GameContent.T_CombatVehicle);

            foreach(var mesh in Model.Meshes){
                foreach(var meshPart in mesh.MeshParts){
                    meshPart.Effect.Parameters["World"]?.SetValue(World);
                }
            mesh.Draw();
            }
        }
    }
}