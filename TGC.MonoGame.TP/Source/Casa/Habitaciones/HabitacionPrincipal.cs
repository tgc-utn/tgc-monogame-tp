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
            
            carpintero.Modelo(TGCGame.GameContent.M_Mesa);
                carpintero.ConPosicion(4150f, 4150f);
                carpintero.ConEscala(12f);
                carpintero.ConRotacion(0, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();

            carpintero.Modelo(TGCGame.GameContent.M_Silla);
                // Común a todas las sillas
                carpintero.ConAltura(370f);
                carpintero.ConEscala(10f);

                carpintero.ConPosicion(4150f, 2950f);
                AddElemento( carpintero.BuildMueble() );

                carpintero.ConPosicion(4150f,5150f);
                carpintero.ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero.ConPosicion(3650f,3650f);
                carpintero.ConRotacion(0, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero.ConPosicion(3650f,4450f);
                AddElemento( carpintero.BuildMueble() );

                carpintero.ConPosicion(4750f,3650f);
                carpintero.ConRotacion(0, -MathHelper.PiOver2*0.7f, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero.ConPosicion(4750f,4450f);
                carpintero.ConRotacion(0, -MathHelper.PiOver2*1.3f, 0);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();
        
            carpintero.Modelo(TGCGame.GameContent.M_Sillon);
                carpintero.ConPosicion(7500f,3500);
                carpintero.ConAltura(100f);
                carpintero.ConRotacion(0, MathHelper.Pi, 0);
                carpintero.ConEscala(10f);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();

            carpintero.Modelo(TGCGame.GameContent.M_MuebleTV);
                carpintero.ConPosicion(7950f,350f);
                carpintero.ConAltura(50f);
                carpintero.ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();
            
            carpintero.Modelo(TGCGame.GameContent.M_Sofa);
                carpintero.ConPosicion(8000f,9350f);
                carpintero.ConRotacion(0, MathHelper.Pi, 0);
                carpintero.ConEscala(1f);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();

            carpintero.Modelo(TGCGame.GameContent.M_Mesita);
                carpintero.ConEscala(20f);
                carpintero.ConPosicion(8000f, 8200f);
                carpintero.ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();

            carpintero.Modelo(TGCGame.GameContent.M_Cafe);
                carpintero.ConEscala(10f);
                carpintero.ConAltura(390f);
                carpintero.ConRotacion(-MathHelper.PiOver2,0f,0f);
                
                carpintero.ConPosicion(8000f, 8150f);
                AddElemento( carpintero.BuildMueble() );

                carpintero.ConPosicion(8050f, 8350f);
                AddElemento( carpintero.BuildMueble() );
            carpintero.Reset();

            carpintero.Modelo(TGCGame.GameContent.M_Aparador);
                carpintero.ConEscala(15f);
                carpintero.ConRotacion(0f,MathHelper.PiOver2,0f);
                carpintero.ConPosicion(750f,8500f);
                carpintero.ConAltura(-400f);
            carpintero.Reset();

            #region CargaMueblesEstáticos

            //AddElemento( new Mueble(TGCGame.GameContent.M_Mesa, new Vector3(4150f,0f,4150f), new Vector3(0, MathHelper.PiOver2, 0), 12f ));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4150f,370f,2950f), Vector3.Zero, 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4150f,370f,5150f), new Vector3(0, MathHelper.Pi, 0), 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(3650f,370f,3650f), new Vector3(0, MathHelper.PiOver2, 0), 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(3650f,370f,4450f), new Vector3(0, MathHelper.PiOver2, 0), 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4750f,370f,3650f), new Vector3(0, -MathHelper.PiOver2*0.7f, 0), 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4750f,370f,4450f), new Vector3(0, -MathHelper.PiOver2*1.3f, 0), 10f));

            //AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(7500f,100f,3500f), new Vector3(0, MathHelper.Pi, 0), 10f));
            AddElemento( new Televisor(new Vector3(7500f,150f,300f), 0f) );
            //AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV,   new Vector3(7950f,50f,350f), new Vector3(0f,MathHelper.Pi,0f)));          

            //AddElemento( new Mueble(TGCGame.GameContent.M_Sofa, new Vector3(8000f, 0f, 9350f), new Vector3(0f, MathHelper.Pi, 0f)));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Mesita, new Vector3(8000f, 0f, 8200f), new Vector3(0f, MathHelper.Pi, 0f), 20f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(8000f, 390f, 8150f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(8050f, 390f, 8350f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));
            //AddElemento( new Mueble(TGCGame.GameContent.M_Aparador, new Vector3(750f,-400f,8500f), new Vector3(0f,MathHelper.PiOver2,0f), 15f));
            AddElemento( new Televisor(new Vector3(150f, 650f, 8800f), MathHelper.PiOver2) );
            #endregion

            #region CargaMueblesDinámicos
            var posicionesAutosIA = new Vector3(1000f,0f,6000f);           
            for(int i=1; i<6; i++){
                AddDinamico(new EnemyCar(new Vector3(i*1000f,0f,6000f), Vector3.Zero));
                AddDinamico(new EnemyCar(new Vector3(1001f,0f,i*1000f), Vector3.Zero));
            }
            #endregion

            
        }
    }    
}