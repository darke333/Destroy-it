using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float ExplosionRadius;
    [SerializeField] float ExplosionForse;

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (Collider nearCollider in colliders)
        {
            Rigidbody rigidbody = nearCollider.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                if (rigidbody.isKinematic)
                    rigidbody.isKinematic = false;

                rigidbody.AddExplosionForce(ExplosionForse, transform.position, 5);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager gameManager = GameManager.gameManager;
        if (collision.collider.tag == "SimpleCollectible")
        {
            Explode();
            gameManager.EndGame(GameManager.EndGameCondition.Won);
        }
        else
        {
            if (gameManager.cannon.BulletsLeft == 0)
                gameManager.EndGame(GameManager.EndGameCondition.Lost);
            else
            {
                GameManager.gameManager.SetCondition(GameManager.Condition.loading);
            } 
        }
        Destroy(gameObject);
    }
}
