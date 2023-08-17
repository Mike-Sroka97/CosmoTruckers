using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class WalkingNoicePassive : EnemyPassiveBase
{
    public override void Activate(int val) 
    {
        GameObject spawn = Instantiate(this.gameObject, GameObject.Find("-----Enemies -----").transform);
        spawn.transform.position = new Vector3(this.gameObject.transform.position.x - 1.5f, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        spawn.GetComponent<Enemy>().Health = val == 1 ? 1 : (int)(val / 2);

        FindObjectOfType<EnemyManager>().Enemies.Add(spawn.GetComponent<Enemy>());
    }
}
