using System.Collections;
using UnityEngine;
/// <summary>
/// Simple helper class for setting the active state of a gameobject to the opposite of the input. (for check boxes)
/// </summary>
public class SetActiveReversed : MonoBehaviour
{


    public void SetDeactivate(bool activate)
    {
        gameObject.SetActive(!activate);
    }
}