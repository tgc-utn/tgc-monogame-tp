using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.References;

public class ScenaryReference: ModelReference
{
    public Vector3 EnemiesSpawn { get; }
    public Vector3 AliesSpawn { get; }
    public List<PropReference> PropsReference { get; }

    public ScenaryReference(string model, float scale, Matrix normal, Color color, Vector3 enemySpawn,
        Vector3 alieSpawn, List<PropReference> propsReference) : base(model, scale, normal, color)
    {
        EnemiesSpawn = enemySpawn;
        AliesSpawn = alieSpawn;
        PropsReference = propsReference;
    }
}