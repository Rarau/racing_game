using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MenuOption : MonoBehaviour
{
    public UnityEvent OnSelect;
    public UnityEvent OnHighlight;
    public UnityEvent OnUnhighlight;
    private Menu menu;

    public void Update()
    {
    }
    //public delegate void OnHighlight();
    //public delegate void OnUnhighlight();
    //these are commented out anticipating possible use
}