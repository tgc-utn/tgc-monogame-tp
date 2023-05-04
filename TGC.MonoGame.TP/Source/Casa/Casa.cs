
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

            for(int i = 0 ; i<10 ; i++)
                Esqueleto[i].Draw(TGCGame.GameContent.T_Ladrillos);
            for(int i = 10; i<15 ; i++)
                Esqueleto[i].Draw(TGCGame.GameContent.T_Concreto);

            for(int i = 15; i< Esqueleto.Count; i++){
                Esqueleto[i].Draw();
            }
                
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

            // Paredes exteriores
            Esqueleto.Add(new Pared(Habitaciones[0].GetSegmentoSuperior().final, Habitaciones[1].GetSegmentoInferior().final, true));
            Esqueleto.Add(new Pared(Habitaciones[1].GetSegmentoIzquierda().inicio, Habitaciones[1].GetSegmentoIzquierda().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[0].GetSegmentoInferior().inicio, Habitaciones[0].GetSegmentoInferior().final, true));
            Esqueleto.Add(new Pared(Habitaciones[0].GetSegmentoIzquierda().inicio, Habitaciones[0].GetSegmentoIzquierda().final));
            Esqueleto.Add(new Pared(Habitaciones[3].GetSegmentoIzquierda().inicio, Habitaciones[3].GetSegmentoIzquierda().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[6].GetSegmentoIzquierda().inicio, Habitaciones[6].GetSegmentoIzquierda().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[6].GetSegmentoInferior ().final, Habitaciones[2].GetSegmentoSuperior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[7].GetSegmentoInferior ().inicio, Habitaciones[7].GetSegmentoInferior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[7].GetSegmentoIzquierda().inicio, Habitaciones[7].GetSegmentoIzquierda().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[8].GetSegmentoInferior ().inicio, Habitaciones[8].GetSegmentoInferior ().final, true ));

            // Paredes HabitacionPrincipal
            Esqueleto.Add(new Pared(Habitaciones[0].GetSegmentoSuperior().inicio, Habitaciones[0].GetSegmentoSuperior().final, true));
            Esqueleto.Add(new Pared(Habitaciones[0].GetSegmentoDerecha().inicio, Habitaciones[0].GetSegmentoDerecha().final));
            // Paredes cortadas del Pasillo 1 (el más cercano a la principal)
            Esqueleto.Add(new Pared(Habitaciones[6].GetSegmentoInferior ().inicio, Habitaciones[6].GetSegmentoInferior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[7].GetSegmentoSuperior ().final, Habitaciones[2].GetSegmentoInferior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[5].GetSegmentoIzquierda().inicio, Habitaciones[7].GetSegmentoDerecha().inicio, false ));
            
            // Paredes cocina ( primera cortada )
            Esqueleto.Add(new Pared(Habitaciones[1].GetSegmentoDerecha().inicio, Habitaciones[1].GetSegmentoDerecha().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[1].GetSegmentoSuperior ().inicio, Habitaciones[1].GetSegmentoSuperior ().final, true ));
            
            // Paredes baños
            Esqueleto.Add(new Pared(Habitaciones[3].GetSegmentoDerecha().inicio, Habitaciones[3].GetSegmentoDerecha().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[3].GetSegmentoSuperior ().inicio, Habitaciones[3].GetSegmentoSuperior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[8].GetSegmentoDerecha  ().inicio, Habitaciones[8].GetSegmentoDerecha  ().final, false ));
            // Paredes Dormitorio1 (Lego)
            Esqueleto.Add(new Pared(Habitaciones[6].GetSegmentoDerecha().inicio, Habitaciones[6].GetSegmentoDerecha().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[6].GetSegmentoSuperior ().inicio, Habitaciones[6].GetSegmentoSuperior ().final, true ));
            // Paredes Dormitorio2 (Dragones)
            Esqueleto.Add(new Pared(Habitaciones[7].GetSegmentoSuperior ().inicio, Habitaciones[7].GetSegmentoSuperior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[7].GetSegmentoDerecha  ().inicio, Habitaciones[7].GetSegmentoDerecha  ().final, false ));
            // Paredes oficina
            Esqueleto.Add(new Pared(Habitaciones[5].GetSegmentoSuperior ().inicio, Habitaciones[5].GetSegmentoSuperior ().final, true ));
            Esqueleto.Add(new Pared(Habitaciones[5].GetSegmentoDerecha  ().inicio, Habitaciones[5].GetSegmentoDerecha  ().final, false ));
            Esqueleto.Add(new Pared(Habitaciones[5].GetSegmentoInferior ().inicio, Habitaciones[5].GetSegmentoInferior ().final, true ));

            Console.WriteLine("CheckPoint");
        }
    }
}