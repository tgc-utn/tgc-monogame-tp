namespace TGC.MonoGame.TP.Types.Tanks;

public class Turret //: ICollidable
{/*
    private Vector3 Position;
    public Matrix World;
    
    private Matrix _rotacion;

    private Cannon Cannon;

    private Boolean CanShoot;

    public Turret(Vector3 position, Cannon cannon)
    {
        Position = position;
        World = world;
        Cannon = cannon;
        
        _rotacion = Matrix.Identity;
    }

    public void Update(GameTime gameTime)
    {
        ProcessMouse();
        
        Position += Vector3.Transform(Vector3.Forward, _rotacion) *  gameTime.ElapsedGameTime.Milliseconds;
        Move(Position,_rotacion);
        
    }

    public void ProcessMouse()
    {
        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton.Equals(ButtonState.Pressed) && CanShoot && _game.Camera.CanShoot)
        {
            CanShoot = false;
            //soundShot.Play();
           
            var normal = (_game.Camera.LookAt - _game.Camera.Position);
            normal.Normalize();
            var distancia = (float) 0;
            if (_game.Camera.LookAt.Y >= _game.Camera.Position.Y)
            {
                distancia = 2000;
            }
            else
            {
                distancia = (_game.Camera.LookAt.Y*2000)/_game.Camera.Position.Y;
            }

            var endPosition = distancia * normal + _game.Camera.Position;
            cannonBalls.Add(new CannonBall(_game.Camera.Position + new Vector3(2*normal.X, -10, 2*normal.Z), endPosition,_game,cannonBall, this,null));
        }

        if (!mouseState.LeftButton.Equals(ButtonState.Pressed))
        {
            CanShoot = true;
        }
    }

    public void CollidedWithSmallProp()
    {
        throw new System.NotImplementedException();
    }

    public void CollidedWithLargeProp()
    {
        throw new System.NotImplementedException();
    }*/
}