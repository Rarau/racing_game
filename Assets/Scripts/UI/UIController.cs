using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour{

    public Font GenericFont;

    private GameObject CreateCanvas(string canvasName)
    {
        GameObject canvas = new GameObject(canvasName);
        canvas.AddComponent<RectTransform>();
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private GameObject CreateText(string textboxName)
    {
        GameObject textbox = new GameObject(textboxName);
        textbox.AddComponent<RectTransform>();
        textbox.AddComponent<CanvasRenderer>();
        textbox.AddComponent<Text>();
        return textbox;
    }

    //Sets up an empty menu ready to be setup as something in particular
    private GameObject CreateMenu(string menuName)
    {
        GameObject menu = new GameObject(menuName);
        menu.AddComponent<Menu>();
        GameObject view = new GameObject("View");
        view.transform.SetParent(menu.transform, false);
        GameObject controller = new GameObject("Controller");
        controller.transform.SetParent(menu.transform, false);
        GameObject options = new GameObject("Options");
        options.transform.SetParent(menu.transform, false);
        return menu;
    }

    public void CreateMainMenu(Camera drawCamera)
    {
        GameObject menu = CreateMenu("Main Menu");
        GameObject view = menu.transform.Find("View").gameObject;
        GameObject controller = menu.transform.Find("Controller").gameObject;
        GameObject options = menu.transform.Find("Options").gameObject;
        Menu menuScript = menu.GetComponent<Menu>();
        MenuViewBasic viewScript = view.AddComponent<MenuViewBasic>();
        MenuControllerBasic controllerScript = controller.AddComponent<MenuControllerBasic>();
        viewScript.textFont = GenericFont;
        viewScript.drawCamera = drawCamera;

        //setting up options
        //option 1
        GameObject curOption = new GameObject("option 1");
        curOption.transform.SetParent(curOption.transform);
        MenuOption curOptionScript = curOption.AddComponent<MenuOption>();
        curOptionScript.OnSelect = new UnityEvent();
        curOptionScript.OnSelect.AddListener(PrintMe);
        menuScript.AddOption(curOption);

        GameObject curViewOption = new GameObject("view option 1");
        MenuViewOptionBasic viewOptionScript = curViewOption.AddComponent<MenuViewOptionBasic>();
        viewOptionScript.optionText = "option 1";
        viewScript.AddOption(curViewOption);

        //option 2

        menuScript.AddOption("option 2");

        curViewOption = new GameObject("view option 2");
        viewOptionScript = curViewOption.AddComponent<MenuViewOptionBasic>();
        viewOptionScript.optionText = "option 2";
        viewScript.AddOption(curViewOption);
    }

    public void PrintMe()
    {
        print("option selected");
    }
    
}
