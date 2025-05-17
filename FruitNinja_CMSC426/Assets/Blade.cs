
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Blade : MonoBehaviour
{
    public float cutVelocityThreshold = 5f;
    public Vector3 Velocity { get; private set; }

    private Camera cam;
    private Vector3 lastPos;
    private TrailRenderer trail;

    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;

    public bool rightHanded = true;
    private DetectionData detectionData;

    private void Awake()
    {
        cam = Camera.main;

        Vector3 centerScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 worldCenter = GetScreenPosition(centerScreen);
        transform.position = lastPos = worldCenter;

        trail = GetComponentInChildren<TrailRenderer>();
        if (trail) trail.enabled = false;
    }

    private void Start()
    {
        ThreadStart ts = new ThreadStart(GetDetectionServer);
        thread = new Thread(ts);
        thread.Start();
        Debug.Log($"TCP server started on port {connectionPort}");
    }

    private void GetDetectionServer()
    {
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        client = server.AcceptTcpClient();
        running = true;

        while (running)
        {
            Connection();
        }

        server.Stop();
        Debug.Log("TCP server stopped");
    }

    private void Connection()
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
        
        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        
        if (data != null && data != "")
        {
            Debug.Log($"data: {data}");
            detectionData = DetectionData.FromJson(data);
        }
    }


    private void Update()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 world = GetScreenPosition(mouseScreen);

        Velocity = (world - lastPos) / Time.deltaTime;
        transform.position = world;
        lastPos = world;

        if (trail != null)
        {
            trail.enabled = Velocity.magnitude >= cutVelocityThreshold;
        }
    }

    /** TOD: CHANGE TO USE SCREEN CAPTURE **/
    private Vector3 GetScreenPosition(Vector3 screenPosition)
    {
        if (detectionData is not null)
        {
            if (rightHanded && detectionData.right.detected)
            {
                screenPosition.x = Screen.width * (1-detectionData.right.x);
                screenPosition.y = Screen.height * (1-detectionData.right.y);
            }
            else if (!rightHanded && detectionData.left.detected)
            {
                screenPosition.x = Screen.width * (1-detectionData.left.x);
                screenPosition.y = Screen.height * (1-detectionData.left.y);
            }
        }
        screenPosition.z = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0; // lock to 2D plane
        return worldPos;
    }
}
