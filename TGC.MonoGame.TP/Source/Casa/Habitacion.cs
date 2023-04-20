using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class Habitacion
    {
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        private Piso Piso;
        private List<Pared> Paredes;
        private List<IElementoDinamico> MueblesDinamicos;
        private List<IElemento> Muebles;

        //Ancho y Alto en Cantidad de Baldosas
        private Habitacion(int ancho, int alto, Vector3 posicionInicial)
        {
            Ancho = ancho;
            Alto = alto;
            PosicionInicial = posicionInicial;

            MueblesDinamicos = new List<IElementoDinamico>();
            Muebles = new List<IElemento>();
            Piso = new Piso(ancho, alto, posicionInicial); // Se carga el default

            Paredes = new List<Pared>();
            Paredes.Add(Pared.Arriba (ancho, alto, posicionInicial));
            Paredes.Add(Pared.Abajo (ancho, alto, posicionInicial));
            Paredes.Add(Pared.Izquierda (ancho, alto, posicionInicial));
            Paredes.Add(Pared.Derecha (ancho, alto, posicionInicial));
        }
        public static Habitacion Oficina(int ancho, int alto, Vector3 posicionInicial){
            var oficina = new Habitacion(alto, ancho, posicionInicial);

            oficina.Piso.Oficina();
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_SillaOficina,new Vector3(1000f,0f,1000f), new Vector3(-MathHelper.PiOver2,MathHelper.PiOver4,0f), 10f));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(1000f,500f,1000f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));

            return oficina;
        }
        public static Habitacion Cocina(int ancho, int alto, Vector3 posicionInicial){
            Habitacion cocina = new Habitacion(ancho,alto,posicionInicial);
            #region Hormiguitas
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                cocina.AddDinamico(new EnemyCar(escala, posicionesAutosIA, Vector3.Zero));
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion

            cocina.Piso.Cocina();
            return cocina;
        }
        
        public static Habitacion SalaConferencias(int ancho, int alto, Vector3 posicionInicial){
            var salaConferencias = new Habitacion(ancho,alto,posicionInicial);
            salaConferencias.Piso.Rojo();

            #region Set Televisión, Rack y Sillas
            for(int i = 2000 ; i<1000*6 ; i+=1000){
                salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 7800f ), 10f));
                salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 7100f ), 10f));
                salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 6400f ), 10f));
                salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 5700f ), 10f));
            }
            salaConferencias.AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV,   new Vector3(2750f,50f,9200f)));
            salaConferencias.AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, new Vector3(3200f,150f,9200f),10f));            
            #endregion
            #region Set Sillones
            var RotacionSet = new Vector3(0f, MathHelper.PiOver4,0f);
            salaConferencias.AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(6000f,150f,4000f), new Vector3(0,MathHelper.PiOver2,0)+RotacionSet, 10f));
            salaConferencias.AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(5950f,150f,1850f),RotacionSet, 10f));
            salaConferencias.AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(8100f,150f,1900f), new Vector3(0,-MathHelper.PiOver2,0)+RotacionSet, 10f));
            #endregion
            #region Set Ajedrez
            var desplazamientoSet = new Vector3(6650f,0f,2650f);
            salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Torre , desplazamientoSet));
            salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , desplazamientoSet + new Vector3(0f,0f,400f)));
            salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Torre , desplazamientoSet + new Vector3(400f,0f,400f)));
            salaConferencias.AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , desplazamientoSet + new Vector3(400f,0f,0f)));

            #endregion

            return salaConferencias;
        }



        public static Habitacion Principal(int ancho, int alto, Vector3 posicionInicial){
            Habitacion principal = new Habitacion(ancho,alto,posicionInicial);
            #region CargaMueblesDinámicos
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                principal.AddDinamico(new EnemyCar(escala, posicionesAutosIA, Vector3.Zero));
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion
            
            #region CargaMueblesEstáticos

            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Mesa, new Vector3(4150f,0f,4150f), new Vector3(0, MathHelper.PiOver2, 0), 12f ));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4150f,370f,2950f), Vector3.Zero, 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4150f,370f,5150f), new Vector3(0, MathHelper.Pi, 0), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(3650f,370f,3650f), new Vector3(0, MathHelper.PiOver2, 0), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(3650f,370f,4450f), new Vector3(0, MathHelper.PiOver2, 0), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4750f,370f,3650f), new Vector3(0, -MathHelper.PiOver2*0.7f, 0), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4750f,370f,4450f), new Vector3(0, -MathHelper.PiOver2*1.3f, 0), 10f));

            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(7500f,100f,3500f), new Vector3(0, MathHelper.Pi, 0), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, new Vector3(7500f,150f,300f), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV,   new Vector3(7950f,50f,350f), new Vector3(0f,MathHelper.Pi,0f)));

            #endregion

            return principal;
        }

        public void AddDinamico( IElementoDinamico elem ){
            elem.newPosicionInicial(PosicionInicial);
            MueblesDinamicos.Add(elem);
        }
        private void AddElemento( IElemento elem ){
            elem.newPosicionInicial(PosicionInicial);
            Muebles.Add(elem);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState){
            foreach(var e in MueblesDinamicos){
                e.Update(gameTime, keyboardState);
            }
            return;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Piso.Draw(view, projection);
            foreach(var pared in Paredes)
                pared.Draw();
            foreach(var elemento in MueblesDinamicos)
                elemento.Draw(view, projection);
            foreach(var elemento in Muebles)
                elemento.Draw(view, projection);
        }
    }
}