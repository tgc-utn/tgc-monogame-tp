using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.References;

public class ScenaryReference
{
    public ModelReference Scenary { get; }
    public Vector3 EnemiesSpawn { get; }
    public Vector3 AliesSpawn { get; }
    public PropReference SkyDome { get; }
    public List<PropReference> PropsReference { get; }
    public List<Tuple<Vector3, Vector3>> Limits { get; }
    public List<PropReference> LimitPropsReference { get; }
    

    public ScenaryReference(ModelReference scenary, Vector3 enemiesSpawn, Vector3 aliesSpawn, List<Tuple<Vector3, Vector3>> limits, List<PropReference> limitPropsReference, PropReference skyDome, List<PropReference> propsReference)
    {
        Scenary = scenary;
        EnemiesSpawn = enemiesSpawn;
        AliesSpawn = aliesSpawn;
        Limits = limits;
        LimitPropsReference = limitPropsReference;
        PropsReference = propsReference;
        SkyDome = skyDome;
    }
}