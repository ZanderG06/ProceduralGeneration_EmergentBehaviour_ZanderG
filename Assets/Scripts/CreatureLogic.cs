using System.Collections;
using UnityEngine;

public class CreatureLogic : MonoBehaviour
{
    public int hunger;
    public int reproductionChance;
    public float shyRadius;

    private void Start()
    {
        hunger = Random.Range(10, 101);
        reproductionChance = Random.Range(1, 101);
        shyRadius = Random.Range(.5f, 3f);

        gameObject.GetComponent<CircleCollider2D>().radius = shyRadius;

        StartCoroutine(StartHungerSystem());
        StartCoroutine(SearchForFood());
    }

    IEnumerator StartHungerSystem()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            hunger -= 1;
            if (hunger <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SearchForFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (hunger < 50)
            {
                // Implement food searching logic here
            }
        }
    }
}