using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float curHealth;

    [Header("Shooting")]
    [SerializeField] private Transform bangPos;
    [SerializeField] private float brrtForce;
    [SerializeField] private float brrtCD;
    [SerializeField] private float booletLife;
    [SerializeField] private float combatRange = 10f;
    private GameObject boolet;
    private float playerDistance;
    private bool canShoot;

    [Header("Pathing")]
    [SerializeField] private List<Transform> paths;
    private int waypointIndex;

    private GameObject player;
    private NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        curHealth = maxHealth;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (playerDistance <= combatRange)
        {
            transform.LookAt(player.transform.position);

            if (canShoot)
            {
                Shoot();
                canShoot = false;
                StartCoroutine(ResetShoot());
            }
        }

        if (paths == null)
        {
            return;
        }
        else if (agent.remainingDistance < 0.2f)
        {
            if (waypointIndex < paths.Count - 1)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
            agent.SetDestination(paths[waypointIndex].position);
        }
    }

    private void Shoot()
    {
        boolet = GameObject.Instantiate(Resources.Load("Prefabs/Boolet") as GameObject, bangPos.position, bangPos.rotation);
        boolet.GetComponent<Rigidbody>().AddForce(brrtForce * bangPos.transform.up, ForceMode.Impulse);
        StartCoroutine(BooletDestroy());
    }
    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(brrtCD);
        canShoot = true;
    }
    private IEnumerator BooletDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(boolet, booletLife);
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
    }
}
