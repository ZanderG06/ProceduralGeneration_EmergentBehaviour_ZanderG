using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    private ServiceHub serviceHub;

    private void Start()
    {
        serviceHub = ServiceHub.Instance;
    }

    private void OnDestroy()
    {
        serviceHub.GameManager.currentFoodCount--;
    }
}