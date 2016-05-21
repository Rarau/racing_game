using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuViewBasic : MenuView {

    public Camera drawCamera;
    public Font textFont;
    public Vector2 position;
    public Color color; 
    private Canvas canvas;
    private GameObject textbox;
    private Text text;
    private RectTransform textPosition;
    private List<MenuViewOptionBasic> options;

	// Use this for initialization
	void Start () {
        //general setup
        menu = GetComponentInParent<Menu>();
        options = new List<MenuViewOptionBasic>();
        GetComponentsInChildren<MenuViewOptionBasic>(options);
        toDraw = true;
        if(options == null) options = new List<MenuViewOptionBasic>();
        //Setup view
        viewObject = new GameObject("MenuViewBasic");
        viewObject.transform.SetParent(transform);
        canvas = viewObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = drawCamera;
        canvas.planeDistance = 1f;
        viewObject.AddComponent<CanvasScaler>();
        viewObject.AddComponent<GraphicRaycaster>();
        //Setup textbox
        textbox = new GameObject("TextBox");
        textPosition = textbox.AddComponent<RectTransform>();
        textPosition.SetParent(viewObject.transform);
        textPosition.localPosition = new Vector3(position.x,position.y, 0);
        textbox.AddComponent<CanvasRenderer>();
        
        text = textbox.AddComponent<Text>();
        text.font = textFont;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(color.r, color.g, color.b);
        if(options.Count > 0) text.text = options[menu.CurOption].optionText;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void GoToOption(int option)
    {
        text.text = options[option].optionText;
    }

    public override void AddOption(GameObject newOption)
    {
        if (options == null) options = new List<MenuViewOptionBasic>();
        newOption.transform.SetParent(transform, false);
        options.Add(newOption.GetComponent<MenuViewOptionBasic>());
    }
}
