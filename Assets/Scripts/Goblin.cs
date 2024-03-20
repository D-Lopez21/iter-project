using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Animator anim;
    public Rigidbody2D rb;
    public PlayerController player;
    private bool lookRigth = true;

    [Header("Health")]
    [SerializeField] public float health;

    [Header("Damage")]
    [SerializeField] public Transform attackControl1;
    [SerializeField] Vector2 attackArea1;
    [SerializeField] public float damage1;
    [SerializeField] public Transform attackControl2;
    [SerializeField] public float radioAttack2;
    [SerializeField] public float damage2;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        anim.SetFloat("Distance", distance);
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            //anim.SetTrigger("Die");
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void lookPlayer()
    {
        if((PlayerController.Instance.transform.position.x > transform.position.x && !lookRigth) || (PlayerController.Instance.transform.position.x < transform.position.x && lookRigth))
        {
            lookRigth = !lookRigth;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    public void Attack1()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackControl1.position, attackArea1, 0);

        foreach (Collider2D colision in objectsToHit)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage1);
            }
        }
    }

    public void Attack2()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(attackControl2.position, radioAttack2);
        foreach(Collider2D colision in objetos)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage2);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackControl1.position, attackArea1);
        Gizmos.DrawWireSphere(attackControl2.position, radioAttack2);
    }
}
