using System.Collections;
using UnityEngine;

public class CreatureLogic : MonoBehaviour
{
    [SerializeField] private int hunger;
    [SerializeField] private int saturation;
    [SerializeField] private int reproductionChance;
    [SerializeField] private int moveSpeed;
    [SerializeField] private Vector2[] corners;
    [SerializeField] private CircleCollider2D grabRadius;

    [SerializeField] private ServiceHub serviceHub;
    private Rigidbody2D rb;

    private bool isTouchingAnything = false;

    private void Start()
    {
        serviceHub = ServiceHub.Instance;
        rb = GetComponent<Rigidbody2D>();

        hunger = 30;
        saturation = Random.Range(10, 31);
        reproductionChance = Random.Range(1, 26);
        moveSpeed = Random.Range(15, 36);
        grabRadius.radius = Random.Range(1f, 1.5f);

        serviceHub.GameManager.totalCreatureCount++;
        name = $"Creature {serviceHub.GameManager.totalCreatureCount}"; //Inspector only, just to keep it tidy

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

            if (hunger < 60) StartCoroutine(GoTowardsFood());
            else
            {
                //StartCoroutine(GoTowardsCorner(corners[Random.Range(0, corners.Length)]));
                StartCoroutine(GoTowardsFood());

                // Implement reproduction logic here
                yield return new WaitForSeconds(3f);
                int willReproduce = Random.Range(1, 101);
                if (willReproduce <= reproductionChance)
                {
                    Debug.Log($"{name} reproduce with {hunger} hunger ({willReproduce}/{reproductionChance} chance)");
                    hunger /= 4;
                    GameObject clone = Instantiate(gameObject, Random.insideUnitCircle * 2f + (Vector2)transform.position, Quaternion.identity);
                    clone.GetComponent<CreatureLogic>().hunger = 30;
                    clone.GetComponent<CreatureLogic>().saturation = saturation + Random.Range(-10, 11);
                    clone.GetComponent<CreatureLogic>().reproductionChance = reproductionChance - Random.Range(-20, 11);
                    clone.GetComponent<CreatureLogic>().moveSpeed = moveSpeed + Random.Range(-5, 6);
                    clone.GetComponent<CreatureLogic>().grabRadius.radius = grabRadius.radius + Random.Range(-.2f, .3f);
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

            /*
            if (hunger > 50) break;
            if (isTouchingAnything)
            {
                StartCoroutine(GoTowardsCorner(corners[Random.Range(0, corners.Length)]));
                break;
            }*/
        }
    }

    IEnumerator GoTowardsCorner(Vector2 position)
    {
        while (hunger > 60)
        {
            yield return new WaitForSeconds(.1f);
            rb.MovePosition(Vector2.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime));

            if (hunger < 60) break;
        }
    }

    private Transform FindClosestFood()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity; //Mathf.Infinity is a positive Infinity, which is useful for the first loop of the foreach statement to save the shortest distance
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
        //isTouchingAnything = true;
        if (collision.gameObject.CompareTag("Food"))
        {
            hunger += saturation - Random.Range(1, 11);
            if(hunger > 100) hunger = 100;
            Destroy(collision.gameObject);
            serviceHub.GameManager.PreventFoodScarcity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingAnything = false;
    }
}