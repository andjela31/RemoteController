using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class ServerManager : MonoBehaviour
{
    public TMP_Text infoPrefab;
    public Transform content;
    public ScrollRect rectContent;

    public Transform contentMsg;
    public ScrollRect rectContentMsg;

    TcpListener server;
    //Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

    Queue<string> uiQueue = new Queue<string>();
    Queue<string> msgQueue = new Queue<string>();

    Dictionary<string, ClientInfo> clients = new Dictionary<string, ClientInfo>();


    public static ServerManager Instance;

    public event System.Action<int> OnClientCountChanged;
    Queue<Action> mainThreadActions = new Queue<Action>();

    private Thread serverThread;
    private bool serverRunning = false;

    private List<string> allMessages = new List<string>();

    private string logPath;
    private string lastPath;

    private readonly object logLock = new object();

    private bool logSaved = false;

    private void Awake()
    {
        Instance = this;

        // da li se svaka sesija cuva u poseban fajl 
        //string sessionTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        //logPath = Path.Combine(UnityEngine.Application.persistentDataPath, $"log_{sessionTime}.txt");
        logPath = Path.Combine(UnityEngine.Application.persistentDataPath, "log.txt");
        Debug.Log(logPath);
        lastPath = Path.Combine(UnityEngine.Application.persistentDataPath, "last.txt");
    }

    public void AddMessage(string message)
    {
        lock (logLock)
        {
            allMessages.Add(message);
            SaveLastMessage(message);
        }
    }

    public void EndCommunication()
    {
        SaveLogFile();
    }

    private void SaveLogFile()
    {
        try
        {
            using (var fs = new FileStream(
                logPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                using (var writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    foreach (var line in allMessages)
                        writer.WriteLine(line);
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogWarning("log file is locked by another process: " + e.Message);
        }
    }


    private void SaveLastMessage(string message)
    {
        try
        {
            using (var fs = new FileStream(
                lastPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None)) 
            {
                using (var writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    writer.Write(message);
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogWarning("last.txt is locked by another process: " + e.Message);
        }
    }


    void Start()
    {
        serverThread = new Thread(StartServer);
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void RunOnMainThread(Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }


    void StartServer()
    {
        server = new TcpListener(IPAddress.Any, 12345);
        server.Start();
        serverRunning = true;

        Debug.Log("Server started on port 12345");

        try
        {
            while (serverRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.IsBackground = true;
                clientThread.Start();
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("Server stopped: " + ex.Message);
        }

    }

    void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            // ČEKAMO HELLO poruku
            int bytes = stream.Read(buffer, 0, buffer.Length);
            string hello = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();

            if (!hello.StartsWith("HELLO:"))
            {
                client.Close();
                return;
            }

            lock (clients)
            {
                if (clients.Count == 0)
                    logSaved = false;
            }
            //string deviceId = hello.Replace("HELLO:", "");
            var parts = hello.Split(':');

            string deviceId = parts[1];
            string deviceName = parts.Length > 2 ? parts[2] : "Unknown";


            lock (clients)
            {
                // ako već postoji isti device
                if (clients.ContainsKey(deviceId))
                {
                    EnqueueUI($"Reconnect: {deviceName}");
                    clients[deviceId].client.Close();
                    clients.Remove(deviceId);
                }

                clients.Add(deviceId, new ClientInfo { client = client, deviceName = deviceName, deviceId = deviceId });
            }

            EnqueueUI($"Connected: {deviceName}");
            // obavesti ServerUI
            int count = clients.Count;

            RunOnMainThread(() =>
            {
                OnClientCountChanged?.Invoke(count);
            });


            // SLUŠAJ DALJE PORUKE
            while (client.Connected)
            {
                bytes = stream.Read(buffer, 0, buffer.Length);
                if (bytes == 0) break;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                Debug.Log(msg);

                if (msg.StartsWith("MSG:"))
                {
                    string text = msg.Replace("MSG:", "");
                    Debug.Log(text);
                    string logLine = $"[{DateTime.Now:HH:mm:ss}] [{deviceName}] {text}";
                    AddMessage(logLine);
                    EnqueueMsg($"[{deviceName}]> {text}");
                }
                else if (msg.StartsWith("BTN:"))
                {
                    string text = msg.Replace("BTN:", "");
                    Debug.Log(text);
                    string logLine = $"[{DateTime.Now:HH:mm:ss}] [{deviceName}] {text}";
                    AddMessage(logLine);
                    EnqueueMsg($"[{deviceName}]> {text}");
                }

            }
        }
        catch (Exception e)
        {
            // ignored
            Debug.Log(e.Message);
        }
        finally
        {
            RemoveClient(client);
        }
    }

    void RemoveClient(TcpClient client)
    {
        string keyToRemove = null;
        string deviceName = "Unknown";


        lock (clients)
        {
            foreach (var kvp in clients)
            {
                if (kvp.Value.client == client)
                {
                    keyToRemove = kvp.Key;
                    deviceName = kvp.Value.deviceName;
                    break;
                }
            }

            if (keyToRemove != null)
            {
                clients.Remove(keyToRemove);
                EnqueueUI($"Disconnected: {deviceName}");

                // obavesti ServerUI
                int count = clients.Count;

                RunOnMainThread(() =>
                {
                    OnClientCountChanged?.Invoke(count);
                });
            }
        }

        client.Close();
        lock (logLock)
        {
            if (!logSaved && clients.Count == 0)
            {
                logSaved = true;
                EndCommunication();
            }
        }
    }

    void EnqueueUI(string message)
    {
        lock (uiQueue)
        {
            uiQueue.Enqueue(message);
        }
    }

    void EnqueueMsg(string message)
    {
        lock (msgQueue)
        {
            msgQueue.Enqueue(message);
        }
    }

    void Update()
    {
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue().Invoke();
            }
        }

        lock (uiQueue)
        {
            while (uiQueue.Count > 0)
            {
                string msg = uiQueue.Dequeue();

                TMP_Text item = Instantiate(infoPrefab, content);
                item.text = msg;
                ThemeManager.Instance.ApplyActiveThemeToMessages(item);
                LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
                Canvas.ForceUpdateCanvases();
                StartCoroutine(ScrollToBottom());
            }
        }

        lock (msgQueue)
        {
            while (msgQueue.Count > 0)
            {
                string msg = msgQueue.Dequeue();

                TMP_Text item = Instantiate(infoPrefab, contentMsg);
                item.text = msg;
                ThemeManager.Instance.ApplyActiveThemeToMessages(item);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentMsg.GetComponent<RectTransform>());
                Canvas.ForceUpdateCanvases();
                StartCoroutine(ScrollToBottom());
            }
        }
    }

    IEnumerator ScrollToBottom()
    {
        yield return null; // sačekaj 1 frame da se layout update-uje
        rectContent.verticalNormalizedPosition = 0f;
        rectContentMsg.verticalNormalizedPosition = 0f;
    }


    public void BroadcastMsg(string text)
    {
        byte[] data = Encoding.UTF8.GetBytes("SRV:" + text + "\n");

        lock (clients)
        {
            foreach (var kvp in clients)
            {
                TcpClient client = kvp.Value.client;

                if (client != null && client.Connected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                    catch
                    {
                        Debug.Log("Error");
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }

    private void OnDestroy()
    {
        StopServer();
    }

    private void StopServer()
    {
        serverRunning = false;

        if (server != null)
        {
            server.Stop(); // prekida AcceptTcpClient
            server = null;
        }

        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Join(); // sačekaj da se thread završi
            serverThread = null;
        }

        lock (clients)
        {
            foreach (var kvp in clients)
            {
                try
                {
                    NetworkStream stream = kvp.Value.client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes("SRV:QUIT\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    kvp.Value.client.Close();
                }
                catch { }
            }

            clients.Clear();
        }

        Debug.Log("Server stopped safely.");
    }




}
