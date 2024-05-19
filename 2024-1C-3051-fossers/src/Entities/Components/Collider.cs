
using System.Numerics;
using WarSteel.Common;


public abstract class Collider
{

    private ColliderType _colliderType;
    private Transform _transform;

    public Collider(ColliderType type, Transform transform)
    {
        _colliderType = type;
        _transform = transform;
    }

    public ColliderType ColliderType
    {
        get => _colliderType;
    }

    public Transform Transform
    {
        get => _transform;
    }

}

public class BoxCollider : Collider
{

    private Vector3 _halfWidths;

    public BoxCollider(Transform transform, Vector3 halfWidths) : base(ColliderType.BOX, transform)
    {
        _halfWidths = halfWidths;
    }

    public Vector3 HalfWidths
    {
        get => _halfWidths;
    }

}

public class SphereCollider : Collider
{
    private float _radius;
    public SphereCollider(Transform transform, float radius) : base(ColliderType.SPHERE, transform)
    {
        _radius = radius;
    }

    public float Radius
    {
        get => _radius;
    }
}

public enum ColliderType
{
    BOX,
    SPHERE
}




