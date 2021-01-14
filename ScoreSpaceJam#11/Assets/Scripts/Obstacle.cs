using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class Obstacle : MonoBehaviour
{
    public float Radius;
    public bool IsPlayer;
    public int LoseAmount;
    public LayerMask mask;
    Animator anim;

    bool isDestroyed;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(!isDestroyed) IsPlayer = Physics2D.OverlapCircle(transform.position, Radius, mask);
        if (IsPlayer)
        {
            CameraShaker.Instance.ShakeOnce(4f,1f,1f,1f);
            anim.Play("Destroy");
            FindObjectOfType<Player>().LoseHealth(LoseAmount);
            FindObjectOfType<SoundManger>().Play("LoseLife2");
            isDestroyed = true;
            IsPlayer = false;
        }
    }





}
