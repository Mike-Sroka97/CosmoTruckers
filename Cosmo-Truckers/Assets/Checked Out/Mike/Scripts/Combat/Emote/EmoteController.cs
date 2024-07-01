using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

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
    private bool canOpen = true;
    public bool CanOpen
    {
        get
        {
            return canOpen;
        }
        set
        {
            //TODO HAVE NETWORK TELL PLAYER TO FUCK OFF IF THEY GET SCHLOPPED INTO INA
            canOpen = value;
        }
    }

    [Header("Emotes")]
    [Space(20)]
    public List<Emote> Emotes;
    private List<Emote> emoteSlots; //TODO pull from character

    private void OnEnable()
    {
        currentlySelectedEmote = 0;

        emoteSlots = GetComponentsInChildren<Emote>().ToList();
        int originalCount = Emotes.Count;

        int counter = 0;
        while(Emotes.Count < emoteSlots.Count)
        {
            Emotes.Add(Emotes[counter]);

            counter++;

            if (counter >= originalCount)
                counter = 0;
        }

        SetEmotes();
        SetTransparency();
    }

    private void SetEmotes()
    {
        int emoteCount = Emotes.Count;
        int emoteIndex = currentlySelectedEmote;

        for (int i = 0; i < emoteSlots.Count; i++)
        {
            if (emoteIndex >= emoteCount)
                emoteIndex = 0;

            emoteSlots[i].InitEmote(Emotes[emoteIndex]);

            emoteIndex++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (GetComponentInParent<PlayerVessel>().CheckAthority())
            {
                if (open)
                {
                    transform.Find("Baby").gameObject.SetActive(false);
                }
                else if (canOpen)
                {
                    transform.Find("Baby").gameObject.SetActive(true);
                    OnEnable();
                }

                open = !open;
            }
        }

        else if (Input.GetKeyDown(KeyCode.A) && open && !scrolling)
            StartCoroutine(Scroll(false));

        else if (Input.GetKeyDown(KeyCode.D) && open && !scrolling)
            StartCoroutine(Scroll(true));

        else if (Input.GetKeyDown(KeyCode.Space) && open && !scrolling)
            SpawnEmote();
    }

    private void SpawnEmote()
    {
        canOpen = false;
        open = false;
        transform.Find("Baby").gameObject.SetActive(false);

        GetComponentInParent<PlayerVessel>().SpawnEmote(emoteSlots[0].EmoteToSpawn);
    }

    IEnumerator Scroll(bool moveLeft)
    {
        scrolling = true;

        UpdateEmoteVisuals(moveLeft);

        Vector3 originalPosition = transform.localPosition;

        if (moveLeft)
            transform.localPosition = new Vector3(transform.localPosition.x + distanceToMove, transform.localPosition.y);
        else
            transform.localPosition = new Vector3(transform.localPosition.x - distanceToMove, transform.localPosition.y);

        float distanceTravelled = 0;

        while(distanceTravelled < distanceToMove)
        {
            float distance = scrollSpeed * Time.deltaTime;
            distanceTravelled += distance;

            if (moveLeft)
                transform.localPosition -= new Vector3(distance, 0);
            else
                transform.localPosition += new Vector3(distance, 0);

            yield return null;
        }

        transform.localPosition = originalPosition;
        scrolling = false;
    }

    private void SetTransparency(int offset = 0)
    {
        for (int i = 0; i < emoteSlots.Count; i++)
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
        if (currentlySelectedEmote >= Emotes.Count)
            currentlySelectedEmote = 0;
        else if (currentlySelectedEmote < 0)
            currentlySelectedEmote = Emotes.Count - 1;

        //Update slots
        SetEmotes();
        SetTransparency();
    }
}
