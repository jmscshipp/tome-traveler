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
