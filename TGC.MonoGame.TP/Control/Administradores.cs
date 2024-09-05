using System;
using System.Collections.Generic;
using Escenografia;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Control
{
    class AdministradorNPCs
    {
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
        public void loadModelosAutos(String direccionModelo, String direccionEfecto, ContentManager content)
        {
            foreach( AutoNPC auto in npcs)
            {
                auto.loadModel(direccionModelo,direccionEfecto,content);
            }
        }
        public void drawAutos(Matrix view, Matrix projeccion, Color color)
        {
            foreach( AutoNPC auto in npcs )
            {
                auto.dibujar(view, projeccion, color);
            }
        }
    }
}