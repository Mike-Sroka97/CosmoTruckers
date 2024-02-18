using Ink.Parsed;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextParser : MonoBehaviour
{
    public string GetActorDialog(TextAsset textFile, string actorName)
    {
        string[] allDialogs = GetAllDialogs(textFile);

        foreach (string dialog in allDialogs)
        {
            string[] lines = dialog.Split('\n');
            string name = lines[0].Trim();
            name.Replace("\r", ""); 

            if (actorName == name)
            {
                return dialog; 
            }
        }

        Debug.LogError("Couldn't find unique actor! Returning BASE dialog instead!"); 
        return allDialogs[0]; 
    }

    //Write all of the dialog options on one text document
    public string[] GetAllDialogs(TextAsset textFile)
    {
        string[] dialogs = textFile.text.Split(new string[] {"/*/", "/*/"}, System.StringSplitOptions.RemoveEmptyEntries); 

        foreach (string dialog in dialogs)
        {
            dialog.Trim(); 
        }

        return dialogs;
    }

    public string[] GetTagsAtCurrentLine(string dialog, int currentLine)
    {
        string[] lines = GetAllLinesInThisDialog(dialog); 
        string[] tags = GetRawTagList(lines[currentLine]).Split(',');

        return tags; 
    }
    
    public string GetTextAtCurrentLine(string dialog, int currentLine)
    {
        string[] lines = GetAllLinesInThisDialog(dialog);
        lines[currentLine].Replace(GetRawTagList(lines[currentLine]), string.Empty);

        return lines[currentLine]; 
    }

    private string[] GetAllLinesInThisDialog(string dialog)
    {
        string[] lines = dialog.Split('\n');

        List<string> realLines = new List<string>();

        //The first line will always be a descriptor of who is talking. We can skip it.
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i] != "\\n\r")
                realLines.Add(lines[i]);
        }

        return realLines.ToArray();
    }

    private string GetRawTagList(string line)
    {
        return Regex.Match(line, @"\{([^}]*)\}").Groups[1].Value;
    }
}
