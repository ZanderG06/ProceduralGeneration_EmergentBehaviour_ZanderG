using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Prefabs")]
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private GameObject foodPrefab;

    [Header("Counts")]
    [SerializeField] private int startingCreatureCount;
    [SerializeField] private int startingFoodCount;
    public int currentFoodCount;
    public int currentCreatureCount;
    public int totalCreatureCount;
    public int maxCreatureCount;

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
        for (int i = currentFoodCount; i < startingFoodCount; i++)
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
        if (currentFoodCount >= startingFoodCount) return;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(foodPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);

        currentFoodCount++;
    }
}