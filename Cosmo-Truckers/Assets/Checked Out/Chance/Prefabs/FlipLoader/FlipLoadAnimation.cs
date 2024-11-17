using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipLoadAnimation : MonoBehaviour
{
    [Header("Flip Tiles")]
    [SerializeField] float flipSpeed = 5;
    [SerializeField] Image[] flipTiles;
    [SerializeField] GameObject flipTileHolder;
    [SerializeField] float waitTime = 1f;

    [Header("Flip Images")]
    [SerializeField] Sprite[] flipSprites;

    int[] flipDirection = new int[20] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19 };
    bool flipping = false;

    public bool IsFlipping { get => flipping; }

    [ContextMenu("Test")]
    public void InitFlip()
    {
        SetUp();
        StartCoroutine(FlipAnimation());
    }

    void CleanUp()
    {
        foreach(var tile in flipTiles)
            tile.gameObject.SetActive(false);

        flipTileHolder.SetActive(false);
    }

    void SetUp()
    {
        flipping = true;
        flipTiles[flipTiles.Length - 1].gameObject.SetActive(true);

        MathHelpers.Shuffle(flipDirection);

        for(int i = 0; i< flipTiles.Length - 1; i++)
        {
            flipTiles[i].sprite = flipSprites[Random.Range(0, flipSprites.Length)];
            flipTiles[i].transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        flipTileHolder.SetActive(true);
    }

    IEnumerator FlipAnimation()
    {
        for (int i = 0; i < flipDirection.Length; i++)
        {
            flipTiles[flipDirection[i]].gameObject.SetActive(true);

            while (flipTiles[flipDirection[i]].transform.localRotation.eulerAngles.y > 0 && flipTiles[flipDirection[i]].transform.localRotation.eulerAngles.y < 91)
            {
                flipTiles[flipDirection[i]].transform.rotation = Quaternion.Euler(0, flipTiles[flipDirection[i]].transform.localRotation.eulerAngles.y - (Time.deltaTime * flipSpeed), 0);
                yield return null;
            }

            flipTiles[flipDirection[i]].transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //Setup combat refs
        DungeonController controller = FindObjectOfType<DungeonController>();
        CameraController.Instance.transform.position = new Vector3(controller.CombatCameraPosition.position.x, controller.CombatCameraPosition.position.y, CameraController.Instance.transform.position.z);

        //Combat over
        if(!CombatManager.Instance.InCombat)
        {
            CameraController.Instance.transform.position = CombatManager.Instance.LastCameraPosition;
            CameraController.Instance.Leader = CombatManager.Instance.DungeonCharacterInstance;

            //Reset shield and Mana
            foreach (PlayerVessel character in PlayerVesselManager.Instance.PlayerVessels)
            {
                character.MyCharacter.GetManaBase.ResetMana();
                character.ManuallySetShield(0);
            }
            //clean up enemy summons
            while (EnemyManager.Instance.EnemySummons.Count > 0)
                EnemyManager.Instance.EnemySummons[0].Die();
        }
        //Wait while screen changes
        yield return new WaitForSeconds(waitTime);

        for (int i = flipDirection.Length - 1; i >= 0; i--)
        {
            while (flipTiles[flipDirection[i]].transform.localRotation.eulerAngles.y < 90)
            {
                flipTiles[flipDirection[i]].transform.rotation = Quaternion.Euler(0, flipTiles[flipDirection[i]].transform.localRotation.eulerAngles.y + (Time.deltaTime * flipSpeed), 0);
                yield return null;
            }

            flipTiles[flipDirection[i]].transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        flipping = false;

        CleanUp();
    }
}
