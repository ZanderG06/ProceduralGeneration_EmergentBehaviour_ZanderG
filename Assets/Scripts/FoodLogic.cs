using System.Collections;
using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    private ServiceHub serviceHub;

    private void Start()
    {
        serviceHub = ServiceHub.Instance;

        name = $"Food"; //Inspector only, just to keep it tidy

        StartCoroutine(Reproduce());
    }

    IEnumerator Reproduce()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            int randomChance = Random.Range(1, 101);
            if (randomChance <= 10)
            {
                Instantiate(gameObject, Random.insideUnitCircle * 0.5f + (Vector2)transform.position, Quaternion.identity);
                serviceHub.GameManager.currentFoodCount++;
            }
        }
    }

    private void OnDestroy()
    {
        serviceHub.GameManager.currentFoodCount--;
    }
}