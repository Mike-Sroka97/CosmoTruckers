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
        /*
        List<GameObject> actors = new List<GameObject>();

        foreach (GameObject actor in playerActors)
        {
            actors.Add(actor); 
        }
        foreach(GameObject actor in otherActors)
        {
            actors.Add(actor);
        }
        */ 

        myDirector.SetScene(textFile, playerActors.ToList());
    }
}
