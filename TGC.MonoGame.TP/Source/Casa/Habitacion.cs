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

            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Torre , new Vector3(1500f,0f,2500f)));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , new Vector3(1500f,0f,3500f)));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Torre , new Vector3(2500f,0f,3500f)));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , new Vector3(2500f,0f,2500f)));


            oficina.Piso.Oficina();
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_SillaOficina,new Vector3(1000f,0f,1000f), new Vector3(-MathHelper.PiOver2,MathHelper.PiOver4,0f), 10f));
            oficina.AddElemento(new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(1000f,500f,1000f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));

            return oficina;
        }
        public static Habitacion Cocina(int ancho, int alto, Vector3 posicionInicial){
            Habitacion cocina = new Habitacion(ancho,alto,posicionInicial);
            cocina.Piso.Cocina();
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
            for(int i = 0 ; i<100*20 ; i+=100){
                principal.AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3(   i * 10, 400f , -300f ), Vector3.Zero, 10f));
            }
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Mesa, new Vector3(4150f,0f,4150f), new Vector3(0, MathHelper.PiOver2, 0), 12f ));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Mesa, new Vector3(300f,0f,4150f), 12f ));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Inodoro, new Vector3(300f,0f,2150f), new Vector3(0, MathHelper.PiOver2, 0), 15f ));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(2500f,300f,2500f), 10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, new Vector3(6500f,1000f,4500f), new Vector3(0, 0, MathHelper.PiOver2),10f));
            principal.AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV, new Vector3(6500f,0f,4500f)));

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