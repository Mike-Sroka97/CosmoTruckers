using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTurnOrder : TurnOrder
{
    int turn = 0;
    AeglarCharacter aeglar;
    SafeTCharacter safeT;
    ProtoCharacter proto;
    SixFaceCharacter sixFace;
    List<Enemy> malites = new List<Enemy>();

    [SerializeField] string sceneToLoad;

    INATalker MyINATalker;
    INAcombat MyINACombat;

    [SerializeField] private List<TextAsset> inaDialogs;
    [SerializeField] private List<Transform> textBoxPositions; 
    float dialogSetupTime = 0.5f;
    float turnStartWaitTime = 1.5f;

    PlayerCharacter currentCharacter = null; 

    protected override void StartTurn()
    {
        if (!aeglar)
            SetCharacters();

        if (turn == 0)
            GetINAScripts();

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

    private void GetINAScripts()
    {
        MyINACombat = FindObjectOfType<INAcombat>(); 
        MyINATalker = FindObjectOfType<INATalker>();
        MyINATalker.SetupINATalker(inaDialogs, textBoxPositions);
    }

    IEnumerator HandleTurnTutorial()
    {
        turn++;

        switch (turn)
        {
            case 1:
                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog(); 

                yield return new WaitForSeconds(dialogSetupTime); 

                while (MyINATalker.DialogPlaying())
                    yield return null;

                aeglar.GetManaBase.SetMaxMana();
                aeglar.GetManaBase.TutorialAttackName = "Porkanator";
                aeglar.SelectionUI.DisableButton(2);
                aeglar.SelectionUI.DisableButton(3);
                currentCharacter = aeglar;
                aeglar.StartTurn();

                MyINATalker.textManager.DialogStarted.AddListener(DialogStartedDefaultCall);
                MyINATalker.textManager.DialogEnded.AddListener(DialogEndedDefaultCall);

                //INA listener to yap when attack wheel opened (2)
                aeglar.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);
                //INA listener to yap when attack is selected.
                //Explain targeting here and player shouldn't be able to target (3)
                aeglar.PlayerAttackUI.AttackSelected.AddListener(MyINATalker.INAStartNextDialog);
                //INA listener to yap when attack starts and set HoldDownCount to true (4)
                MyINACombat.AttackStarted.AddListener(SetHoldCountDownTrue);
                MyINACombat.AttackStarted.AddListener(MyINATalker.INAStartNextDialog);
                //Then set to false once INA finishes shitting and cumming
                break;
            case 2:
                // Remove Aeglar's current listeners. Aeglar is still the current player.
                RemoveCurrentPlayerListeners(); 
                aeglar.SelectionUI.EnableAllButtons();

                proto.GetManaBase.TutorialAttackName = "Electro-Whip";
                proto.SelectionUI.DisableButton(1);
                proto.SelectionUI.DisableButton(3);
                currentCharacter = proto;

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                proto.StartTurn();
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];

                MyINATalker.textManager.DialogStarted.AddListener(DialogStartedDefaultCall);
                MyINATalker.textManager.DialogEnded.AddListener(DialogEndedDefaultCall);

                //INA listener for Aug list open (TODO: reenable combat button)
                proto.AUGListOpenedEvent.AddListener(EnableCurrentPlayerAttackUI);

                //INA listener to yap when attack wheel opened (2)
                proto.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);

                //INA Listener to yap when aug list is closed (3) 
                proto.AUGListClosedEvent.AddListener(AugListClosedStartDialog);

                //INA Listener to yap when attack is selected (4) 
                proto.PlayerAttackUI.AttackSelected.AddListener(MyINATalker.INAStartNextDialog);

                //INA listener to yap when attack starts and set HoldDownCount to true (5)
                MyINACombat.AttackStarted.AddListener(SetHoldCountDownTrue);
                MyINACombat.AttackStarted.AddListener(MyINATalker.INAStartNextDialog);
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

    private void DialogStartedDefaultCall()
    {
        CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>().RevokeControls = true;
    }
    private void DialogEndedDefaultCall()
    {
        MyINACombat.HoldCountDown = false;
        CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>().RevokeControls = false; 
    }
    private void SetHoldCountDownTrue()
    {
        MyINACombat.HoldCountDown = true;
    }
    private void EnableCurrentPlayerAttackUI()
     {
        if (currentCharacter != null)
            currentCharacter.SelectionUI.EnableButton(1); 
    }
    private void RemoveCurrentPlayerListeners()
    {
        if (currentCharacter != null)
        {
            currentCharacter.AttackWheelOpenedEvent.RemoveAllListeners();
            currentCharacter.AUGListOpenedEvent.RemoveAllListeners();
            currentCharacter.AUGListClosedEvent.RemoveAllListeners();
            currentCharacter.PlayerAttackUI.AttackSelected.RemoveAllListeners(); 
        }
    }
    
    // The following methods are going to fire off the dialog and then remove the listeners from the event
    private void AttackWheelOpenStartDialog()
    {
        currentCharacter.AttackWheelOpenedEvent.RemoveAllListeners();
        MyINATalker.INAStartNextDialogWithNewTextPosition();
    }
    private void AugListClosedStartDialog()
    {
        currentCharacter.AUGListClosedEvent.RemoveAllListeners();
        MyINATalker.INAStartNextDialog();
    }
}
