using UnityEngine;

public class MainHUD : MonoBehaviour
{
    public enum UILayerType { Game, GameMenu, Menu, Popup }

    [System.Serializable]
    public struct UILayerRequest
    {
        public GameObject prefab;
        public UILayerType layer;
    }

    [SerializeField] private GameObject gameLayer;
    [SerializeField] private GameObject gameMenuLayer;
    [SerializeField] private GameObject menuLayer;
    [SerializeField] private GameObject popupLayer;
    [SerializeField] private UILayerRequest initialScreen;

    private void Awake()
    {
        GameAccess.RegisterMainHUD(this);
        var instance = GameAccess.GetGameInstance();
        if (instance != null)
            instance.onInitialized.AddListener(SpawnInitialScreen);
    }

    private void SpawnInitialScreen()
    {
        if (initialScreen.prefab != null)
        {
            GameObject screen = Instantiate(initialScreen.prefab);
            PushToLayer(new UILayerRequest { prefab = screen, layer = initialScreen.layer });
        }
    }

    public void PushToLayer(UILayerRequest request)
    {
        GameObject targetLayer = GetLayer(request.layer);
        if (targetLayer != null && request.prefab != null)
        {
            request.prefab.transform.SetParent(targetLayer.transform, false);
            request.prefab.SetActive(true);
        }
    }

    public void PopFromLayer(GameObject obj)
    {
        Destroy(obj);
    }

    private GameObject GetLayer(UILayerType layer)
    {
        return layer switch
        {
            UILayerType.Game => gameLayer,
            UILayerType.GameMenu => gameMenuLayer,
            UILayerType.Menu => menuLayer,
            UILayerType.Popup => popupLayer,
            _ => null
        };
    }
}
