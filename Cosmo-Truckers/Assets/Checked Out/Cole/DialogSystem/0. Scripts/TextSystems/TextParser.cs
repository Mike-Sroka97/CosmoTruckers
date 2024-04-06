using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextParser : MonoBehaviour
{
    private const string betweenDialogs = "\r\n\r\n";

    #region Dialog Text Parsing
    public string GetActorDialog(TextAsset textFile, string actorName)
    {
        string[] allDialogs = GetAllDialogs(textFile);

        foreach (string dialog in allDialogs)
        {
            string[] lines = dialog.Split('\n');
            string name = lines[1].Trim();
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

    public string[] GetTagsAtCurrentDialogLine(string dialog, int currentLine)
    {
        string[] lines = GetAllLinesInThisDialog(dialog);
        // Split the characters to get what's inside of the brackets
        string rawTagList = GetRawTagList(lines[currentLine]);
        string[] tags = rawTagList.Trim('{', '}').Split(new[] {"}{"}, StringSplitOptions.None); 
        return tags; 
    }
    
    public string GetDialogTextAtCurrentLine(string dialog, int currentLine)
    {
        string[] lines = GetAllLinesInThisDialog(dialog);
        // Replace tags and {} with ""
        string noTagsLine = Regex.Replace(lines[currentLine], @"\{{.*?\}}", "");

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
        // Search for {{ and }}. Return each bracketed item inside. 
        // Then remove the first and last bracket
        string entireLine = Regex.Match(line, @"\{\{(.*?)\}\}").Groups[0].Value;

        string modifiedString = string.Empty; 

        if (entireLine.Length > 0)
            modifiedString = entireLine.Substring(1, entireLine.Length - 2);

        return modifiedString;
    }

    #endregion

    #region Regular Text Parsing

    public string[] GetAllLinesInRegularText(TextAsset textFile)
    {
        string allText = textFile.text; 

        string[] lines = allText.Split('\n');
        List<string> realLines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            //Remove \r from lines
            lines[i] = Regex.Replace(lines[i], "\r", "");

            if (lines[i] != "\n\r" && lines[i] != "")
                realLines.Add(lines[i]);
        }

        return realLines.ToArray();
    }

    public string[] GetTagsInRegularTextLine(string line)
    {
        // Split the characters to get what's inside of the brackets
        string rawTagList = GetRawTagList(line);
        string[] tags = rawTagList.Trim('{', '}').Split(new[] { "}{" }, StringSplitOptions.None);
        return tags;
    }

    public string GetTrueRegularTextLine(string currentLine)
    {
        // Replace tags and {} with ""
        string noTagsLine = Regex.Replace(currentLine, @"\{{.*?\}}", "");

        return noTagsLine;
    }

    #endregion
}
