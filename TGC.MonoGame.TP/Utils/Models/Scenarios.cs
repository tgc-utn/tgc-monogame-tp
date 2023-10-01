using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Scenarios
{
    public static readonly ScenaryReference Plane = new ScenaryReference(
        $"{ContentFolder.Models}/scenary/plane",
        1.4f,
        Matrix.Identity,
        Color.Black,
        new Vector3(-475f, 0f, 0f),
        new Vector3(475f, 0f, 0f),
        new List<PropReference>
        {
            // El mapa mide (si tomamos como escala 1f) 550f x 550f. Entonces -275f <= x <= 275f

            #region Rocas Externas

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                100,
                new FunctionLinear
                {
                    StartX = 182f,
                    StartZ = 182f,
                    EndX = 182f,
                    EndZ = -182f
                }
            )),

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                10,
                new FunctionLinear
                {
                    StartX = 182f,
                    StartZ = 182f,
                    EndX = 182f,
                    EndZ = -182f
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                100,
                new FunctionLinear
                {
                    StartX = -182f,
                    StartZ = 182f,
                    EndX = -182f,
                    EndZ = -182f
                }
            )),

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                10,
                new FunctionLinear
                {
                    StartX = -182f,
                    StartZ = 182f,
                    EndX = -182f,
                    EndZ = -182f
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                100,
                new FunctionLinear
                {
                    StartX = 182f,
                    StartZ = 182f,
                    EndX = -182f,
                    EndZ = 182f
                }
            )),

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                10,
                new FunctionLinear
                {
                    StartX = 182f,
                    StartZ = 182f,
                    EndX = -182f,
                    EndZ = 182f
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                100,
                new FunctionLinear
                {
                    StartX = 182f,
                    StartZ = -182f,
                    EndX = -182f,
                    EndZ = -182f
                }
            )),

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                10,
                new FunctionLinear
                {
                    StartX = 182f,
                    StartZ = -182f,
                    EndX = -182f,
                    EndZ = -182f
                }
            )),

            # endregion

            #region Zona Aliada

            #region Rocas Base

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                7,
                new FunctionLinear
                {
                    StartX = 123.8f,
                    StartZ = 182f,
                    EndX = 123.8f,
                    EndZ = 26f,
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                20,
                new FunctionSinusoidal
                {
                    StartX = 123.8f,
                    StartZ = 182f,
                    EndX = 123.8f,
                    EndZ = 130f,
                    Amplitude = 5f,
                    Periods = -1f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = 123.8f,
                    StartZ = 104f,
                    EndX = 123.8f,
                    EndZ = 26f,
                    Amplitude = 5f,
                    Periods = 1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                7,
                new FunctionLinear
                {
                    StartX = 123.8f,
                    StartZ = -26f,
                    EndX = 123.8f,
                    EndZ = -182f,
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = 123.8f,
                    StartZ = -26f,
                    EndX = 123.8f,
                    EndZ = -104f,
                    Amplitude = 5f,
                    Periods = 1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                20,
                new FunctionSinusoidal
                {
                    StartX = 123.8f,
                    StartZ = -130f,
                    EndX = 123.8f,
                    EndZ = -182f,
                    Amplitude = 5f,
                    Periods = 1f,
                    UseX = true
                }
            )),

            #endregion

            #region Rocas - Medio

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                15,
                new FunctionLinear
                {
                    StartX = 60.5f,
                    StartZ = 182f,
                    EndX = 60.5f,
                    EndZ = -182f,
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = 60.5f,
                    StartZ = 182f,
                    EndX = 60.5f,
                    EndZ = 104f,
                    Amplitude = 5f,
                    Periods = 1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = 60.5f,
                    StartZ = 78f,
                    EndX = 60.5f,
                    EndZ = 0f,
                    Amplitude = 5f,
                    Periods = 1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = 60.5f,
                    StartZ = 0f,
                    EndX = 60.5f,
                    EndZ = -78f,
                    Amplitude = 5f,
                    Periods = 1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = 60.5f,
                    StartZ = -104f,
                    EndX = 60.5f,
                    EndZ = -182f,
                    Amplitude = 5f,
                    Periods = 1.5f,
                    UseX = true
                }
            )),

            #endregion

            #region Ciudad Aliada

            #region Mitad Izquierda

            //Limite Lateral Arriba
            new PropReference(Props.BuildingHouse2, new Vector3(100f, 0f, -190f)),
            new PropReference(Props.BuildingHouse5, new Vector3(95f, 0f, -210f)),
            new PropReference(Props.BuildingHouse13, new Vector3(110f, 0f, -210f)),
            new PropReference(Props.BuildingHouse11, new Vector3(110f, 0f, -260f)),
            new PropReference(Props.BuildingHouse0, new Vector3(105f, 0f, -197.5f)),
            new PropReference(Props.BuildingHouse3, new Vector3(115f, 0f, -225f)),
            new PropReference(Props.BuildingHouse2, new Vector3(115f, 0f, -235f)),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, -225f)),
            new PropReference(Props.BuildingHouse10, new Vector3(115f, 0f, -245f)),
            new PropReference(Props.BuildingHouse7, new Vector3(125f, 0f, -250f)),
            new PropReference(Props.BuildingHouse14, new Vector3(120f, 0f, -240f)),
            new PropReference(Props.BuildingHouse6, new Vector3(105f, 0f, -225f)),

            //Limite Lateral Abajo
            new PropReference(Props.BuildingHouse12, new Vector3(150f, 0f, -260f)),
            new PropReference(Props.BuildingHouse9, new Vector3(160f, 0f, -240f)),
            new PropReference(Props.BuildingHouse1, new Vector3(175f, 0f, -235f)),
            new PropReference(Props.BuildingHouse4, new Vector3(180f, 0f, -215f)),
            new PropReference(Props.BuildingHouse17, new Vector3(175f, 0f, -230f)),
            new PropReference(Props.BuildingHouse18, new Vector3(170f, 0f, -260f)),
            new PropReference(Props.BuildingHouse19, new Vector3(180f, 0f, -200f)),
            new PropReference(Props.BuildingHouse0, new Vector3(170f, 0f, -250f)),
            new PropReference(Props.BuildingHouse3, new Vector3(165f, 0f, -225f)),
            new PropReference(Props.BuildingHouse2, new Vector3(170f, 0f, -240f)),
            new PropReference(Props.BuildingHouse13, new Vector3(163f, 0f, -251f)),
            new PropReference(Props.BuildingHouse16, new Vector3(175f, 0f, -260f)),

            //Centro 
            new PropReference(Props.BuildingHouse10, new Vector3(125f, 0f, -9f)),
            new PropReference(Props.BuildingHouse16, new Vector3(135f, 0f, -9f)),
            new PropReference(Props.BuildingHouse10, new Vector3(145f, 0f, -9f)),
            new PropReference(Props.BuildingHouse13, new Vector3(120f, 0f, -20f)),
            new PropReference(Props.BuildingHouse12, new Vector3(135f, 0f, -20f)),
            new PropReference(Props.BuildingHouse13, new Vector3(150f, 0f, -20f)),
            new PropReference(Props.BuildingHouse15, new Vector3(120f, 0f, -30f)),
            new PropReference(Props.BuildingHouse16, new Vector3(120f, 0f, -35f)),
            new PropReference(Props.BuildingHouse11, new Vector3(135f, 0f, -35f)),
            new PropReference(Props.BuildingHouse16, new Vector3(150f, 0f, -35f)),
            new PropReference(Props.BuildingHouse15, new Vector3(150f, 0f, -30f)),
            new PropReference(Props.BuildingHouse3, new Vector3(125f, 0f, -47.5f)),
            new PropReference(Props.BuildingHouse18, new Vector3(135f, 0f, -47.5f)),
            new PropReference(Props.BuildingHouse3, new Vector3(145f, 0f, -47.5f)),
            new PropReference(Props.BuildingHouse12, new Vector3(135f, 0f, -60f)),
            new PropReference(Props.BuildingHouse19, new Vector3(140f, 0f, -72.5f)),
            new PropReference(Props.BuildingHouse19, new Vector3(130f, 0f, -72.5f)),
            new PropReference(Props.BuildingHouse5, new Vector3(130f, 0f, -82.5f)),
            new PropReference(Props.BuildingHouse5, new Vector3(140f, 0f, -82.5f)),
            new PropReference(Props.BuildingHouse2, new Vector3(130f, 0f, -92.5f)),
            new PropReference(Props.BuildingHouse2, new Vector3(140f, 0f, -92.5f)),
            new PropReference(Props.BuildingHouse4, new Vector3(135f, 0f, -102.5f)),
            new PropReference(Props.BuildingHouse16, new Vector3(137.5f, 0f, -112.5f)),
            new PropReference(Props.BuildingHouse16, new Vector3(132.5f, 0f, -112.5f)),
            new PropReference(Props.BuildingHouse10, new Vector3(135f, 0f, -117.5f)),
            new PropReference(Props.BuildingHouse14, new Vector3(135f, 0f, -125f)),
            new PropReference(Props.BuildingHouse7, new Vector3(135f, 0f, -132.5f)),
            new PropReference(Props.BuildingHouse18, new Vector3(135f, 0f, -142.5f)),
            new PropReference(Props.BuildingHouse17, new Vector3(135f, 0f, -150f)),

            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(130f, 0f, -180f)),
            new PropReference(Props.BuildingHouse15, new Vector3(143f, 0f, -180f)),
            new PropReference(Props.BuildingHouse13, new Vector3(155f, 0f, -160f)),
            new PropReference(Props.BuildingHouse10, new Vector3(155f, 0f, -170f)),
            new PropReference(Props.BuildingHouse12, new Vector3(155f, 0f, -180f)),
            new PropReference(Props.BuildingHouse0, new Vector3(170f, 0f, -175f)),
            new PropReference(Props.BuildingHouse3, new Vector3(155f, 0f, -195f)),
            new PropReference(Props.BuildingHouse4, new Vector3(142.5f, 0f, -195f)),
            new PropReference(Props.BuildingHouse10, new Vector3(145f, 0f, -205f)),
            new PropReference(Props.BuildingHouse5, new Vector3(145f, 0f, -215f)),
            new PropReference(Props.BuildingHouse14, new Vector3(130f, 0f, -187.5f)),
            new PropReference(Props.BuildingHouse0, new Vector3(130f, 0f, -195f)),
            new PropReference(Props.BuildingHouse19, new Vector3(130f, 0f, -205f)),
            new PropReference(Props.BuildingHouse13, new Vector3(135f, 0f, -220f)),
            new PropReference(Props.BuildingHouse3,
                new Vector3(120f, 0f, -165f)), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(120f, 0f, -172.5f)),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, -165f)),
            new PropReference(Props.BuildingHouse6, new Vector3(105f, 0f, -100f)),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, -25f)),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, -35f)),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, -40f)),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, -45f)),
            new PropReference(Props.BuildingHouse19, new Vector3(95f, 0f, -55f)),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, -65f)),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, -70f)),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, -75f)),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, -85f)),
            new PropReference(Props.BuildingHouse8, new Vector3(100f, 0f, -1.5f)),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(165f, 0f, -120f)),
            new PropReference(Props.BuildingHouse12, new Vector3(180f, 0f, -130f)),
            new PropReference(Props.BuildingHouse13, new Vector3(180f, 0f, -145f)),
            new PropReference(Props.BuildingHouse6, new Vector3(180f, 0f, -120f)),
            new PropReference(Props.BuildingHouse0, new Vector3(165f, 0f, -130f)),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, -110f)),
            new PropReference(Props.BuildingHouse16, new Vector3(177.5f, 0f, -110f)),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, -104f)),
            new PropReference(Props.BuildingHouse15, new Vector3(175f, 0f, -104f)),
            new PropReference(Props.BuildingHouse19, new Vector3(165f, 0f, -95f)),
            new PropReference(Props.BuildingHouse18, new Vector3(172.5f, 0f, -95f)),
            new PropReference(Props.BuildingHouse3, new Vector3(165f, 0f, -85f)),
            new PropReference(Props.BuildingHouse16, new Vector3(175f, 0f, -85f)),
            new PropReference(Props.BuildingHouse4, new Vector3(170f, 0f, -70f)),
            new PropReference(Props.BuildingHouse15, new Vector3(180f, 0f, -70f)),
            new PropReference(Props.BuildingHouse14, new Vector3(180f, 0f, -65f)),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, -60f)),
            new PropReference(Props.BuildingHouse13, new Vector3(185f, 0f, -55f)),
            new PropReference(Props.BuildingHouse5, new Vector3(175f, 0f, -50f)),
            new PropReference(Props.BuildingHouse0, new Vector3(175f, 0f, -40f)),
            new PropReference(Props.BuildingHouse6, new Vector3(182.5f, 0f, -40f)),
            new PropReference(Props.BuildingHouse1, new Vector3(175f, 0f, -32.5f)),
            new PropReference(Props.BuildingHouse16, new Vector3(181f, 0f, -32.5f)),

            #endregion

            #region Mitad Derecha

            //Limite Lateral Arriba
            new PropReference(Props.BuildingHouse2, new Vector3(100f, 0f, 190f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(95f, 0f, 210f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(110f, 0f, 210f + 8f)),
            new PropReference(Props.BuildingHouse11, new Vector3(110f, 0f, 260f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(105f, 0f, 197.5f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(115f, 0f, 225f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(115f, 0f, 235f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, 225f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(115f, 0f, 245f + 8f)),
            new PropReference(Props.BuildingHouse7, new Vector3(125f, 0f, 250f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(120f, 0f, 240f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(105f, 0f, 225f + 8f)),

            //Limite Lateral Abajo
            new PropReference(Props.BuildingHouse12, new Vector3(150f, 0f, 260f + 8f)),
            new PropReference(Props.BuildingHouse9, new Vector3(160f, 0f, 240f + 8f)),
            new PropReference(Props.BuildingHouse1, new Vector3(175f, 0f, 235f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(180f, 0f, 215f + 8f)),
            new PropReference(Props.BuildingHouse17, new Vector3(175f, 0f, 230f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(170f, 0f, 260f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(180f, 0f, 200f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(170f, 0f, 250f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(165f, 0f, 225f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(170f, 0f, 240f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(163f, 0f, 251f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(175f, 0f, 260f + 8f)),

            //Centro 
            new PropReference(Props.BuildingHouse10, new Vector3(125f, 0f, 9f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(135f, 0f, 9f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(145f, 0f, 9f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(120f, 0f, 20f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(135f, 0f, 20f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(150f, 0f, 20f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(120f, 0f, 30f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(120f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse11, new Vector3(135f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(150f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(150f, 0f, 30f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(125f, 0f, 47.5f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(135f, 0f, 47.5f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(145f, 0f, 47.5f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(135f, 0f, 60f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(140f, 0f, 72.5f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(130f, 0f, 72.5f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(130f, 0f, 82.5f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(140f, 0f, 82.5f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(130f, 0f, 92.5f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(140f, 0f, 92.5f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(135f, 0f, 102.5f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(137.5f, 0f, 112.5f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(132.5f, 0f, 112.5f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(135f, 0f, 117.5f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(135f, 0f, 125f + 8f)),
            new PropReference(Props.BuildingHouse7, new Vector3(135f, 0f, 132.5f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(135f, 0f, 142.5f + 8f)),
            new PropReference(Props.BuildingHouse17, new Vector3(135f, 0f, 150f + 8f)),

            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(130f, 0f, 180f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(143f, 0f, 180f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(155f, 0f, 160f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(155f, 0f, 170f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(155f, 0f, 180f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(170f, 0f, 175f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(155f, 0f, 195f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(142.5f, 0f, 195f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(145f, 0f, 205f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(145f, 0f, 215f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(130f, 0f, 187.5f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(130f, 0f, 195f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(130f, 0f, 205f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(135f, 0f, 220f + 8f)),
            new PropReference(Props.BuildingHouse3,
                new Vector3(120f, 0f, 165f + 8f)), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(120f, 0f, 172.5f + 8f)),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, 165f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(105f, 0f, 100f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, 25f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, 40f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, 45f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(95f, 0f, 55f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, 65f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, 70f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, 75f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, 85f + 8f)),
            new PropReference(Props.BuildingHouse8, new Vector3(100f, 0f, 1.5f + 8f)),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(165f, 0f, 120f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(180f, 0f, 130f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(180f, 0f, 145f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(180f, 0f, 120f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(165f, 0f, 130f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, 110f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(177.5f, 0f, 110f + 8f)),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, 104f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(175f, 0f, 104f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(165f, 0f, 95f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(172.5f, 0f, 95f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(165f, 0f, 85f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(175f, 0f, 85f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(170f, 0f, 70f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(180f, 0f, 70f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(180f, 0f, 65f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, 60f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(185f, 0f, 55f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(175f, 0f, 50f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(175f, 0f, 40f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(182.5f, 0f, 40f + 8f)),
            new PropReference(Props.BuildingHouse1, new Vector3(175f, 0f, 32.5f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(181f, 0f, 32.5f + 8f)),

            #endregion

            #endregion

            #endregion

            #region Zona Enemiga

            #region Rocas Enemigas

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                7,
                new FunctionLinear
                {
                    StartX = -123.8f,
                    StartZ = 182f,
                    EndX = -123.8f,
                    EndZ = 26f,
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                20,
                new FunctionSinusoidal
                {
                    StartX = -123.8f,
                    StartZ = 182f,
                    EndX = -123.8f,
                    EndZ = 130f,
                    Amplitude = 5f,
                    Periods = 1f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = -123.8f,
                    StartZ = 104f,
                    EndX = -123.8f,
                    EndZ = 26f,
                    Amplitude = 5f,
                    Periods = -1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                7,
                new FunctionLinear
                {
                    StartX = -123.8f,
                    StartZ = -26f,
                    EndX = -123.8f,
                    EndZ = -182f,
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = -123.8f,
                    StartZ = -26f,
                    EndX = -123.8f,
                    EndZ = -104f,
                    Amplitude = 5f,
                    Periods = -1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                20,
                new FunctionSinusoidal
                {
                    StartX = -123.8f,
                    StartZ = -130f,
                    EndX = -123.8f,
                    EndZ = -182f,
                    Amplitude = 5f,
                    Periods = -1f,
                    UseX = true
                }
            )),

            #endregion

            #region Rocas Enemigas - Centro

            new PropReference(Props.Rock1, new Vector3(0, 0, 0), new Repetition(
                15,
                new FunctionLinear
                {
                    StartX = -60.5f,
                    StartZ = 182f,
                    EndX = -60.5f,
                    EndZ = -182f,
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = -60.5f,
                    StartZ = 182f,
                    EndX = -60.5f,
                    EndZ = 104f,
                    Amplitude = 5f,
                    Periods = -1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = -60.5f,
                    StartZ = 78f,
                    EndX = -60.5f,
                    EndZ = 0f,
                    Amplitude = 5f,
                    Periods = -1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = -60.5f,
                    StartZ = 0f,
                    EndX = -60.5f,
                    EndZ = -78f,
                    Amplitude = 5f,
                    Periods = -1.5f,
                    UseX = true
                }
            )),

            new PropReference(Props.Rock2, new Vector3(0, 0, 0), new Repetition(
                30,
                new FunctionSinusoidal
                {
                    StartX = -60.5f,
                    StartZ = -104f,
                    EndX = -60.5f,
                    EndZ = -182f,
                    Amplitude = 5f,
                    Periods = -1.5f,
                    UseX = true
                }
            )),

            #endregion

            #region Ciudad Enemiga

            #region Mitad Izquierda

            //Limite Lateral Arriba
            new PropReference(Props.BuildingHouse2, new Vector3(-100f, 0f, -190f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-95f, 0f, -210f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-110f, 0f, -210f)),
            new PropReference(Props.BuildingHouse11, new Vector3(-110f, 0f, -260f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-105f, 0f, -197.5f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-115f, 0f, -225f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-115f, 0f, -235f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, -225f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-115f, 0f, -245f)),
            new PropReference(Props.BuildingHouse7, new Vector3(-125f, 0f, -250f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-120f, 0f, -240f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-105f, 0f, -225f)),

            //Limite Lateral Abajo
            new PropReference(Props.BuildingHouse12, new Vector3(-150f, 0f, -260f)),
            new PropReference(Props.BuildingHouse9, new Vector3(-160f, 0f, -240f)),
            new PropReference(Props.BuildingHouse1, new Vector3(-175f, 0f, -235f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-180f, 0f, -215f)),
            new PropReference(Props.BuildingHouse17, new Vector3(-175f, 0f, -230f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-170f, 0f, -260f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-180f, 0f, -200f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-170f, 0f, -250f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-165f, 0f, -225f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-170f, 0f, -240f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-163f, 0f, -251f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-175f, 0f, -260f)),

            //Centro 
            new PropReference(Props.BuildingHouse10, new Vector3(-125f, 0f, -9f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-135f, 0f, -9f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-145f, 0f, -9f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-120f, 0f, -20f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-135f, 0f, -20f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-150f, 0f, -20f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-120f, 0f, -30f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-120f, 0f, -35f)),
            new PropReference(Props.BuildingHouse11, new Vector3(-135f, 0f, -35f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-150f, 0f, -35f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-150f, 0f, -30f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-125f, 0f, -47.5f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-135f, 0f, -47.5f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-145f, 0f, -47.5f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-135f, 0f, -60f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-140f, 0f, -72.5f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-130f, 0f, -72.5f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-130f, 0f, -82.5f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-140f, 0f, -82.5f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-130f, 0f, -92.5f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-140f, 0f, -92.5f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-135f, 0f, -102.5f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-137.5f, 0f, -112.5f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-132.5f, 0f, -112.5f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-135f, 0f, -117.5f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-135f, 0f, -125f)),
            new PropReference(Props.BuildingHouse7, new Vector3(-135f, 0f, -132.5f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-135f, 0f, -142.5f)),
            new PropReference(Props.BuildingHouse17, new Vector3(-135f, 0f, -150f)),

            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(-130f, 0f, -180f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-143f, 0f, -180f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-155f, 0f, -160f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-155f, 0f, -170f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-155f, 0f, -180f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-170f, 0f, -175f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-155f, 0f, -195f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-142.5f, 0f, -195f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-145f, 0f, -205f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-145f, 0f, -215f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-130f, 0f, -187.5f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-130f, 0f, -195f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-130f, 0f, -205f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-135f, 0f, -220f)),
            new PropReference(Props.BuildingHouse3,
                new Vector3(-120f, 0f, -165f)), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(-120f, 0f, -172.5f)),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, -165f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-105f, 0f, -100f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, -25f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, -35f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, -40f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, -45f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-95f, 0f, -55f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, -65f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, -70f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, -75f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, -85f)),
            new PropReference(Props.BuildingHouse8, new Vector3(-100f, 0f, -1.5f)),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(-165f, 0f, -120f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-180f, 0f, -130f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-180f, 0f, -145f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-180f, 0f, -120f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-165f, 0f, -130f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, -110f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-177.5f, 0f, -110f)),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, -104f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-175f, 0f, -104f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-165f, 0f, -95f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-172.5f, 0f, -95f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-165f, 0f, -85f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-175f, 0f, -85f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-170f, 0f, -70f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-180f, 0f, -70f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-180f, 0f, -65f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, -60f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-185f, 0f, -55f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-175f, 0f, -50f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-175f, 0f, -40f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-182.5f, 0f, -40f)),
            new PropReference(Props.BuildingHouse1, new Vector3(-175f, 0f, -32.5f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-181f, 0f, -32.5f)),

            #endregion

            #region Mitad Derecha

            //Limite Lateral Arriba
            new PropReference(Props.BuildingHouse2, new Vector3(-100f, 0f, 190f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-95f, 0f, 210f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-110f, 0f, 210f + 8f)),
            new PropReference(Props.BuildingHouse11, new Vector3(-110f, 0f, 260f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-105f, 0f, 197.5f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-115f, 0f, 225f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-115f, 0f, 235f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, 225f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-115f, 0f, 245f + 8f)),
            new PropReference(Props.BuildingHouse7, new Vector3(-125f, 0f, 250f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-120f, 0f, 240f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-105f, 0f, 225f + 8f)),

            //Limite Lateral Abajo
            new PropReference(Props.BuildingHouse12, new Vector3(-150f, 0f, 260f + 8f)),
            new PropReference(Props.BuildingHouse9, new Vector3(-160f, 0f, 240f + 8f)),
            new PropReference(Props.BuildingHouse1, new Vector3(-175f, 0f, 235f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-180f, 0f, 215f + 8f)),
            new PropReference(Props.BuildingHouse17, new Vector3(-175f, 0f, 230f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-170f, 0f, 260f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-180f, 0f, 200f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-170f, 0f, 250f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-165f, 0f, 225f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-170f, 0f, 240f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-163f, 0f, 251f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-175f, 0f, 260f + 8f)),

            //Centro 
            new PropReference(Props.BuildingHouse10, new Vector3(-125f, 0f, 9f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-135f, 0f, 9f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-145f, 0f, 9f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-120f, 0f, 20f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-135f, 0f, 20f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-150f, 0f, 20f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-120f, 0f, 30f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-120f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse11, new Vector3(-135f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-150f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-150f, 0f, 30f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-125f, 0f, 47.5f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-135f, 0f, 47.5f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-145f, 0f, 47.5f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-135f, 0f, 60f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-140f, 0f, 72.5f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-130f, 0f, 72.5f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-130f, 0f, 82.5f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-140f, 0f, 82.5f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-130f, 0f, 92.5f + 8f)),
            new PropReference(Props.BuildingHouse2, new Vector3(-140f, 0f, 92.5f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-135f, 0f, 102.5f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-137.5f, 0f, 112.5f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-132.5f, 0f, 112.5f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-135f, 0f, 117.5f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-135f, 0f, 125f + 8f)),
            new PropReference(Props.BuildingHouse7, new Vector3(-135f, 0f, 132.5f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-135f, 0f, 142.5f + 8f)),
            new PropReference(Props.BuildingHouse17, new Vector3(-135f, 0f, 150f + 8f)),

            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(-130f, 0f, 180f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-143f, 0f, 180f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-155f, 0f, 160f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-155f, 0f, 170f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-155f, 0f, 180f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-170f, 0f, 175f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-155f, 0f, 195f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-142.5f, 0f, 195f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-145f, 0f, 205f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-145f, 0f, 215f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-130f, 0f, 187.5f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-130f, 0f, 195f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-130f, 0f, 205f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-135f, 0f, 220f + 8f)),
            new PropReference(Props.BuildingHouse3,
                new Vector3(-120f, 0f, 165f + 8f)), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(-120f, 0f, 172.5f + 8f)),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, 165f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-105f, 0f, 100f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, 25f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, 35f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, 40f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, 45f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-95f, 0f, 55f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, 65f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, 70f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, 75f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, 85f + 8f)),
            new PropReference(Props.BuildingHouse8, new Vector3(-100f, 0f, 1.5f + 8f)),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(-165f, 0f, 120f + 8f)),
            new PropReference(Props.BuildingHouse12, new Vector3(-180f, 0f, 130f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-180f, 0f, 145f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-180f, 0f, 120f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-165f, 0f, 130f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, 110f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-177.5f, 0f, 110f + 8f)),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, 104f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-175f, 0f, 104f + 8f)),
            new PropReference(Props.BuildingHouse19, new Vector3(-165f, 0f, 95f + 8f)),
            new PropReference(Props.BuildingHouse18, new Vector3(-172.5f, 0f, 95f + 8f)),
            new PropReference(Props.BuildingHouse3, new Vector3(-165f, 0f, 85f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-175f, 0f, 85f + 8f)),
            new PropReference(Props.BuildingHouse4, new Vector3(-170f, 0f, 70f + 8f)),
            new PropReference(Props.BuildingHouse15, new Vector3(-180f, 0f, 70f + 8f)),
            new PropReference(Props.BuildingHouse14, new Vector3(-180f, 0f, 65f + 8f)),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, 60f + 8f)),
            new PropReference(Props.BuildingHouse13, new Vector3(-185f, 0f, 55f + 8f)),
            new PropReference(Props.BuildingHouse5, new Vector3(-175f, 0f, 50f + 8f)),
            new PropReference(Props.BuildingHouse0, new Vector3(-175f, 0f, 40f + 8f)),
            new PropReference(Props.BuildingHouse6, new Vector3(-182.5f, 0f, 40f + 8f)),
            new PropReference(Props.BuildingHouse1, new Vector3(-175f, 0f, 32.5f + 8f)),
            new PropReference(Props.BuildingHouse16, new Vector3(-181f, 0f, 32.5f + 8f)),

            #endregion

            #endregion

            #endregion

            #region Rocas Centro

            new PropReference(Props.Rock0, new Vector3(0, 0, 0), new Repetition(
                25,
                new FunctionCircular
                {
                    CenterX = 0f,
                    CenterZ = 0f,
                    Radius = 40f,
                }
            )),

            #endregion

            #region Granjas Aliadas

            new PropReference(Props.Farm, new Vector3(0, 0, 0), new Repetition(
                3,
                new FunctionLinear
                {
                    StartX = 475f,
                    StartZ = 450f,
                    EndX = 475f,
                    EndZ = -450f,
                }
            )),

            #endregion

            #region Granjas Enemigas

            new PropReference(Props.Farm2, new Vector3(0, 0, 0), new Repetition(
                3,
                new FunctionLinear
                {
                    StartX = -475f,
                    StartZ = 450f,
                    EndX = -475f,
                    EndZ = -450f,
                }
            )),

            #endregion
        }
    );
}