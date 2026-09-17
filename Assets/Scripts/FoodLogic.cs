using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private int startingFoodCount = 5;

    public GameObject food;

    private void Start()
    {
        for(int i = 0; i < startingFoodCount; i++)
        {
            CreateFood();
        }
    }

    public void CreateFood()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(food, new Vector3(randomX, randomY, 0), Quaternion.identity);
    }
}