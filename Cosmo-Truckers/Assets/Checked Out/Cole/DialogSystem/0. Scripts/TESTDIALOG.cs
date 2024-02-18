using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTDIALOG : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private TextAsset textFile;

    [SerializeField] string[] allTags; 
    [SerializeField] string[] allDialogs; 
    [SerializeField] string[] allLinesInDialog; 
    [SerializeField] int currentLine = 0; 
    [SerializeField] string currentText; 

    TextParser textParser;

    private void Start()
    {
        textParser = GetComponent<TextParser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            allDialogs = textParser.GetAllDialogs(textFile);
            //allLinesInDialog = textParser.GetAllLinesInThisDialog(allDialogs[currentLine]);
            //allTags = textParser.GetTagsAtCurrentLine(allLinesInDialog, currentLine); 
            //currentText = textParser.GetTextAtCurrentLine(allLinesInDialog, currentLine); 
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentLine++; 
        }
    }
}
