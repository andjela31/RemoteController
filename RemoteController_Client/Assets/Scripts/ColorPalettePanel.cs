using UnityEngine;
using UnityEngine.UI;

public class ColorPalettePanel : MonoBehaviour
{
    public GameObject colorPalettePanel;
    public Button btn;
    public void Open()
    {
        colorPalettePanel.SetActive(true);
    }

    public void Close()
    {
        colorPalettePanel.SetActive(false);
    }

    public void ColorOriginal()
    {
        ThemeManager.Instance.SelectTheme("original");
        ThemeManager.Instance.ApplyActiveThemeToNavButton(btn.image, btn);
    }

    public void ColorGreen()
    {
        ThemeManager.Instance.SelectTheme("green");
        ThemeManager.Instance.ApplyActiveThemeToNavButton(btn.image, btn);
    }

    public void ColorBlue()
    {
        ThemeManager.Instance.SelectTheme("blue");
        ThemeManager.Instance.ApplyActiveThemeToNavButton(btn.image, btn);
    }

    public void ColorDark()
    {
        ThemeManager.Instance.SelectTheme("dark");
        ThemeManager.Instance.ApplyActiveThemeToNavButton(btn.image, btn);
    }
}
