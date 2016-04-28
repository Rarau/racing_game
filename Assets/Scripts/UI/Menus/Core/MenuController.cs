using UnityEngine;
using System.Collections;

//An abstract class telling the game how to process player input with a menu; the C in MVC
public abstract class MenuController : MonoBehaviour
{
    protected Menu menu;

    void Start()
    {
        menu = GetComponentInParent<Menu>();
    }

    void Update()
    {

    }
}
