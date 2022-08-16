using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.IO;

public class SerialReader : MonoBehaviour
{
    public GameObject Target;
    public Vector3 DeltaAcceleration;
    public Quaternion Quaternion;
    public bool ButtonPress;
    public bool ButtonDown;
    public bool JoystickLeft;
    public bool JoysticX;
    public bool JoystickRight;
    public bool JoystickFront;
    public bool JoysticY;
    public bool JoystickBack;

    private SerialPort serialPort = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
    private string data = null;
    List<string> csvlines = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (serialPort.IsOpen)
            {
                data = serialPort.ReadLine();
                serialPort.ReadTimeout = 30;

                if (data[0] == 'q')
                {
                    string[] quaternions = data.Split(' ');

                    //Debug.Log("q: " + quaternions[1] + ' ' + quaternions[2] + ' ' + quaternions[3] + ' ' + quaternions[4]);
                    Quaternion = new Quaternion(-float.Parse(quaternions[3]), -float.Parse(quaternions[4]), float.Parse(quaternions[2]), float.Parse(quaternions[1]));

                }

                if (data[0] == 'a')
                {
                    string[] acceleration = data.Split(' ');

                    //Debug.Log("a: " + deltaAcceleration[0] + ' ' + deltaAcceleration[1] + ' ' + deltaAcceleration[2]);
                    DeltaAcceleration.x = -float.Parse(acceleration[2]);
                    DeltaAcceleration.y = float.Parse(acceleration[3]);
                    DeltaAcceleration.z = float.Parse(acceleration[1]);

                    csvlines.Add(DeltaAcceleration.x.ToString() + "," + DeltaAcceleration.y.ToString() + "," + DeltaAcceleration.z.ToString());
                }

                if (data[0] == 'j')
                {
                    string[] joystick = data.Split(' ');
                    int joystickButton = int.Parse(joystick[3]);
                    int joystickX = int.Parse(joystick[1]);
                    int joystickY = int.Parse(joystick[2]);

                    if (joystickButton == 1)
                    {
                        if (!ButtonDown && !ButtonPress)
                        {
                            ButtonDown = true;
                        }

                        else
                        {
                            ButtonPress = true;
                            ButtonDown = false;
                        }
                    }
                    else
                    {
                        ButtonPress = false;
                    }

                    if (joystickX < 100)
                    {
                        if (!JoystickLeft && !JoysticX)
                        {
                            JoystickLeft = true;
                            //Debug.Log("left");
                        }

                        else
                        {
                            JoysticX = true;
                            JoystickLeft = false;
                        }
                    }
                    else if (joystickX > 900)
                    {
                        if (!JoystickRight && !JoysticX)
                        {
                            JoystickRight = true;
                            //Debug.Log("right");
                        }

                        else
                        {
                            JoysticX = true;
                            JoystickRight = false;
                        }
                    }
                    else 
                    {
                        JoysticX = false;
                    }

                    //=========================================
                    if (joystickY < 100)
                    {
                        if (!JoystickFront && !JoysticY)
                        {
                            JoystickFront = true;
                            //Debug.Log("left");
                        }

                        else
                        {
                            JoysticY = true;
                            JoystickFront = false;
                        }
                    }
                    else if (joystickY > 900)
                    {
                        if (!JoystickBack && !JoysticY)
                        {
                            JoystickBack = true;
                            //Debug.Log("right");
                        }

                        else
                        {
                            JoysticY = true;
                            JoystickBack = false;
                        }
                    }
                    else
                    {
                        JoysticY = false;
                    }
                }
            }
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void OnApplicationQuit()
    {
        //File.Open(@"objectRotationTranslation.csv",)
        using (StreamWriter file = new StreamWriter(@"accleration.csv"))
        {
            file.WriteLine("ax,ay,az");
            for (int i = 0; i < csvlines.Count; i++)
            {
                file.WriteLine(csvlines[i]);
            }
            file.Close();
        }
        serialPort.Close();
    }
}
