using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private string player_Tag;
    [SerializeField]
    private Portal target;
    [SerializeField]
    private Transform spawnPoint;

    public Transform SpawnPoint { get  { { return spawnPoint; } } }

    void Start()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player")
            other.transform.position = target.SpawnPoint.position;
    }
}
