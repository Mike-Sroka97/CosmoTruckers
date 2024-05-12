using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixFaceMana : Mana
{
    public enum FaceTypes
    { 
        Smug,
        Megalomanic,
        Sad,
        Hype,
        Dizzy,
        Money
    }

    public FaceTypes FaceType;

    public override void CheckCastableSpells()
    {
        if (Tutorial)
        {
            TutorialCheckCastableSpells();
        }
        else if (freeSpells)
        {
            foreach (SixFaceAttackSO attack in attacks)
            {
                attack.CanUse = true;
            }
        }
        else
        {
            foreach (SixFaceAttackSO attack in attacks)
            {
                if(!attack.RequiresMegaloManic || (attack.RequiresMegaloManic && FaceType == FaceTypes.Megalomanic))
                    attack.CanUse = true;
                else if(attack.RequiresMegaloManic && FaceType != FaceTypes.Megalomanic)
                    attack.CanUse = false;
            }
        }
    }

    public void UpdateFace()
    {
        SixFaceAttackSO attack = (SixFaceAttackSO)CombatManager.Instance.CurrentAttack;
        SixFaceVessel vessel = MyVessel.GetComponent<SixFaceVessel>();

        if (FaceType == FaceTypes.Megalomanic)
        {
            FaceType = FaceTypes.Smug;
            vessel.ClearFaceBG();
            vessel.UpdateFace(FaceTypes.Smug);
        }
        else
        {
            FaceType = attack.faceType;
            vessel.UpdateFace(attack.faceType);
            if (vessel.CheckMegalomanicMode())
                FaceType = FaceTypes.Megalomanic;
        }
    }

    public override void SetMaxMana()
    {
        SixFaceVessel vessel = MyVessel.GetComponent<SixFaceVessel>();

        vessel.UpdateFace(FaceTypes.Smug);
        vessel.UpdateFace(FaceTypes.Sad);
        vessel.UpdateFace(FaceTypes.Hype);
        vessel.UpdateFace(FaceTypes.Dizzy);
        vessel.UpdateFace(FaceTypes.Money);

        if (vessel.CheckMegalomanicMode())
            FaceType = FaceTypes.Megalomanic;
    }
}
