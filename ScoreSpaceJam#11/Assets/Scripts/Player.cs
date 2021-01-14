using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
public enum MoveState
{
    None,
    MoveLeft,
    MoveRight
}
public class Player : MonoBehaviour
{
    public float MoveDistance;
    public float MoveSpeed;
    int playerPosition = 1;
    public Vector3 OldPos;
    public Transform RightMove, LeftMove;
    public MoveState state;
    public Vector2 test;

    public int Healthpoints = 3;

    public Transform PlayerParent;

    public static bool IsDead;

    Animator anim;

    public Color LoseHealthcol;

    public float TimeToLoseRate;

    public float GameRateInInspector;

    Vignette vig;

    public PostProcessVolume volume;

    public static bool IsHit;


    private void Awake()
    {
        IsDead = false;
        IsHit = false;
    }
    void Start()
    {
        volume.profile.TryGetSettings(out vig);
        anim = GetComponent<Animator>();
        anim.SetInteger("HealthPoints", Healthpoints);
       
    }

    // Update is called once per frame

    private void Update()
    {
        if(!GameManger.isTeleporting) Move();

        GameRateInInspector = VerticalMove.GameRate;
        if(Healthpoints <= 1)
        {
            vig.color.value = Color.Lerp(vig.color.value, Color.red, 2 * Time.deltaTime);
        }
        if (Healthpoints >= 2)
        {
            vig.color.value = Color.Lerp(vig.color.value, Color.black, 2 * Time.deltaTime);
        }

    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.D) && state == MoveState.None)
        {
            if (playerPosition < 2)
            {
                OldPos = RightMove.position;
                state = MoveState.MoveRight;
                playerPosition++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) && state == MoveState.None)
        {
            if (playerPosition > 0)
            {
                OldPos = LeftMove.position;
                state = MoveState.MoveLeft;
                playerPosition--;
            }
        }

        if (state == MoveState.MoveRight)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(OldPos.x,PlayerParent.position.y + 1.16f, 0f), MoveSpeed * Time.deltaTime); ;
            if (Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(OldPos.x, 0f, 0f) ) < 0.08f) state = MoveState.None;
        }
        else if (state == MoveState.MoveLeft)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(OldPos.x, PlayerParent.position.y + 1.16f, 0f), MoveSpeed * Time.deltaTime);
            if (Vector3.Distance(new Vector3(transform.position.x, 0f, 0f), new Vector3(OldPos.x, 0f, 0f)) < 0.08f) state = MoveState.None;
        }
    }

    public void LoseHealth(int loseamount)
    {
        anim.SetTrigger("HitTrigger");
        Healthpoints -= loseamount;
        anim.SetInteger("HealthPoints", Healthpoints);
        float oldgamerate = VerticalMove.GameRate;
        StartCoroutine(SlowRateWhenLoseHealth(oldgamerate));
        if (Healthpoints <= 0 && !IsDead)
        {
            FindObjectOfType<GameManger>().EndGame();
            IsDead = true;
            FindObjectOfType<SoundManger>().StopMusic();
        }
    }
       


    public void GainHealth(int healthgain)
    {
        if (Healthpoints == 2) healthgain = 1;
        if (Healthpoints < 3)
        {
            Healthpoints += healthgain;
            anim.SetInteger("HealthPoints", Healthpoints);
        }
    }

    IEnumerator SlowRateWhenLoseHealth(float OldRate)
    {
        bool Loopdone = false;
        while (true)
        {
            IsHit = true;
            if(!Loopdone)VerticalMove.GameRate = Mathf.Lerp(VerticalMove.GameRate, 2f, TimeToLoseRate * Time.deltaTime);
            bool Lerping = Mathf.Abs(VerticalMove.GameRate - 2) > 0.05f && !Loopdone;

            //print(Lerping);
            if (Lerping) yield return null;
            else
            {
                yield return new WaitWhile(() => Lerping);
                Loopdone = true;
                VerticalMove.GameRate = Mathf.Lerp(VerticalMove.GameRate, OldRate, TimeToLoseRate * 3 * Time.deltaTime);
                bool secondLerpDone = Mathf.Abs(VerticalMove.GameRate - OldRate) > 0.05f;
                if (!secondLerpDone)
                {
                    IsHit = false;
                    break;
                }
            }
        }
        yield return null;
    }



    //IEnumerator MoveTodir(bool MoveToLeft)
    //{
    //    isMoving = true;
    //    while (true)
    //    {
    //        if (MoveToLeft)
    //        {
    //            if (Mathf.RoundToInt(transform.position.x) == Mathf.RoundToInt(LeftMove)) { print("done"); isMoving = false; break; }
    //            float MoveS = Mathf.Lerp(transform.position.x, LeftMove, MoveSpeed * Time.deltaTime);
    //            transform.position = new Vector3(MoveS, -18.391f, 8f);
    //        }
    //        else
    //        {
    //            if (Mathf.RoundToInt(transform.position.x) == Mathf.RoundToInt(RightMove)) { print("done"); isMoving = false; break; }
    //            float MoveS = Mathf.Lerp(transform.position.x, RightMove, MoveSpeed * Time.deltaTime);
    //            transform.position = new Vector3(MoveS, -18.391f, 8f);
    //        }
    //        yield return new WaitUntil(() => isMoving);
    //    }
    //}
}
