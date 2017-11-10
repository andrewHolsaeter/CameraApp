using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;

namespace CameraApp
{
    internal class SerialPortReceiver
    {
        private SerialPort serialPort;
        int state = 0;
        private string inProgressBuffer;
        private Action<String> parser;

        public SerialPortReceiver(SerialPort serialPort, Action<String> parser)
        {
            this.serialPort = serialPort;
            this.parser = parser;
            serialPort.DataReceived += SerialDataReceived;
        }

        public void SerialDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadExisting();
            foreach (char c in data)
            {
                if (state == 0 && c == 'd')
                {
                    state = 2;
                    //Debug.Write(c);
                }
                else if (state == 2 && c == 'e')
                {
                    state = 0;
                }
                else if (state == 0 && c != 'x')
                {
                    // drop the data
                }
                else if (state == 0 && c == 'x')
                {
                    state = 1;
                    inProgressBuffer = String.Empty;
                    //inProgressBuffer += c;
                }
                else if (state == 1 && c != 'z')
                {
                    inProgressBuffer += c;
                }
                else if (state == 1 && c == 'z')
                {
                    parser.Invoke(inProgressBuffer);
                    //this.Invoke((MethodInvoker)delegate { parseCirculationData(inProgressBuffer); });
                    state = 0;
                }
            }
        }
    }
}