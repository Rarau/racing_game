using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this is the logic model of the menu; the M in MVC
public class Menu : MonoBehaviour
{
    private List<MenuOption> options;
    private int curOption = 0;//index
    private List<MenuView> views;
    private MenuController controller = null;

    void Start()
    {
        options = new List<MenuOption>();
        GetComponentsInChildren<MenuOption>(options);//auto-adds any option children to options
        views = new List<MenuView>();
        GetComponentsInChildren<MenuView>(views);//auto-adds any view children to views
        controller = GetComponentInChildren<MenuController>();//auto-sets controller
    }

    public void GoToOption(int newOption)
    {
        if (options.Count > 0)
        {
            while (newOption < 0) newOption += options.Count;
            newOption = newOption % options.Count;
            if(options[curOption].OnUnhighlight != null) options[curOption].OnUnhighlight.Invoke();
            curOption = newOption;
            if (options[curOption].OnHighlight != null) options[curOption].OnHighlight.Invoke();
            if (views.Count > 0)
            {
                for(int i = 0; i < views.Count; ++i)
                {
                    views[i].GoToOption(newOption);
                }

            }
        }
        else print("No options in menu");
    }

    public void NextOption()
    {
        GoToOption(curOption + 1);
    }

    public void PrevOption()
    {
        GoToOption(curOption - 1);
    }

    public int CurOption
    {
        get
        {
            return curOption;
        }
    }

    public void SelectCurOption()
    {
        if(options[curOption].OnSelect != null)
        {
            options[curOption].OnSelect.Invoke();
        }
    }

    public void printThis(string toPrint)
    {
        print(toPrint);
    }

    public MenuOption AddOption(string name)
    {
        GameObject optionObj = new GameObject(name);
        MenuOption returnOption = optionObj.AddComponent<MenuOption>();
        optionObj.transform.SetParent(transform.Find("Options"));
        return returnOption;
    }

    public void AddOption(GameObject newOption)
    {
        GameObject optionsGroup = transform.Find("Options").gameObject;
        newOption.transform.SetParent(optionsGroup.transform);
    }

    public void AddView(MenuView view)
    {
        views.Add(view);
    }
}
