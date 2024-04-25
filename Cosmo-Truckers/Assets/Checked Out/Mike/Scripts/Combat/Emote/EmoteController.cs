using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteController : MonoBehaviour
{
    [Header("Transparencies")]
    const float selectedAlpha = 1f;
    [SerializeField] float oneAwayAlpha;
    [SerializeField] float twoAwayAlpha;
    const float threePlusAway = 0f;

    [Header("ControllerStuffs")]
    [Space(20)]
    [SerializeField] float distanceToMove = 0.8f;
    [SerializeField] float scrollSpeed;
    int currentlySelectedEmote;
    bool open;
    bool scrolling;

    [Header("Emotes")]
    [Space(20)]
    public Emote[] Emotes;
    private Emote[] emoteSlots; //TODO pull from character

    private void OnEnable()
    {
        currentlySelectedEmote = 0;

        emoteSlots = GetComponentsInChildren<Emote>();

        SetEmotes();
        SetTransparency();
    }

    private void SetEmotes()
    {
        int emoteCount = Emotes.Length;
        int emoteIndex = currentlySelectedEmote;

        for (int i = 0; i < emoteSlots.Length; i++)
        {
            if (emoteIndex >= emoteCount)
                emoteIndex = 0;

            emoteSlots[i].InitEmote(Emotes[emoteIndex]);

            emoteIndex++;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(open)
            {
                transform.Find("Baby").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("Baby").gameObject.SetActive(true);
                OnEnable();
            }

            open = !open;
        }

        else if(Input.GetKeyDown(KeyCode.A) && open && !scrolling)
            StartCoroutine(Scroll(false));

        else if (Input.GetKeyDown(KeyCode.D) && open && !scrolling)
            StartCoroutine(Scroll(true));
    }


    IEnumerator Scroll(bool moveLeft)
    {
        scrolling = true;

        UpdateEmoteVisuals(moveLeft);

        Vector3 originalPosition = transform.position;

        if (moveLeft)
            transform.position += new Vector3(transform.position.x + distanceToMove, transform.position.y);
        else
            transform.position += new Vector3(transform.position.x - distanceToMove, transform.position.y);

        float distanceTravelled = 0;

        while(distanceTravelled < distanceToMove)
        {
            float distance = scrollSpeed * Time.deltaTime;
            distanceTravelled += distance;

            if (moveLeft)
                transform.position -= new Vector3(distance, 0);
            else
                transform.position += new Vector3(distance, 0);

            yield return null;
        }

        transform.position = originalPosition;
        scrolling = false;
    }

    private void SetTransparency()
    {
        for (int i = 0; i < emoteSlots.Length; i++)
        {
            if (i == 2 || i == 8)
                emoteSlots[i].Icon.color = new Color(1, 1, 1, twoAwayAlpha);
            else if(i == 1 || i == 9)
                emoteSlots[i].Icon.color = new Color(1, 1, 1, oneAwayAlpha);
            else if(i == 0)
                emoteSlots[i].Icon.color = new Color(1, 1, 1, selectedAlpha);
            else
                emoteSlots[i].Icon.color = new Color(1, 1, 1, threePlusAway);
        }
    }

    private void UpdateEmoteVisuals(bool increment)
    {
        //Set slot
        if (increment)
            currentlySelectedEmote++;
        else
            currentlySelectedEmote--;

        //Correct slot
        if (currentlySelectedEmote >= Emotes.Length)
            currentlySelectedEmote = 0;
        else if (currentlySelectedEmote < 0)
            currentlySelectedEmote = Emotes.Length - 1;

        //Update slots
        SetEmotes();
        SetTransparency();
    }
}
