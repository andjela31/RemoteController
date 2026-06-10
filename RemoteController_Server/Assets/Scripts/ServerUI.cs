using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerUI : MonoBehaviour
{
    public Image status;
    public TMP_Text statusText;
    public Color connectedColor = Color.green;
    public Color disconnectedColor = Color.red;

    public RectTransform rect;

    private void OnEnable()
    {
        ServerManager.Instance.OnClientCountChanged += UpdateStatus;
    }

    private void OnDisable()
    {
        ServerManager.Instance.OnClientCountChanged -= UpdateStatus;
    }

    private void Start()
    {
        // inicijalno stanje
        UpdateStatus(0);
    }

    void UpdateStatus(int clientCount)
    {
        status.color = clientCount > 0
            ? connectedColor
            : disconnectedColor;

        statusText.text = clientCount > 0
            ? $"Connected ({clientCount})"
            : "No clients";
    }
}
