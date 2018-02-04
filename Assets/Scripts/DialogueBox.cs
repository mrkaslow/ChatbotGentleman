using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Text m_dialogueText;
    [SerializeField] private Image m_dialogueImage;

    public Image DialogueImage
    {
        get { return m_dialogueImage; }
        set { m_dialogueImage = value; }
    }

    public Text DialogueText
    {
        get { return m_dialogueText; }
        set { m_dialogueText = value; }
    }
}
