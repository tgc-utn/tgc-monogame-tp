using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionPrincipal : IHabitacion{
        private const int Size = 10;
        public HabitacionPrincipal(Vector3 posicionInicial):base(Size,Size,posicionInicial){

            Piso = Piso.Madera();
            
            // Esto debería abstraerse a una entidad CASA que cree y ubique las paredes 
            // según la ubicacion de la casa
            AddPared(Pared.Izquierda (Size, Size, posicionInicial));
            AddPuerta(Puerta.Arriba(3f, Size, posicionInicial));
            AddPared(Pared.Abajo(Size, Size, posicionInicial));
            AddPuerta(Puerta.Derecha(7f, Size, posicionInicial));
            
            var carpintero = new ElementoBuilder();
            
            carpintero.Modelo(TGCGame.GameContent.M_Mesa)
                .ConPosicion(4150f, 4150f)
                .ConEscala(12f)
                .ConRotacion(0, MathHelper.PiOver2, 0);
                
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Silla)
                .ConAltura(370f)
                .ConEscala(10f)
                .ConPosicion(4150f, 2950f);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4150f,5150f)
                .ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero
                .ConPosicion(3650f,3650f)
                .ConRotacion(0, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero
                .ConPosicion(3650f,4450f);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4750f,3650f)
                .ConRotacion(0, -MathHelper.PiOver2*0.7f, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4750f,4450f)
                .ConRotacion(0, -MathHelper.PiOver2*1.3f, 0);
                AddElemento( carpintero.BuildMueble() );
            
        
            carpintero.Modelo(TGCGame.GameContent.M_Sillon);
                carpintero
                .ConPosicion(7500f,3500)
                .ConAltura(100f)
                .ConRotacion(0, MathHelper.Pi, 0)
                .ConEscala(10f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_MuebleTV);
                carpintero.ConPosicion(7950f,350f)
                .ConAltura(50f)
                .ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            
            
            carpintero.Modelo(TGCGame.GameContent.M_Sofa)
                .ConPosicion(8000f,9350f)
                .ConRotacion(0, MathHelper.Pi, 0)
                .ConEscala(1f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Mesita)
                .ConEscala(20f)
                .ConPosicion(8000f, 8200f)
                .ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Cafe)
                .ConEscala(10f)
                .ConAltura(390f)
                .ConRotacion(-MathHelper.PiOver2,0f,0f)
          
                .ConPosicion(8000f, 8150f);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(8050f, 8350f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Aparador)
                .ConEscala(15f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConPosicion(750f,8500f)
                .ConAltura(-400f);
                AddElemento( carpintero.BuildMueble() );

            carpintero.Modelo(TGCGame.GameContent.M_Televisor1)
                .ConEscala(10f)
                .ConShader(TGCGame.GameContent.E_SpiralShader)
                .ConPosicion(8500f,750f)
                .ConAltura(150);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConAltura(650f)
                .ConPosicion(150f, 8800f)
                .ConRotacion(0f,MathHelper.PiOver2,0f);
                
                AddElemento( carpintero.BuildMueble() );

            #region Autos Enemigos

                var posicionesAutosIA = new Vector3(1000f,0f,6000f);           
                for(int i=1; i<6; i++){
                    AddDinamico(new EnemyCar(i*1000f,0f,6000f, Vector3.Zero));
                    AddDinamico(new EnemyCar(1001f,0f,i*1000f, Vector3.Zero));
                }
            
            #endregion            
        }
    }    
}