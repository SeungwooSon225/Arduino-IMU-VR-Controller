using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public SerialReader SerialReader;
    public CalibrateAcceleration CalibrateAcceleration;

    [SerializeField]
    private Vector3 thresholdValue = Vector3.zero;
    [SerializeField]
    private Vector3 deltaAcceleration;

    private float speed = 3.0f;

    private float mXTimer = 0.0f;
    private float xDeltaTime = 0.0f;
    private bool xFlag = true;
    private bool xDeltaFlag = false;

    private float mYTimer = 0.0f;
    private float yDeltaTime = 0.0f;
    private bool yFlag = true;
    private bool yDeltaFlag = false;

    private float mZTimer = 0.0f;
    private float zDeltaTime = 0.0f;
    private bool zFlag = true;
    private bool zDeltaFlag = false;

    private float[] timer = { 0.0f, 0.0f, 0.0f };
    private float[] movingPeriod = { 0.0f, 0.0f, 0.0f };
    [SerializeField]
    private bool[] movingFlag = { true, true, true };
    private bool[] timerFlag = { true, true, true };
    private Vector3[] movingDirection = {
                                            new Vector3(-1, 0, 0),
                                            new Vector3(0, 1, 0),
                                            new Vector3(0, 0, 1),
                                        };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!CalibrateAcceleration.IsCalibrated) return;

        gameObject.transform.rotation = SerialReader.Quaternion;
        //gameObject.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));

        deltaAcceleration = SerialReader.DeltaAcceleration;
        deltaAcceleration += CalibrateAcceleration.CompensationVector;

        //for (int i = 0; i < 3; i++)
        //{
        //    if (deltaAcceleration[i] > thresholdValue[i] && movingFlag[i] == true)
        //    {
        //        movingFlag[i] = false;
        //        gameObject.GetComponent<Rigidbody>().velocity = ChangeSpecificValueOfVector(gameObject.GetComponent<Rigidbody>().velocity, i, speed);

        //        timer[i] = Time.time;
        //        Debug.Log("1: " + i + " d: " + movingDirection[i] + " a: " + deltaAcceleration);
        //    }
        //    else if (deltaAcceleration[i] < -thresholdValue[i] && movingFlag[i] == true)
        //    {
        //        movingFlag[i] = false;
        //        gameObject.GetComponent<Rigidbody>().velocity = ChangeSpecificValueOfVector(gameObject.GetComponent<Rigidbody>().velocity, i, -speed);
        //        timer[i] = Time.time;
        //        Debug.Log("2");
        //    }
        //    else if (Mathf.Abs(deltaAcceleration[i]) < thresholdValue[i] && movingFlag[i] == false && timerFlag[i] == true)
        //    {
        //        movingPeriod[0] = (Time.time - timer[i]) * 4.0f;
        //        //Debug.Log("DeltaTime: " + movingPeriod[0]);
        //        timerFlag[i] = false;
        //    }

        //    if (movingPeriod[i] > 0.0f)
        //    {
        //        movingPeriod[i] -= Time.deltaTime;
        //    }
        //    else if (movingPeriod[i] < 0.0f && movingFlag[i] == false)
        //    {
        //        gameObject.GetComponent<Rigidbody>().velocity = ChangeSpecificValueOfVector(gameObject.GetComponent<Rigidbody>().velocity, i, 0);
        //        movingPeriod[i] = 0.0f;
        //        movingFlag[i] = true;
        //        timerFlag[i] = true;
        //    }
        //}

        //if (deltaAcceleration.magnitude > thresholdValue.x && xFlag == true)
        //{
        //    xFlag = false;
        //    gameObject.GetComponent<Rigidbody>().velocity = deltaAcceleration * 0.003f;
        //    mXTimer = Time.time;
        //    //Debug.Log("1: " + mXTimer);
        //}
        //else if (deltaAcceleration.magnitude < thresholdValue.x && xFlag == false && xDeltaFlag == false)
        //{
        //    xDeltaTime = (Time.time - mXTimer) * 5.0f;
        //    //Debug.Log("DeltaTime: " + xDeltaTime);
        //    xDeltaFlag = true;
        //}

        //if (xDeltaTime > 0.0f)
        //{
        //    xDeltaTime -= Time.deltaTime;
        //}
        //else if (xDeltaTime < 0.0f && xFlag == false)
        //{
        //    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    xDeltaTime = 0.0f;
        //    xFlag = true;
        //    xDeltaFlag = false;
        //}

        //==============================================================
        if (deltaAcceleration.x > thresholdValue.x && xFlag == true)
        {
            xFlag = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-speed, gameObject.GetComponent<Rigidbody>().velocity.y, gameObject.GetComponent<Rigidbody>().velocity.z);
            mXTimer = Time.time;
            //Debug.Log("1: " + mXTimer);
        }
        else if (deltaAcceleration.x < -thresholdValue.x && xFlag == true)
        {
            xFlag = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(speed, gameObject.GetComponent<Rigidbody>().velocity.y, gameObject.GetComponent<Rigidbody>().velocity.z);
            mXTimer = Time.time;
            //Debug.Log("2");
        }
        else if (Mathf.Abs(deltaAcceleration.x) < thresholdValue.x && xFlag == false && xDeltaFlag == false)
        {
            xDeltaTime = (Time.time - mXTimer) * 5.0f;
            //Debug.Log("DeltaTime: " + xDeltaTime);
            xDeltaFlag = true;
        }

        if (xDeltaTime > 0.0f)
        {
            xDeltaTime -= Time.deltaTime;
        }
        else if (xDeltaTime < 0.0f && xFlag == false)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, gameObject.GetComponent<Rigidbody>().velocity.y, gameObject.GetComponent<Rigidbody>().velocity.z);
            xDeltaTime = 0.0f;
            xFlag = true;
            xDeltaFlag = false;
        }

        //==============================================================
        if (deltaAcceleration.y > thresholdValue.y && yFlag == true)
        {
            yFlag = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, speed, gameObject.GetComponent<Rigidbody>().velocity.z);
            mYTimer = Time.time;
            //Debug.Log("1: " + mYTimer);
        }
        else if (deltaAcceleration.y < -thresholdValue.y && yFlag == true)
        {
            yFlag = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, -speed, gameObject.GetComponent<Rigidbody>().velocity.z);
            mYTimer = Time.time;
            //Debug.Log("2");
        }
        else if (Mathf.Abs(deltaAcceleration.y) < thresholdValue.y && yFlag == false && yDeltaFlag == false)
        {
            yDeltaTime = (Time.time - mYTimer) * 5.0f;
            //Debug.Log("DeltaTime: " + xDeltaTime);
            yDeltaFlag = true;
        }

        if (yDeltaTime > 0.0f)
        {
            yDeltaTime -= Time.deltaTime;
        }
        else if (yDeltaTime < 0.0f && yFlag == false)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, 0, gameObject.GetComponent<Rigidbody>().velocity.z);
            yDeltaTime = 0.0f;
            yFlag = true;
            yDeltaFlag = false;
        }

        //==============================================================
        if (deltaAcceleration.z > thresholdValue.z && zFlag == true)
        {
            zFlag = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, -speed * 0.8f);
            mZTimer = Time.time;
            //Debug.Log("1: " + mYTimer);
        }
        else if (deltaAcceleration.z < -thresholdValue.z && zFlag == true)
        {
            zFlag = false;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, speed * 0.8f);
            mZTimer = Time.time;
            //Debug.Log("2");
        }
        else if (Mathf.Abs(deltaAcceleration.z) < thresholdValue.z && zFlag == false && zDeltaFlag == false)
        {
            zDeltaTime = (Time.time - mZTimer) * 5.0f;
            //Debug.Log("DeltaTime: " + xDeltaTime);
            zDeltaFlag = true;
        }

        if (zDeltaTime > 0.0f)
        {
            zDeltaTime -= Time.deltaTime;
        }
        else if (zDeltaTime < 0.0f && zFlag == false)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
            zDeltaTime = 0.0f;
            zFlag = true;
            zDeltaFlag = false;
        }

    }

    Vector3 ChangeSpecificValueOfVector(Vector3 vector, int index, float value)
    {
        Vector3 result = vector;
        result[index] = value;
        return result;
    }
}
