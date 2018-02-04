using AIMLbot;
using System;
using System.IO;
using UnityEngine;
using System.Xml;

public class Chatbot
{
    const string UserId = "consoleUser";
    public Bot AimlBot;
    public User myUser;
    private string keyUserSettings = "Brain-Graphmaster";

    public Chatbot()
    {
        AimlBot = new Bot();
        myUser = new User(UserId, AimlBot);
    }


    public void loadAIMLFromXML(XmlDocument[] aiml, string[] aimlFileName)
    {
        AimlBot.isAcceptingUserInput = false;

        for (int i = 0; i <= aiml.Length - 1; i++)
        {
            AimlBot.loadAIMLFromXML(aiml[i], aimlFileName[i]);
        }

        AimlBot.isAcceptingUserInput = true;
    }


    public void LoadSettings(string GlobalSettings, string GenderSubstitutions, string Person2Substitutions, string PersonSubstitutions, string Substitutions, string DefaultPredicates, string Splitters)
    {
        XmlDocument a = new XmlDocument();
        a.LoadXml(GlobalSettings);
        AimlBot.GlobalSettings.loadSettings(a);
        //
        XmlDocument b = new XmlDocument();
        b.LoadXml(GenderSubstitutions);
        AimlBot.GenderSubstitutions.loadSettings(b);
        //
        XmlDocument c = new XmlDocument();
        c.LoadXml(Person2Substitutions);
        AimlBot.Person2Substitutions.loadSettings(c);
        //
        XmlDocument d = new XmlDocument();
        d.LoadXml(PersonSubstitutions);
        AimlBot.PersonSubstitutions.loadSettings(d);
        //        
        XmlDocument e = new XmlDocument();
        e.LoadXml(Substitutions);
        AimlBot.Substitutions.loadSettings(e);
        //
        XmlDocument f = new XmlDocument();
        f.LoadXml(DefaultPredicates);
        AimlBot.DefaultPredicates.loadSettings(f);
        //
        XmlDocument g = new XmlDocument();
        g.LoadXml(Splitters);
        AimlBot.loadSplittersXml(g);
    }

    public String getOutput(String input)
    {
        Request r = new Request(input, myUser, AimlBot);
        Result res = AimlBot.Chat(r);
        return (res.Output);
    }


    public void SaveBrain()
    {
        string XMLAsString = myUser.Predicates.DictionaryAsXML.OuterXml;
        PlayerPrefs.SetString(keyUserSettings, XMLAsString);
        Debug.Log("Brain saved");

    }


    public void LoadBrain()
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            string XMLAsString = PlayerPrefs.GetString(keyUserSettings);
            doc.LoadXml(XMLAsString);
            myUser.Predicates.loadSettings(doc);
            Debug.Log("Brain loaded");
        }
        catch (Exception e)
        {
            Debug.Log("Brain not loaded");
            Debug.Log(e);
        }
    }

}