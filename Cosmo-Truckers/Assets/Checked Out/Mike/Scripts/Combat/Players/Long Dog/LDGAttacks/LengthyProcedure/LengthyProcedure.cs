using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthyProcedure : MonoBehaviour
{
    //layouts[]

    [SerializeField] GameObject[] layouts;
    [HideInInspector] public int Score = 0;

    LPSuccess[] successNodes;
    List<GameObject> activatedNodes = new List<GameObject>();
    bool minigameEnded = false;
    int random;

    private void Start()
    {
        random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform.Find("Layout"));

        successNodes = FindObjectsOfType<LPSuccess>();
        foreach(LPSuccess success in successNodes)
        {
            success.gameObject.SetActive(false);
        }
        random = UnityEngine.Random.Range(0, successNodes.Length);
        successNodes[random].gameObject.SetActive(true);
        activatedNodes.Add(successNodes[random].gameObject);
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

    public void EndMinigame()
    {
        if(!minigameEnded)
        {
            minigameEnded = true;
            //TODO: add insta end to minigame
            Debug.Log("minigame over");
        }
    }
}
