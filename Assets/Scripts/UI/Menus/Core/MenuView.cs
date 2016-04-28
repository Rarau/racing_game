using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//An abstract class telling the game how to display a type of menu; the V in MVC
public abstract class MenuView : MonoBehaviour
{
    protected Menu menu;
    protected GameObject viewObject;
    protected bool toDraw;

    void Start()
    {
        menu = GetComponentInParent<Menu>();
    }

    public abstract void GoToOption(int option);
    public abstract void AddOption(GameObject newOption);
}
