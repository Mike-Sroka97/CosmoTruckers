using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextParser : MonoBehaviour
{
    private const string betweenDialogs = "\r\n\r\n"; 

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
        List<string> dialogs = textFile.text.Split(new string[] {"/*/", "/*/"}, System.StringSplitOptions.RemoveEmptyEntries).ToList();

        dialogs = dialogs.Where(dialog => dialog.Trim() != betweenDialogs.Trim()).ToList();

        foreach (string dialog in dialogs)
        {
            dialog.Trim();
        }

        return dialogs.ToArray();
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
        // Replace tags and {} with ""
        string noTagsLine = Regex.Replace(lines[currentLine], @"\{.*?\}", "");

        return noTagsLine; 
    }

    private string[] GetAllLinesInThisDialog(string dialog)
    {
        string[] lines = dialog.Split('\n');
        List<string> realLines = new List<string>();
        int startPosition = 1;

        for (int i = startPosition; i < lines.Length; i++)
        {
            //Remove \r from lines
            lines[i] = Regex.Replace(lines[i], "\r", ""); 

            if (lines[i] != "\\n\r" && lines[i] != "")
                realLines.Add(lines[i]);
        }

        return realLines.ToArray();
    }

    public int GetAllLinesInThisDialogCount(string dialog)
    {
        string[] allLines = GetAllLinesInThisDialog(dialog);
        return allLines.Length;
    }

    private string GetRawTagList(string line)
    {
        return Regex.Match(line, @"\{([^}]*)\}").Groups[1].Value;
    }
}
