using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TESTDIALOG : MonoBehaviour
{
    [SerializeField] private TextAsset textFile;
    [SerializeField] private BaseActor[] playerActors;

    private DialogDirector myDirector; 


    private void Start()
    {
        myDirector = FindObjectOfType<DialogDirector>();

        SetUpDialog();
    }

    private void SetUpDialog()
    {
        myDirector.SetScene(textFile, playerActors.ToList());
    }
}
