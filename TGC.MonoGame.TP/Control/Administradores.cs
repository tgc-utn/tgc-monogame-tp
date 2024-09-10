using System;
using System.Collections.Generic;
using System.Timers;
using Escenografia;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Control
{
    class AdministradorNPCs
    {
        static Random RNG = new Random();
        List<AutoNPC> npcs;
        //genera un monton de npcs al azar en el mapa ( suponiendo que es plano por ahora )
        public void generarNPCsV1(Vector3 minPos,Vector3 maxPos)
        {
            float ancho = maxPos.X - minPos.X;
            float alto = maxPos.Z - minPos.Z;
            npcs = new List<AutoNPC>(50);
            AutoNPC holder;
            float desplazamiento = Math.Max(Math.Min(ancho,alto),1f);
            int autos_linea = 0;
            for ( int i=0; i<ancho; i++)
            {
                autos_linea = 0;
                for ( int j=0; j<alto; j++)
                {
                    if ( autos_linea < 10)
                    {
                        holder = new AutoNPC(minPos + new Vector3(j,0f,i) * desplazamiento);
                        npcs.Add(holder);
                        autos_linea ++;
                    }
                    else{
                        
                        break;
                    }
                }
                if ( npcs.Count >= 50)
                {
                    break;
                }
            }
        }
        //crea un monton de autos identicos
        //este los genera en un circulo ( me gustan mas los escenarios circulares, todavia mas para este caso )
        public void generadorNPCsV2(Vector3 centro, float radio, int numeroNPCs)
        {
            float distanciaCentro, anguloDesdeCentro;
            Vector3 puntoPlano;
            npcs = new List<AutoNPC>(numeroNPCs);
            AutoNPC holder;
            for ( int i=0; i< numeroNPCs; i++)
            {
                distanciaCentro = (float)(RNG.NextDouble() * radio);
                anguloDesdeCentro = (float)(RNG.NextDouble() * Math.Tau);
                puntoPlano = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(anguloDesdeCentro)) * distanciaCentro;
                //Console.WriteLine(distanciaCentro);
                holder = new AutoNPC(puntoPlano + centro, 
                0, 
                Convert.ToSingle(RNG.NextDouble() * Math.PI),
                0, 
                new Color( (float)RNG.NextDouble(), (float)RNG.NextDouble(), (float)RNG.NextDouble()));
                //Console.WriteLine(holder.getWorldMatrix());
                npcs.Add(holder);
            }
        }
        public void loadModelosAutos(String[] direccionesModelos, String[] direccionesEfectos, ContentManager content)
        {
            //cargamos todos los modelos al azar
            foreach( AutoNPC auto in npcs)
            {
                Random rangen = new Random();
                
                auto.loadModel(direccionesModelos[rangen.Next(direccionesModelos.Length)],
                direccionesEfectos[rangen.Next(direccionesEfectos.Length)],content);
            }
        }
        public void drawAutos(Matrix view, Matrix projeccion)
        {
            
            foreach( AutoNPC auto in npcs )
            {
                auto.dibujar(view, projeccion, auto.color);
            }
        }
    }

    public class AdministradorConos{
        static Random RNG = new Random();
        List<Cono> conos;

        public void generarConos(Vector3 centro, float radio, int numeroNPCs)
        {
            float distanciaCentro, anguloDesdeCentro;
            Vector3 puntoPlano;
            conos = new List<Cono>(numeroNPCs);
            Cono holder;
            for ( int i=0; i< numeroNPCs; i++)
            {
                distanciaCentro = (float)(Math.Clamp(RNG.NextDouble() + 0.05f, 0.05f, 1f) * radio);
                anguloDesdeCentro = (float)(RNG.NextDouble() * Math.Tau);
                puntoPlano = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(anguloDesdeCentro)) * distanciaCentro;
                //Console.WriteLine(distanciaCentro);
                holder = new Cono(puntoPlano + centro);
                //Console.WriteLine(holder.getWorldMatrix());
                conos.Add(holder);
            }
        }
        public void loadModelosConos(String direccionesModelos, String direccionesEfectos, ContentManager content)
        {
            //cargamos todos los modelos al azar
            foreach( Cono cono in conos)
            {
                Random rangen = new Random();
                
                cono.loadModel(direccionesModelos,
                direccionesEfectos,content);
            }
        }
        public void drawConos(Matrix view, Matrix projeccion)
        {
            
            foreach( Cono cono in conos )
            {
                cono.SetScale(20f);
                cono.dibujar(view, projeccion, Color.Orange);
            }
        }
    }
}

    