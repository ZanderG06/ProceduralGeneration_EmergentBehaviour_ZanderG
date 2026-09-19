using System.Collections;
using UnityEngine;

public class CreatureLogic : MonoBehaviour
{
    public int hunger;
    public int reproductionChance;
    public float reachRadius;
    public int moveSpeed;

    [SerializeField] private ServiceHub serviceHub;
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
            hunger -= Random.Range(1,4);
            if (hunger <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SearchForFood()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (hunger < 50)
            {
                StartCoroutine(GoTowardsFood());
            }
            else
            {
                // Implement reproduction logic here
                int willReproduce = Random.Range(1, 101);
                if(willReproduce <= reproductionChance)
                {
                    hunger -= 30;

                }
            }
        }
    }

    IEnumerator GoTowardsFood()
    {
        while (!isTouchingAnything)
        {
            yield return new WaitForSeconds(.1f);
            rb.MovePosition(Vector2.MoveTowards(transform.position, FindClosestFood().position, moveSpeed * Time.deltaTime));

            if (hunger > 50) break;
        }
    }

    private Transform FindClosestFood()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity; //Mathf.Infinity is a positive Infinity, which is useful for the first loop of the foreach statement to save the shortest distance
        foreach(GameObject target in GameObject.FindGameObjectsWithTag("Food")) //I'll admit this might be unoptimized, I will look into better functions since there will be lots of creatures
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < closestDistance) //After 5 loops, this should save the closest position
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
            serviceHub.GameManager.InstantiateFood();
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