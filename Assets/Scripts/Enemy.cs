using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// Destroy if bullets hit you
// Move towards target
// Increase speed over duration of game
// 


[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : NetworkBehaviour
{
    int health = 2;
    [SerializeField] GameObject Target;
    [SerializeField] float enemySpeed = 1f;
    [SerializeField] EnemyHandler handler;
    
    [SerializeField] private BoxCollider2D boxcollider;
    NetworkVariable<int> netHealth = new NetworkVariable<int>();
    NetworkVariable<Vector2> TargetDirection = new NetworkVariable<Vector2>();
    

    private void Start()
    {
        Target = GameObject.Find("Target");
        Vector2 direction = Target.transform.position - transform.position;
        direction.Normalize();
        TargetDirection.Value = direction;
        netHealth.Value = health;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            DamageRPC();
        }
        if (Target == collision.gameObject)
        {
            properdestroy();
        }

        
    }

    private void properdestroy()
    {
        GetComponent<NetworkObject>().Despawn();
    }

    private void Update()
    {
        if (IsServer && netHealth.Value <= 0)
        {
            properdestroy();
        }

        if (IsServer)
        {
            
            transform.position += (Vector3)TargetDirection.Value * Time.deltaTime * enemySpeed;
        }
    }

    [Rpc(SendTo.Server)]
    private void DamageRPC()
    {
        health--;
        netHealth.Value--;
    }       


}
