namespace TGC.MonoGame.TP.Types;

public enum GameStatus
{
    MainMenu,
    DeathMenu,
    NormalGame,
    GodModeGame,
    Exit,
}

public class GameState
{
    public GameStatus CurrentStatus { get; private set; } = GameStatus.NormalGame;
    public GameStatus PreviousStatus { get; set; } = GameStatus.MainMenu;
    public bool FirstUpdate { get; set; } = true;
    
    public void Set(GameStatus status)
    {
        PreviousStatus = CurrentStatus;
        CurrentStatus = status;
        FirstUpdate = true;
    }
}