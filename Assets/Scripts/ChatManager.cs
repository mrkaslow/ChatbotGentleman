using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    private const string TALKANIM = "Talk";

    private TextAsset[] aimlFiles;
    private List<string> aimlXmlDocumentListFileName = new List<string>();
    private List<XmlDocument> aimlXmlDocumentList = new List<XmlDocument>();
    private TextAsset GlobalSettings, GenderSubstitutions, Person2Substitutions, PersonSubstitutions, Substitutions, DefaultPredicates, Splitters;
    private Chatbot bot;
    [SerializeField] private InputField inputField;
    [Range(0,1)]
    [SerializeField] private float m_displayDelay;
    private DialogueBox m_botText;
    private DialogueBox m_userText;
    private string m_currentAnswer;
    private string m_usersInput;
    [SerializeField] private Animator m_animator;
    [SerializeField] private GameObject m_dialogueContainer;
    [SerializeField] private GameObject m_userBoxPrefab;
    [SerializeField] private GameObject m_gentlemanBoxPrefab;
    private Queue<GameObject> m_dialogueBoxes;

    void Start()
    {
        bot = new Chatbot();
        LoadFilesFromConfigFolder();
        bot.LoadSettings(GlobalSettings.text, GenderSubstitutions.text, Person2Substitutions.text, PersonSubstitutions.text, Substitutions.text, DefaultPredicates.text, Splitters.text);
        TextAssetToXmlDocumentAIMLFiles();
        bot.loadAIMLFromXML(aimlXmlDocumentList.ToArray(), aimlXmlDocumentListFileName.ToArray());
        bot.LoadBrain();
        inputField.ActivateInputField();
        m_dialogueBoxes = new Queue<GameObject>();
    }

    public string GetAnswer()
    {
        if (string.IsNullOrEmpty(inputField.text) == false)
        {
            var answer = bot.getOutput(inputField.text);
            m_usersInput = inputField.text;
            inputField.text = string.Empty;
            return answer;
        }
        return null;
    }

    void LoadFilesFromConfigFolder()
    {
        GlobalSettings = Resources.Load<TextAsset>("config/Settings");
        GenderSubstitutions = Resources.Load<TextAsset>("config/GenderSubstitutions");
        Person2Substitutions = Resources.Load<TextAsset>("config/Person2Substitutions");
        PersonSubstitutions = Resources.Load<TextAsset>("config/PersonSubstitutions");
        Substitutions = Resources.Load<TextAsset>("config/Substitutions");
        DefaultPredicates = Resources.Load<TextAsset>("config/DefaultPredicates");
        Splitters = Resources.Load<TextAsset>("config/Splitters");
    }

    void TextAssetToXmlDocumentAIMLFiles()
    {
        aimlFiles = Resources.LoadAll<TextAsset>("aiml");
        foreach (TextAsset aimlFile in aimlFiles)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(aimlFile.text);
                aimlXmlDocumentListFileName.Add(aimlFile.name);
                aimlXmlDocumentList.Add(xmlDoc);
            }
            catch (System.Exception e)
            {
                Debug.Log("FILE NOT LOADED: " + aimlFile.name);
                Debug.LogWarning(e.ToString());
            }
        }
    }

    void OnDisable()
    {
        bot.SaveBrain();
    }

    public void SendQuestionToBot()
    {
        m_currentAnswer = GetAnswer();

        if (string.IsNullOrEmpty(m_usersInput))
            return;
        
        if (string.IsNullOrEmpty(m_currentAnswer))
        {
            m_currentAnswer = "I don't know what to say...";
        }

        var botAnswer = Instantiate(m_gentlemanBoxPrefab, m_dialogueContainer.transform);
        var userQuestion = Instantiate(m_userBoxPrefab, m_dialogueContainer.transform);

        userQuestion.transform.SetAsFirstSibling();
        botAnswer.transform.SetAsFirstSibling();

        m_dialogueBoxes.Enqueue(botAnswer);
        m_dialogueBoxes.Enqueue(userQuestion);

        if (m_dialogueBoxes.Count >= 20)
        {
            Destroy(m_dialogueBoxes.Dequeue());
            Destroy(m_dialogueBoxes.Dequeue());
        }

        m_userText = userQuestion.GetComponent<DialogueBox>();
        m_botText = botAnswer.GetComponent<DialogueBox>();

        m_userText.DialogueText.text = m_usersInput;

        StartCoroutine(AnimateText(m_currentAnswer, m_botText.DialogueText));
        StartCoroutine(AnimateFrame());
    }

    public void SkipToNextText()
    {
        StopAllCoroutines();
        if(m_botText == null)
            return;
        m_botText.DialogueText.text = m_currentAnswer;
    }

    IEnumerator AnimateText(string currentlyDisplayingText, Text editedText)
    {
        m_animator.SetTrigger(TALKANIM);
        
        for (int i = 0; i < (currentlyDisplayingText.Length + 1); i++)
        {
            editedText.text = currentlyDisplayingText.Substring(0, i);
            yield return new WaitForSeconds(m_displayDelay);
        }
        m_animator.SetTrigger(TALKANIM);
        m_currentAnswer = null;
        yield return null;
    }

    IEnumerator AnimateFrame()
    {
        var wait = new WaitForEndOfFrame();
        var totalTime = 0.0f;
        var time = 1f;
        while (totalTime < time)
        {
            totalTime += Time.deltaTime;
            m_userText.DialogueImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Easing.easeOutExpo(totalTime, 1));
            m_botText.DialogueImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Easing.easeOutExpo(totalTime, 1));
            yield return wait;
        }
    }
    
}
