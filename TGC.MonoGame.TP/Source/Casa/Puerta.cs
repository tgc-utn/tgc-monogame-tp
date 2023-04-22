using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Puerta{
        private static float Escala = 1000f;
        private List<Pared> Paredes;
        
        private Puerta(List<Pared> paredes){
            Paredes = paredes;
        }

        public static Puerta Arriba(float comienzo, float ancho , Vector3 posicionInicial){
            List<Pared> paredes = new List<Pared>();
            paredes.Add(Pared.Arriba((ancho-comienzo-1.5f),ancho ,posicionInicial));
            paredes.Add(Pared.Arriba(comienzo,ancho,posicionInicial + new Vector3(0f, 0f, (ancho - comienzo) * Escala)));
            return new Puerta(paredes);
        }

        public static Puerta Abajo(float comienzo, float ancho, Vector3 posicionInicial){
            List<Pared> paredes = new List<Pared>();
            paredes.Add(Pared.Abajo((ancho-comienzo-1.5f), ancho,posicionInicial));
            paredes.Add(Pared.Abajo(comienzo, ancho, posicionInicial+ new Vector3(0f, 0f, (ancho - comienzo) * Escala)));
            return new Puerta(paredes);
        }

        public static Puerta Izquierda(float comienzo, float ancho, Vector3 posicionInicial){
            List<Pared> paredes = new List<Pared>();
            paredes.Add(Pared.Izquierda((ancho-comienzo-1.5f), ancho, posicionInicial + new Vector3(0f, 0f, (comienzo + 1.5f) * Escala)));
            paredes.Add(Pared.Izquierda(comienzo, ancho, posicionInicial + new Vector3((ancho - comienzo) * Escala, 0f, (ancho - comienzo) * Escala)));
            return new Puerta(paredes);
        }

        public static Puerta Derecha(float comienzo, float ancho, Vector3 posicionInicial){
            List<Pared> paredes = new List<Pared>();
            paredes.Add(Pared.Derecha((ancho-comienzo-1.5f), ancho, posicionInicial));
            paredes.Add(Pared.Derecha(comienzo, ancho, posicionInicial + new Vector3((ancho - comienzo) * Escala, 0f, 0f)));
            return new Puerta(paredes);
            
        }

        public void Draw(){
            foreach(var pared in Paredes){
                pared.Draw();
            }
        }
    }
}