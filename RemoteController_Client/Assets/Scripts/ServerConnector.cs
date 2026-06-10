using System.Collections.Concurrent;
using System.Net.Sockets; // Za TCP
using System.Text;
using System.Threading;
using UnityEngine;


public class ServerConnector : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    public static ServerConnector Instance;
    public ConcurrentQueue<string> incomingMessages = new ConcurrentQueue<string>();

    private string deviceId;
    private string deviceName;

    public bool IsDisconnected { get; private set; } = true;

    private Thread listenThread;

    public event System.Action<bool> OnConnectionResult;

    private void Awake()
    {
        Instance = this;
        deviceId = SystemInfo.deviceUniqueIdentifier;
        deviceName = SystemInfo.deviceName;
    }

    public void ConnectInBackground(string address, int port)
    {
        Thread thread = new Thread(() =>
        {
            bool success = Connect(address, port);

            OnConnectionResult?.Invoke(success);
        });

        thread.IsBackground = true;
        thread.Start();
    }

    public bool Connect(string address, int port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(address, port);

            stream = client.GetStream();
            SendHello();

            IsDisconnected = false;

            listenThread = new Thread(ListenForMessages);
            listenThread.Start();
            listenThread.IsBackground = true;

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    private void ListenForMessages()
    {
        byte[] buffer = new byte[1024];

        try
        {
            while (!IsDisconnected)
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                Debug.Log(bytes);
                if (bytes == 0)
                {
                    Debug.Log("Server closed connection (FIN)");
                    IsDisconnected = true;
                    break;
                }

                string msg = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();

                if (msg == "SRV:QUIT")
                {
                    Debug.Log("Server shutdown received");
                    IsDisconnected = true;

                    stream?.Close();
                    client?.Close();
                    return;
                }

                incomingMessages.Enqueue(msg);
            }
        }
        catch
        {
            Debug.Log("Disconnected from server");
            IsDisconnected = true;
            stream?.Close();
            client?.Close();
        }
    }

    private void SendHello()
    {
        if (client == null || !client.Connected || stream == null)
            return;
        string hello = $"HELLO:{deviceId}:{deviceName}\n";
        byte[] data = Encoding.UTF8.GetBytes(hello);
        stream.Write(data, 0, data.Length);
    }

    public void SendButtonCommand(string displayName)
    {
        if (client == null || !client.Connected || stream == null)
            return;
        string payload = "BTN:" + displayName + "\n";
        byte[] data = Encoding.UTF8.GetBytes(payload);
        stream.Write(data, 0, data.Length);
    }

    public void SendTextMessage(string text)
    {
        if (client == null || !client.Connected || stream == null)
            return;

        text = text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        string payload = "MSG:" + text + "\n";
        byte[] data = Encoding.UTF8.GetBytes(payload);
        stream.Write(data, 0, data.Length);
    }

    private void OnApplicationQuit()
    {
        IsDisconnected = true;
        stream?.Close();
        client?.Close();
    }
}
