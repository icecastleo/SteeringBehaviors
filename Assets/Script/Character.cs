using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    int hp;

    [SerializeField]
    int maxHp;

    public int MaxHp { get { return maxHp; } internal set {maxHp = value; } }

    [SerializeField]
    protected int minAttack;

    [SerializeField]
    protected int maxAttack;

    public int Hp
    {
        get
        {
            return hp;
        }
        internal set
        {
            hp = value;
        }
    }

    [SerializeField]
    protected float maxSpeed = 1.0f;

    [SerializeField]
    protected float maxForce;

    protected bool isAnimating;

    protected Rigidbody2D rb2D;

    [SerializeField]
    Vector2 velocity;

    [SerializeField]
    protected Vector2 desired;

    // Use this for initialization
    protected void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	protected void FixedUpdate () {
        Steering();

        velocity = rb2D.velocity;

        if (rb2D.velocity.x >= 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    protected void Steering()
    {
        calculateDesiredDirection();

        Vector2 steer = desired.normalized * maxSpeed - rb2D.velocity;
        steer = steer.normalized * maxForce;

        rb2D.AddForce(steer);

        rb2D.velocity = Vector3.ClampMagnitude(rb2D.velocity, maxSpeed);
    }

    internal abstract void calculateDesiredDirection();

    //abstract protected void calculateDesiredDirection();

    public void Attack(Character other)
    {
        other.GetDamage(Random.Range(minAttack, maxAttack+1), this);
    }

    public virtual void GetDamage(int damage, Character other)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            dead();
        }
    }

    virtual protected void dead()
    {
        Destroy(this.gameObject);
    }

    public virtual void OnVisionEnter(Collider2D other)
    {

    }

    public virtual void OnVisionStay(Collider2D other)
    {
        
    }

    public virtual void OnVisionExit(Collider2D other)
    {
       
    }
}
