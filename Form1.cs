using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

using Nikon;
using System.IO;
using System.Diagnostics;
using System.IO.Ports;


//to do:
//add button to stop (stepper and timelapse)
//add sender to image ready (timelapse or regular capture)
//Add option to name folder for timelapses and create folder and name photofiles that + appender
//check if that option is already existing to prevent overwrite
//make file to store number of captures already existing, then find max of that, and append from there for captures
//find way to get current camera mode
//add ways to manually set aperature, shutterspeed, iso etc dependent on current camera mode...


namespace CameraApp
{
    public partial class Form1 : Form
    {
        private NikonManager manager;
        private NikonDevice device;
        private Timer liveViewTimer;
        private Timer screenRefreshTimer;
        private SerialPortReceiver stepperReceiver;

        int captureAppender = 0;
        int timelapseAppender = 0;
        string temppath = @"C:\Users\hols\Desktop\tmp.jpg";
        string rawpath = @"C:\Users\hols\Desktop\Computer App Photos\";
        string saveType = "";

        //timelapse
        private Timer timelapseTimer;
        int timelapseCounter;
        int numberOfPictures;
        bool timeLapseActive = false;

        //timer to see how long it takes to capture image
        Stopwatch timer = new Stopwatch();

        Stepper stepperMotor = null;

        private Timer testTimer;

        public Form1()
        {
            InitializeComponent();
            
            //Disable buttons
            ToggleButtons(false);
            
            // Initialize the live view timer
            liveViewTimer = new Timer();
            liveViewTimer.Tick += new EventHandler(liveViewTimer_Tick);
            liveViewTimer.Interval = 1000 / 30; //maybe change this to 1000/24 because fps

            //initialize UI refresh timer. See if there is a better way to do this
            screenRefreshTimer = new Timer();
            screenRefreshTimer.Tick += new EventHandler(screenRefreshTimer_Tick);
            screenRefreshTimer.Interval = 500;

            // Initialize Nikon manager
            manager = new NikonManager("Type0004.md3");
            manager.DeviceAdded += new DeviceAddedDelegate(manager_DeviceAdded);
            manager.DeviceRemoved += new DeviceRemovedDelegate(manager_DeviceRemoved);
            
            //initiliaze timer for for timelapse
            timelapseTimer = new Timer();
            timelapseTimer.Tick += new EventHandler(timelapseTimer_Tick);
            timelapseTimer.Enabled = false;

            //initiliaze serial port for stepper motor
            string[] serialPorts = System.IO.Ports.SerialPort.GetPortNames();
            comboBoxStepperPort.Items.AddRange(serialPorts);
            comboBoxStepperPort.SelectedIndex = comboBoxStepperPort.FindStringExact("COM3");

            //initialize test timer
            testTimer = new Timer();
            testTimer.Tick += new EventHandler(testRefreshTimer_Tick);
            testTimer.Enabled = false;
        }

        private void testRefreshTimer_Tick(object sender, EventArgs e)
        {
            string status = stepperMotor.status;
            bool moving = stepperMotor.isMoving;
            textBoxStepper.Text = status;
        }

        
        private void timelapseTimer_Tick(object sender, EventArgs e)
        {
            //check to see if number of pictures has been reached
            if(timelapseCounter == numberOfPictures){
                timelapseTimer.Enabled = false;     //turn off timer to preserve resources
                timeLapseActive = false;
                ToggleButtons(true);
                return;
            }
            try
            {
                device.Capture();
                timelapseCounter++;
            }
            catch
            {

            }          
        }

        private void screenRefreshTimer_Tick(object sender, EventArgs e)
        {
            if(stepperMotor != null)
            {
                try
                {
                    string status = stepperMotor.status;
                    bool moving = stepperMotor.isMoving;
                    textBoxStepper.Text = status;
                }
                catch
                {

                }
            }
            try
            {
                NikonEnum aperture = device.GetEnum(eNkMAIDCapability.kNkMAIDCapability_Aperture);
                float exposure = (float)device.GetFloat(eNkMAIDCapability.kNkMAIDCapability_ExposureStatus);
                NikonEnum shutterspeed = device.GetEnum(eNkMAIDCapability.kNkMAIDCapability_ShutterSpeed);
                int batterylevel = (int)device.GetInteger(eNkMAIDCapability.kNkMAIDCapability_BatteryLevel);
              
                progressBar1.Value = Convert.ToInt32(batterylevel);
                labelBatteryLevel.Text = batterylevel.ToString() + "%";

                textBoxShutterSpeed.Text = shutterspeed.ToString();
                textBoxAperature.Text = aperture.ToString();
                textBoxEV.Text = exposure.ToString();
            }
            catch
            {
               // MessageBox.Show("Unable to get data");
            }
        }

        void liveViewTimer_Tick(object sender, EventArgs e)
        {
            if (timeLapseActive) return;

            //Get live view image
            NikonLiveViewImage image = null;

            try
            {
                image = device.GetLiveViewImage();              
            }
            catch
            {
                liveViewTimer.Stop();
            }

            // Set live view image on picture box
            if (image != null)
            {
                MemoryStream stream = new MemoryStream(image.JpegBuffer);
                pictureBox1.Image = Image.FromStream(stream);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            }
        }
        void manager_DeviceAdded(NikonManager sender, NikonDevice device)
        {
            this.device = device;

            //to do: set the device name
            label1.Text = device.Name;

            ToggleButtons(true);
            //Hook up device capture events
            device.ImageReady += new ImageReadyDelegate(device_ImageReady);
            device.CaptureComplete += new CaptureCompleteDelegate(device_CaptureComplete);
            device.CapabilityValueChanged += new CapabilityChangedDelegate(device_CapValueChange);
            //this will writre to sd card, computer or both
            device.SetUnsigned(eNkMAIDCapability.kNkMAIDCapability_SaveMedia,
            (uint)eNkMAIDSaveMedia.kNkMAIDSaveMedia_SDRAM);

            screenRefreshTimer.Start();
            
        }
        int capValueCounter = 0;
        private void device_CapValueChange(NikonDevice sender, eNkMAIDCapability capability)
        {
            string cap = capability.ToString();
            capValueCounter++;
            //MessageBox.Show(capValueCounter.ToString());
        }

        void manager_DeviceRemoved(NikonManager sender, NikonDevice device)
        {
            this.device = null;
            //stop live view timer
            liveViewTimer.Stop();

            //clear device name
            label1.Text = "No Camera Attached";
            labelBatteryLevel.Text = "";

            //disable buttons
            ToggleButtons(false);

            //clear picture box
            pictureBox1.Image = null;
            
            //stop refreshing screen
            screenRefreshTimer.Stop();
        }
        void device_ImageReady(NikonDevice sender, NikonImage image)
        {          
            //save preview
            if(image.Type == NikonImageType.Jpeg)
            {
                using (FileStream stream = new FileStream(temppath, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(image.Buffer, 0, image.Buffer.Length);
                }
                //preview the jpeg image
                pictureBox1.ImageLocation = temppath;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            if (image.Type == NikonImageType.Raw)
            {
                bool saveImage = false;
                if (timeLapseActive)
                {
                    saveImage = true;
                    timelapseAppender++; //append to keep track and avoid overwrite
                    saveType = "timelapse-" + timelapseAppender.ToString();
                }
                //prompt to save image if not timelapse               
                else 
                {
                    if (MessageBox.Show("Would you like to save this image?", "caption", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        saveImage = true;
                        captureAppender++; //append to keep track and avoid overwrite
                        saveType = "capture-" + captureAppender.ToString();
                    }
                }
                if(saveImage == true)
                {                    
                    //set savepath
                    string currentPath = rawpath + saveType + ((image.Type == NikonImageType.Raw) ? ".NEF" : ".jpg");
                    //save photo
                    using (FileStream stream = new FileStream(currentPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.Write(image.Buffer, 0, image.Buffer.Length);
                    }
                }              
            }
            string elapseTime = timer.Elapsed.TotalSeconds.ToString();
           // MessageBox.Show(String.Format(@"Time it took to process {0}, was {1} seconds", image.Type.ToString(), elapseTime));
        }

        public void device_CaptureComplete(NikonDevice sender, int data)
        {
            timer.Stop();
            if (timeLapseActive)
            {
                if (stepperMotor.isMoving)
                {
                    MessageBox.Show("Cannot perform. Moving");
                    return;
                }

                //if moving time lapse
                stepperMotor.move(1000, "1", 1000);

            }
            //re-enable buttons
            ToggleButtons(true);           
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {            
            timer.Start(); //timer to see how long a capture takes
            if (device == null) return;

            if (stepperMotor != null && stepperMotor.isMoving) return;

            ToggleButtons(false);

            try
            {
                device.Capture();
            }
            catch (NikonException ex)
            {
                MessageBox.Show(ex.Message);
                ToggleButtons(true);
            }

        }
        void ToggleButtons(bool status)
        {
                buttonCapture.Enabled = status;
                buttonPreview.Enabled = status;
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (device == null) return;

            //is on, means we want to turn it off
            //backwards because its a toggle switch
            if (device.LiveViewEnabled)
            {
                device.LiveViewEnabled = false;
                liveViewTimer.Stop();
                pictureBox1.Image = null;
                buttonCapture.Enabled = true;
            }
            else
            {
                buttonCapture.Enabled = false;
                device.LiveViewEnabled = true;
                liveViewTimer.Start();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //disable live view if it is on
            if (device != null)
            {
                device.LiveViewEnabled = false;
            }

            //shut down nikon manager
            manager.Shutdown();
            base.OnClosing(e);
        }

        private void buttonTimeLapse_Click(object sender, EventArgs e)
        {
            if (timeLapseActive) return;

            if (device == null) return;

            if(MessageBox.Show("Enter TimeLapes?", "caption", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                timeLapseActive = true;
            }
    
            //prompt user for inputs
            numberOfPictures = Convert.ToInt32(Microsoft.VisualBasic.Interaction.InputBox("Number of Pictures",
                       "Inputs", "", 10, 50));
            int pictureInterval = Convert.ToInt32(Microsoft.VisualBasic.Interaction.InputBox("Time Between Photos",
                        "Inputs", "", 10, 50));
            if (pictureInterval < 3 || pictureInterval > 60) //min determined by write speed of camera. max for typos
            {
                return;
            }
            timelapseTimer.Enabled = true;
            timelapseTimer.Interval = pictureInterval*1000; //set the time for the timer to fire in milliseconds
            timeLapseActive = true;
            timelapseCounter = 0;
            ToggleButtons(false);
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            //move 1000 steps(5 rotations at 200 steps/rev), clockwise (1), at a speed of 500 Hz
            stepperMotor.move(1000, "1", 500);
        }

        private void buttonOpenPorts_Click(object sender, EventArgs e)
        {
            if (stepperMotor == null)
            {
                stepperMotor = new Stepper("COM3");              
                stepperReceiver = stepperMotor.getReceiver((string s) => stepperMotor.parseStepper(s));
                buttonOpenPorts.Enabled = false;
                testTimer.Enabled = true;
                testTimer.Interval = 200;
            }
        }
    }

}
