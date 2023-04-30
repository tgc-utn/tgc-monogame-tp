using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionCocina : IHabitacion{
        private const int Size = 6;
        public HabitacionCocina(Vector3 posicionInicial):base(Size,Size,posicionInicial){

            Piso.Cocina();
            AddPared(Pared.Izquierda (Size, Size, posicionInicial));
            AddPared(Pared.Arriba(Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));
            
            var alturaMesada = 600f;

            var carpintero = new ElementoBuilder();

            carpintero.Modelo(TGCGame.GameContent.M_Mesada)
                .ConPosicion(100f,150f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_MesadaLateral2)
                .ConPosicion(100f,1000f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_PlatosApilados)
                .ConPosicion(300f,500f)
                .ConAltura(alturaMesada)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_MesadaCentral)
                .ConPosicion(100f,1500f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Olla)
                .ConPosicion(400f,1500f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(2350f,400f)
                .ConAltura(alturaMesada);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Plato)
                .ConPosicion(500f,1200f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(500f,1600f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
                carpintero
                .ConPosicion(500f,1800f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_MesadaLateral)
                .ConPosicion(100f,2100f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_PlatoGrande)
                .ConPosicion(300f,2300f)
                .ConAltura(alturaMesada+100f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Botella)
                .ConPosicion(1200f,200f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Maceta2)
                .ConPosicion(300f,2400f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta3)
                .ConPosicion(300f,2900f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_ParedCocina)
                .ConPosicion(100f,150f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Cocine)
                .ConPosicion(2500f,500f)
                .ConAltura(500f)
                .ConEscala(2f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Alacena)
                .ConPosicion(200f,3900f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

                carpintero.ConPosicion(3000f,0f);
                AddElemento(carpintero.BuildMueble());
                            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta)
                .ConPosicion(5400f,5400f)
                .ConEscala(60f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta4)
                .ConPosicion(400f,5400f)
                .ConEscala(60f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta4)
                .ConPosicion(400f,5400f)
                .ConAltura(700f);
                AddElemento(carpintero.BuildMueble());
        }
    }    
}