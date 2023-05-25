using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBrain : MonoBehaviour
{
    //A reference to the game manager script
    GameManager gm = GameManager.gm;

    [Header ("Properties")]
    [SerializeField] private float fov = 15f;
    [SerializeField] private float attackRng = 2f;
    [SerializeField] private float chaseDistance = 10f;

    private float cooldownTimer = 10f; //seconds
    private float waitTime = 0;
    private bool onCooldown = false;

    private Transform target;

    [Header("Stats")]
    [SerializeField] private int hp = 100;
    [SerializeField] private int baseDmg = 15;

    private MouseMove moveScript;

    //Default state is following
    private SoldierState lastState = SoldierState.FOLLOWING;
    private SoldierState state = SoldierState.FOLLOWING;

    private GameObject king;

    private void Awake()
    {
        moveScript = GetComponent<MouseMove>();
        king = FindKing();
        SetTarget(king.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(onCooldown)
        {
            waitTime += Time.deltaTime;

            if(waitTime >= cooldownTimer)
            {
                onCooldown = false;
            }
        }

        GameObject closestEnemy = TargetEnemy(FindEnemies());
        //Transform target = GetTarget();

        float distToTarget = Vector3.Distance(transform.position, target.position);

        if (closestEnemy != null && !onCooldown)
        {
            if (!state.Equals(SoldierState.CHASING))
            {
                lastState = state;
                state = SoldierState.CHASING;
            }

            if(distToTarget > chaseDistance)
            {
                state = lastState;
                lastState = SoldierState.CHASING;

                waitTime = 0f;
                onCooldown = true;
            }

            //Chase and attack enemy
            ChaseEnemy(closestEnemy.transform);
        }
        
        if(!state.Equals(SoldierState.CHASING))
        {
            switch (state)
            {
                case SoldierState.DEFENDING:
                    //Receives target from outside (GameManager most likely)
                    DefendPosition(target);
                    break;

                case SoldierState.CHARGING:
                    FindEnemyBase();
                    ChargeEnemyBase();
                    break;

                case SoldierState.FOLLOWING:
                    FollowKing();
                    break;

                default:
                    Debug.Log("Error! No valid SoldierState?");
                    break;
            }
        }
    }

    //Might be better to save a "lastTarget" to avoid refetching of "CombatManager" Component,
    //as well as other benefits on other functions
    private void ChaseEnemy(Transform target)
    {
        moveScript.SetDestination(target.position);

        if (InAttackRange(target))
        {
            CombatManager enemyCM = target.gameObject.GetComponent<CombatManager>();

            //For calculating eventual effects, like king presence
            int damageDealt = baseDmg;

            //If enemy dies ("TakeDamage" function returns true)
            if (enemyCM.TakeDamage(damageDealt))
            {
                return;
            }
        }
        else
        {
            //Move to enemy
            moveScript.MoveToPos();
        }
    }

    private bool InAttackRange(Transform target)
    {
        return (Vector3.Distance(transform.position, target.position) <= attackRng);
    }

    private void DefendPosition(Transform target)
    {
        if(Vector3.Distance(transform.position, target.position) > 1f)
        {
            moveScript.SetDestination(target.position);
            //There should be some kind of formation algorithm here
            moveScript.MoveToPos();
        }
    }

    //Finds the closest enemy building that is revealed to the player
    //If no enemy buildings are revealed, should the soldier "explore",
    //meaning they would pick a random point on the map and walk there?
    private void FindEnemyBase()
    {
        //Get all Enemy Objects
        List<GameObject> allEnemyObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        //Filter to all (revealed?) enemy buildings
        List<GameObject> enemyBuildings = allEnemyObjects.FindAll(obj => obj.layer == LayerMask.NameToLayer("Buildings"));

        float minDist = float.MaxValue;
        GameObject target = null;

        foreach(GameObject building in enemyBuildings)
        {
            float dist = Vector3.Distance(king.transform.position, building.transform.position);

            if(dist < minDist)
            {
                minDist = dist;
                target = building;
            }
        }

        if(target == null)
        {
            Explore();
            return;
        }

        SetTarget(target.transform);
    }

    private void Explore()
    {
        //TODO
    }

    private void ChargeEnemyBase()
    {
        //TODO
    }

    private void FollowKing()
    {
        if(king == null)
        {
            moveScript.SetDestination(transform.position);
            lastState = state;
            state = SoldierState.DEFENDING;
        }
        else if (Vector3.Distance(transform.position, king.transform.position) > 3f)
        {
            moveScript.SetDestination(king.transform.position);
            //There should be some kind of formation algorithm here
        }

        moveScript.MoveToPos();
    }

    private GameObject FindKing()
    {
        return GameObject.Find("King");
    }

    private GameObject TargetEnemy(List<GameObject> enemies)
    {
        float minDistance = float.MaxValue;
        GameObject target = null;

        foreach(var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);


            if(minDistance < distance)
            {
                minDistance = distance;
                target = enemy;
            }
        }

        if(target != null)
        {
            Debug.Log("Target: " + target.name);
        }

        //Target can be null
        return target;
    }

    //Returns a List of all enemies present within a radius of the character
    private List<GameObject> FindEnemies()
    {
        List<GameObject> enemies = new();

        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, fov);

        foreach (var collider in surroundingColliders)
        {
            Debug.Log("Colliders found: " + surroundingColliders.Length);
            //Checks the collider's GameObject's tag
            if (collider.CompareTag("Enemy"))
            {
                //Add the enemy to the list of enemies
                enemies.Add(collider.gameObject);

            }
                Debug.Log("Enemies found: " + enemies.Count);
        }

        return enemies;
    }

    public void SetTarget(Transform target)
    {
        if(target == null)
        {
            this.target = transform;
        }
        else
        {
            this.target = target;
        }
    }

    private enum SoldierState
    {
        DEFENDING,
        CHASING,
        CHARGING,
        FOLLOWING
    }
}
