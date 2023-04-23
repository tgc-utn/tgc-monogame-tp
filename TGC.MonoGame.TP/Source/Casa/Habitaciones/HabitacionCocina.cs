using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionCocina : Habitacion{
        public HabitacionCocina(int ancho, int alto, Vector3 posicionInicial):base(ancho,alto,posicionInicial){
            #region Hormiguitas
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                AddDinamico(new EnemyCar(escala, posicionesAutosIA, Vector3.Zero));
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion

            Piso.Cocina();
            AddPared(Pared.Izquierda (ancho, alto, posicionInicial));
            AddPared(Pared.Arriba(ancho, alto, posicionInicial));
            AddPared(Pared.Derecha(ancho, alto, posicionInicial));
            AddPuerta(Puerta.Abajo(3f, ancho, posicionInicial));


            AddElemento(new Mueble(TGCGame.GameContent.M_Cocine, new Vector3(500f,500f,500f), new Vector3(0f,0f,0f), 2f));

        }
    }    
}