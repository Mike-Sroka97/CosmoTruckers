using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthyProcedure : MonoBehaviour
{
    //layouts[]

    [SerializeField] GameObject tempLayout; //delete dis
    [HideInInspector] public int Score = 0;

    LPSuccess[] successNodes;
    List<GameObject> activatedNodes = new List<GameObject>();

    int random;
    List<GameObject> extraNodes = new List<GameObject>();

    private void Start()
    {
        successNodes = FindObjectsOfType<LPSuccess>();
        foreach(LPSuccess success in successNodes)
        {
            success.gameObject.SetActive(false);
        }
        random = UnityEngine.Random.Range(0, successNodes.Length);
        successNodes[random].gameObject.SetActive(true);
        activatedNodes.Add(successNodes[random].gameObject);
    }

    private void Update()
    {

    }

    public void NextNode()
    {
        if(activatedNodes.Count != successNodes.Length)
        {
            while (activatedNodes.Contains(successNodes[random].gameObject))
            {
                random = UnityEngine.Random.Range(0, successNodes.Length);
            }
            successNodes[random].gameObject.SetActive(true);
            activatedNodes.Add(successNodes[random].gameObject);
        }
    }
}
