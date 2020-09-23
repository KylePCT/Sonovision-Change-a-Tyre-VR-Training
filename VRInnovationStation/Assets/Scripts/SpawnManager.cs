using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    GameObject GenericVRPlayerPrefab;

    public Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        //If we are connected to the server successfully.
        if (PhotonNetwork.IsConnectedAndReady)
        {
            //Spawn for all clients.
            //For this to work, the prefab must exist in the Resources folder and have a Photon View component.
            PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
