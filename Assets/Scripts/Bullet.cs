using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// handle velocity
// handle damage
// handle destroying itself
public class Bullet : NetworkBehaviour
{

    [SerializeField] float bulletSpeed = 25f;
    [SerializeField] NetworkObject networkObject;

    NetworkVariable<Vector2> bulletVelocity = new NetworkVariable<Vector2>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        bulletVelocity.Value = new Vector2(0f, 1f);
        StartCoroutine(LifeTime());
    }


    private IEnumerator LifeTime()
    {
        if (!IsServer) yield break;
        yield return new WaitForSeconds(2.5f);
        GetComponent<NetworkObject>().Despawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }


    private void Update()
    {
        if (IsServer)
        {
            transform.position += (Vector3)bulletVelocity.Value * Time.deltaTime * bulletSpeed;
            
        }
        
    }

    
}
