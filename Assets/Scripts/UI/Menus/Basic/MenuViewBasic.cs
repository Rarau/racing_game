using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuViewBasic : MenuView {

    public Camera drawCamera;
    public Font textFont;
    private Canvas canvas;
    private GameObject textbox;
    private Text text;
    public List<MenuViewOptionBasic> options;

	// Use this for initialization
	void Start () {
        //general setup
        menu = GetComponentInParent<Menu>();
        toDraw = true;
        if(options == null) options = new List<MenuViewOptionBasic>();
        //Setup view
        viewObject = new GameObject("MenuViewBasic");
        viewObject.transform.SetParent(transform);
        canvas = viewObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = drawCamera;
        viewObject.AddComponent<CanvasScaler>();
        viewObject.AddComponent<GraphicRaycaster>();
        //Setup textbox
        textbox = new GameObject("TextBox");
        textbox.transform.SetParent(viewObject.transform);
        textbox.transform.position = viewObject.transform.position;
        RectTransform textScaler = textbox.AddComponent<RectTransform>();
        textbox.AddComponent<CanvasRenderer>();
        text = textbox.AddComponent<Text>();
        text.font = textFont;
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
