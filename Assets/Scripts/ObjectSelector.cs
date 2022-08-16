using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public List<GameObject> ObjectCandidates = new List<GameObject>();
    public GameObject SelectedObject;
    public GameObject Cone;
    public GameObject Ray;
    public SerialReader SerialReader;

    private int objectIndex = 0;
    [SerializeField]
    private AudioSource selectSound;

    enum ControllerState
    { 
        None,
        Active,
        Select
    }

    ControllerState state;

    // Start is called before the first frame update
    void Start()
    {
        state = ControllerState.None;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ControllerState.None:
                if (SerialReader.ButtonDown)
                {
                    SerialReader.ButtonPress = true;
                    SerialReader.ButtonDown = false;
                    Cone.SetActive(true);
                    state = ControllerState.Active;
                    //Debug.Log("Active");
                }
                break;

            case ControllerState.Active:
                if (ObjectCandidates.Count != 0)
                {
                    if (SerialReader.JoystickRight)
                    {
                        SerialReader.JoystickRight = false;
                        SerialReader.JoysticX = true;

                        if (SelectedObject == null)
                        {
                            objectIndex = 0;
                        }
                        else
                        {
                            ObjectCandidates[objectIndex].GetComponent<Renderer>().material.color = Color.green;
                            objectIndex++;

                            if (objectIndex == ObjectCandidates.Count)
                            {
                                objectIndex = 0;
                            }
                        }

                        SelectedObject = ObjectCandidates[objectIndex];
                        SelectedObject.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    else if (SerialReader.JoystickLeft)
                    {
                        SerialReader.JoystickLeft = false;
                        SerialReader.JoysticX = true;

                        if (SelectedObject == null)
                        {
                            objectIndex = 0;
                        }
                        else
                        {
                            ObjectCandidates[objectIndex].GetComponent<Renderer>().material.color = Color.green;
                            objectIndex--;

                            if (objectIndex == -1)
                            {
                                objectIndex = ObjectCandidates.Count - 1;
                            }
                        }

                        SelectedObject = ObjectCandidates[objectIndex];
                        SelectedObject.GetComponent<Renderer>().material.color = Color.blue;
                    }
                }

                if (SerialReader.ButtonDown)
                {
                    SerialReader.ButtonPress = true;
                    SerialReader.ButtonDown = false;
                    Cone.SetActive(false);

                    foreach (GameObject obj in ObjectCandidates)
                    {
                        obj.GetComponent<Renderer>().material.color = Color.white;

                    }

                    ObjectCandidates.Clear();

                    if (SelectedObject != null)
                    {
                        selectSound.Play();
                        Debug.Log("sound");
                        SelectedObject.GetComponent<Renderer>().material.color = Color.red;

                        SelectedObject.transform.parent = this.transform;

                        Ray.transform.up = SelectedObject.transform.position - this.transform.position;
                        state = ControllerState.Select;
                        //Debug.Log("Select");
                    }
                    else
                    {
                        state = ControllerState.None;
                        //Debug.Log("None");
                    }
                }
                break;

            case ControllerState.Select:
                if (SerialReader.JoystickFront)
                {
                    SerialReader.JoystickFront = false;
                    SerialReader.JoysticY = true;

                    SelectedObject.transform.position -= (this.transform.position - SelectedObject.transform.position).normalized * 10f;
                }
                else if (SerialReader.JoystickBack)
                {
                    SerialReader.JoystickBack = false;
                    SerialReader.JoysticY = true;

                    SelectedObject.transform.position += (this.transform.position - SelectedObject.transform.position).normalized * 10f;
                }

                if (SerialReader.ButtonDown)
                {
                    SerialReader.ButtonPress = true;
                    SerialReader.ButtonDown = false;
                    SelectedObject.transform.parent = null;
                    SelectedObject.GetComponent<Renderer>().material.color = Color.white;
                    SelectedObject = null;
                    selectSound.Play();
                    state = ControllerState.None;
                    //Debug.Log("None");
                }
                break;
        }

        
    }
}
