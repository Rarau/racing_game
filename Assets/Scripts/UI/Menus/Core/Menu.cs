using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this is the logic model of the menu; the M in MVC
public class Menu : MonoBehaviour
{
    public List<MenuOption> options;
    private int curOption = 0;//index
    public MenuView view = null;
    public MenuController controller = null;

    public void GoToOption(int newOption)
    {
        if (options.Count > 0)
        {
            while (newOption < 0) newOption += options.Count;
            newOption = newOption % options.Count;
            if(options[curOption].OnUnhighlight != null) options[curOption].OnUnhighlight.Invoke();
            curOption = newOption;
            if (options[curOption].OnHighlight != null) options[curOption].OnHighlight.Invoke();
            if (view != null) { view.GoToOption(curOption); }
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

    public void AddOption(GameObject newOption)
    {
        if (options == null) options = new List<MenuOption>();
        GameObject optionsGroup = transform.Find("Options").gameObject;
        newOption.transform.SetParent(optionsGroup.transform);
        options.Add(newOption.GetComponent<MenuOption>());
    }
}
