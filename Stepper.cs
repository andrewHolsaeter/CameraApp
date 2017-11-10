using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;

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
    public class Stepper
    {
        public bool _isMoving { get; private set; }
        private System.IO.Ports.SerialPort serialPortStepperMotor;
        public Stepper(string serialPortStepperString)
        {
            this.serialPortStepperMotor = new System.IO.Ports.SerialPort(serialPortStepperString, 9600); //changing this
            serialPortStepperMotor.Open();
            serialPortStepperMotor.DtrEnable = true;
            serialPortStepperMotor.DtrEnable = false;

            dataRing[ringPushIdx] = new StepperData(
            stepperStatus: 0,
            stepperPosition: 0,
            stepperTime: 0
            );
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
            var stepperData = new StepperData();
            //implement switch for sendType enum
            switch (stepperCommand)
            {
                case (int)SendType.STATUS:
                    stepperData.stepperStatus = data[1];
                    /*
                    stepperData = new StepperData(stepperPosition: data[0],stepperTime: data[1]);
                    Interlocked.Exchange(ref ringPushIdx, (ringPushIdx + 1) & 3);
                    dataRing[(ringPushIdx + 1) & 3] = stepperData; 
                    */
                    break;
              
            }
           
                  

        }
        internal SerialPortReceiver getReceiver(Action<String> p)
        {
            return new SerialPortReceiver(serialPortStepperMotor, p);
        }

        private readonly StepperData[] dataRing = new StepperData[3];
        private int ringPushIdx;

        public StepperData getData(SendType data)
        {
            int index = (int)(data);
            return dataRing[index];
        }

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


