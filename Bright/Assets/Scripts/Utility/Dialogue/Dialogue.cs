using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public float interval;
    public List<string> dialogue = new List<string>();
    public int index;
    public string display;
    public DialogueState dialogueState = DialogueState.Idle;

    public TextMeshProUGUI dialogueText;    
    public GameObject dialogueParent;

    public void Update()
    {
        if (dialogueState == DialogueState.Load)
        {
            StartCoroutine(DisplayText(dialogue[index]));
            dialogueState = DialogueState.Normal;
        }
    }

    public void ClearDialogue()
    {
        dialogue.Clear();
        index = 0;
        dialogueParent.SetActive(false);
    }

    public void UpdateText()
    {
        dialogueText.text = display;
    }

    public IEnumerator DisplayText(string sentence)
    {
        for (int i = 0; i <= sentence.Length; i++)
        {
            display = sentence.Substring(0, i);
            UpdateText();
            yield return new WaitForSeconds(interval);
        }

        index++;
        dialogueState = DialogueState.Idle;
    }
}

public enum DialogueState
{
    Load,
    Idle,
    Normal,
}