using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class TESTDIALOG : MonoBehaviour
{
    [SerializeField] private TextAsset dialogFile;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private BaseActor[] playerActors;
    [SerializeField] GameObject sceneLayout;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Image nextLineIndicator; 
    public KeyCode inputKey; 
    public KeyCode dialogInputKey; 

    private bool startedSceneLoad; 
    private DialogManager myDialogManager;

    private void Start()
    {
        myDialogManager = FindObjectOfType<DialogManager>();
    }

    private void Update()
    {
        if (Input.GetKey(dialogInputKey) && !startedSceneLoad)
        {
            startedSceneLoad= true;
            SetUpDialog();
        }

        if (Input.GetKey(inputKey))
        {
            DialogManager.Instance.StartRegularTextMode(textFile, textBox, nextLineIndicator); 
        }
    }

    private void SetUpDialog()
    {
        DialogManager.Instance.LoadDialogScene(sceneLayout, dialogFile, playerActors.ToList());
    }
}
