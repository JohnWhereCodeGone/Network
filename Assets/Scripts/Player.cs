using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
public class Player : NetworkBehaviour
{
    [SerializeField] InputReader inputs;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] SpriteRenderer emoteImage;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform muzzle;
    


    NetworkVariable<bool> isEmoting = new NetworkVariable<bool>();
    NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>(); //is set on every copy of the network 3
    private void Start()
    {
        if (inputs != null && IsLocalPlayer) // seperate the client and server in the same script
        {
            inputs.MoveEvent += OnMove; // 4
            inputs.EmoteEvent += OnEmote;
            inputs.ShootEvent += OnShoot;
            emoteImage = transform.GetChild(0).GetComponent<SpriteRenderer>();
            muzzle = transform.GetChild(1).GetComponent<Transform>();
        }

    }
    private IEnumerator DisableEmote()
    {

        yield return new WaitForSeconds(4f);
        isEmoting.Value = false;


    }

    private void OnShoot()
    {
        OnShootRPC();
    }
    private void OnEmote()
    {
        EmoteRPC();
    }

    private void OnMove(Vector2 value) //5
    {
        MoveRPC(value);
    }

    private void Update() //7
    {
        if (IsServer)
        {
            transform.position += (Vector3)moveInput.Value * Time.deltaTime;

        }

        emoteImage.enabled = isEmoting.Value; //moving this made it work

    }

    //rpc "hey I wanna call another function on another computer accross the network

    [Rpc(SendTo.Everyone)] //setting this to everyone made it work
    private void EmoteRPC()
    {
        Debug.Log("is emoting");
        if (IsServer) //only server can execute emotes
        {
            isEmoting.Value = true;
            StartCoroutine(DisableEmote());

        }
    }

    [Rpc(SendTo.Server)]
    private void OnShootRPC()
    {
        Transform spawnedObjectTransform = Instantiate(bullet).transform;
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        bullet.transform.position = muzzle.transform.position;
    }

    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 data) //6
    {
        data.x *= moveSpeed;
        data.y *= moveSpeed;
        moveInput.Value = data;
    }
}
