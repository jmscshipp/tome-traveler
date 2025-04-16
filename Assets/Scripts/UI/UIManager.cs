using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // dialogue popup variables
    [SerializeField]
    private GameObject dialoguePopup;
    [SerializeField]
    private TMP_Text dialoguePopupText;

    // location popup variables
    [SerializeField]
    private LocalePopupUI localePopup;

    private static UIManager instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static UIManager Instance()
    {
        return instance;
    }

    public void OpenLocalePopup(Locale locale)
    {
        localePopup.gameObject.SetActive(true);
        localePopup.ActivatePopup(locale);
    }

    public void CloseLocalePopup()
    {
        localePopup.gameObject.SetActive(false);
    }

    public void OpenDialoguePopup(string dialogue)
    {
        dialoguePopup.SetActive(true);
        dialoguePopupText.text = dialogue;
    }

    public void CloseDialoguePopup()
    {
        dialoguePopup.SetActive(false);
    }
}
