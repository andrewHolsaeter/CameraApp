using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;
using System.Data;

namespace CameraApp
{
    public enum CommandType
    {
        STOP = 1,
        CALIBRATE = 2,
        MOVE = 3,
        SLIDE = 4
    }
    public enum SendType
    {
        NONE = 0,
        STATUS = 1,
        POSITION = 2,
        DIRECTION = 3,
        STEPPERCOUNTER = 4
    }
    public enum StatusType
    {
        CREATED = 3,
        STOPPED = 0,
        MOVING = 1
    }

    public class Stepper
    {
        public bool isMoving { get; private set; }
        public double position { get; private set; }
        public string status { get; private set; }

        private System.IO.Ports.SerialPort serialPortStepperMotor;

        public Stepper(string serialPortStepperString)
        {
            this.serialPortStepperMotor = new System.IO.Ports.SerialPort(serialPortStepperString, 9600); //changing this
            serialPortStepperMotor.Open();
            serialPortStepperMotor.DtrEnable = true;
            serialPortStepperMotor.DtrEnable = false;
            status = "created";
        }
        public void updateData(SendType sendtype, double data)
        {
            int stepperCommand = (int)sendtype;
            switch (stepperCommand)
            {
                case (int)SendType.STATUS:
                    int stepperStatus = (int)data;
                    switch (stepperStatus)
                    {
                        case (int)StatusType.CREATED:
                            isMoving = false;
                            status = "created";
                            break;
                        case (int)StatusType.STOPPED:
                            isMoving = false;
                            status = "stopped";
                            break;
                        case (int)StatusType.MOVING:
                            isMoving = true;
                            status = "moving";
                            break;
                    }
                    break;
                case (int)SendType.POSITION:
                    position = data;
                    break;
                case (int)SendType.STEPPERCOUNTER:
                    //to do
                    break;      
            }

        }
        public void parseStepper(string completedBuffer)
        {
            double[] data = new double[2];
            string[] stepperInString = completedBuffer.Split('y');
            if (stepperInString.Length != 3) { return; }
            // we ignore the 3rd, it is always empty           
            for (int k = 0; k < 2; k++)
            {
                Double.TryParse(stepperInString[k], out data[k]);
            }
            Int32.TryParse(stepperInString[0],out int stepperCommand);
            updateData((SendType)data[0], data[1]);
                     
        }
        internal SerialPortReceiver getReceiver(Action<String> p)
        {
            return new SerialPortReceiver(serialPortStepperMotor, p);
        }     
        /*
        public StepperData getData()
        {
            StepperData s = new StepperData();
            return s; 
        }
        */     
        public void reopenPort()
        {
            if (!serialPortStepperMotor.IsOpen)
            {
                try
                {
                    serialPortStepperMotor.Open();
                }
                catch (Exception e)
                { }
            }
        }

        public void move(double distance, string direction, double speed)
        {
            //if (!Double.TryParse(direction, out dir)) return;
            int command = (int)CommandType.MOVE;
            string output = command + ";" + distance + ";" + direction + ";" + speed + ";";
            this.serialPortStepperMotor.WriteLine(output); //writing to the debugger gets weird when i do this
        }
    }
}


