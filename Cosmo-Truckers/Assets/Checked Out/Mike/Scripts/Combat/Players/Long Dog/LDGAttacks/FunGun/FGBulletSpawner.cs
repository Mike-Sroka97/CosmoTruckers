using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGBulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawn;
    [SerializeField] private AnimationClip spawnAnimation;
    [SerializeField] private Vector2 force; 

    private Animator myAnimator;
    private CombatMove minigame; 

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        minigame = FindObjectOfType<CombatMove>();
    }

    public void SpawnBullet()
    {
        myAnimator.Play(spawnAnimation.name);
        GameObject myBullet = Instantiate(bullet, spawn.position, Quaternion.identity, minigame.transform);
        myBullet.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
