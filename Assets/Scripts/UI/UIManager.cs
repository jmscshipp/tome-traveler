using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Runtime.InteropServices.WindowsRuntime;

public class UIManager : MonoBehaviour
{
    // dialogue popup variables
    [SerializeField]
    protected GameObject dialoguePopup;
    [SerializeField]
    private TMP_Text dialoguePopupText;

    public bool DialogueOpen = false;

    List<Popup> popupQueue = new List<Popup>();

    [SerializeField]
    public abstract class Popup
    {
        public abstract void ActivateWindow();
    }

    [SerializeField]
    public class DialoguePopup : Popup {
        [SerializeField]
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

    [SerializeField]
    public class SecretLocalePopup : Popup
    {
        [SerializeField]
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

    public void Update()
    {
        if (!DialogueOpen && popupQueue.Count > 0)
        {
            MapController.Instance().GetCurrentLocation().MakeConnectionsSelectable(false);
            DialogueOpen = true;
            Popup ActivePopup = popupQueue[0];
            popupQueue.RemoveAt(0);
            ActivePopup.ActivateWindow();
        }
    }

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
        DialogueOpen = false;
        MapController.Instance().GetCurrentLocation().MakeConnectionsSelectable(true);
    }

    public void OpenDialoguePopup(string dialogue)
    {
        popupQueue.Add(new DialoguePopup(dialogue));
    }

    public void CloseDialoguePopup()
    {
        dialoguePopup.SetActive(false);
        DialogueOpen = false;
        MapController.Instance().GetCurrentLocation().MakeConnectionsSelectable(true);
    }
}
