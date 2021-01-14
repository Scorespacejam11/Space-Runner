using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float Radius;
    public bool IsPlayer;

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
            anim.Play("Destroy");
            FindObjectOfType<GameManger>().Score += 1000;
            FindObjectOfType<SoundManger>().Play("Tp");
            FindObjectOfType<GameManger>().ChangeDimension();
            isDestroyed = true;
            IsPlayer = false;
        }
    }
}

