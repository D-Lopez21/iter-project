using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Animator anim;
    public Rigidbody2D rb;
    public PlayerController player;
    private bool lookRigth = true;
    private bool alredyDead = false;
    public GameObject secret;
    private bool dropped = false;

    [Header("Health")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;

    [Header("Damage")]
    [SerializeField] public Transform attackControl;
    [SerializeField] public Vector2 attackArea;
    [SerializeField] public float damage;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        anim.SetFloat("Distance", distance);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            anim.SetTrigger("Die");
            Death(2);
            if(!dropped)
            {
                DropItem();
                dropped = true;
            }
        }
    }

    private void Death(float _destroyTime)
    {
        if(!alredyDead){
            PlayerController.Instance.Exp += 20;
            alredyDead = true;
        }
        Destroy(gameObject, _destroyTime);
    }

    public void lookPlayer()
    {
        if((PlayerController.Instance.transform.position.x > transform.position.x && lookRigth) || (PlayerController.Instance.transform.position.x < transform.position.x && !lookRigth))
        {
            lookRigth = !lookRigth;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    private void DropItem()
    {
        Instantiate(secret, transform.position, Quaternion.identity);
    }

    public void Attack()
    {
        Collider2D[] objetos = Physics2D.OverlapBoxAll(attackControl.position, attackArea, 0);
        foreach(Collider2D colision in objetos)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage);

                if(health + damage > maxHealth)
                {
                    health = maxHealth;
                }else{
                    health += damage;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackControl.position, attackArea);
    }
}
