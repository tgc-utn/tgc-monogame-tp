using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Scenarios
{
    public static readonly ScenaryReference Plane = new ScenaryReference(
        Props.PlaneScene,
        new Vector3(-400f, 2f, 0f),
        new Vector3(400f, 2f, 0f),
        new List<Tuple<Vector3, Vector3>>
        {
            new(new Vector3(-525f, 0f, -525f), new Vector3(525f, 200f, -525f)),
            new(new Vector3(-525f, 0f, 525f), new Vector3(525f, 200f, 525f)),
            new(new Vector3(-525f, 0f, -525f), new Vector3(-525f, 200f, 525f)),
            new(new Vector3(525f, 0f, -525f), new Vector3(525f, 200f, 525f)),
        },
        new List<PropReference>
        {
            
            #region Rocas Externas

            new (Props.Rock2x5, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                19,
                new FunctionLinear
                {
                    StartX = 93.5f,
                    StartZ = 80f,
                    EndX = 93.5f,
                    EndZ = -100f
                }
            )),
            new (Props.Rock2x5, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                19,
                new FunctionLinear
                {
                    StartX = -93.5f,
                    StartZ = 80f,
                    EndX = -93.5f,
                    EndZ = -100f
                }
            )),
            new (Props.Rock2x5, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                19,
                new FunctionLinear
                {
                    StartX = 85f,
                    StartZ = 87.5f,
                    EndX = -85f,
                    EndZ = 87.5f
                }
            )),
            new (Props.Rock2x5, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                19,
                new FunctionLinear
                {
                    StartX = 85f,
                    StartZ = -102.5f,
                    EndX = -85f,
                    EndZ = -102.5f
                }
            )),

            # endregion
        },
        new PropReference(Props.SkyDome, new Vector3(0, -20, 0), PropType.Dome),
        new List<PropReference>
        {
            // El mapa mide (si tomamos como escala 1f) 550f x 550f. Entonces -275f <= x <= 275f

            #region Zona Aliada
            
            #region Ciudad Aliada

            #region Mitad Izquierda
            
            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(130f, 0f, -180f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(143f, 0f, -180f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(155f, 0f, -160f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(155f, 0f, -170f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(155f, 0f, -180f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(170f, 0f, -175f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(155f, 0f, -195f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(142.5f, 0f, -195f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(145f, 0f, -205f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(145f, 0f, -215f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(130f, 0f, -187.5f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(130f, 0f, -195f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(130f, 0f, -205f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(135f, 0f, -220f), PropType.Large),
            new PropReference(Props.BuildingHouse3,
                new Vector3(120f, 0f, -165f), PropType.Large), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(120f, 0f, -172.5f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, -165f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(105f, 0f, -100f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, -25f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, -35f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, -40f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, -45f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(95f, 0f, -55f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, -65f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, -70f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, -75f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, -85f), PropType.Large),
            new PropReference(Props.BuildingHouse8, new Vector3(100f, 0f, -1.5f), PropType.Large),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(165f, 0f, -120f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(180f, 0f, -130f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(180f, 0f, -145f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(180f, 0f, -120f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(165f, 0f, -130f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, -110f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(177.5f, 0f, -110f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, -104f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(175f, 0f, -104f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(165f, 0f, -95f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(172.5f, 0f, -95f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(165f, 0f, -85f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(175f, 0f, -85f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(170f, 0f, -70f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(180f, 0f, -70f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(180f, 0f, -65f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, -60f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(185f, 0f, -55f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(175f, 0f, -50f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(175f, 0f, -40f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(182.5f, 0f, -40f), PropType.Large),
            new PropReference(Props.BuildingHouse1, new Vector3(175f, 0f, -32.5f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(181f, 0f, -32.5f), PropType.Large),

            #endregion

            #region Mitad Derecha
            
            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(130f, 0f, 180f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(143f, 0f, 180f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(155f, 0f, 160f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(155f, 0f, 170f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(155f, 0f, 180f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(170f, 0f, 175f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(155f, 0f, 195f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(142.5f, 0f, 195f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(145f, 0f, 205f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(145f, 0f, 215f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(130f, 0f, 187.5f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(130f, 0f, 195f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(130f, 0f, 205f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(135f, 0f, 220f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse3,
                new Vector3(120f, 0f, 165f + 8f), PropType.Large), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(120f, 0f, 172.5f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, 165f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(105f, 0f, 100f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, 25f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, 35f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, 40f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, 45f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(95f, 0f, 55f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(95f, 0f, 65f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(95f, 0f, 70f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(100f, 0f, 75f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(102.5f, 0f, 85f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse8, new Vector3(100f, 0f, 1.5f + 8f), PropType.Large),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(165f, 0f, 120f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(180f, 0f, 130f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(180f, 0f, 145f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(180f, 0f, 120f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(165f, 0f, 130f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, 110f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(177.5f, 0f, 110f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(165f, 0f, 104f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(175f, 0f, 104f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(165f, 0f, 95f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(172.5f, 0f, 95f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(165f, 0f, 85f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(175f, 0f, 85f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(170f, 0f, 70f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(180f, 0f, 70f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(180f, 0f, 65f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(170f, 0f, 60f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(185f, 0f, 55f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(175f, 0f, 50f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(175f, 0f, 40f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(182.5f, 0f, 40f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse1, new Vector3(175f, 0f, 32.5f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(181f, 0f, 32.5f + 8f), PropType.Large),

            #endregion

            #endregion

            #endregion

            #region Zona Enemiga
            
            #region Ciudad Enemiga

            #region Mitad Izquierda
            
            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(-130f, 0f, -180f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-143f, 0f, -180f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-155f, 0f, -160f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-155f, 0f, -170f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(-155f, 0f, -180f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-170f, 0f, -175f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(-155f, 0f, -195f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(-142.5f, 0f, -195f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-145f, 0f, -205f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(-145f, 0f, -215f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(-130f, 0f, -187.5f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-130f, 0f, -195f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(-130f, 0f, -205f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-135f, 0f, -220f), PropType.Large),
            new PropReference(Props.BuildingHouse3,
                new Vector3(-120f, 0f, -165f), PropType.Large), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(-120f, 0f, -172.5f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, -165f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(-105f, 0f, -100f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, -25f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, -35f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, -40f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, -45f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(-95f, 0f, -55f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, -65f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, -70f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, -75f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, -85f), PropType.Large),
            new PropReference(Props.BuildingHouse8, new Vector3(-100f, 0f, -1.5f), PropType.Large),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(-165f, 0f, -120f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(-180f, 0f, -130f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-180f, 0f, -145f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(-180f, 0f, -120f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-165f, 0f, -130f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, -110f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-177.5f, 0f, -110f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, -104f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-175f, 0f, -104f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(-165f, 0f, -95f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(-172.5f, 0f, -95f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(-165f, 0f, -85f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-175f, 0f, -85f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(-170f, 0f, -70f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-180f, 0f, -70f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(-180f, 0f, -65f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, -60f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-185f, 0f, -55f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(-175f, 0f, -50f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-175f, 0f, -40f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(-182.5f, 0f, -40f), PropType.Large),
            new PropReference(Props.BuildingHouse1, new Vector3(-175f, 0f, -32.5f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-181f, 0f, -32.5f), PropType.Large),

            #endregion

            #region Mitad Derecha
            
            //Centro Superior y Lateral
            new PropReference(Props.BuildingHouse11, new Vector3(-130f, 0f, 180f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-143f, 0f, 180f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-155f, 0f, 160f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-155f, 0f, 170f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(-155f, 0f, 180f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-170f, 0f, 175f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(-155f, 0f, 195f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(-142.5f, 0f, 195f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-145f, 0f, 205f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(-145f, 0f, 215f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(-130f, 0f, 187.5f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-130f, 0f, 195f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(-130f, 0f, 205f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-135f, 0f, 220f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse3,
                new Vector3(-120f, 0f, 165f + 8f), PropType.Large), // Se puede eliminar si es dificil pasar uwu
            new PropReference(Props.BuildingHouse15, new Vector3(-120f, 0f, 172.5f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, 165f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(-105f, 0f, 100f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, 25f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, 35f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, 40f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, 45f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(-95f, 0f, 55f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-95f, 0f, 65f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-95f, 0f, 70f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(-100f, 0f, 75f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-102.5f, 0f, 85f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse8, new Vector3(-100f, 0f, 1.5f + 8f), PropType.Large),

            // Centro Inferior
            new PropReference(Props.BuildingHouse11, new Vector3(-165f, 0f, 120f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(-180f, 0f, 130f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-180f, 0f, 145f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(-180f, 0f, 120f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-165f, 0f, 130f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, 110f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-177.5f, 0f, 110f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse9, new Vector3(-165f, 0f, 104f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-175f, 0f, 104f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse19, new Vector3(-165f, 0f, 95f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse18, new Vector3(-172.5f, 0f, 95f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(-165f, 0f, 85f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-175f, 0f, 85f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse4, new Vector3(-170f, 0f, 70f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse15, new Vector3(-180f, 0f, 70f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse14, new Vector3(-180f, 0f, 65f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse10, new Vector3(-170f, 0f, 60f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(-185f, 0f, 55f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(-175f, 0f, 50f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse0, new Vector3(-175f, 0f, 40f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse6, new Vector3(-182.5f, 0f, 40f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse1, new Vector3(-175f, 0f, 32.5f + 8f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(-181f, 0f, 32.5f + 8f), PropType.Large),

            #endregion

            #endregion

            #endregion

            #region Rocas Centro

            new PropReference(Props.Rock0x3, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                6,
                new FunctionCircular
                {
                    CenterX = 0f,
                    CenterZ = 0f,
                    Radius = 30f,
                }
            )),

            #endregion

            #region Granjas Aliadas

            new PropReference(Props.Farm, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                2,
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

            new PropReference(Props.Farm2, new Vector3(0, 0, 0), PropType.Large, new Repetition(
                2,
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

    public static readonly ScenaryReference Menu = new ScenaryReference(
        Props.PlaneScene,
        new Vector3(-400f, 2f, 0f),
        new Vector3(0f, 0f, 0f),
        new List<Tuple<Vector3, Vector3>>
        {
            new(new Vector3(-400f, 0f, -400f), new Vector3(400f, 0f, -400f)),
            new(new Vector3(-400f, 0f, 400f), new Vector3(400f, 0f, 400f)),
            new(new Vector3(-400f, 0f, -400f), new Vector3(-400f, 0f, 400f)),
            new(new Vector3(400f, 0f, -400f), new Vector3(400f, 0f, 400f)),
        },
        new List<PropReference>(),
        new PropReference(Props.SkyDome, new Vector3(0, -20f, 0), PropType.Dome),
        new List<PropReference>
        {
            new PropReference(Props.Farm3, new Vector3(25.5f, -1.5f, 25f), PropType.Large),
            new PropReference(Props.BuildingHouse11, new Vector3(90f, -1.5f, 65f), PropType.Large),
            new PropReference(Props.BuildingHouse12, new Vector3(60f, -1.5f, 60f), PropType.Large),
            new PropReference(Props.BuildingHouse16, new Vector3(60f, -1.5f, 40f), PropType.Large),
            new PropReference(Props.BuildingHouse5, new Vector3(50f, -1.5f, 20f), PropType.Large),
            new PropReference(Props.BuildingHouse3, new Vector3(50f, -1.5f, 10f), PropType.Large),
            new PropReference(Props.BuildingHouse13, new Vector3(75f, -1.5f, 7.5f), PropType.Large)
        }
    );
}