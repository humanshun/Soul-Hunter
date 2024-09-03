using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisMovement : BaseEnemyMovement
{
    public Animator animator;
    public float minAttackInterval = 3.0f;
    public float maxAttackInterval = 6.0f;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking; // isAttackingのプロパティを公開

    protected override void Start()
    {
        StartCoroutine(RandomCoroutine());
        base.Start();
    }

    protected override void Move()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        base.Move();
    }
    
    private IEnumerator RandomCoroutine()
    {
        float randomInterval = Random.Range(minAttackInterval, maxAttackInterval);
        yield return new WaitForSeconds(randomInterval);
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsPreliminaryAttack", true);

        yield return new WaitForSeconds(1.0f);

        animator.SetBool("IsPreliminaryAttack", false);
        animator.SetBool("IsAttack", true);
        AudioM.Instance.PlayMantisAttackSound();

        yield return new WaitForSeconds(1.3f);
        
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsWalk", true);

        isAttacking = false;
        StartCoroutine(RandomCoroutine());
    }
}
