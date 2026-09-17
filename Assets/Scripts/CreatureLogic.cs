using System.Collections;
using UnityEngine;

public class CreatureLogic : MonoBehaviour
{
    public int hunger;
    public int reproductionChance;
    public float reachRadius;
    public int moveSpeed;

    private ServiceHub serviceHub;
    private Rigidbody2D rb;

    private bool isTouchingAnything = false;

    private void Start()
    {
        serviceHub = ServiceHub.Instance;
        rb = GetComponent<Rigidbody2D>();

        hunger = Random.Range(10, 101);
        reproductionChance = Random.Range(1, 101);
        reachRadius = Random.Range(.5f, 3f);
        moveSpeed = Random.Range(10, 31);

        gameObject.GetComponent<CircleCollider2D>().radius = reachRadius;

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
                StartCoroutine(GoTowardsFood());
            }
            else
            {
                //hunger -= 30;
                // Implement reproduction logic here
            }
        }
    }

    IEnumerator GoTowardsFood()
    {
        while (!isTouchingAnything)
        {
            yield return new WaitForSeconds(.1f);
            rb.MovePosition(Vector2.MoveTowards(transform.position, FindClosestFood().position, moveSpeed * Time.deltaTime));
        }
    }

    private Transform FindClosestFood()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach(GameObject target in GameObject.FindGameObjectsWithTag("Food"))
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        return closestTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTouchingAnything = true;
        if (collision.gameObject.CompareTag("Food"))
        {
            hunger += 30;
            Destroy(collision.gameObject);
            serviceHub.FoodLogic.CreateFood();
        }
        if (collision.gameObject.CompareTag("Creature"))
        {
            // Impliment run away logic here
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingAnything = false;
    }
}