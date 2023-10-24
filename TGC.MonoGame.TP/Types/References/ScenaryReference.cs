using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.References;

public class ScenaryReference
{
    public ModelReference Scenary { get; }
    public Vector3 EnemiesSpawn { get; }
    public Vector3 AliesSpawn { get; }
    public PropReference SkyDome { get; }
    public List<PropReference> PropsReference { get; }

    public ScenaryReference(ModelReference scenary, Vector3 enemiesSpawn, Vector3 aliesSpawn, PropReference skyDome, List<PropReference> propsReference)
    {
        Scenary = scenary;
        EnemiesSpawn = enemiesSpawn;
        AliesSpawn = aliesSpawn;
        PropsReference = propsReference;
        SkyDome = skyDome;
    }
}