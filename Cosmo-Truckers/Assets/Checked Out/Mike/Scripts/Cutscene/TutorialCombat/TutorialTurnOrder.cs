using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTurnOrder : TurnOrder
{
    int turn = 13;
    AeglarCharacter aeglar;
    SafeTCharacter safeT;
    ProtoCharacter proto;
    SixFaceCharacter sixFace;
    List<Enemy> malites = new List<Enemy>();

    [SerializeField] private List<TextAsset> inaDialogs;
    int inaDialogCounter = 0;
    float dialogSetupTime = 0.5f; 
    RegularTextManager textManager;

    [SerializeField] string sceneToLoad;

    private void Start()
    {
        textManager = FindObjectOfType<RegularTextManager>();
    }

    protected override void StartTurn()
    {
        if (!aeglar)
            SetCharacters();

        StartCoroutine(HandleTurnTutorial());
    }

    private void SetCharacters()
    {
        //Grab ya boys
        aeglar = FindObjectOfType<AeglarCharacter>();
        safeT = FindObjectOfType<SafeTCharacter>();
        proto = FindObjectOfType<ProtoCharacter>();
        sixFace = FindObjectOfType<SixFaceCharacter>();

        aeglar.GetManaBase.Tutorial = true;
        safeT.GetManaBase.Tutorial = true;
        proto.GetManaBase.Tutorial = true;
        sixFace.GetManaBase.Tutorial = true;

        //Grab ya enemies
        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            if (enemy.CharacterName == "Malite")
                malites.Add(enemy);
    }

    IEnumerator HandleTurnTutorial()
    {
        turn++;

        switch (turn)
        {
            case 1:
                //INA goes BLAH BLAH
                //yield return new WaitForSeconds(2f); 

                //textManager.StartRegularTextMode(inaDialogs[inaDialogCounter]);
                //inaDialogCounter++;

                //yield return new WaitForSeconds(dialogSetupTime); 

                //while (textManager.DialogIsPlaying)
                //    yield return null;

                aeglar.GetManaBase.SetMaxMana();
                aeglar.GetManaBase.TutorialAttackName = "Porkanator";
                aeglar.SelectionUI.DisableButton(2);
                aeglar.SelectionUI.DisableButton(3);
                aeglar.StartTurn();
                //INA listener to yap when attack wheel opened "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                //INA listener to yap when attack is selected "PlayerCharacter.PlayerAttackUI.AttackSelected.AddListener(cum and piss and shit domain)"
                //INA listener to yap when attack starts (LOL) => Set INACombat.HoldCountDown to true. Then set to false once INA finishes shitting and cumming (INACombat.AttackStarted.AddListener(cum and piss and cum)
                break;
            case 2:
                //INA yaps
                aeglar.SelectionUI.EnableAllButtons();
                proto.GetManaBase.TutorialAttackName = "Electro-Whip";
                proto.SelectionUI.DisableButton(1);
                proto.SelectionUI.DisableButton(3);
                //INA listener for Aug list open (TODO: reenable combat button) "PlayerCharacter.AUGListOpened.AddListener(PlayerCharacter.SelectionUI.EnableButton(1))
                proto.StartTurn();
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];
                //INA listener to yap when attack wheel is open "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                //INA listener to yap when attack starts (LOL) => Set INACombat.HoldCountDown to true. Then set to false once INA finishes shitting and cumming (INACombat.AttackStarted.AddListener(cum and piss and cum)
                break;
            case 3:
                proto.SelectionUI.EnableAllButtons();
                //INA yaps about taunting from last turn at this point
                sixFace.GetManaBase.TutorialAttackName = "Petty Theft";
                sixFace.SelectionUI.DisableButton(1);
                sixFace.SelectionUI.DisableButton(2);
                //INA listener for Insight open (TODO: reenable combat button and aug list) => use PlayerCharacter.SelectionUI.EnableAllButtons()
                sixFace.StartTurn();
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[0];
                //INA listener to yap when attack wheel is open "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                //INA listener to yap when attack starts (LOL) => Set INACombat.HoldCountDown to true. Then set to false once INA finishes shitting and cumming (INACombat.AttackStarted.AddListener(cum and piss and cum)
                break;
            case 4:
                sixFace.SelectionUI.EnableAllButtons();
                safeT.GetManaBase.TutorialAttackName = "Clock Out";
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];
                safeT.StartTurn();
                //INA listener to yap when attack wheel is open "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                //INA listener to yap when attack starts (LOL) => Set INACombat.HoldCountDown to true. Then set to false once INA finishes shitting and cumming (INACombat.AttackStarted.AddListener(cum and piss and cum)
                break;
            case 5:
                malites[0].CurrentTargets.Clear();
                malites[0].CurrentTargets.Add(proto);
                malites[0].StartTurn();
                //INA listener to yap when attack starts (LOL) => Set INACombat.HoldCountDown to true. Then set to false once INA finishes shitting and cumming (INACombat.AttackStarted.AddListener(cum and piss and cum)
                break;
            case 6:
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(safeT);
                malites[1].StartTurn();
                break;
            case 7:
                aeglar.GetManaBase.TutorialAttackName = "Veggie Vengeance";
                CombatManager.Instance.MyTargeting.ForcedTarget = FindObjectOfType<ProtoCharacter>();
                aeglar.StartTurn();
                //INA listener to yap when attack wheel is open "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                break;
            case 8:
                proto.GetManaBase.TutorialAttackName = "Spark Shield";
                proto.StartTurn();
                //INA listener to yap when attack wheel is open"PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                break;
            case 9:
                sixFace.GetManaBase.TutorialAttackName = "Bribery";
                sixFace.StartTurn();
                //INA listener to yap when attack wheel is open "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                break;
            case 10:
                safeT.GetManaBase.SetMaxMana();
                safeT.GetManaBase.TutorialAttackName = "Power Pummel";
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];
                safeT.StartTurn();
                //INA listener to yap when attack wheel is open (TODO make the safeT.GetManaBase.SetMaxMana(); call here after INA brings attention to it) "PlayerCharacter.AttackWheelOpened.AddListener(piss and shit domain);"
                break;
            case 11:
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(aeglar);
                malites[1].StartTurn();
                break;
            case 12:
                CombatManager.Instance.MyTargeting.ForcedTarget = FindObjectOfType<AeglarCharacter>();
                aeglar.StartTurn();
                break;
            case 13:
                proto.GetManaBase.TutorialAttackName = "Energon Jab";
                proto.StartTurn();
                break;
            case 14:
                StartCoroutine(FindObjectOfType<MaliceAI>().Fall());
                break;
            case 15:
                //add Malice dance
                yield return new WaitForSeconds(10);
                CameraController camera = FindObjectOfType<CameraController>();
                StartCoroutine(camera.FadeVignette(false));
                while (camera.CommandsExecuting > 0)
                    yield return null;
                //Fade music
                yield return new WaitForSeconds(5);
                SceneManager.LoadScene(sceneToLoad);
                break;
            default:
                break;
        }

        yield return null;
    }
}
