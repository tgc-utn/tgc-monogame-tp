using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionCocina : IHabitacion{
        private const int Size = 6;
        public HabitacionCocina(Vector3 posicionInicial):base(Size,Size,posicionInicial){
            #region Hormiguitas
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                //AddDinamico(new EnemyCar(escala, posicionesAutosIA, Vector3.Zero));
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion

            Piso.Cocina();
            AddPared(Pared.Izquierda (Size, Size, posicionInicial));
            AddPared(Pared.Arriba(Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));


            
            var sentidoHorizontal = new Vector3(0f,MathHelper.PiOver2,0f);
            var alturaMesada = 600f;
            #region SetCocina
            AddElemento(new Mueble(TGCGame.GameContent.M_Mesada,         new Vector3(100f,alturaMesada,150f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_MesadaLateral2, new Vector3(100f,0f,1000f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_PlatosApilados, new Vector3(300f,alturaMesada,500f), 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_MesadaCentral,  new Vector3(100f,0f,1500f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Olla,           new Vector3(400f,alturaMesada+100f,1500f), 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Plato,          new Vector3(500f,alturaMesada*2,1200f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Plato,          new Vector3(500f,alturaMesada*2,1600f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Plato,          new Vector3(500f,alturaMesada*2,1800f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_MesadaLateral,  new Vector3(100f,0f,2100f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_PlatoGrande,    new Vector3(300f,alturaMesada+100f,2300f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Botella,        new Vector3(1200f,alturaMesada+100f,200f), 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Maceta2,        new Vector3(300f,alturaMesada+100f,2400f), 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Maceta3,        new Vector3(300f,alturaMesada+100f,2900f), 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_ParedCocina,    new Vector3(100f,0,150f), sentidoHorizontal, 20f));
            #endregion


            AddElemento(new Mueble(TGCGame.GameContent.M_Cocine,    new Vector3(2500f,500f,500f), 2f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Olla,      new Vector3(2350f,alturaMesada,400f), 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Alacena,   new Vector3(200f,alturaMesada*2,3900f), sentidoHorizontal, 20f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Alacena,   new Vector3(3000f,alturaMesada*2,0f), 20f));
            
            AddElemento(new Mueble(TGCGame.GameContent.M_Maceta,    new Vector3(5400f,0f,5400f), 60f));
            
            AddElemento(new Mueble(TGCGame.GameContent.M_Maceta4,   new Vector3(400f,0f,5400f), 60f));
            
        }
    }    
}