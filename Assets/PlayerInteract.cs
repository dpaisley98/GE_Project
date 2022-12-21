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

    [SerializeField]
    GameObject terrin;

    Vector3 buttonPosition; // store the position of the button

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
                TerrainGenerator terrainValue = terrin.GetComponent<TerrainGenerator>();
                terrainValue.buttonCheck = true;
                destinationPosition.y += 60f;
                player.transform.position = destinationPosition;

                // store the position of the button
                buttonPosition = hitInfo.collider.transform.position;

                // start the countdown co-routine
                StartCoroutine(Countdown());
            }
        }
    }

    IEnumerator Countdown()
    {
        StartCoroutine(WaitForLandGeneration());
        // wait for a minute
        yield return new WaitForSeconds(60f);

        // move the player back to the button position
        player.transform.position = buttonPosition;        
    }

    IEnumerator WaitForLandGeneration()
    {
        // wait for the land to be generated
        while (terrin.GetComponent<TerrainGenerator>().buttonCheck)
        {
            yield return null;
        }
    }
}
