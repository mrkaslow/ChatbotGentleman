using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatbotManager: MonoBehaviour
{
    private Chatbot bot;
    public InputField inputField;
    public Text gentlemanOutput;

    void Start()
    {
        bot = new Chatbot();
        bot.LoadBrain();
    }

    void OnDisable()
    {
        bot.SaveBrain();
    }

    public void SendQuestionToRobot()
    {
        if (string.IsNullOrEmpty(inputField.text) == false)
        {
            var answer = bot.getOutput(inputField.text);
            gentlemanOutput.text = answer;
            inputField.text = string.Empty;
        }
    }


}
