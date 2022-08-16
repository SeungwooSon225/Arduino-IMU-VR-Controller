using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateAcceleration : MonoBehaviour
{
    public SerialReader SerialReader;
    
    public bool IsCalibrated = false;
    public Vector3 CompensationVector;

    private Vector3 initialG;
    private int calibrationCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrationCount < 250) 
        {
            initialG += SerialReader.DeltaAcceleration;
    
            calibrationCount++;
        }
        else if (calibrationCount == 250)
        {
            initialG /= 250;

            CompensationVector = new Vector3(0, -1.0f, 0) * initialG.y;
            CompensationVector = transform.InverseTransformPoint(transform.position + CompensationVector);

            Debug.Log("Calibtraion Done!");
            IsCalibrated = true;
            calibrationCount = 9999;
        }
    }
}
