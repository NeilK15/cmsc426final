using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] private UILayerRequest[] initialScreens;


    private Dictionary<UILayerType, Stack<GameObject>> layerStacks = new();

    private void Awake()
    {
        GameAccess.RegisterMainHUD(this);

        // Initialize stacks
        foreach (UILayerType layer in System.Enum.GetValues(typeof(UILayerType)))
        {
            layerStacks[layer] = new Stack<GameObject>();
        }

        var instance = GameAccess.GetGameInstance();
        if (instance != null)
            instance.onInitialized.AddListener(SpawnInitialScreens);
    }

    private void SpawnInitialScreens()
    {
        foreach (var screenData in initialScreens)
        {
            if (screenData.prefab != null)
            {
                GameObject screen = Instantiate(screenData.prefab);
                PushToLayer(new UILayerRequest { prefab = screen, layer = screenData.layer });
            }
        }
    }


    public void PushToLayer(UILayerRequest request)
    {
        GameObject targetLayer = GetLayer(request.layer);
        if (targetLayer == null || request.prefab == null) return;

        var stack = layerStacks[request.layer];

        // Hide current top if exists
        if (stack.Count > 0)
            stack.Peek().SetActive(false);

        // Instantiate and push new
        request.prefab.transform.SetParent(targetLayer.transform, false);
        request.prefab.SetActive(true);
        stack.Push(request.prefab);
    }

    public void PopFromLayer(GameObject obj)
    {
        foreach (var kvp in layerStacks)
        {
            var stack = kvp.Value;
            if (stack.Count == 0 || stack.Peek() != obj) continue;

            // Remove top
            stack.Pop();
            Destroy(obj);

            // Re-enable new top if exists
            if (stack.Count > 0)
                stack.Peek().SetActive(true);

            return;
        }

        Debug.LogWarning("Tried to pop an object not on top of any UI layer stack.");
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
