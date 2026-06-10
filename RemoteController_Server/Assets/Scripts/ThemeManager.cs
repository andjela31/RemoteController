using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public Image background;
    public Image messagesPanel;
    public Image infoPanel;
    public TMP_Text infoText;
    public Image paletteButton;
    public GameObject msgPanelGO;
    public GameObject infoPanelGO;

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

    public void ApplyActiveThemeToMessages(TMP_Text msg)
    {
        ThemeData t = themesById[data.activeThemeId];
        msg.color = t.TextColor;
    }

    void ApplyTheme(ThemeData t)
    {
        background.color = t.BackgroundColor;
        messagesPanel.color = t.MessageColor;
        infoPanel.color = t.MessageColor;
        infoText.color = t.TextColor;
        paletteButton.color = t.TextColor;

        TMP_Text[] text = msgPanelGO.GetComponentsInChildren<TMP_Text>(true);
        foreach(TMP_Text txt in text)
        {
            txt.color = t.TextColor;
        }
        text = infoPanelGO.GetComponentsInChildren<TMP_Text>(true);
        foreach (TMP_Text txt in text)
        {
            txt.color = t.TextColor;
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
