using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadButtonsDataManager : MonoBehaviour
{
    public List<RemoteButtonData> buttonsData;
    public string fileName = "buttons.json";

    public static LoadButtonsDataManager Instance;

    private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

    private void Awake()
    {
        Instance = this;

        // Proveri da li već postoji fajl u persistent folderu
        if (!File.Exists(FilePath))
        {
            string defaultPath = Path.Combine(
                Application.streamingAssetsPath,
                fileName
            );

            if (File.Exists(defaultPath))
            {
                File.Copy(defaultPath, FilePath);
                Debug.Log("Default buttons.json kopiran u persistent folder");
            }
            else
            {
                Debug.LogWarning("Default buttons.json nije pronađen u StreamingAssets!");
            }
        }


        LoadFromFile();  
    }

    public void LoadFromFile()
    {

        if (!File.Exists(FilePath))
        {
            Debug.LogError("buttons.json not found");
            buttonsData = new List<RemoteButtonData>();
            return;
        }

        string json = File.ReadAllText(FilePath);
        RemoteButtonDataList wrapper =
            JsonUtility.FromJson<RemoteButtonDataList>(json);

        buttonsData = wrapper.buttons;
    }


    public void SaveToFile()
    {
        //foreach (var d in buttonsData)
        //{
        //    Debug.Log($"{d.displayName} | {d.action} | added={d.isAdded} | size={d.size}");
        //}
        RemoteButtonDataList wrapper = new RemoteButtonDataList
        {
            buttons = buttonsData
        };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(FilePath, json);
    }
}