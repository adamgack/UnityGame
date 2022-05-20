using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public GameObject player;
    PlayerMovement playerMovement;

    List<Collider> colliders = new List<Collider>();


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colliders.Count > 0)
        {
            playerMovement.isGrounded = colliders.Count(x => x.gameObject.layer == LayerMask.NameToLayer("Ground")) > 0;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        colliders.Add(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        colliders.Remove(collider);
    }
}
