using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum BoarState { Normal, Flee};

public class Boar : Character
{
    [SerializeField]
    BoarState state;

    Animator animator;

    [SerializeField]
    Transform foodTarget;

    [SerializeField]
    Transform boarTarget;

    Vector2 wander;

    bool waitAttack;

    float visionRadius;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        // default speed
        //rb2D.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;

        state = BoarState.Normal;
        StartCoroutine(Wander());

        animator = GetComponent<Animator>();
        animator.SetBool("Move", true);

        visionRadius = transform.Find("Vision").GetComponent<CircleCollider2D>().radius;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (Hp < MaxHp * 0.2f)
        {  
            state = BoarState.Flee;
        }

        if (state == BoarState.Flee && Hp > MaxHp * 0.7f)
        {
            state = BoarState.Normal;
        }

        if(foodTarget)
        {
            // food is too far to see
            if((foodTarget.position - transform.position).magnitude > visionRadius)
            {
                foodTarget = null;
            }
        }

        if (!foodTarget)
        {
            boarTarget = null;
        }

        if (boarTarget)
        {
            if(state == BoarState.Normal)
            {
                // other boar runs away
                if ((boarTarget.position - foodTarget.position).magnitude > (visionRadius - 1.0f))
                {
                    boarTarget = null;
                }
            } 
            else if(state == BoarState.Flee)
            {
                // TODO: multiple boar flee
                if((boarTarget.position - transform.position).magnitude > (visionRadius + 1.0f))
                {
                    boarTarget = null;
                }
            }
        }

        if (state == BoarState.Normal && boarTarget && waitAttack == false)
        {
            if ((boarTarget.position - transform.position).magnitude < 1.5f)
            {
                Attack(boarTarget.GetComponent<Character>());
                waitAttack = true;

                StartCoroutine(attackCooldown());
            }
        }
    }

    public IEnumerator Wander()
    {
        while (true)
        {
            // Change the target direction every 3 seconds

            //float rangeX = Mathf.Abs(desired.x);
            //float rangeY = Mathf.Abs(desired.y);

            //wander = new Vector2(Random.Range(-rangeX * 0.25f, rangeX * 0.25f), Random.Range(-rangeY * 0.25f, rangeY * 0.25f));

            //float wanderX = Random.Range(0.1f, 0.2f);
            //float wanderY = Random.Range(0.1f, 0.2f);

            //wander = new Vector2(Random.Range(0, 2) == 0 ? -wanderX : wanderX, Random.Range(0, 2) == 0 ? -wanderY : wanderY);

            float angle = Random.Range(0, 360f / Mathf.PI);

            wander = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            yield return new WaitForSeconds(3f);
        }
    }

    public IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(3f);
        waitAttack = false;
    }

    internal override void calculateDesiredDirection()
    {
        // wander
        Vector2 seek = wander;

        // seeking food subsume wander
        if (foodTarget)
        {
            seek = foodTarget.position - transform.position;
        }

        // flee or attack other boar subsume other steering 
        if(boarTarget)
        {
            seek = boarTarget.position - transform.position;

            if (state == BoarState.Flee)
            {
                // flee
                seek = -seek;
            }
        }

        desired = seek.normalized;

        // Obstacle avoidance
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, GetComponent<CircleCollider2D>().radius, rb2D.velocity, visionRadius, 0x100);

        if (hit.collider != null)
        {
            Vector2 avoid = transform.position - hit.collider.transform.position;

            // weight avoid vector by distance
            float weight = (visionRadius - avoid.magnitude) * 1.5f;

            avoid = avoid.normalized * weight;

            // Inhibit wander
            wander = avoid;

            desired += avoid;
        }

        // rotate vector for random wander
        desired = Vector3.RotateTowards(desired, -desired, Random.Range(-0.2f, 0.2f), 0f);
    }

    protected override void dead()
    {
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.CompareTag("Food") && boarTarget == null)
        {
            eat(other);
        }
    }

    void eat(Collider2D other)
    {
        Hp = (int)Mathf.Min(Hp + (MaxHp * 0.3f), MaxHp);

        foodTarget = null;

        Destroy(other.gameObject);
    }

    public override void OnVisionStay(Collider2D other)
    {
        // exclude self
        if (other.gameObject == gameObject)
            return;

        // food sensor
        if (other.CompareTag("Food"))
        {
            if (!foodTarget)
            {
                foodTarget = other.transform;
            }
            else
            {
                // nearer food 
                if ((other.transform.position - transform.position).magnitude < (foodTarget.position - transform.position).magnitude)
                {
                    foodTarget = other.transform;
                }
            }
        }

        // boar sensor
        if (other.CompareTag("Animal") && foodTarget)
        {
            if (!boarTarget)
            {
                // find other boar nearing the food
                if ((other.transform.position - foodTarget.position).magnitude <= (visionRadius - 1.0f))
                {
                    boarTarget = other.transform;
                }
            }
        }
    }

    public override void GetDamage(int damage, Character other)
    {
        base.GetDamage(damage, other);

        //if (boarTarget == null)
        //{
        //    boarTarget = other;
        //}
    }
}
