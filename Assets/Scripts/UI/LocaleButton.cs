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

    void RequeueLocaleWindow()
    {
        UIManager.Instance().OpenLocalePopup(ui.currentLocale);
        UIManager.Instance().CloseLocalePopup();
    }

    public void Explore()
    {
        ui.Explore();
        RequeueLocaleWindow();
    }

    public void Camp()
    {
        ui.Camp();
        RequeueLocaleWindow();
    }

    public void Hunt()
    {
        ui.Hunt();
        RequeueLocaleWindow();
    }

    public void Sleep()
    {
        ui.Sleep();
        RequeueLocaleWindow();
    }

    public void Talk()
    {
        ui.Talk();
        RequeueLocaleWindow();
    }

    public void BuyAndSell()
    {
        ui.BuyAndSell();
        RequeueLocaleWindow();
    }

    public void LeaveShop()
    {
        ui.LeaveShop();
    }

    public void Leave()
    {
        ui.Leave();
    }
}
