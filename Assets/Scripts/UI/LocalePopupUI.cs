using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalePopupUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private Transform buttonParent;

    [SerializeField]
    private GameObject leaveButtonPrefab;
    [SerializeField]
    private GameObject exploreButtonPrefab;
    [SerializeField]
    private GameObject campButtonPrefab;
    [SerializeField]
    private GameObject huntButtonPrefab;
    [SerializeField]
    private GameObject sleepButtonPrefab;
    [SerializeField]
    private GameObject talkButtonPrefab;
    [SerializeField]
    private GameObject buySellButtonPrefab;
    [SerializeField]
    private GameObject leaveShopButton;

    public Locale currentLocale;

    public void ActivatePopup(Locale locale)
    {
        //Debug.Log("activating locale popup " + locale.GetLocaleType());
        currentLocale = locale;

        // turn on child objects
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
            child.gameObject.SetActive(true);

        descriptionText.text = locale.GetLocaleDescription();
        AddButtons(locale.GetLocaleType());
    }

    public void ClosePopup()
    {
        // delete buttons for location we're leaving
        foreach (Transform button in buttonParent.transform)
            Destroy(button.gameObject);

        // turn off child objects
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
            child.gameObject.SetActive(false);

        UIManager.Instance().DialogueOpen = false;
        MapController.Instance().GetCurrentLocation().MakeConnectionsSelectable(true);
    }

    public void AddButtons(LocaleTypes localeType)
    {
        // if inside shop, only create this one button
        if (localeType == LocaleTypes.InsideShop)
        {
            GameObject button = Instantiate(leaveShopButton, buttonParent);
            button.GetComponent<LocaleButton>().AssignLocalePopupUI(this);
            return;
        }

        // explore button
        Instantiate(exploreButtonPrefab, buttonParent);

        switch (localeType)
        {
            case LocaleTypes.Town:
                Instantiate(sleepButtonPrefab, buttonParent);
                Instantiate(talkButtonPrefab, buttonParent);
                break;
            case LocaleTypes.Wilds:
                Instantiate(campButtonPrefab, buttonParent);
                Instantiate(huntButtonPrefab, buttonParent);
                break;
            case LocaleTypes.Shop:
                Instantiate(buySellButtonPrefab, buttonParent);
                break;
            case LocaleTypes.City:
                Instantiate(sleepButtonPrefab, buttonParent);
                Instantiate(talkButtonPrefab, buttonParent);
                Instantiate(buySellButtonPrefab, buttonParent);
                break;
            case LocaleTypes.Ruins:
                Instantiate(campButtonPrefab, buttonParent);
                break;
            case LocaleTypes.Cabin:
                Instantiate(sleepButtonPrefab, buttonParent);
                Instantiate(talkButtonPrefab, buttonParent);
                break;
            default:
                break;
        }

        // leave button
        Instantiate(leaveButtonPrefab, buttonParent);

        // assign local popup ref to all buttons
        foreach (LocaleButton button in buttonParent.GetComponentsInChildren<LocaleButton>())
            button.AssignLocalePopupUI(this);
    }

    public void Explore()
    {
        currentLocale.Explore();
    }

    public void Camp()
    {
        if (currentLocale is Wild wild)
        {
            wild.Camp();
        }
    }

    public void Hunt()
    {
        if (currentLocale is Wild wild)
        {
            wild.Hunt();
        }
    }

    public void Sleep()
    {
        // cabin
        if (currentLocale is Cabin cabin)
        {
            cabin.Sleep();
        }
        else if (currentLocale is City city)
        {
            city.Sleep();
        }
        else if (currentLocale is Town town)
        {
            town.Sleep();
        }
    }

    public void Talk()
    {
        currentLocale.Talk();
    }

    public void BuyAndSell()
    {
        // shop
        if (currentLocale is Shop shop)
        {
            shop.BuyAndSell();
        }
        // city
        else if (currentLocale is City city)
        {
            city.BuyAndSell();
        }
    }

    public void LeaveShop()
    {
        InsideShop castLocale = (InsideShop)currentLocale;
        castLocale.LeaveShop ();
    }

    public void Leave()
    {
        ClosePopup();
    }
}
