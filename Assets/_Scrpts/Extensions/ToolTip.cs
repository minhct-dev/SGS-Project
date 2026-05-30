using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ToolTip : Singleton<ToolTip>
{
    [SerializeField]
    private Camera uiCamera;
    private TextMeshProUGUI toolTipText;
    private TextMeshProUGUI description;
    [SerializeField] private GameObject background;
    protected override void Awake()
    {
        base.Awake();
        toolTipText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        HideToolTip();
    }
    public void Update()
    {
        UnityEngine.Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }
    public void ShowToolTip(string toolTipString, string toolTipDescription)
    {
        gameObject.SetActive(true);
        toolTipText.text = toolTipString;
        description.text = toolTipDescription;
        UnityEngine.Vector2 descriptionSize = new UnityEngine.Vector2(300f, description.preferredHeight);
        description.rectTransform.sizeDelta = descriptionSize;
    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

}
