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
    private GameObject sellButtonPrefab;
    [SerializeField]
    private GameObject buyButtonPrefab;

    private Locale currentLocal;

    public void ActivatePopup(Locale locale)
    {
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
    }

    public void AddButtons(LocaleTypes localeType)
    {
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
                Instantiate(buyButtonPrefab, buttonParent);
                Instantiate(sellButtonPrefab, buttonParent);
                break;
            case LocaleTypes.City:
                Instantiate(sleepButtonPrefab, buttonParent);
                Instantiate(talkButtonPrefab, buttonParent);
                Instantiate(buyButtonPrefab, buttonParent);
                Instantiate(sellButtonPrefab, buttonParent);
                break;
            case LocaleTypes.Ruins:
                Instantiate(sleepButtonPrefab, buttonParent);
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

    }

    public void Camp()
    {

    }

    public void Hunt()
    {

    }

    public void Sleep()
    {

    }

    public void Talk()
    {

    }

    public void Buy()
    {
        // make merchant inventory available for selling
    }

    public void Sell()
    {
        // make player inventory available for selling
    }

    public void Leave()
    {
        ClosePopup();
    }
}
