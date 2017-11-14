using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CameraApp
{ 
    public struct StepperData 
    {
        //public string stepperName;
        public double stepperPosition { get; set; }
        public double stepperTime { get; set; }
        public int stepperStatus {
            get;
            set; }

        //public readonly enum moving;

        public StepperData(StepperData stepperData, int sendtype, double data) 
        {
            //this.stepperData = stepperName;
            switch (sendtype)
            {
                case 1:
                    stepperData.stepperStatus = (int)data;
                    stepperData.stepperTime = 6;
                    break;
            }
            stepperStatus = 0;
            stepperPosition = 0.0;
            stepperTime = 0.0;
        }
    }
}
