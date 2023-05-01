
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{

    public class Casa {

        private const float S_METRO = TGCGame.S_METRO;
        private List<IHabitacion> Habitaciones;
        private List<Pared> Esqueleto;
        public Casa(){
            this.Habitaciones = new List<IHabitacion>();
            this.Esqueleto = new List<Pared>();
        }
        public void LoadContent(){
            disponerHabitaciones();
            construirParedes();
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState){
            foreach(var h in Habitaciones)
                h.Update(gameTime, keyboardState);
        }
        public void Draw(){
            foreach(var h in Habitaciones) h.Draw();
            foreach(var p in Esqueleto) p.Draw();
        }

        public Vector3 GetCenter(int indexHabitacion){
            if(indexHabitacion>0 && indexHabitacion < Habitaciones.Count)
                return Habitaciones[indexHabitacion].GetCenter();
            else
                return Habitaciones[0].GetCenter();
        }
        private void disponerHabitaciones(){
            // DISPOSICIÓN RELATIVA DE HABITACIONES. 
            //      TAREA :  Sacar el S_METRO de acá y de Elemento. 
            //               Dejarlo solo que dependa de la Habitación definir las posiciones
            //               de los objetos (en AddElemento y cuandodefinimos la PosicionInicial)
            Habitaciones.Add( new HabitacionPrincipal(0f,0f));
            Habitaciones.Add( new HabitacionCocina(-HabitacionCocina.Size * S_METRO, S_METRO * HabitacionPrincipal.Size/2) );
            Habitaciones.Add( new HabitacionPasillo1(0f, -HabitacionPasillo1.Size * S_METRO) );
            Habitaciones.Add( new HabitacionToilette(-HabitacionToilette.Size * S_METRO, 0));
            Habitaciones.Add( new HabitacionPasillo2(0f, -Habitaciones[2].Ancho*2 * S_METRO));
            Habitaciones.Add( new HabitacionOficina(0f, -S_METRO *(Habitaciones[2].Ancho*2+HabitacionOficina.Size)));
            Habitaciones.Add( new HabitacionDormitorio1(-S_METRO * HabitacionDormitorio1.Size , -Habitaciones[2].Ancho*2 * S_METRO ));
            Habitaciones.Add( new HabitacionDormitorio2(Habitaciones[2].Ancho * S_METRO , -Habitaciones[2].Ancho*2 * S_METRO));
            Habitaciones.Add( new HabitacionToilette(Habitaciones[5].GetVerticeExtremo().X,Habitaciones[5].GetVerticeExtremo().Z - HabitacionToilette.Size*S_METRO));

            foreach(var h in Habitaciones)
                Console.WriteLine("Habitacion cargada con {0:D}"+ " modelos.", h.cantidadElementos());
        }

        private void construirParedes(){
            // Establezco segmentos de habitaciones
            var segmentosHorizontales = new List<(Vector3 inicio,Vector3 final)>();
            var segmentosVerticales   = new List<(Vector3 inicio,Vector3 final)>();
            
            foreach(var h in Habitaciones){
                segmentosHorizontales.Add(h.GetSegmentoSuperior());
                segmentosHorizontales.Add(h.GetSegmentoInferior());
                
                segmentosVerticales.Add(h.GetSegmentoDerecha());
                segmentosVerticales.Add(h.GetSegmentoIzquierda());
            }

            // Poner todas las paredes
            foreach(var s in segmentosHorizontales)
                Esqueleto.Add(new Pared(s.inicio, s.final, true));
            foreach(var s in segmentosVerticales)
                Esqueleto.Add(new Pared(s.inicio, s.final, false));

            Console.WriteLine("CheckPoint");

            /* // Redefino Segmentos para que no sean Colineares
            for(int i = 0 ; i<segmentosHorizontales.Count ; i++){
                var sTest = segmentosHorizontales[i];
                
                for(int j = 0 ; j<segmentosHorizontales.Count ; j++){
                    var s = segmentosHorizontales[j];
                    // Colineares
                    if(sTest.inicio.X == s.inicio.X){
                        if(sTest.inicio.Z < s.final.Z && sTest.inicio.Z >= s.inicio.Z){
                            
                            //ActualizarSegmento
                            if(i!=j) segmentosHorizontales[i] = (s.final, sTest.final);
                        }
                    }
                }
            }
            Console.WriteLine("CheckPoint"); */
        }

    }
}