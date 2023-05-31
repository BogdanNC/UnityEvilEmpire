using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBrain : MonoBehaviour
{
    //A reference to the game manager script
    GameManager gm;

    [Header ("Properties")]
    [SerializeField] private float fov = 15f;
    [SerializeField] private float attackRng = 2f;

    private float cooldownTimer = 10f; //seconds
    private float waitTime = 0f;
    private bool onCooldown = false;

    [SerializeField] private const float MAX_CHASE_TIME = 15.0f; //seconds
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
    private GameObject defendPos;
    private GameObject closestEnemy = null;

    private void Awake()
    {
        gm = GameManager.gm;

        moveScript = GetComponent<MouseMove>();
        king = FindKing();
        defendPos = FindDefendPos();
        SetTarget(king.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current Soldier State: " + state);
        //Debug.Log("Current Soldier LastState: " + lastState);

        if (onCooldown)
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
        else if(state.Equals(SoldierState.CHASING))
        {
            //No enemies found but still tried to chase
            SetTarget(secTarget);
            SetSecTarget(null);

            state = lastState;
            lastState = SoldierState.CHASING;
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

        if (chaseTime >= MAX_CHASE_TIME)
        {
            Debug.Log("Stop Chase!");

            //Stop chase
            SetTarget(secTarget);
            SetSecTarget(null);

            state = lastState;
            lastState = SoldierState.CHASING;

            waitTime = 0f;
            onCooldown = true;

            closestEnemy = null;

            return;
        }

        moveScript.SetDestination(target.position);

        if (InRange(target.position, attackRng))
        {
            if (Attack(target))
            {
                //Enemy killed or destroyed
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
        this.target.position = target.position;

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

    private GameObject FindDefendPos()
    {
        return GameObject.Find("DefendPos");
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


            if (minDistance > distance)
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
            //Checks the collider's GameObject's tag
            if (collider.CompareTag("Enemy"))
            {
                //Add the enemy to the list of enemies
                enemies.Add(collider.gameObject);
            }
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

    public void SetDefendPos(Vector3 pos)
    {
        if(pos == null)
        {
            Debug.Log("DefendPos was null?!?!?");
            SetTarget(null);
            return;
        }

        if (defendPos == null)
        {
            defendPos = new GameObject("DefendPos");
        }

        defendPos.transform.position = pos;

        SetTarget(defendPos.transform);
    }

    public void SetState(SoldierState newState)
    {
        lastState = state;
        state = newState;
    }

    public enum SoldierState
    {
        DEFENDING,
        CHASING,
        CHARGING,
        FOLLOWING
    }
}
