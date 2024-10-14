using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AUG_DESCRIPTION : MonoBehaviour
{
    Character currentChar;
    List<AugmentStackSO> charactersAUGS;


    [SerializeField] List<GameObject> selectable;
    [SerializeField] Color selectedColor;
    [SerializeField] Color baseColor;
    [SerializeField] Color nonFadingColor;
    [SerializeField] Color fadingColor;
    [SerializeField] TMP_Text AUGNameText;
    [SerializeField] TMP_Text AUGDescriptionText;
    [SerializeField] Sprite[] bgs;
    [SerializeField] Transform characterSlots;
    [SerializeField] Image mainSlot;
    [SerializeField] float flickerTime = 0.1f;
    int currentLocation = 0;
    int currentCharacterLocation;
    List<AugmentCharacterIcon> actualCharacters = new List<AugmentCharacterIcon>();

    Image leftArrow;
    Image rightArrow;

    private void OnEnable()
    {
        leftArrow = transform.Find("Canvas/Arrows/Left Arrow").GetComponent<Image>();
        rightArrow = transform.Find("Canvas/Arrows/Right Arrow").GetComponent<Image>();
    }

    private void OnDisable()
    {
        currentChar = null;
        charactersAUGS.Clear();

        foreach(var stack in selectable)
            stack.GetComponentInChildren<TMP_Text>().text = "";

        AUGNameText.text = "";  
        AUGDescriptionText.text = "";
    }

    /// <summary>
    /// Init the AUG list to show all AUGs on character
    /// </summary>
    /// <param name="character">Base class for all characters that holds the AUGS</param>
    public void InitList(Character character, bool init = true)
    {
        if(character)
        {
            currentChar = character;

            charactersAUGS = new(currentChar.GetAUGS);

            for (int i = 0; i < charactersAUGS.Count; i++)
            {
                selectable[i].GetComponentInChildren<TMP_Text>().text = charactersAUGS[i].CurrentStacks.ToString();

                if (charactersAUGS[i].GetFade() != 0)
                    selectable[i].GetComponentsInChildren<TMP_Text>()[1].text = "-" + charactersAUGS[i].GetFade().ToString();
                else
                    selectable[i].GetComponentsInChildren<TMP_Text>()[1].text = null;

                selectable[i].GetComponentsInChildren<Image>()[1].sprite = charactersAUGS[i].AugmentSprite;
            }
            for (int i = 0; i < 6; i++)
            {
                DetermineBG(i);
            }

            if (init)
                DetermineCharacterSlots();
        }

        ShowSelection(0);
    }

    //Temp movement system
    private void Update()
    {
        if (CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>().RevokeControls)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            //-4
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //-1
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //+4
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //+1
            MoveRight();
        }

        if(Input.GetKeyDown(KeyCode.Q))
            SelectCharacter(true);
        else if(Input.GetKeyDown(KeyCode.E))
            SelectCharacter(false);

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    void SetDescription()
    {
        string AUGName = charactersAUGS.Count > currentLocation ? charactersAUGS[currentLocation].DebuffName : "Empty";
        string AUGDes = charactersAUGS.Count > currentLocation ? charactersAUGS[currentLocation].DebuffDescription : "";

        AUGNameText.text = $"<b>{AUGName}</b>"; 
        AUGDescriptionText.text = $"{AUGDes}";
    }

    void MoveUp()
    {
        int newLocation = 0;

        if(currentLocation - 3 < 0)
        {
            newLocation = currentLocation + (selectable.Count - 3);
        }
        else
        {
            newLocation = currentLocation - 3;
        }

        ShowSelection(newLocation);
    }
    void MoveDown()
    {
        int newLocation = 0;

        if (currentLocation + 3 >= selectable.Count)
        {
            newLocation = currentLocation - (selectable.Count - 3);
        }
        else
        {
            newLocation = currentLocation + 3;
        }

        ShowSelection(newLocation);
    }
    void MoveRight()
    {
        int newLocation = 0;

        if (currentLocation + 1 >= selectable.Count)
        {
            newLocation = 0;
        }
        else
        {
            newLocation = currentLocation + 1;
        }

        ShowSelection(newLocation);
    }
    void MoveLeft()
    {
        int newLocation = 0;

        if (currentLocation - 1 < 0)
        {
            newLocation = selectable.Count - 1;
        }
        else
        {
            newLocation = currentLocation - 1;
        }

        ShowSelection(newLocation);
    }

    void ShowSelection(int newLocation)
    {
        selectable[currentLocation].GetComponent<Image>().color = baseColor;
        selectable[newLocation].GetComponent<Image>().color = selectedColor;

        if(currentLocation + 1 <= charactersAUGS.Count)
            selectable[currentLocation].GetComponentsInChildren<Image>()[1].color = baseColor;
        if (newLocation + 1 <= charactersAUGS.Count)
            selectable[newLocation].GetComponentsInChildren<Image>()[1].color = selectedColor;

        currentLocation = newLocation;
        SetDescription();
    }

    private void DetermineBG(int currentSlot)
    {
        selectable[currentSlot].GetComponentsInChildren<Image>()[1].color = baseColor;
        selectable[currentSlot].GetComponentsInChildren<Image>()[0].color = baseColor;

        if (currentSlot + 1 > charactersAUGS.Count)
        {
            selectable[currentSlot].GetComponentInChildren<TMP_Text>().text = null;
            selectable[currentSlot].GetComponentsInChildren<TMP_Text>()[1].text = null;
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[6];
            selectable[currentSlot].GetComponentsInChildren<Image>()[1].sprite = null;
            selectable[currentSlot].GetComponentsInChildren<Image>()[1].color = Color.clear;
            return;
        }


        //Colors fun
        if (!charactersAUGS[currentSlot].IsBuff && !charactersAUGS[currentSlot].IsDebuff && charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[0];

        else if(!charactersAUGS[currentSlot].IsBuff && !charactersAUGS[currentSlot].IsDebuff && !charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[1];

        else if (charactersAUGS[currentSlot].IsBuff && charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[2];

        else if(charactersAUGS[currentSlot].IsBuff && !charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[3];

        else if (charactersAUGS[currentSlot].IsDebuff && charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[4];

        else if (charactersAUGS[currentSlot].IsDebuff && !charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[5];
    }

    private void DetermineCharacterSlots()
    {
        actualCharacters.Clear();

        //Change icons and darken. Have to reverse the enemymanager lists to make this not shit itself
        List<PlayerCharacter> pc = EnemyManager.Instance.Players;
        pc.Reverse();
        foreach (PlayerCharacter playerCharacter in pc)
        {
            InitializeCharacterIcon(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[playerCharacter.CombatSpot], playerCharacter);
            actualCharacters.Add(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[playerCharacter.CombatSpot]);
        }

        List<PlayerCharacterSummon> pcs = EnemyManager.Instance.PlayerSummons;
        pcs.Reverse();
        foreach (PlayerCharacterSummon playerCharacterSummon in pcs)
        {
            InitializeCharacterIcon(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[playerCharacterSummon.CombatSpot], playerCharacterSummon);
            actualCharacters.Add(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[playerCharacterSummon.CombatSpot]);
        }

        List<EnemySummon> es = EnemyManager.Instance.EnemySummons;
        es.Reverse();
        foreach (EnemySummon enemySummon in es)
        {
            InitializeCharacterIcon(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[enemySummon.CombatSpot], enemySummon);
            actualCharacters.Add(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[enemySummon.CombatSpot]);
        }

        List<Enemy> e = EnemyManager.Instance.Enemies;
        e.Reverse();
        EnemyCorrection(e);
        foreach (Enemy enemy in e)
        {
            InitializeCharacterIcon(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[enemy.CombatSpot + 12], enemy);
            actualCharacters.Add(characterSlots.GetComponentsInChildren<AugmentCharacterIcon>()[enemy.CombatSpot + 12]);
        }

        //Select current character
        foreach (AugmentCharacterIcon icon in characterSlots.GetComponentsInChildren<AugmentCharacterIcon>())
            SetIconColor(icon);
    }

    //TODO probably needs to be totally reworked (LOL) to work with odd enemy layouts created by summoning and what not
    private void EnemyCorrection(List<Enemy> e)
    {
        Enemy tempEnemy;

        if(e.Count >= 8)
        {
            tempEnemy = e[7];
            e[7] = e[3];
            e[3] = tempEnemy;
            tempEnemy = e[6];
            e[6] = e[2];
            e[2] = tempEnemy;
            tempEnemy = e[5];
            e[5] = e[1];
            e[1] = tempEnemy;
            tempEnemy = e[4];
            e[4] = e[0];
            e[0] = tempEnemy;
        }
        else if(e.Count >= 7)
        {
            tempEnemy = e[6];
            e[6] = e[2];
            e[2] = tempEnemy;
            tempEnemy = e[5];
            e[5] = e[1];
            e[1] = tempEnemy;
            tempEnemy = e[4];
            e[4] = e[0];
            e[0] = tempEnemy;

            tempEnemy = e[3];
            e.RemoveAt(3);
            e.Add(tempEnemy);
        }
        else if(e.Count >= 6)
        {
            tempEnemy = e[2];
            e[2] = e[0];
            e[0] = tempEnemy;
            tempEnemy = e[3];
            e[3] = e[1];
            e[1] = tempEnemy;

            tempEnemy = e[5];
            e[5] = e[1];
            e[1] = tempEnemy;
            tempEnemy = e[4];
            e[4] = e[0];
            e[0] = tempEnemy;
        }
        else if(e.Count >= 5)
        {
            tempEnemy = e[4];
            e[4] = e[0];
            e[0] = tempEnemy;

            tempEnemy = e[4];
            Enemy tempEnemy2 = e[1];
            Enemy tempEnemy3 = e[2];
            Enemy tempEnemy4 = e[3];

            e[1] = tempEnemy;
            e[2] = tempEnemy2;
            e[3] = tempEnemy3;
            e[4] = tempEnemy4;
        }
    }

    private void SetIconColor(AugmentCharacterIcon icon)
    {
        if (!icon.MyImage)
        {
            icon.MyImage = icon.GetComponent<Image>();
            icon.MyImage.color = Color.gray;
        }
        else if (icon.MyCharacter == CombatManager.Instance.GetCurrentCharacter)
        {
            mainSlot.sprite = icon.BigImage;
            icon.MyImage.color = Color.white;

            for (int i = 0; i < actualCharacters.Count; i++)
                if (actualCharacters[i].MyCharacter == icon.MyCharacter)
                    currentCharacterLocation = i;
        }
    }

    private void InitializeCharacterIcon(AugmentCharacterIcon icon, Character character)
    {
        icon.MyImage = icon.GetComponent<Image>();
        icon.MyImage.sprite = character.MiniAugSprite;
        icon.BigImage = character.BigAugSprite;
        icon.MyImage.color = Color.gray;
        icon.MyCharacter = character;
    }

    private void SelectCharacter(bool left)
    {
        if(left)
        {
            StartCoroutine(ArrowFlicker(true));

            actualCharacters[currentCharacterLocation].MyImage.color = Color.gray;

            currentCharacterLocation--;

            if (currentCharacterLocation < 0)
                currentCharacterLocation = actualCharacters.Count - 1;

            actualCharacters[currentCharacterLocation].MyImage.color = Color.white;
            mainSlot.sprite = actualCharacters[currentCharacterLocation].BigImage;

            InitList(actualCharacters[currentCharacterLocation].MyCharacter, false);
        }
        else
        {
            StartCoroutine(ArrowFlicker(false));

            actualCharacters[currentCharacterLocation].MyImage.color = Color.gray;

            currentCharacterLocation++;

            if (currentCharacterLocation >= actualCharacters.Count)
                currentCharacterLocation = 0;

            actualCharacters[currentCharacterLocation].MyImage.color = Color.white;
            mainSlot.sprite = actualCharacters[currentCharacterLocation].BigImage;

            InitList(actualCharacters[currentCharacterLocation].MyCharacter, false);
        }
    }

    IEnumerator ArrowFlicker(bool left)
    {
        if (left)
            leftArrow.color = Color.black;
        else
            rightArrow.color = Color.black;

        yield return new WaitForSeconds(flickerTime);

        if (left)
            leftArrow.color = Color.white;
        else
            rightArrow.color = Color.white;
    }
}
