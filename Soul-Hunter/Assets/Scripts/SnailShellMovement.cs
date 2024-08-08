using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class SnailShellMovement : MonoBehaviour
{
    private Animator ani;
    public GameObject SnailPrefab;
    void Start()
    {
        StartCoroutine(OutShell());
        ani = GetComponent<Animator>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
    IEnumerator OutShell()
    {
        yield return new WaitForSeconds(5.0f);

        ani.SetBool("IsOutShell", true);

        yield return new WaitForSeconds(3.0f);

        ani.SetBool("IsOutShell", false);

        DestroyObject();
        Destroy(gameObject);
    }

    void DestroyObject()
    {
        Instantiate(SnailPrefab, transform.position, transform.rotation);
    }
}
