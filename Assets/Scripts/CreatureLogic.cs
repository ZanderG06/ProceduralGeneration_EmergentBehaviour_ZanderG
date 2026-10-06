using System.Collections;
using UnityEngine;

public class CreatureLogic : MonoBehaviour
{
    [SerializeField] private int hunger;
    [SerializeField] private int reproductionChance;
    [SerializeField] private int moveSpeed;
    [SerializeField] private Vector2[] corners;
    [SerializeField] private CircleCollider2D grabRadius;
    [SerializeField] private CircleCollider2D shyRadius;

    [SerializeField] private ServiceHub serviceHub;
    private Rigidbody2D rb;

    private bool isTouchingAnything = false;

    private void Start()
    {
        serviceHub = ServiceHub.Instance;
        rb = GetComponent<Rigidbody2D>();

        hunger = Random.Range(10, 101);
        reproductionChance = Random.Range(1, 101);
        moveSpeed = Random.Range(10, 31);

        shyRadius.radius = Random.Range(.5f, 3f);
        grabRadius.radius = Random.Range(1f, 1.5f);
        
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
                if (willReproduce <= reproductionChance)
                {
                    hunger -= 10;
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

    IEnumerator GoTowardsCorner(Vector2 position)
    {
        while (isTouchingAnything)
        {
            yield return new WaitForSeconds(.1f);
            rb.MovePosition(Vector2.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime));

            if (!isTouchingAnything) break;
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
        if (collision.gameObject.CompareTag("Creature"))
        {
            Vector2 randomCorner = corners[Random.Range(0, corners.Length)];
            StartCoroutine(GoTowardsCorner(randomCorner));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            hunger += 30;
            Destroy(collision.gameObject);
            serviceHub.GameManager.StartFoodLoop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingAnything = false;
    }
}