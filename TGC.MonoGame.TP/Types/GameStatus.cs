namespace TGC.MonoGame.TP.Types;

public enum GameStatus
{
    MainMenu,
    DeathMenu,
    WinMenu,
    NormalGame,
    GodModeGame,
    Exit,
}

public class GameState
{
    public GameStatus CurrentStatus { get; private set; } = GameStatus.MainMenu;
    public GameStatus PreviousStatus { get; set; } = GameStatus.MainMenu;
    public bool FirstUpdate { get; set; } = false;
    
    public void Set(GameStatus status)
    {
        PreviousStatus = CurrentStatus;
        CurrentStatus = status;
        FirstUpdate = true;
    }
}