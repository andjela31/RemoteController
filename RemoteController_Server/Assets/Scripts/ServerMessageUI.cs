using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerMessageUI : MonoBehaviour
{
    public TMP_InputField messageInput;
    public Button sendButton;
    public ServerManager serverManager;

    private void Start()
    {
        sendButton.onClick.AddListener(Send);
    }

    void Send()
    {
        if (string.IsNullOrWhiteSpace(messageInput.text))
            return;

        serverManager.BroadcastMsg(messageInput.text);
        messageInput.text = "";
    }
}
