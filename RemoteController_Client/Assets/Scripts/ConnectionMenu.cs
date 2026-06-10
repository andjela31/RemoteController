using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionMenu : MonoBehaviour
{
    public GameObject connectionPanel;
    public TMP_InputField addressInput;
    public TMP_InputField portInput;
    public Button connectButton;
    public Image statusCircle1;
    public Image statusCircle2;

    private bool? connectionResult = null;
    private readonly object lockObj = new object();

    private void Start()
    {
        connectButton.onClick.AddListener(OnConnectClicked);

        if (ServerConnector.Instance != null)
            ServerConnector.Instance.OnConnectionResult += OnConnectionResult;

        SetStatusRed();
    }

    void OnConnectionResult(bool success)
    {
        lock (lockObj)
        {
            connectionResult = success;
        }
    }

    void Update()
    {
        bool? result = null;

        lock (lockObj)
        {
            result = connectionResult;
            connectionResult = null;
        }

        if (result.HasValue)
        {
            connectButton.interactable = true;

            if (result.Value) SetStatusGreen();
            else SetStatusRed();
        }
    }


    private void OnConnectClicked()
    {
        string address = addressInput.text;
        if (!int.TryParse(portInput.text, out int port)) return;

        connectButton.interactable = false;

        ServerConnector.Instance.ConnectInBackground(address, port);
    }

    private void SetStatusGreen() 
    {
        statusCircle1.color = Color.green;
        statusCircle2.color = Color.green;
    }
    private void SetStatusRed()
    {
        statusCircle1.color = Color.red;
        statusCircle2.color = Color.red;
    }

    public void Open()
    {
        connectionPanel.SetActive(true);
    }

    public void Close()
    {
        connectionPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (ServerConnector.Instance != null)
            ServerConnector.Instance.OnConnectionResult -= OnConnectionResult;
    }
}
