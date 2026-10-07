using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private GameObject foodPrefab;

    [SerializeField] private int startingCreatureCount;
    [SerializeField] private int minFoodCount;

    public int currentFoodCount;
    public int totalCreatureCount;

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

    public void StartFoodLoop()
    {
        for (int i = currentFoodCount; i < minFoodCount; i++)
        {
            InstantiateFood();
        }
    }

    public void PreventFoodScarcity()
    {
        if(currentFoodCount < 3)
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

    private void InstantiateFood()
    {
        if (currentFoodCount >= minFoodCount) return;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(foodPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);

        currentFoodCount++;
    }
}