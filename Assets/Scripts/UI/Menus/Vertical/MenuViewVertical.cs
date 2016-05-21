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
    private List<MenuViewOptionBasic> options;
    public int curOption = 0;
    public Color unhighlightedColor = new Color(1, 1, 1);
    public Color highlightedColor = new Color(1, 0, 0);
    public TextAnchor viewAnchor = TextAnchor.MiddleCenter;
    public bool fillScreen = true;
    public Sprite backgroundImage;
    public Color backgroundColor = new Color(1, 1, 1, 1);

    public Vector2 size;
    public Vector2 offset;

    public GameObject verticalViewPrefab;
    public GameObject menuTextPrefab;
    private GridLayoutGroup layoutText;
    private GridLayoutGroup layoutBack;

    // Use this for initialization
    void Start()
    {
        //general setup
        menu = GetComponentInParent<Menu>();
        toDraw = true;
        if (options == null) options = new List<MenuViewOptionBasic>();
        GetComponentsInChildren<MenuViewOptionBasic>(options);
        textboxes = new List<GameObject>();
        //Setup view
        viewObject = Instantiate(verticalViewPrefab);
        viewObject.transform.SetParent(transform);
        canvas = viewObject.GetComponent<Canvas>();
        canvas.worldCamera = drawCamera;
        canvas.planeDistance = 1;
        if (fillScreen) { size.y = drawCamera.pixelHeight; }
        layoutText = viewObject.transform.Find("TextGroup").GetComponentInChildren<GridLayoutGroup>();
        layoutText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset.x, offset.y);
        layoutText.GetComponent<RectTransform>().sizeDelta = new Vector2(drawCamera.pixelWidth, drawCamera.pixelHeight);
        layoutText.childAlignment = viewAnchor;
        layoutText.cellSize = new Vector2(size.x, size.y / options.Count);

        //Setup textboxes
        if (options.Count > 0)
        {
            GameObject curTextbox;
            Text curText;
            for (int i = 0; i < options.Count; i++)
            {
                curTextbox = Instantiate<GameObject>(menuTextPrefab);
                curTextbox.transform.SetParent(layoutText.transform, false);
                curText = curTextbox.GetComponent<Text>();
                curText.text = options[i].optionText;
                curText.font = textFont;
                if (i == menu.CurOption) { curOption = i; curText.color = highlightedColor; }
                else { curText.color = unhighlightedColor; }
                textboxes.Add(curTextbox);
            }
        }

        //Setup background

        Image background = viewObject.GetComponentInChildren<Image>();
        GameObject backObj = background.gameObject;
        background.sprite = backgroundImage;
        background.color = backgroundColor;
        backObj.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, size.y);
        backObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset.x, offset.y);
        layoutBack = backObj.transform.parent.GetComponent<GridLayoutGroup>();
        layoutBack.cellSize = new Vector2(size.x, size.y);
        layoutBack.GetComponent<RectTransform>().sizeDelta = new Vector2(drawCamera.pixelWidth, drawCamera.pixelHeight);
        layoutBack.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset.x, offset.y);
        layoutBack.childAlignment = viewAnchor;
        //background.set
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
        //layout.cellSize = new Vector2(viewWidth / options.Count, 50);
    }
}