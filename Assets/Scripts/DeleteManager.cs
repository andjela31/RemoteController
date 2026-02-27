using UnityEngine;
using UnityEngine.UI;

public class DeleteManager : MonoBehaviour
{
    public static DeleteManager Instance;
    public bool deleteMode = false;
    public Button btn;

    public Image img;
    public Sprite normalSprite;
    public Sprite deleteModeSprite;

    private void Awake() => Instance = this;

    public void EnterDeleteMode()
    {
        deleteMode = true;

        img.sprite = deleteModeSprite;
        
        ThemeManager.Instance.ApplyActiveThemeToNavButton(img, btn);
    }

    public void ExitDeleteMode()
    {
        deleteMode = false;

        img.sprite = normalSprite;
        Color c = Color.white;
        c.a = 0f; 
        btn.image.color = c;
        ThemeManager.Instance.ApplyActiveThemeToNavButton(img);
    }

    public void ToggleDeleteMode()
    {
        if (deleteMode)
        {
            ExitDeleteMode();
        }
        else
        {
            UIPanelManager.Instance.CloseAll();
            EnterDeleteMode();
        }
    }

    public void EnableButton(RemoteButtonData data)
    {
        MenuButtonsUI.Instance.EnableButton(data);
    }

}
