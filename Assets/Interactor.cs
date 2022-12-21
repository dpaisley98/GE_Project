using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public string promptMessage;
    float interactionPointRadius;

    public void BaseInteract()
    {
        Interact();
    }

    //Temp function to be overwritten by subclass
    protected virtual void Interact() {}
}

