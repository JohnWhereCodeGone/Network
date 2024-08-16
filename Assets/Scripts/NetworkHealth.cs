using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [SerializeField] private int healthCount = 1;
    NetworkVariable<int> health = new NetworkVariable<int>();
    // Start is called before the first frame update
    

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        health.Value = healthCount;
    }


}
