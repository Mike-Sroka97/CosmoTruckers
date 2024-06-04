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
    [SerializeField] GameObject malice;
    [SerializeField] Transform maliceSpawn; 

    INATalker MyINATalker;
    INAcombat MyINACombat;

    [SerializeField] private List<TurnDialog> turnDialogs;

    float dialogSetupTime = 0.5f;
    float turnStartWaitTime = 1.5f;

    PlayerCharacter currentCharacter = null;
    MaliceAI maliceCharacter; 

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
        MyINATalker.GetTextManager(); 
    }

    IEnumerator HandleTurnTutorial()
    {
        turn++;

        switch (turn)
        {
            // If you want to skip dialog, comment out "NewTurnStartup" and listener for "SetHoldCountDownTrue"
            //Aeglar
            case 1:
                NewTurnStartup(0); 

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
                //INA listener to yap when attack is selected. Explain targeting here and player shouldn't be able to target (3)
                aeglar.PlayerAttackUI.AttackSelected.AddListener(MyINATalker.INAStartNextDialog);
                //INA listener to yap when attack starts and set HoldDownCount to true (4)
                AttackStartedAddListeners(); 
                break;
            //Proto
            case 2:
                NewTurnStartup(1);

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

                //INA listener for Aug list open (TODO: reenable combat button)
                proto.AUGListOpenedEvent.AddListener(EnableCurrentPlayerAttackUI);

                //INA listener to yap when attack wheel opened (2)
                proto.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);

                //INA Listener to yap when aug list is closed (3) 
                proto.AUGListClosedEvent.AddListener(AugListClosedStartDialog);

                //INA Listener to yap when attack is selected (4) 
                proto.PlayerAttackUI.AttackSelected.AddListener(MyINATalker.INAStartNextDialog);

                //INA listener to yap when attack starts and set HoldDownCount to true (5)
                AttackStartedAddListeners();
                break;
            //Six-face
            case 3:
                NewTurnStartup(2);

                proto.SelectionUI.EnableAllButtons();

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                //INA yaps about taunting from last turn at this point (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                sixFace.GetManaBase.TutorialAttackName = "Petty Theft";
                sixFace.SelectionUI.DisableButton(1);
                sixFace.SelectionUI.DisableButton(2);
                currentCharacter = sixFace;

                // Enable all buttons after Insight is open
                sixFace.InsightOpenedEvent.AddListener(sixFace.SelectionUI.EnableAllButtons);

                //fuggin set the enemy intentions so they aren't wrong :cheems:
                malites[0].CurrentTargets.Clear();
                malites[0].CurrentTargets.Add(proto);
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(safeT);

                //INA listener for Insight open and yaps (2)
                sixFace.InsightOpenedEvent.AddListener(InsightOpenStartDialog); 

                sixFace.StartTurn();
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[0];

                //INA listener to yap when attack wheel is open. Explain mana and tell Six Face to target bottom enemy (3)
                sixFace.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);

                //INA listener to yap when attack starts. Explain Six Face movement, wind, and down attack (4)
                AttackStartedAddListeners();
                break;
            //Safe-T
            case 4:
                NewTurnStartup(3);

                sixFace.SelectionUI.EnableAllButtons();
                safeT.GetManaBase.TutorialAttackName = "Clock Out";
                currentCharacter = safeT;

                //set malite intention for insight
                malites[0].CurrentTargets.Clear();
                malites[0].CurrentTargets.Add(sixFace);
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(aeglar);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[0];
                safeT.StartTurn();

                //INA listener to yap about mana and using COKO on top Malite (2)
                safeT.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);

                //INA listener to yap when attack starts (3)
                AttackStartedAddListeners();
                break;
            //Malite 1
            case 5:
                NewTurnStartup(4);
                currentCharacter = proto; 
                malites[0].StartTurn();
                //INA listener to yap when attack starts (1)
                AttackStartedAddListeners();
                break;
            //Malite 2
            case 6:
                RemoveCurrentListeners(); 
                currentCharacter = safeT; 
                malites[1].StartTurn();
                break;
            //Aeglar
            case 7:
                NewTurnStartup(5);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                currentCharacter = aeglar; 
                aeglar.GetManaBase.TutorialAttackName = "Veggie Vengeance";
                CombatManager.Instance.MyTargeting.ForcedTarget = FindObjectOfType<ProtoCharacter>();
                aeglar.StartTurn();
                //INA listener to yap when attack wheel is open (2)
                aeglar.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);
                break;
            //Proto
            case 8:
                NewTurnStartup(6);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                currentCharacter = proto;
                proto.GetManaBase.TutorialAttackName = "Spark Shield";
                proto.StartTurn();
                //INA listener to yap when attack wheel is open (2)
                proto.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);
                break;
            //Six-face
            case 9:
                NewTurnStartup(7);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                currentCharacter = sixFace;
                sixFace.GetManaBase.TutorialAttackName = "Bribery";
                sixFace.StartTurn();
                //INA listener to yap when attack wheel is open (2)
                sixFace.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);
                break;
            //Safe-T
            case 10:
                NewTurnStartup(8);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                currentCharacter = safeT;
                safeT.GetManaBase.SetMaxMana();
                safeT.GetManaBase.TutorialAttackName = "Power Pummel";
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[0];
                safeT.StartTurn();
                //INA listener to yap when attack wheel is open (2)
                safeT.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);
                break;
            //Malite 2
            case 11:
                NewTurnStartup(9);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                malites[1].StartTurn();
                break;
            //Aeglar
            case 12:
                NewTurnStartup(10);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                CombatManager.Instance.MyTargeting.ForcedTarget = FindObjectOfType<AeglarCharacter>();
                aeglar.StartTurn();
                //INA listener to yap when attack wheel is open (2)
                aeglar.AttackWheelOpenedEvent.AddListener(AttackWheelOpenStartDialog);
                break;
            //Proto
            case 13:
                NewTurnStartup(11);

                // Wait to Blah Blah
                yield return new WaitForSeconds(turnStartWaitTime);

                // INA goes BLAH BLAH (1)
                MyINATalker.INAStartNextDialog();

                yield return new WaitForSeconds(dialogSetupTime);

                while (MyINATalker.DialogPlaying())
                    yield return null;

                proto.GetManaBase.TutorialAttackName = "Energon Jab";
                proto.StartTurn();
                break;
            //Malice
            case 14:
                malites[1].Die(); 
                maliceCharacter = Instantiate(malice, maliceSpawn).GetComponent<MaliceAI>(); 
                StartCoroutine(FindObjectOfType<MaliceAI>().Fall());
                break;
            //Malice
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

    private void NewTurnStartup(int turnDialogNumber)
    {
        MyINATalker.SetupINATalker(turnDialogs[turnDialogNumber].inaTurnDialogs, turnDialogs[turnDialogNumber].textBoxPositions);

        if (currentCharacter != null)
            RemoveCurrentListeners(); 
    }
    private void DialogStartedDefaultCall()
    {
        PlayerCharacter roundCharacter = CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>();

        if (roundCharacter != null)
            roundCharacter.RevokeControls = true;

        else
            currentCharacter.RevokeControls = true; 
    }
    private void DialogEndedDefaultCall()
    {
        MyINACombat.HoldCountDown = false;
        PlayerCharacter roundCharacter = CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>();

        if (roundCharacter != null)
            roundCharacter.RevokeControls = false;
        else
            currentCharacter.RevokeControls = false;
    }
    private void AttackStartedAddListeners()
    {
        MyINACombat.AttackStarted.AddListener(SetHoldCountDownTrue);
        MyINACombat.AttackStarted.AddListener(MyINATalker.INAStartNextDialog);
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
    private void RemoveCurrentListeners()
    {
        MyINACombat.AttackStarted.RemoveAllListeners();

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
    private void InsightOpenStartDialog()
    {
        currentCharacter.InsightOpenedEvent.RemoveAllListeners();
        MyINATalker.INAStartNextDialog();
    }
}

[System.Serializable]
public struct TurnDialog
{
    public List<TextAsset> inaTurnDialogs;
    public List<Transform> textBoxPositions;
}
