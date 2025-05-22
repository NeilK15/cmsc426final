using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UILayerType { Game, GameMenu, Menu, Popup }
[System.Serializable]
public struct UILayerRequest
{
    public GameObject prefab;
    public UILayerType layer;
}

public class MainHUD : MonoBehaviour
{
    [SerializeField, Tooltip("The main in-game HUD (health bars, score, etc.). Visible during normal gameplay.")]
    private GameObject gameLayer;

    [SerializeField, Tooltip("UI for game-specific menus like inventory, pause, or equipment. Overlays the game without leaving it.")]
    private GameObject gameMenuLayer;

    [SerializeField, Tooltip("Standalone full-screen menus like the main menu or settings. Usually shown outside active gameplay.")]
    private GameObject menuLayer;

    [SerializeField, Tooltip("Temporary UI popups like dialogue boxes, alerts, or confirmation dialogs. Always on top.")]
    private GameObject popupLayer;

    [SerializeField] private UILayerRequest[] initialScreens;

    private Dictionary<UILayerType, Stack<GameObject>> layerStacks = new();

    public IEnumerator Init()
    {
        GameAccess.RegisterMainHUD(this);

        // Initialize stacks
        foreach (UILayerType layer in System.Enum.GetValues(typeof(UILayerType)))
        {
            layerStacks[layer] = new Stack<GameObject>();
        }

        // Delay until GameInstance is ready (optional based on your setup)
        var instance = GameAccess.GetGameInstance();
        if (instance != null)
            instance.onInitialized.AddListener(SpawnInitialScreens);

        yield return null;
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

        if (stack.Count > 0)
            stack.Peek().SetActive(false);

        // ✅ Instantiate the prefab and parent it
        GameObject instance = Instantiate(request.prefab, targetLayer.transform);
        instance.SetActive(true);
        stack.Push(instance);
    }


    public void PopFromLayer(UILayerType layer)
    {
        if (!layerStacks.TryGetValue(layer, out var stack) || stack.Count == 0)
        {
            Debug.LogWarning($"PopFromLayer: No objects on layer {layer} to pop.");
            return;
        }

        GameObject obj = stack.Pop();
        Destroy(obj);

        if (stack.Count > 0)
            stack.Peek().SetActive(true);
    }
    public void RemoveObject(GameObject obj)
    {
        UILayerType? layer = FindLayerOfObject(obj);

        if (layer == null)
        {
            Debug.LogWarning("RemoveObject: Object not found in any UI layer stack. Destroying it.");
            Destroy(obj);
            return;
        }

        var stack = layerStacks[layer.Value];

        // If it's at the top, use standard pop logic
        if (IsTop(obj))
        {
            PopFromLayer(layer.Value);
            return;
        }

        // Otherwise, rebuild the stack without it
        var temp = new Stack<GameObject>();
        while (stack.Count > 0)
        {
            var top = stack.Pop();
            if (top == obj)
            {
                Destroy(obj);
                break;
            }
            temp.Push(top);
        }

        while (temp.Count > 0)
            stack.Push(temp.Pop());
    }

    public bool IsTop(GameObject obj)
    {
        foreach (var stack in layerStacks.Values)
        {
            if (stack.Count > 0 && stack.Peek() == obj)
                return true;
        }
        return false;
    }

    public UILayerType? FindLayerOfObject(GameObject obj)
    {
        foreach (var kvp in layerStacks)
        {
            if (kvp.Value.Contains(obj))
                return kvp.Key;
        }
        return null;
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
