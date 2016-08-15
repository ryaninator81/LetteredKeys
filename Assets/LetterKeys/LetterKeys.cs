using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LetterKeys : MonoBehaviour {

    public KMSelectable[] buttons;

    string correctLetter;
    int correctIndex;
    int magicNum;
    public TextMesh textMesh;

    void Start()
    {
        Init();
    }

    void Init()
    {
        correctIndex = Random.Range(0, 4);
        magicNum = Random.Range(0, 100);
        string[] temp2 = { "A", "B", "C", "D" };
        List<int> temp1 = new List<int>();
        while (temp1.Count != 4)
        {
            int i = Random.Range(0, 4);
            if (!temp1.Contains(i))
            {
                temp1.Add(i);
            }
        }


        TextMesh numberText = textMesh;
        numberText.text = magicNum.ToString();
        for (int i = 0; i < buttons.Length; i++)
        {

            TextMesh buttonText = buttons[i].GetComponentInChildren<TextMesh>();
            buttonText.text = temp2[temp1[i]];
            int j = i;
            buttons[i].OnInteract += delegate () { OnPress(temp2[temp1[j]]); return false; };
        }
    }

    void OnPress(string button)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        List<string> responses = null;
        int batteryCount = 0;
        responses = GetComponent<KMBombInfo>().QueryWidgets(KMBombInfo.QUERYKEY_GET_BATTERIES, null);
        foreach (string response in responses)
        {
            Dictionary<string, int> responseDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);
            batteryCount += responseDict["numbatteries"];
        }

        string serial = "";
        responses = GetComponent<KMBombInfo>().QueryWidgets(KMBombInfo.QUERYKEY_GET_SERIAL_NUMBER, null);
        foreach (string response in responses)
        {
            Dictionary<string, string> responseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            serial = responseDict["serial"];
        }
        //write(batteryCount.ToString());
        // write(serial);
       
        string s = getCorrectButton(batteryCount, serial);
        if (button.Equals(s))
        {
            GetComponent<KMBombModule>().HandlePass();
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
        }
        
    }

    private string getCorrectButton(int batteryCount, string serial)
    {
        if (magicNum == 69)
        {
            return "D";
        }
        else if (magicNum % 6 == 0)
        {
            return "A";
        }
        else if (magicNum % 3 == 0 && batteryCount >= 2)
        {
            return "B";
        }
        else if (serial.Contains("E") || serial.Contains("C") || serial.Contains("3"))
        {
            if (magicNum >= 22 && magicNum <= 79)
            {
                return "B";
            }
            else
            {
                return "C";
            }
        }
        else if (magicNum < 46)
        {
            return "D";
        }
        else
        {
            return "A";
        }
    }

    /*
    private void write(string s)
    {
        using (StreamWriter writer = new StreamWriter("E:\\Documents\\test.txt", true))
        {
            writer.WriteLine(s);
        }
    }
    */
}
