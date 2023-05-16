using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP
{
    // PISO
    public class Piso
    {
        private const float S_METRO = TGCGame.S_METRO;
        
        private Vector3 ColorDefault = new Color(19, 38, 47).ToVector3();
        private Texture2D TexturaBaldosa = TGCGame.GameContent.T_PisoMadera;
        private float CantidadBaldosas = 1f;
        private Matrix World;
        internal Vector3 PosicionInicial;
        private Effect Effect = TGCGame.GameContent.E_BasicShader;
        private readonly float MetrosAncho;
        private readonly float MetrosLargo;
        //private readonly int MetrosCuadrados;

        public Piso(int metrosAncho, int metrosLargo, Vector3 posicionInicial)
        {
            PosicionInicial = posicionInicial; 
            MetrosAncho = metrosAncho * S_METRO;
            MetrosLargo = metrosLargo * S_METRO;
            
            Matrix Scale = Matrix.CreateScale(MetrosAncho, 0f, MetrosLargo);
            World = Scale 
                    * Matrix.CreateTranslation(PosicionInicial);

            var boxito = new Box(MetrosAncho*2,1f, MetrosLargo*2);
            
            TGCGame.Simulation.Statics.Add( new StaticDescription(
                                                PosicionInicial.ToBepu()-Vector3.UnitY.ToBepu(),
                                                TGCGame.Simulation.Shapes.Add(boxito)
                                                ));
        }
        public Vector3 GetVerticeExtremo(){
            return PosicionInicial + ( new Vector3(MetrosLargo,0f,MetrosAncho) );
        }
        public Vector3 GetMiddlePoint(){
            return PosicionInicial + ( new Vector3(MetrosLargo*0.5f,0f,MetrosAncho*0.5f) );
        }
        public Vector3 getCenter() => this.GetVerticeExtremo()*0.5f;
        
        public Piso ConColor(Color color){
            ColorDefault = color.ToVector3();
            return this;
        }
        public Piso ConTextura(Texture2D texturaPiso, float cantidadBaldosas = 1){
            Effect = TGCGame.GameContent.E_TextureTiles;
            CantidadBaldosas = cantidadBaldosas;
            TexturaBaldosa = texturaPiso;
            return this;
        }
        public void Draw()
        {
            Effect.Parameters["DiffuseColor"]?.SetValue(ColorDefault);
            Effect.Parameters["Texture"]?.SetValue(TexturaBaldosa);
            Effect.Parameters["TileAmount"]?.SetValue(CantidadBaldosas);
            Effect.Parameters["World"].SetValue(World);
            TGCGame.GameContent.G_Quad.Draw(Effect);
        }
    }
}