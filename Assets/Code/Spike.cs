using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private bool isPlayerOnSpike = false;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnSpike = true;
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamage(collision.GetComponent<Health>()));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnSpike = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamage(Health playerHealth)
    {
        while (isPlayerOnSpike)
        {
            playerHealth.TakeDamage(1);
            yield return new WaitForSeconds(2.0f);
        }
    }
}

