using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    private float distance = 3f;
    [SerializeField]
    LayerMask interactableMask;
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, interactableMask))
        {
            if(hitInfo.collider.GetComponent<Interactor>() != null && Input.GetButton("Interact"))
            {
                Debug.Log(hitInfo.collider.GetComponent<Interactor>().promptMessage);
                Vector3 destinationPosition = player.transform.position;
                destinationPosition.y += 60f;
                player.transform.position = destinationPosition;
            }
        }
    }
}
