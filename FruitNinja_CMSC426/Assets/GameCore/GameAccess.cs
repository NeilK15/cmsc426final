public static class GameAccess
{
    private static GameInstance _gameInstance;
    private static GameState _gameState;
    private static MainHUD _mainHUD;
    private static GameMode _gameMode;

    public static void RegisterGameInstance(GameInstance gi)
    {
        if (_gameInstance != null && _gameInstance != gi)
            UnityEngine.Object.Destroy(gi.gameObject);
        else
            _gameInstance = gi;
    }

    public static void RegisterGameState(GameState gs)
    {
        if (_gameState != null) UnityEngine.Object.Destroy(gs.gameObject);
        else _gameState = gs;
    }

    public static void RegisterMainHUD(MainHUD hud)
    {
        if (_mainHUD != null) UnityEngine.Object.Destroy(hud.gameObject);
        else _mainHUD = hud;
    }

    public static void RegisterGameMode(GameMode gm)
    {
        if (_gameMode != null) UnityEngine.Object.Destroy(gm.gameObject);
        else _gameMode = gm;
    }

    public static GameInstance GetGameInstance() => _gameInstance;
    public static GameState GetGameState() => _gameState;
    public static MainHUD GetMainHUD() => _mainHUD;
    public static GameMode GetGameMode() => _gameMode;
}
