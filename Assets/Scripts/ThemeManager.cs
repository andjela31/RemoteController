using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public Image navbar;
    public Image buttonsPanel;
    public Image messagePanel;
    public Image optionsPanel;
    public Image connectionPanel;
    public Image palettePanel;
    public Image editPanel;
    public GameObject navBar;

    public string fileName = "themes.json";

    private ThemeCollection data;
    private Dictionary<string, ThemeData> themesById;

    private string FilePath =>
        Path.Combine(Application.persistentDataPath, fileName);

    private void Awake()
    {
        Instance = this;
        LoadThemes();
        ApplyActiveTheme();
        Debug.Log("Theme file path: " + FilePath);

    }

    void LoadThemes()
    {
        // Kopiranje iz StreamingAssets u persistent (prvi launch)
        if (!File.Exists(FilePath))
        {
            string defaultPath = Path.Combine(
                Application.streamingAssetsPath,
                fileName
            );
            File.Copy(defaultPath, FilePath);
        }

        string json = File.ReadAllText(FilePath);
        data = JsonUtility.FromJson<ThemeCollection>(json);

        themesById = new Dictionary<string, ThemeData>();
        foreach (var t in data.themes)
            themesById[t.id] = t;
    }

    public void ApplyActiveTheme()
    {
        if (!themesById.ContainsKey(data.activeThemeId))
            return;

        ApplyTheme(themesById[data.activeThemeId]);
    }

    void ApplyTheme(ThemeData t)
    {
        navbar.color = t.BackgroundColor;
        buttonsPanel.color = t.BackgroundColor;
        messagePanel.color = t.MessageColor;
        optionsPanel.color = t.MessageColor;
        connectionPanel.color = t.MessageColor;
        palettePanel.color = t.MessageColor;
        editPanel.color = t.MessageColor;
        RemoteUIManager.Instance.ApplyButtonsTheme();
        RemoteUIManager.Instance.ApplyPlaceholdersTheme();
        ApplyActiveThemeToNavButtons();
    }

    public void ApplyActiveThemeToButton(Button button)
    {
        if (button == null)
            return;

        ThemeData t = themesById[data.activeThemeId];

        Image img = button.GetComponent<Image>();
        if (img != null)
        {
            img.color = t.ButtonBackgroundColor;
        }

        TMP_Text txt = button.GetComponentInChildren<TMP_Text>();
        if (txt != null)
        {
            txt.color = t.ButtonTextColor;
        }

        ColorBlock cb = button.colors;
        cb.normalColor = t.ButtonBackgroundColor;
        cb.highlightedColor = t.ButtonBackgroundColor;
        cb.pressedColor = t.ButtonPressedColor;
        cb.selectedColor = t.ButtonBackgroundColor;
        cb.disabledColor = t.ButtonBackgroundColor * 0.5f;
        cb.colorMultiplier = 1f;

        button.colors = cb;
    }

    public void ApplyActiveThemeToPlaceholder(GameObject ph)
    {
        if (ph == null)
            return;

        ThemeData t = themesById[data.activeThemeId];
        Image i = ph.GetComponent<Image>();
        i.color = t.PlaceholderColor;
    }

    public void ApplyActiveThemeToMessagePanel(Transform msg)
    {
        if (msg == null)
            return;

        ThemeData t = themesById[data.activeThemeId];
        Image i = msg.GetComponent<Image>();
        i.color = t.MessageColor;
    }

    public void ApplyActiveThemeToNavButton(Image i)
    {
        ThemeData t = themesById[data.activeThemeId];
        i.color = t.NavButtons;
    }

    public void ApplyActiveThemeToNavButtons()
    {
        NavButtonUI[] buttons = navBar.GetComponentsInChildren<NavButtonUI>(true);
        ThemeData t = themesById[data.activeThemeId];

        foreach (NavButtonUI btn in buttons)
        {
            btn.ApplyTheme(t);
        }
    }

    public void SelectTheme(string themeId)
    {
        if (!themesById.ContainsKey(themeId))
            return;

        data.activeThemeId = themeId;
        ApplyTheme(themesById[themeId]);
        Save();
    }

    void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
    }
}
