using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float Radius;
    public bool IsPlayer;
    public int healthGain;
    public LayerMask mask;
    Animator anim;

    bool isDestroyed;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!isDestroyed) IsPlayer = Physics2D.OverlapCircle(transform.position, Radius, mask);
        if (IsPlayer)
        {
            FindObjectOfType<SoundManger>().Play("Heal");
            anim.Play("Destroy");
            FindObjectOfType<Player>().GainHealth(healthGain);
            isDestroyed = true;
            IsPlayer = false;
        }
    }
}
