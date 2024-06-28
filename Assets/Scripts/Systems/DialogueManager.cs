using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox, nameBox;
    [SerializeField] Text dialogText;
    [SerializeField] Text nameText;

    [SerializeField] int lettersPerSecond;
    private bool isDialogActive = false;
    public static DialogueManager Instance { get; private set; }
    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nameBox.SetActive(false);
    }


    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    public IEnumerator ShowDialog(Dialog dialog)
    {
        if (isDialogActive) yield break;

        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        this.dialog = dialog;
        nameBox.SetActive(true);
        dialogBox.SetActive(true);
        isDialogActive = true;
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                nameBox.SetActive(false);
                dialogBox.SetActive(false);
                dialogText.text = string.Empty;
                currentLine = 0;
                isDialogActive = false;
                OnHideDialog?.Invoke();
            }
        }
    }

    void OnDestroy()
    {
        TriggerEvent.OnTrigger -= HandleUpdate;
    }

    public IEnumerator TypeDialog(DialogLine dialogLine)
    {
        isTyping = true;
        nameText.text = dialogLine.speaker;
        dialogText.text = "";
        foreach(var letter in dialogLine.line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
        isTyping = false;
    }
}
