using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public GameObject creaturePrefab;
    public GameObject foodPrefab;

    public int startingCreatureCount;
    public int startingFoodCount;

    private void Start()
    {
        for (int i = 0; i < startingCreatureCount; i++)
        {
            InstantiateCreature();
        }

        StartCoroutine(PauseBeforeMethod());
    }

    //Pause at the start to allow food to spawn
    IEnumerator PauseBeforeMethod()
    {
        yield return new WaitForSeconds(1f);

        StartFoodLoop();
    }

    private void StartFoodLoop()
    {
        for (int i = 0; i < startingFoodCount; i++)
        {
            InstantiateFood();
        }
    }

    private void InstantiateCreature()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(creaturePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
    }

    public void InstantiateFood()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(foodPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
    }
}