using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    [SerializeField] private int width = 640;
    [SerializeField] private int height = 480;

    [SerializeField] private int fps = 30;
    [SerializeField] private DetectionServer detectionServer;
    private RectTransform rightHandMarker;
    private RectTransform leftHandMarker;

    private WebCamTexture webcamTexture;
    void Start()
    {

        InitializeCamera();
    }

    private void InitializeCamera()
    {

        WebCamTexture webcamTexture = new WebCamTexture(width, height, fps);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void Update()
    {
        DetectionData detectionData = detectionServer.GetLatestDetectionData();
        if (detectionData != null)
        {
            if (detectionData.right.detected)
            {
                Vector2 rightScreenPosition = new Vector2(
                    Screen.width * (1 - detectionData.right.x),
                    Screen.height * (1 - detectionData.right.y)
                );
                rightHandMarker.anchoredPosition = rightScreenPosition;
            }
            else
            {
                rightHandMarker.gameObject.SetActive(false);
            }

            if (detectionData.left.detected)
            {
                Vector2 leftScreenPosition = new Vector2(
                    Screen.width * (1 - detectionData.left.x),
                    Screen.height * (1 - detectionData.left.y)
                );
                leftHandMarker.anchoredPosition = leftScreenPosition;
            }
            else
            {
                leftHandMarker.gameObject.SetActive(false);
            }

        }    
        
    }
}