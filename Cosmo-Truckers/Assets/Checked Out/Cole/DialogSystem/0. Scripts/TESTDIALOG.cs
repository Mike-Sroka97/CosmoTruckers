using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TESTDIALOG : MonoBehaviour
{
    [SerializeField] private TextAsset textFile;
    [SerializeField] private BaseActor[] playerActors;
    [SerializeField] GameObject sceneLayout;

    private bool startedSceneLoad; 
    private DialogManager myDialogManager; 


    private void Start()
    {
        myDialogManager = FindObjectOfType<DialogManager>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !startedSceneLoad)
        {
            startedSceneLoad= true;
            SetUpDialog();
        }
    }

    private void SetUpDialog()
    {
        StartCoroutine(myDialogManager.LoadDialogScene(sceneLayout, textFile, playerActors.ToList()));
    }
}
