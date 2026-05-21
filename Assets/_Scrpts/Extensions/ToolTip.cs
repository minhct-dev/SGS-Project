using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ToolTip : Singleton<ToolTip>
{
    [SerializeField]
    private Camera uiCamera;
    private Text toolTipText;
    private Text description;
    private RectTransform backgroundRectTransform;
    [SerializeField] private GameObject toolText;
    [SerializeField] private GameObject background;
    protected override void Awake()
    {
        base.Awake();
        backgroundRectTransform = transform.Find("BackGround").GetComponent<RectTransform>();
        toolTipText = transform.Find("Name").GetComponent<Text>();
        description = transform.Find("Description").GetComponent<Text>();
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
        float textPaddingSize = 4f;
        UnityEngine.Vector2 backgroundSize = new UnityEngine.Vector2(300f + textPaddingSize * 2f, 150f + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        UnityEngine.Vector2 descriptionSize = new UnityEngine.Vector2(300f + textPaddingSize * 2f, description.preferredHeight - textPaddingSize * 2f);
        description.rectTransform.sizeDelta = descriptionSize;
    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

}
