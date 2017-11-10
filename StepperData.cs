using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraApp
{ 
    public struct StepperData
    {
        public double stepperPosition;
        public double stepperTime;
        public double stepperStatus { get; set; }

        //public readonly enum moving;

        public StepperData(
            double stepperStatus,
            double stepperPosition,
            double stepperTime
            )
        {
            this.stepperStatus = stepperStatus;
            this.stepperPosition = stepperPosition;
            this.stepperTime = stepperTime;
        }
    }
}
