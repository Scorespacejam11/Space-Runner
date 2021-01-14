using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManger : MonoBehaviour
{
    // Start is called before the first frame update
    public int Phase;
    public float TimeToChangePhase;
    public Animator Run;
    public int Score;
    public Text ScoreText;

    public GameObject[] Chunks;

    public List<GameObject> AddedChunks;

    public Transform Player;

    public Transform NextChunckPos;

    public float DistanceBetweenEachChunck = 350f;

    public bool DestroyedLast;

    public Animator fade;

    public GameObject ScoreBoard;

    public float[] weights;

    float ScoreIncrease = 0.11f;


    public Sprite[] mainSprite;

    public Sprite[] borderSprite;

    private int DesiredDimensionIndex;

    public GameObject TpPanel;



    public static bool isTeleporting;
    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(GamePhase());
        StartCoroutine(UpdateScore());
        DestroyedLast = true;
        ChangeDimension();
    }

    private void Update()
    {
        ScoreText.text = Score.ToString();
        if (Vector3.Distance(Player.position, NextChunckPos.position) < DistanceBetweenEachChunck)
        {
            AddChunk();
        }
    }

    public void EndGame()
    {
        FindObjectOfType<SoundManger>().Play("EndGame");
        Time.timeScale = 0;
        fade.Play("FadeIn");
        ScoreBoard.SetActive(true);
        FindObjectOfType<SoundManger>().StopMusic();
    }


    void AddChunk()
    {
        GameObject chunk = Instantiate(Chunks[0], new Vector3(NextChunckPos.position.x, NextChunckPos.position.y + Player.position.y, NextChunckPos.position.z), Quaternion.identity);
        Transform mainparent = chunk.transform.Find("Ground");
        Transform borderparent =  chunk.transform.Find("Borders");
        NextChunckPos.position += new Vector3(0f, 104.4f, 0f);
        if (borderparent != null && mainparent != null)
        {
            
           Transform[] AllMainSprites = new Transform[21];
           Transform[] AllBorderSprites = new Transform[2];

            for (int i = 0; i < AllBorderSprites.Length; i++)
            {
                AllBorderSprites[i] = borderparent.Find("Border" + (i + 1));
                AllBorderSprites[i].gameObject.GetComponent<SpriteRenderer>().sprite = borderSprite[DesiredDimensionIndex];
            }
            for (int i = 0; i < AllMainSprites.Length; i++)
            {
                AllMainSprites[i] = mainparent.Find("Back" + (i + 1));
                AllMainSprites[i].gameObject.GetComponent<SpriteRenderer>().sprite = mainSprite[DesiredDimensionIndex];
            }
        }
        AddedChunks.Add(chunk);
        
        DestroyedLast = !DestroyedLast;
        if (DestroyedLast)
        {
            Destroy(AddedChunks[0]);
            AddedChunks.RemoveAt(0);
        }
        
    }

    IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(ScoreIncrease);
            Score += 1;
        }
    }

    IEnumerator GamePhase()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToChangePhase);
            if (Phase < 10 && !isTeleporting)
            {
                Run.speed += 0.2f;
                ScoreIncrease -= 0.01f;
                FindObjectOfType<SpawnItems>().TimeBetweenEachSpawn -= 0.2f;
                VerticalMove.GameRate+=3;
                Phase++;
            }
        }
    }
    public void ChangeDimension()
    {
        float OldRate = VerticalMove.GameRate;
        VerticalMove.GameRate = 0f;
        TpPanel.SetActive(true);
        isTeleporting = true;
        StartCoroutine(BacktoNormal(OldRate));
        StartCoroutine(disablefade());
        int OldDimensionIndex = DesiredDimensionIndex;
        DesiredDimensionIndex = Random.Range(0,borderSprite.Length);
        if(DesiredDimensionIndex == OldDimensionIndex) DesiredDimensionIndex = Random.Range(0, borderSprite.Length);
        for (int i = 0; i < AddedChunks.Count; i++)
        {
            Transform mainparent = AddedChunks[i].transform.Find("Ground");
            Transform borderparent = AddedChunks[i].transform.Find("Borders");
            Transform[] AllMainSprites = new Transform[21];
            Transform[] AllBorderSprites = new Transform[2];

            for (int j = 0; j < AllBorderSprites.Length; j++)
            {
                AllBorderSprites[j] = borderparent.Find("Border" + (j+1));
                AllBorderSprites[j].gameObject.GetComponent<SpriteRenderer>().sprite = borderSprite[DesiredDimensionIndex];

            }
            for (int k = 0; k < AllMainSprites.Length; k++)
            {
                AllMainSprites[k] = mainparent.Find("Back" + (k+1));
                AllMainSprites[k].gameObject.GetComponent<SpriteRenderer>().sprite = mainSprite[DesiredDimensionIndex];
            }
        }
    }

    IEnumerator BacktoNormal(float OldRate)
    {
        yield return new WaitForSeconds(2f);

        isTeleporting = false;
        VerticalMove.GameRate = OldRate;
        
    }

    IEnumerator disablefade()
    {
        yield return new WaitForSeconds(3f);
        TpPanel.SetActive(false);
    }
    public int GetRandomWeightedIndex()
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }
}
