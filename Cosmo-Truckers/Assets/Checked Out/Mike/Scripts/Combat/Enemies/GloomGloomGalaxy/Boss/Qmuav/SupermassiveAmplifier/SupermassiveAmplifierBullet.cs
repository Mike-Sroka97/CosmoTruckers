using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermassiveAmplifierBullet : QmuavProjectile
{
    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector2.right * maxVelocity * Time.deltaTime);
    }
}
