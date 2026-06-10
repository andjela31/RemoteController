using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMPGrowUp : MonoBehaviour
{
    public TMP_InputField input;
    public float minHeight = 87f;
    public float padding = 10f;

    RectTransform rt;

    void Awake()
    {
        rt = input.GetComponent<RectTransform>();
        // Pivot mora biti bottom center da raste na gore
        //rt.pivot = new Vector2(0.5f, 0f);
        input.onValueChanged.AddListener(OnTextChanged);
    }

    void OnTextChanged(string _)
    {
        float h = input.textComponent.preferredHeight + padding;
        rt.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            Mathf.Max(minHeight, h)
        );

        //LayoutRebuilder.ForceRebuildLayoutImmediate(rt);

        //input.textComponent.ForceMeshUpdate();
        //input.caretPosition = input.text.Length;
        //input.MoveTextEnd(false);
    }
}
