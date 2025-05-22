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

    // public event Action<DetectionData> OnDetectionUpdated;

    private Thread thread;
    private TcpListener server;
    private TcpClient client;
    private bool running;

    // private bool restartRequested = false;

    private bool hasNewData;
    private string writeBuffer;
    private DetectionData readBuffer;
    // private readonly object dataLock = new object();

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

        if (hasNewData)
        {
            try
            {
                readBuffer = DetectionData.FromJson(writeBuffer);
                hasNewData = false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse detection data: {e.Message}");
            }
            
        }
    }

    private void GetDetectionServer()
    {
        while (running)
        {
            try
            {
                // Try starting the server/
                server = new TcpListener(IPAddress.Any, connectionPort);
                server.Start();
                Debug.Log($"TCP server started on port {connectionPort}");
                GetDataStream();
            }
            catch (Exception e)
            {
                Debug.LogError($"Server error: {e.Message}");

                if (running)
                {
                    Debug.Log($"Retrying server start in {reconnectDelay} seconds...");
                    Thread.Sleep((int)(reconnectDelay * 1000));
                }
            }
            finally
            {
                server?.Stop();
            }
        }

    }
    private void GetDataStream()
    {
        using TcpClient client = server.AcceptTcpClient();
        using NetworkStream stream = client.GetStream();

        while (running && client.Connected)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);

            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (!string.IsNullOrEmpty(data))
            {
                writeBuffer = data;
                hasNewData = true;
                Debug.Log($"data: {data}"); // Received data


            }
        }
    }

    public DetectionData GetLatestDetectionData()
    {   
        return readBuffer;

    }

}