using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapBack : MonoBehaviour
{
    private GameObject Player;
    public GameObject Tablet;
    public ParticleSystem SpawnParticles;

    private Vector3 playerPos;
    private Vector3 tabletPos;
    private float fraction;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Tablet = GameObject.FindGameObjectWithTag("Tablet");

        playerPos = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        tabletPos = new Vector3(Tablet.transform.position.x, Tablet.transform.position.y, Tablet.transform.position.z);
    }

    //Reactivate can be called and the tablet will snap back.
    public void ReactivateUI()
    {
        playerPos = Player.transform.position;
        Tablet.SetActive(true);
        Tablet.transform.position = Vector3.Lerp(playerPos, tabletPos, fraction);

        Vector3 direction = Player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction + new Vector3(0,60,0));
        Tablet.transform.rotation = rotation;

        Instantiate(SpawnParticles, Tablet.transform.position, Quaternion.identity);
        tabletPos = Tablet.transform.position;
    }
}
