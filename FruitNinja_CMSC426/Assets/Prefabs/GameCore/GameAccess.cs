using UnityEngine;
public static class GameAccess
{
    private static GameInstance _gameInstance;
    private static GameState _gameState;
    private static MainHUD _mainHUD;
    private static GameMode _gameMode;
    private static Controller _controller;

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

    public static void RegisterController(Controller ctrl)
    {
        if (_controller != null) UnityEngine.Object.Destroy(ctrl.gameObject);
        else _controller = ctrl;
    }

    public static Controller GetController()
    {
        if (_controller == null)
            Debug.LogWarning("GameAccess: Controller accessed before registration.");
        return _controller;
    }

    public static GameInstance GetGameInstance()
    {
        if (_gameInstance == null)
            Debug.LogWarning("GameAccess: GameInstance accessed before registration.");
        return _gameInstance;
    }

    public static GameState GetGameState()
    {
        if (_gameState == null)
            Debug.LogWarning("GameAccess: GameState accessed before registration.");
        return _gameState;
    }

    public static MainHUD GetMainHUD()
    {
        if (_mainHUD == null)
            Debug.LogWarning("GameAccess: MainHUD accessed before registration.");
        return _mainHUD;
    }

    public static GameMode GetGameMode()
    {
        if (_gameMode == null)
            Debug.LogWarning("GameAccess: GameMode accessed before registration.");
        return _gameMode;
    }
}
