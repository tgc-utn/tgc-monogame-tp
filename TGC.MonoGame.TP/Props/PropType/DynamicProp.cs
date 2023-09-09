using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Props.PropType;

public abstract class DynamicProp : Prop
{
    protected Vector3 Position { get; set; }
    protected Quaternion Rotation { get; set; }
    
    /*protected Quaternion Rotation() => Body().Pose.Orientation.ToQuaternion();
    protected Vector3 Position() => Body().Pose.Position;
    protected override Matrix World  =>  Matrix.CreateScale(Scale()) * 
                                         Matrix.CreateFromQuaternion(Rotation()) * 
                                         Matrix.CreateTranslation(Position());*/
    
    public abstract void Update(float dTime, KeyboardState keyboard);
}