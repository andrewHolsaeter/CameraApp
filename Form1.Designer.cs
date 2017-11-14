namespace CameraApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCapture = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.textBoxShutterSpeed = new System.Windows.Forms.TextBox();
            this.textBoxAperature = new System.Windows.Forms.TextBox();
            this.textBoxEV = new System.Windows.Forms.TextBox();
            this.labelEV = new System.Windows.Forms.Label();
            this.labelShutterSpeed = new System.Windows.Forms.Label();
            this.labelAperature = new System.Windows.Forms.Label();
            this.buttonTimeLapse = new System.Windows.Forms.Button();
            this.textBoxStepper = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelBatteryLevel = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.comboBoxStepperPort = new System.Windows.Forms.ComboBox();
            this.buttonOpenPorts = new System.Windows.Forms.Button();
            this.labelStepperStatus = new System.Windows.Forms.Label();
            this.textBoxCameraMode = new System.Windows.Forms.TextBox();
            this.textBoxDistance = new System.Windows.Forms.TextBox();
            this.textBoxSpeed = new System.Windows.Forms.TextBox();
            this.comboBoxDirection = new System.Windows.Forms.ComboBox();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxStepCount = new System.Windows.Forms.TextBox();
            this.labelStepCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCapture
            // 
            this.buttonCapture.Location = new System.Drawing.Point(172, 81);
            this.buttonCapture.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(142, 97);
            this.buttonCapture.TabIndex = 2;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(338, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1275, 780);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 56);
            this.label1.TabIndex = 4;
            this.label1.Text = "No Camera Attached";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(172, 190);
            this.buttonPreview.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(142, 97);
            this.buttonPreview.TabIndex = 5;
            this.buttonPreview.Text = "Preview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // textBoxShutterSpeed
            // 
            this.textBoxShutterSpeed.Enabled = false;
            this.textBoxShutterSpeed.Location = new System.Drawing.Point(172, 452);
            this.textBoxShutterSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxShutterSpeed.Name = "textBoxShutterSpeed";
            this.textBoxShutterSpeed.Size = new System.Drawing.Size(144, 31);
            this.textBoxShutterSpeed.TabIndex = 7;
            // 
            // textBoxAperature
            // 
            this.textBoxAperature.Enabled = false;
            this.textBoxAperature.Location = new System.Drawing.Point(172, 407);
            this.textBoxAperature.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxAperature.Name = "textBoxAperature";
            this.textBoxAperature.Size = new System.Drawing.Size(144, 31);
            this.textBoxAperature.TabIndex = 8;
            // 
            // textBoxEV
            // 
            this.textBoxEV.Enabled = false;
            this.textBoxEV.Location = new System.Drawing.Point(172, 496);
            this.textBoxEV.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEV.Name = "textBoxEV";
            this.textBoxEV.Size = new System.Drawing.Size(144, 31);
            this.textBoxEV.TabIndex = 9;
            // 
            // labelEV
            // 
            this.labelEV.AutoSize = true;
            this.labelEV.Location = new System.Drawing.Point(17, 501);
            this.labelEV.Name = "labelEV";
            this.labelEV.Size = new System.Drawing.Size(103, 25);
            this.labelEV.TabIndex = 10;
            this.labelEV.Text = "Exposure";
            // 
            // labelShutterSpeed
            // 
            this.labelShutterSpeed.AutoSize = true;
            this.labelShutterSpeed.Location = new System.Drawing.Point(17, 456);
            this.labelShutterSpeed.Name = "labelShutterSpeed";
            this.labelShutterSpeed.Size = new System.Drawing.Size(149, 25);
            this.labelShutterSpeed.TabIndex = 11;
            this.labelShutterSpeed.Text = "Shutter Speed";
            // 
            // labelAperature
            // 
            this.labelAperature.AutoSize = true;
            this.labelAperature.Location = new System.Drawing.Point(17, 412);
            this.labelAperature.Name = "labelAperature";
            this.labelAperature.Size = new System.Drawing.Size(106, 25);
            this.labelAperature.TabIndex = 12;
            this.labelAperature.Text = "Aperature";
            // 
            // buttonTimeLapse
            // 
            this.buttonTimeLapse.Location = new System.Drawing.Point(172, 298);
            this.buttonTimeLapse.Name = "buttonTimeLapse";
            this.buttonTimeLapse.Size = new System.Drawing.Size(142, 97);
            this.buttonTimeLapse.TabIndex = 13;
            this.buttonTimeLapse.Text = "TimeLapse";
            this.buttonTimeLapse.UseVisualStyleBackColor = true;
            this.buttonTimeLapse.Click += new System.EventHandler(this.buttonTimeLapse_Click);
            // 
            // textBoxStepper
            // 
            this.textBoxStepper.Enabled = false;
            this.textBoxStepper.Location = new System.Drawing.Point(172, 660);
            this.textBoxStepper.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxStepper.Name = "textBoxStepper";
            this.textBoxStepper.Size = new System.Drawing.Size(140, 30);
            this.textBoxStepper.TabIndex = 14;
            this.textBoxStepper.Text = "Disconnected";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(172, 24);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(142, 40);
            this.progressBar1.TabIndex = 16;
            // 
            // labelBatteryLevel
            // 
            this.labelBatteryLevel.BackColor = System.Drawing.Color.Transparent;
            this.labelBatteryLevel.Location = new System.Drawing.Point(172, 24);
            this.labelBatteryLevel.Margin = new System.Windows.Forms.Padding(0);
            this.labelBatteryLevel.Name = "labelBatteryLevel";
            this.labelBatteryLevel.Size = new System.Drawing.Size(142, 81);
            this.labelBatteryLevel.TabIndex = 15;
            this.labelBatteryLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonTest
            // 
            this.buttonTest.Enabled = false;
            this.buttonTest.Location = new System.Drawing.Point(501, 933);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(142, 40);
            this.buttonTest.TabIndex = 17;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // comboBoxStepperPort
            // 
            this.comboBoxStepperPort.FormattingEnabled = true;
            this.comboBoxStepperPort.Location = new System.Drawing.Point(9, 605);
            this.comboBoxStepperPort.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxStepperPort.Name = "comboBoxStepperPort";
            this.comboBoxStepperPort.Size = new System.Drawing.Size(140, 40);
            this.comboBoxStepperPort.TabIndex = 18;
            // 
            // buttonOpenPorts
            // 
            this.buttonOpenPorts.Location = new System.Drawing.Point(172, 605);
            this.buttonOpenPorts.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOpenPorts.Name = "buttonOpenPorts";
            this.buttonOpenPorts.Size = new System.Drawing.Size(142, 40);
            this.buttonOpenPorts.TabIndex = 19;
            this.buttonOpenPorts.Text = "Open";
            this.buttonOpenPorts.UseVisualStyleBackColor = true;
            this.buttonOpenPorts.Click += new System.EventHandler(this.buttonOpenPorts_Click);
            // 
            // labelStepperStatus
            // 
            this.labelStepperStatus.AutoSize = true;
            this.labelStepperStatus.Location = new System.Drawing.Point(17, 660);
            this.labelStepperStatus.Name = "labelStepperStatus";
            this.labelStepperStatus.Size = new System.Drawing.Size(73, 25);
            this.labelStepperStatus.TabIndex = 20;
            this.labelStepperStatus.Text = "Status";
            // 
            // textBoxCameraMode
            // 
            this.textBoxCameraMode.Enabled = false;
            this.textBoxCameraMode.Location = new System.Drawing.Point(9, 81);
            this.textBoxCameraMode.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxCameraMode.Name = "textBoxCameraMode";
            this.textBoxCameraMode.Size = new System.Drawing.Size(144, 31);
            this.textBoxCameraMode.TabIndex = 21;
            // 
            // textBoxDistance
            // 
            this.textBoxDistance.Enabled = false;
            this.textBoxDistance.Location = new System.Drawing.Point(501, 890);
            this.textBoxDistance.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDistance.Name = "textBoxDistance";
            this.textBoxDistance.Size = new System.Drawing.Size(144, 31);
            this.textBoxDistance.TabIndex = 22;
            this.textBoxDistance.Text = "200";
            // 
            // textBoxSpeed
            // 
            this.textBoxSpeed.Enabled = false;
            this.textBoxSpeed.Location = new System.Drawing.Point(338, 938);
            this.textBoxSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxSpeed.Name = "textBoxSpeed";
            this.textBoxSpeed.Size = new System.Drawing.Size(144, 31);
            this.textBoxSpeed.TabIndex = 23;
            this.textBoxSpeed.Text = "1000";
            // 
            // comboBoxDirection
            // 
            this.comboBoxDirection.FormattingEnabled = true;
            this.comboBoxDirection.Location = new System.Drawing.Point(338, 888);
            this.comboBoxDirection.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxDirection.Name = "comboBoxDirection";
            this.comboBoxDirection.Size = new System.Drawing.Size(144, 33);
            this.comboBoxDirection.TabIndex = 24;
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Enabled = false;
            this.textBoxPosition.Location = new System.Drawing.Point(172, 705);
            this.textBoxPosition.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.Size = new System.Drawing.Size(140, 30);
            this.textBoxPosition.TabIndex = 25;
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(17, 711);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(89, 25);
            this.labelPosition.TabIndex = 26;
            this.labelPosition.Text = "Position";
            // 
            // textBoxStepCount
            // 
            this.textBoxStepCount.Enabled = false;
            this.textBoxStepCount.Location = new System.Drawing.Point(172, 750);
            this.textBoxStepCount.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxStepCount.Name = "textBoxStepCount";
            this.textBoxStepCount.Size = new System.Drawing.Size(140, 30);
            this.textBoxStepCount.TabIndex = 27;
            // 
            // labelStepCount
            // 
            this.labelStepCount.AutoSize = true;
            this.labelStepCount.Location = new System.Drawing.Point(17, 756);
            this.labelStepCount.Name = "labelStepCount";
            this.labelStepCount.Size = new System.Drawing.Size(119, 25);
            this.labelStepCount.TabIndex = 28;
            this.labelStepCount.Text = "Step Count";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1729, 1134);
            this.Controls.Add(this.labelStepCount);
            this.Controls.Add(this.textBoxStepCount);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.textBoxPosition);
            this.Controls.Add(this.comboBoxDirection);
            this.Controls.Add(this.textBoxSpeed);
            this.Controls.Add(this.textBoxDistance);
            this.Controls.Add(this.textBoxCameraMode);
            this.Controls.Add(this.labelStepperStatus);
            this.Controls.Add(this.buttonOpenPorts);
            this.Controls.Add(this.comboBoxStepperPort);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.labelBatteryLevel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBoxStepper);
            this.Controls.Add(this.buttonTimeLapse);
            this.Controls.Add(this.labelAperature);
            this.Controls.Add(this.labelShutterSpeed);
            this.Controls.Add(this.labelEV);
            this.Controls.Add(this.textBoxEV);
            this.Controls.Add(this.textBoxAperature);
            this.Controls.Add(this.textBoxShutterSpeed);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonCapture);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.TextBox textBoxShutterSpeed;
        private System.Windows.Forms.TextBox textBoxAperature;
        private System.Windows.Forms.TextBox textBoxEV;
        private System.Windows.Forms.Label labelEV;
        private System.Windows.Forms.Label labelShutterSpeed;
        private System.Windows.Forms.Label labelAperature;
        private System.Windows.Forms.Button buttonTimeLapse;
        private System.Windows.Forms.TextBox textBoxStepper;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelBatteryLevel;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.ComboBox comboBoxStepperPort;
        private System.Windows.Forms.Button buttonOpenPorts;
        private System.Windows.Forms.Label labelStepperStatus;
        private System.Windows.Forms.TextBox textBoxCameraMode;
        private System.Windows.Forms.TextBox textBoxDistance;
        private System.Windows.Forms.TextBox textBoxSpeed;
        private System.Windows.Forms.ComboBox comboBoxDirection;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox textBoxStepCount;
        private System.Windows.Forms.Label labelStepCount;
    }

}

