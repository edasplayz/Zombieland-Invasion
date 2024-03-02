using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;
    
    //public PlayerHealth playerhp;

    private void Start()
    {
        damage = Random.Range(1, 5);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //playerhp.TakeDamage(damage);
        }
    }

}
