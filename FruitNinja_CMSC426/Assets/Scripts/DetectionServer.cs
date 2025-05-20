using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

public class DetectionServer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public int connectionPort = 25001;
    [SerializeField] private float reconnectDelay = 2f;

    public event Action<DetectionData> OnDetectionUpdated;

    private Thread thread;
    private TcpListener server;
    private TcpClient client;
    private bool running;

    private DetectionData detectionData;

    private string pendingData;
    private bool hasPendingData = false;

    private ConcurrentQueue<DetectionData> detectionQueue = new ConcurrentQueue<DetectionData>();
    private bool restartRequested = false;

    private void Start()
    {
        StartServer();
    }
    private void StartServer()
    {
        ThreadStart ts = new ThreadStart(GetDetectionServer);
        thread = new Thread(ts);
        thread.Start();
        running = true;
        Debug.Log($"TCP server started on port {connectionPort}");
    }
    private void OnDestroy()
    {
        StopServer();
    }

    private void StopServer()
    {
        running = false;
        server?.Stop();
        thread?.Abort();
        Debug.Log("TCP server stopped");
    }

    private void Update()
    {
        // if (hasPendingData)
        // {
        //     try
        //     {
        //         hasPendingData = false;
        //         detectionData = DetectionData.FromJson(pendingData);
        //         OnDetectionUpdated?.Invoke(detectionData);
        //     }
        //     catch (ArgumentException e)
        //     {
        //         // Parse error occured.
        //         Debug.LogError($"Parse error: {e.Message}\nData:{pendingData}");
        //     }
        // }


        while (detectionQueue.TryDequeue(out DetectionData data))
        {
            OnDetectionUpdated?.Invoke(data);
        }
        if (restartRequested)
        {
            restartRequested = false;
            StopServer();
            StartServer();
        }
    }

    private void GetDetectionServer()
    {
        try
        {
            // Try starting the server/
            server = new TcpListener(IPAddress.Any, connectionPort);
            server.Start();

            while (running)
            {
                
                GetDataStream();

            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Server error: {e.Message}");

            if (running)
            {
                restartRequested = true;
            }
        }

    }
    private void GetDataStream()
    {   
        using TcpClient client = server.AcceptTcpClient();
        using NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);

        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        if (data != null && data != "")
        {
            Debug.Log($"data: {data}"); // Received data

            // pendingData = data;
            // hasPendingData = true;
            detectionQueue.Enqueue(detectionData);
            
        } 
    }

}