using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{

    public HealthManager healthBar;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided death!");

        if (collision.tag == "Zombie")
        {
            //Ending Code here
            Debug.Log("Died!");
            GameManager.DecreaseHealth(5);
            healthBar.TakeDamage(5);
            Destroy(collision.gameObject);
        }
    }
}
