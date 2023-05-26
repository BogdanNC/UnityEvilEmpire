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
    private float waitTime = 0f;
    private bool onCooldown = false;

    private float maxChaseTime = 10f; //seconds
    private float chaseTime = 0f;

    private Transform target;
    private Transform secTarget;

    [Header("Stats")]
    [SerializeField] private int hp = 100;
    [SerializeField] private int baseDmg = 15;

    private MouseMove moveScript;

    //Default state is following
    private SoldierState lastState = SoldierState.FOLLOWING;
    private SoldierState state = SoldierState.FOLLOWING;

    private GameObject king;
    private GameObject closestEnemy = null;

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

        if (closestEnemy == null)
        {
            closestEnemy = TargetEnemy(FindEnemies());
        }

        if (closestEnemy != null && !onCooldown)
        {
            if (!state.Equals(SoldierState.CHASING))
            {
                lastState = state;
                state = SoldierState.CHASING;

                SetSecTarget(target);
                SetTarget(closestEnemy.transform);
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
        chaseTime += Time.deltaTime;

        if (chaseTime >= maxChaseTime)
        {
            //Stop chase
            state = lastState;
            lastState = SoldierState.CHASING;

            waitTime = 0f;
            onCooldown = true;
            closestEnemy = null;

            SetTarget(secTarget);
            SetSecTarget(null);

            return;
        }

        moveScript.SetDestination(target.position);

        if (InRange(target.position, attackRng))
        {
            if (Attack(target))
            {
                //Enemy killed or destroyed
                closestEnemy = null;
            }
        }
        else
        {
            //Move to enemy
            moveScript.MoveToPos();
        }
    }


    private void DefendPosition(Transform target)
    {
        moveScript.SetDestination(target.position);

        if(!InRange(target.position, fov))
        {
            //There should be some kind of formation algorithm here
            moveScript.MoveToPos();
        }
    }

    //Finds the closest enemy building that is revealed to the player
    //If no enemy buildings are revealed, should the soldier "explore",
    //meaning they would pick a random point on the map and walk there?
    private void FindEnemyBase()
    {
        //If we already target an enemy building skip search
        bool isBuilding = this.target.gameObject.layer == LayerMask.NameToLayer("Buildings");
        bool isEnemy = this.target.CompareTag("Enemy");

        if (!isBuilding || !isEnemy)
        {
            //Get all Enemy Objects
            List<GameObject> allEnemyObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

            //Filter to all (revealed?) enemy buildings
            List<GameObject> enemyBuildings = allEnemyObjects.FindAll(obj => obj.layer == LayerMask.NameToLayer("Buildings"));

            float minDist = float.MaxValue;
            GameObject target = null;

            foreach (GameObject building in enemyBuildings)
            {
                float dist = Vector3.Distance(king.transform.position, building.transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    target = building;
                }
            }

            if (target == null)
            {
                SetTarget(null);
                Debug.Log("Target became null...?");
                return;
            }

            SetTarget(target.transform);
        }

        ChargeEnemyBase();
    }

    private void ChargeEnemyBase()
    {
        Vector3 closestPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
        moveScript.SetDestination(target.position);

        if (!InRange(closestPoint, attackRng))
        {
            //Move to base
            moveScript.MoveToPos();
        }
        else
        {
            Attack(target);
        }
    }

    private void FollowKing()
    {
        float range = 3f;

        if(king == null)
        {
            moveScript.SetDestination(transform.position);
            lastState = state;
            state = SoldierState.DEFENDING;
        }
        else if(!InRange(king.transform.position, range))
        {
            moveScript.SetDestination(king.transform.position);
            //There should be some kind of formation algorithm here
        }

        moveScript.MoveToPos();
    }
    private bool InRange(Vector3 target, float range)
    {
        return (Vector3.Distance(transform.position, target) <= range);
    }

    /*
     * Returns ture if object "died"
     */
    private bool Attack(Transform target)
    {
        CombatManager enemyCM = target.gameObject.GetComponent<CombatManager>();

        //For calculating eventual effects, like king presence
        int damageDealt = baseDmg;

        //If enemy dies ("TakeDamage" function returns true)
        return enemyCM.TakeDamage(damageDealt);
    }

    private GameObject FindKing()
    {
        return GameObject.Find("King");
    }

    private GameObject TargetEnemy(List<GameObject> enemies)
    {
        float minDistance = float.MaxValue;
        GameObject target = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);


            if (minDistance < distance)
            {
                minDistance = distance;
                target = enemy;
            }
        }

        if (target != null)
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

    public void SetSecTarget(Transform target)
    {
        if (target == null)
        {
            secTarget = transform;
        }
        else
        {
            secTarget = target;
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
