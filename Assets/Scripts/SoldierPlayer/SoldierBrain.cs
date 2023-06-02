using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBrain : MonoBehaviour
{
    //A reference to the game manager script
    GameManager gm;

    [Header ("Properties")]
    [SerializeField] private float fov = 15.0f;
    [SerializeField] private float attackRng = 2.0f;

    private float cooldownTimer = 10.0f; //seconds
    private float waitTime = 0.0f;
    private bool onCooldown = false;

    private bool canChase = true;

    [SerializeField] private const float MAX_CHASE_TIME = 15.0f; //seconds
    private float chaseTime = 0.0f;

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
      // Debug.Log("Current Soldier State: " + state);
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

        if (closestEnemy != null && !onCooldown && canChase)
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
            //Return to original objective after chasing
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

            waitTime = 0.0f;
            onCooldown = true;
            chaseTime = 0.0f;

            closestEnemy = null;

            return;
        }

        moveScript.SetDestination(target.position);

        if (InRange(target.position, attackRng))
        {
            moveScript.Stop();

            if (Attack(target))
            {
                //Enemy killed or destroyed
            }
        }
        else
        {
            //Ensure the soldier can move
            moveScript.ResumeMovement();

            //Move to enemy
            moveScript.MoveToPos();
        }
    }


    private void DefendPosition(Transform target)
    {
        this.target.position = target.position;
        canChase = true;

        moveScript.SetDestination(target.position);

        if (lastState.Equals(SoldierState.CHARGING) && !onCooldown)
        {
            onCooldown = true;
            waitTime = 0.0f;
        }

        if(!InRange(target.position, fov))
        {
            //There should be some kind of formation algorithm here
            moveScript.MoveToPos();
        }
    }

    private void FindEnemyBase()
    {
        //If we already target an enemy building skip search
        //if (target)
        //{
        bool isBuilding = this.target.gameObject.layer == LayerMask.NameToLayer("Buildings");
        bool isEnemy = this.target.CompareTag("Enemy");
        //}

        if (!isBuilding || !isEnemy)
        {
            //Get all Enemy Objects
            List<GameObject> allEnemyObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

            //Filter to all enemy buildings
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
                canChase = true;
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
        moveScript.SetDestination(closestPoint);

        if(InRange(closestPoint, fov))
        {
            //Prohibit Chasing
            canChase = false;
        }
        else
        {
            canChase = true;
        }

        if (!InRange(closestPoint, attackRng))
        {
            Debug.Log("OUT of Range!!");
            //Move to base
            moveScript.MoveToPos();
        }
        else
        {
            moveScript.Stop();
            Debug.Log("IN Range!!");

            if (Attack(target))
            {
                SetTarget(null);
            }
        }
    }

    private void FollowKing()
    {
        float range = 10f;
        bool inRange = InRange(king.transform.position, range);
        canChase = true;

        if (lastState.Equals(SoldierState.CHARGING) && !onCooldown)
        {
            onCooldown = true;
            waitTime = 0.0f;
        }

        if (king == null)
        {
            moveScript.SetDestination(transform.position);
            lastState = state;
            state = SoldierState.DEFENDING;
        }
        else if(!inRange)
        {
            moveScript.SetDestination(king.transform.position);
            //There should be some kind of formation algorithm here
        }

        moveScript.MoveToPos();

        if (inRange)
        {
            moveScript.Stop();
        }
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
