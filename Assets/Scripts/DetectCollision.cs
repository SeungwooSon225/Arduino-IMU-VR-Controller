using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public ObjectSelector ObjectSelector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Selectable")
        { 
            //Debug.Log("In: " + other.gameObject.name);

            ObjectSelector.ObjectCandidates.Add(other.gameObject);
            other.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Selectable")
        {
            //Debug.Log("Out: " + other.gameObject.name);

            if (ObjectSelector.SelectedObject == other.gameObject)
            { 
                ObjectSelector.SelectedObject = null;
            }

            ObjectSelector.ObjectCandidates.Remove(other.gameObject);

            other.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
