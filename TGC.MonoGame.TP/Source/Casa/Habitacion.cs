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
            Paredes.Add(Pared.Arriba (10, 10, posicionInicial));
            Paredes.Add(Pared.Abajo (10, 10, posicionInicial));
            Paredes.Add(Pared.Izquierda (10, 10, posicionInicial));
            Paredes.Add(Pared.Derecha (10, 10, posicionInicial));
        }
        public static Habitacion Oficina(int ancho, int alto, Vector3 posicionInicial){
            var oficina = new Habitacion(alto, ancho, posicionInicial);

            oficina.Piso.Oficina();
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Misil, 5f,new Vector3(0f,0f,0f), new Vector3(0f,0f,0f)));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_SillaOficina, 10f,new Vector3(100f,0f,100f), new Vector3(-MathHelper.PiOver2,MathHelper.PiOver4,0f)));
            //oficina.AddElemento(new Mueble("Gabinete", 10f,Vector3.Zero, new Vector3(0f,0f,MathHelper.Pi)));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Cafe, 10f,new Vector3(100f,50f,100f), new Vector3(-MathHelper.PiOver2,0f,0f)));

            return oficina;
        }
        public static Habitacion Cocina(int ancho, int alto, Vector3 posicionInicial){
            Habitacion cocina = new Habitacion(ancho,alto,posicionInicial);
            cocina.Piso.Cocina();
            cocina.AddElemento( new Mueble(TGCGame.GameContent.M_Inodoro, 15f, new Vector3(100f,0f,100f), new Vector3(0, MathHelper.PiOver2*3f, 0)));
            cocina.AddElemento( new Mueble(TGCGame.GameContent.M_Inodoro, 15f, new Vector3(200f,0f,200f), new Vector3(0, 0, 0)));
            cocina.AddElemento( new Mueble(TGCGame.GameContent.M_Inodoro, 15f, new Vector3(300f,100f,300f), new Vector3(0, MathHelper.PiOver2, MathHelper.Pi)));
            cocina.AddElemento( new Mueble(TGCGame.GameContent.M_Inodoro, 15f, new Vector3(400f,0f,400f), new Vector3(0, MathHelper.PiOver2, 0)));
            return cocina;
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

            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Mesa, 12f, new Vector3(4150f,0f,4150f), new Vector3(0, MathHelper.PiOver2, 0) ));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, 10f, new Vector3(3650f,370f,3650f), Vector3.Zero));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, 10f, new Vector3(5150f,370f,4150f), new Vector3(0, MathHelper.PiOver2, 0)));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, 10f, new Vector3(4150f,370f,4650f), new Vector3(0, MathHelper.PiOver2, 0)));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, 10f, new Vector3(4150f,370f,5150f), new Vector3(0, MathHelper.PiOver2, 0)));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, 10f, new Vector3(4650f,370f,4650f), new Vector3(0, MathHelper.PiOver2, 0)));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Silla, 10f, new Vector3(5150f,370f,5150f), new Vector3(0, MathHelper.PiOver2, 0)));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Inodoro, 15f, new Vector3(300f,0f,2150f), new Vector3(0, MathHelper.PiOver2, 0) ));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, 10f, new Vector3(2500f,300f,2500f), Vector3.Zero));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, 10f, new Vector3(6500f,1000f,4500f), new Vector3(0, 0, MathHelper.PiOver2)));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV, 1f, new Vector3(6500f,0f,4500f), Vector3.Zero));

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