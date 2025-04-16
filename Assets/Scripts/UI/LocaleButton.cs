using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocaleButton : MonoBehaviour
{
    private LocalePopupUI ui;

    public void AssignLocalePopupUI(LocalePopupUI assignedUI)
    {
        ui = assignedUI;
    }

    public void Explore()
    {
        ui.Explore();
    }

    public void Camp()
    {
        ui.Camp();
    }

    public void Hunt()
    {
        ui.Hunt();
    }

    public void Sleep()
    {
        ui.Sleep();
    }

    public void Talk()
    {
        ui.Talk();
    }

    public void Buy()
    {
        ui.Buy();
    }

    public void Sell()
    {
        ui.Sell();
    }

    public void Leave()
    {
        ui.Leave();
    }
}
