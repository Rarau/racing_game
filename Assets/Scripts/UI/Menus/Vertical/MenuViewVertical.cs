using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuViewVertical : MenuView
{

    public Camera drawCamera;
    public Font textFont;
    private Canvas canvas;
    private List<GameObject> textboxes;
    private GameObject pointer;
    private Text text;
    public List<MenuViewOptionBasic> options;
    public int curOption = 0;
    public Color unhighlightedColor = new Color(1, 1, 1);
    public Color highlightedColor = new Color(1, 0, 0);
    public TextAnchor viewAnchor = TextAnchor.UpperCenter;
    public bool fillScreen = true;
    public float viewHeight = 0;
    private GridLayoutGroup layout;

    // Use this for initialization
    void Start()
    {
        //general setup
        menu = GetComponentInParent<Menu>();
        toDraw = true;
        if (options == null) options = new List<MenuViewOptionBasic>();
        textboxes = new List<GameObject>();
        //Setup view
        viewObject = new GameObject("MenuViewHorizontal");
        viewObject.transform.SetParent(transform);
        canvas = viewObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = drawCamera;
        viewObject.AddComponent<CanvasScaler>();
        viewObject.AddComponent<GraphicRaycaster>();
        layout = viewObject.AddComponent<GridLayoutGroup>();
        layout.cellSize = new Vector2(100, 50);
        layout.childAlignment = viewAnchor;
        layout.startAxis = GridLayoutGroup.Axis.Vertical;
        if (fillScreen) { viewHeight = canvas.GetComponent<RectTransform>().sizeDelta.y; }
        //Setup textboxes

        if (options.Count > 0)
        {
            GameObject curTextbox;
            RectTransform curTransform;
            Text curText;
            for (int i = 0; i < options.Count; i++)
            {
                curTextbox = new GameObject();
                curTransform = curTextbox.AddComponent<RectTransform>();
                curTextbox.AddComponent<CanvasRenderer>();
                curText = curTextbox.AddComponent<Text>();
                curText.text = options[i].optionText;
                curText.font = textFont;
                curText.alignment = TextAnchor.MiddleCenter;
                if (i == menu.CurOption) { curOption = i; curText.color = highlightedColor; }
                else { curText.color = unhighlightedColor; }
                curTextbox.transform.SetParent(canvas.transform);
                textboxes.Add(curTextbox);
                layout.cellSize = new Vector2(100, viewHeight / options.Count);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void GoToOption(int option)
    {
        //pointer moves
        textboxes[option].GetComponent<Text>().color = highlightedColor;
        textboxes[curOption].GetComponent<Text>().color = unhighlightedColor;
        curOption = option;

    }

    public override void AddOption(GameObject newOption)
    {
        if (options == null) options = new List<MenuViewOptionBasic>();
        newOption.transform.SetParent(transform);
        options.Add(newOption.GetComponent<MenuViewOptionBasic>());
        layout.cellSize = new Vector2(100, viewHeight / options.Count);
    }
}
