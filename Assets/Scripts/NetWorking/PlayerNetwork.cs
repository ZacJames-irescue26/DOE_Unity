using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform SpawnedObjectPrefab;

    private Transform SpawnedObjectTransform;
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
    new MyCustomData {
        _int = 56,
        _bool = true,
    },
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    


    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }
    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + ": " + newValue._int + " " + newValue._bool);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector3 moveDir = new Vector3(0, 0, 0);
        if(Input.GetKeyDown(KeyCode.T))
        {
            SpawnedObjectTransform = Instantiate(SpawnedObjectPrefab);
            SpawnedObjectPrefab.GetComponent<NetworkObject>().Spawn(true);
            /*randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
            };*/
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            moveDir.z = 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDir.x = -1.0f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDir.z = -1.0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDir.x = 1.0f;
        }

        float MoveSpeed = 3.0f;

        transform.position = moveDir * MoveSpeed * Time.deltaTime;
    }
    [ServerRpc]
    private void TestServerRpc()
    {
        //code doesnt run on client only server
        Debug.Log("TestServerRpc " + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc");
    }
}
