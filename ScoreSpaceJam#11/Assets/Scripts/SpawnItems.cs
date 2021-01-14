using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeleportChunk
{
    public Sprite mainroadsprite;
    public Sprite bordersprite;
}
public class SpawnItems : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] SpawnPrefab;
    public Transform[] Lanes;

    public float TimeBetweenEachSpawn;

    public float[] weights;


    public Transform ItemParent;

    float StartTime = 0.3f;


    void Start()
    {
        StartCoroutine(StartSpawnItems());
    }

    IEnumerator StartSpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(StartTime,TimeBetweenEachSpawn));
            if (!GameManger.isTeleporting && !Player.IsHit)
            {
                StartTime = 0.3f;
                Vector3 RandomizedNumber = Vector3.zero;
                for (int i = 0; i < Random.Range(1, 3); i++)
                {
                    Vector3 randLane = RandomizedLane();
                    if (i == 0) RandomizedNumber = randLane;
                    while (randLane == RandomizedNumber && i != 0)
                    {
                        print("IDK");
                        StartTime = 1f;
                        randLane = RandomizedLane();
                    }
                    int rand = GetRandomWeightedIndex();


                    GameObject spawnObject = Instantiate(SpawnPrefab[rand], randLane, Quaternion.Euler(-45f, 0f, 180f));
                    spawnObject.transform.parent = ItemParent;
                    if (FindObjectOfType<GameManger>().Phase > 2)
                    {
                        Destroy(spawnObject, 15f);
                    }

                }
            }
        }
    }

    Vector3 RandomizedLane()
    {
        int rand = Random.Range(0, Lanes.Length);
        Vector3 returnVector = Lanes[rand].position;
        returnVector.y += 70f; 
        return returnVector;
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
    public void TeleportPlayer()
    {

    }

}
