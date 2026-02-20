using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class RemoteButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI label;
    private RemoteButtonData data;
    public UnityEngine.UI.Image background;

    private bool selected = false;

    

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color deleteColor = Color.gainsboro;
    public Color deleteHoverColor = Color.whiteSmoke;

    public Color selectedColor = Color.darkGray;


    private bool isHovered = false;

    float aspectRatio;
    RectTransform rt;
    [SerializeField] private float baseWidth = 500f; // početna širina dugmeta
    [SerializeField] private float baseFontSize = 90f; // početni font size

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        aspectRatio = rt.sizeDelta.x / rt.sizeDelta.y;
    }

    public void Setup(RemoteButtonData d)
    {
        data = d;
        label.text = d.displayName;
        ApplySize(d.size);
    }

    void Update()
    {
        RefreshVisual();
    }

    public void RefreshVisual()
    {
        if (DeleteManager.Instance != null && DeleteManager.Instance.deleteMode)
        {
            if (isHovered)
                background.color = deleteHoverColor;
            else
                background.color = deleteColor;
        }
        else if (EditManager.Instance != null && EditManager.Instance.editMode && selected)
        {
             background.color = selectedColor;
        }
        else
        {
            background.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnClick()
    {
        RemoteUIManager.Instance.HandleClick(this, data);
    }

    public void SetSelected(bool value)
    {
        if(EditManager.Instance.editMode)
        {
            selected = value;
        }
    }

    public void SetText(string newText)
    {
        data.displayName = newText;
        label.text = newText;
        LoadButtonsDataManager.Instance.SaveToFile();
    }

    public void SetSize(float width)
    {
        data.size = width;
        ApplySize(width);
        LoadButtonsDataManager.Instance.SaveToFile();
    }

    private void ApplySize(float width)
    {
        float height = width / aspectRatio;
        rt.sizeDelta = new Vector2(width, height);

        float scale = width / baseWidth;
        label.fontSize = baseFontSize * scale;
    }


}
