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
    public class Stepper
    {
        public bool _isMoving { get; private set; }
        private System.IO.Ports.SerialPort serialPortStepperMotor;
        private readonly StepperData[] dataRing = new StepperData[3];
        private int ringPushIdx;
        StepperData stepperMotorData = new StepperData();
        public Stepper(string serialPortStepperString)
        {
            this.serialPortStepperMotor = new System.IO.Ports.SerialPort(serialPortStepperString, 9600); //changing this
            serialPortStepperMotor.Open();
            serialPortStepperMotor.DtrEnable = true;
            serialPortStepperMotor.DtrEnable = false;
            DataTable stepperTable = CreateTable();
            DataTable stepperDetailTable = CreateStepperDetailTable();

            DataSet stepperSet = new DataSet();
            stepperSet.Tables.Add(stepperTable);
            stepperSet.Tables.Add(stepperDetailTable);

            // Set the relations between the tables and create the related constraint.
            stepperSet.Relations.Add("StepperStepperDetail", stepperTable.Columns["Stepper ID"], stepperDetailTable.Columns["Stepper ID"], true);
            /*dataRing[ringPushIdx] = new StepperData(
            stepperStatus: 0,
            stepperPosition: 0,
            stepperTime: 0
            );*/

        }
        private static DataTable CreateTable()
        {
            DataTable dataTableStepper = new DataTable();
            DataRow status = dataTableStepper.NewRow();

            DataColumn colValue = new DataColumn("Stepper ID", typeof(double));
            dataTableStepper.Columns.Add(colValue);

            DataColumn colTime = new DataColumn("Time", typeof(DateTime));
            dataTableStepper.Columns.Add(colTime);

            // Set the OrderId column as the primary key.
            dataTableStepper.PrimaryKey = new DataColumn[] { colValue };

            return dataTableStepper;
        }
        private static DataTable CreateStepperDetailTable()
        {
            DataTable stepperDetailTable = new DataTable("stepperDetail");

            // Define all the columns once.
            DataColumn[] cols ={
                                  new DataColumn("Stepper ID",typeof(String)),
                                  new DataColumn("Status",typeof(String)),
                                  new DataColumn("Position",typeof(Double)),
                                  new DataColumn("Direction",typeof(String)),
                                  new DataColumn("Remaining",typeof(Double)),
                              };

            stepperDetailTable.Columns.AddRange(cols);
            stepperDetailTable.PrimaryKey = new DataColumn[] { stepperDetailTable.Columns["StepperDetailValue"] };
            return stepperDetailTable;
        }
        private static void InsertOrders(DataTable orderTable)
        {
            // Add one row once.
            DataRow row1 = orderTable.NewRow();
            row1["Stepper ID"] = "Stepper 1";
            row1["Time"] = new DateTime();
            orderTable.Rows.Add(row1);
        }
        private static void InsertStepperDetails(DataTable orderDetailTable)
        {
            // Use an Object array to insert all the rows .
            // Values in the array are matched sequentially to the columns, based on the order in which they appear in the table.
            Object[] rows = {
                                 new Object[]{1,"Stepper 1","Mountain Bike",1419.5,36},
                                 new Object[]{2,"O0001","Road Bike",1233.6,16},
                                 new Object[]{3,"O0001","Touring Bike",1653.3,32},
                                 new Object[]{4,"O0002","Mountain Bike",1419.5,24},
                                 new Object[]{5,"O0002","Road Bike",1233.6,12},
                                 new Object[]{6,"O0003","Mountain Bike",1419.5,48},
                                 new Object[]{7,"O0003","Touring Bike",1653.3,8},
                             };

            foreach (Object[] row in rows)
            {
                orderDetailTable.Rows.Add(row);
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
            
            //implement switch for sendType enum
            switch (stepperCommand)
            {
                case (int)SendType.STATUS:
                    stepperMotorData.stepperStatus = (int)data[1];
                    stepperMotorData.stepperTime = 6;
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
        
        public StepperData getData()
        {
            StepperData s = new StepperData();
            return s; 
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


