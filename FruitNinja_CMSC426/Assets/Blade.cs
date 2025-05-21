
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Blade : MonoBehaviour
{
    public float cutVelocityThreshold = 5f;
    public Vector3 Velocity { get; private set; }

    private Camera cam;
    private Vector3 lastPos;
    private TrailRenderer trail;

    // private Thread thread;
    // public int connectionPort = 25001;
    // private TcpListener server;
    // private TcpClient client;
    // private bool running;

    public bool rightHanded = true;

    [SerializeField] private DetectionServer detectionServer;

    // private void OnEnable() => server.OnDetectionUpdated += UpdatePosition;
    // private void OnDisable() => server.OnDetectionUpdated -= UpdatePosition;


    private DetectionData detectionData;

    private Vector3 lastScreenPosition;

    private void Awake()
    {
        cam = Camera.main;

        if (detectionServer == null)
        {
            detectionServer = FindFirstObjectByType<DetectionServer>();
            if (detectionServer == null)
            {
                Debug.LogError("DetectionServer not found in the scene!");
            }
        }

        Vector3 centerScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        lastScreenPosition = centerScreen;
        Vector3 worldCenter = GetScreenPosition(centerScreen);
        transform.position = lastPos = worldCenter;

        trail = GetComponentInChildren<TrailRenderer>();
        if (trail) trail.enabled = true;
    }



    private void Update()
    {
        DetectionData detectionData = detectionServer?.GetLatestDetectionData();
      
        Vector3 screenPosition = UpdatePosition(detectionData);
        Vector3 world = GetScreenPosition(screenPosition);
        
        Velocity = (world - lastPos) / Time.deltaTime;
        transform.position = world;
        lastPos = world;
        lastScreenPosition = screenPosition;
        // if (trail != null)
        // {
        //     trail.enabled = Velocity.magnitude >= cutVelocityThreshold;
        // }
    }

    /** TOD: CHANGE TO USE SCREEN CAPTURE **/
    private Vector3 GetScreenPosition(Vector3 screenPosition)
    {
        screenPosition.z = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0; // lock to 2D plane
        return worldPos;
    }


    private Vector3 UpdatePosition(DetectionData detectionData)
    {
        Vector3 screenPosition = lastScreenPosition;
        if (detectionData is not null)
        {

            if (rightHanded && detectionData.right.detected)
            {
                screenPosition.x = Screen.width * (1 - detectionData.right.x);
                screenPosition.y = Screen.height * (1 - detectionData.right.y);
                
            }
            else if (!rightHanded && detectionData.left.detected)
            {
                screenPosition.x = Screen.width * (1 - detectionData.left.x);
                screenPosition.y = Screen.height * (1 - detectionData.left.y);

            }
            
        }
        screenPosition.z = 0.0f;
        return screenPosition;
    }
}
