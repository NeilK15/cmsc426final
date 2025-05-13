using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SmartCanvas : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                canvas.worldCamera = mainCam;
            }
            else
            {
                Debug.LogWarning("SmartCanvas: No MainCamera found in the scene.");
            }
        }
    }
}
