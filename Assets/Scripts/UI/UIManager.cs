using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    // dialogue popup variables
    [SerializeField]
    protected GameObject dialoguePopup;
    [SerializeField]
    private TMP_Text dialoguePopupText;

    List<Popup> popupQueue = new List<Popup>();

    public abstract class Popup
    {
        public abstract void ActivateWindow();
    }

    public class DialoguePopup : Popup {
        public string dialogue;

        public DialoguePopup(string dialogue)
        {
            this.dialogue = dialogue;
        }

        public override void ActivateWindow()
        {
            Instance().dialoguePopup.SetActive(true);
            Instance().dialoguePopupText.text = dialogue;
        }

    }

    public class LocalePopup : Popup
    {
        public Locale locale;
        public LocalePopup(Locale locale)
        {
            this.locale = locale;
        }
        public override void ActivateWindow()
        {
            Instance().localePopup.gameObject.SetActive(true);
            Instance().localePopup.ActivatePopup(locale);
        }
    }

    public class SecretLocalePopup : Popup
    {
        SecretLocale secretLocale;
        public SecretLocalePopup(SecretLocale secretLocale)
        {
            this.secretLocale = secretLocale;
        }
        public override void ActivateWindow()
        {
            Instance().dialoguePopup.SetActive(true);
            Instance().dialoguePopupText.text = secretLocale.Dialogue;
        }

    }

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
        popupQueue.Add(new LocalePopup(locale));
    }

    public void CloseLocalePopup()
    {
        localePopup.gameObject.SetActive(false);
    }

    public void OpenDialoguePopup(string dialogue)
    {
        popupQueue.Add(new DialoguePopup(dialogue));
    }

    public void CloseDialoguePopup()
    {
        dialoguePopup.SetActive(false);
    }
}
