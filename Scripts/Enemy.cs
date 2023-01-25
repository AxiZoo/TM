using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;

    private float distance;

    public int health;
    public GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Death();   
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Death()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }
    }

}
