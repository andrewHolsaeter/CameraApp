using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Nikon;
using System.Threading;
using System.ComponentModel;

namespace CameraApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    class DemoCapabilities
    {
        NikonDevice _device;
        AutoResetEvent _waitForDevice = new AutoResetEvent(false);

        public void Run()
        {
            try
            {
                // Create manager object - make sure you have the correct MD3 file for your Nikon DSLR (see https://sdk.nikonimaging.com/apply/)
                NikonManager manager = new NikonManager("Type0004.md3");

                // Listen for the 'DeviceAdded' event
                manager.DeviceAdded += manager_DeviceAdded;

                // Wait for a device to arrive
                _waitForDevice.WaitOne();

                // Get 'info' struct for each supported capability
                NkMAIDCapInfo[] caps = _device.GetCapabilityInfo();

                // Iterate through all supported capabilities
                foreach (NkMAIDCapInfo cap in caps)
                {
                    // Print ID, description and type
                    Console.WriteLine(string.Format("{0, -14}: {1}", "Id", cap.ulID.ToString()));
                    Console.WriteLine(string.Format("{0, -14}: {1}", "Description", cap.GetDescription()));
                    Console.WriteLine(string.Format("{0, -14}: {1}", "Type", cap.ulType.ToString()));

                    // Try to get the capability value
                    string value = null;

                    // First, check if the capability is readable
                    if (cap.CanGet())
                    {
                        // Choose which 'Get' function to use, depending on the type
                        switch (cap.ulType)
                        {
                            case eNkMAIDCapType.kNkMAIDCapType_Unsigned:
                                value = _device.GetUnsigned(cap.ulID).ToString();
                                break;

                            case eNkMAIDCapType.kNkMAIDCapType_Integer:
                                value = _device.GetInteger(cap.ulID).ToString();
                                break;

                            case eNkMAIDCapType.kNkMAIDCapType_String:
                                value = _device.GetString(cap.ulID);
                                break;

                            case eNkMAIDCapType.kNkMAIDCapType_Boolean:
                                value = _device.GetBoolean(cap.ulID).ToString();
                                break;

                                // Note: There are more types - adding the rest is left
                                //       as an exercise for the reader.
                        }
                    }

                    // Print the value
                    if (value != null)
                    {
                        Console.WriteLine(string.Format("{0, -14}: {1}", "Value", value));
                    }

                    // Print spacing between capabilities
                    Console.WriteLine();
                    Console.WriteLine();
                }

                // Shutdown
                manager.Shutdown();
            }
            catch (NikonException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void manager_DeviceAdded(NikonManager sender, NikonDevice device)
        {
            if (_device == null)
            {
                // Save device
                _device = device;

                // Signal that we got a device
                _waitForDevice.Set();
            }
        }
    }

    abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    class Capabilities : ViewModelBase
    {
        NikonBase _object;
        NkMAIDCapInfo _cap;

        public Capabilities(NikonBase obj, NkMAIDCapInfo cap)
        {
            _object = obj;
            _cap = cap;
            NikonDevice device = _object as NikonDevice;
            if (device != null)
            {
                device.CapabilityValueChanged += new CapabilityChangedDelegate(device_CapabilityValueChanged);
            }
        }

        void device_CapabilityValueChanged(NikonDevice sender, eNkMAIDCapability capability)
        {
            throw new NotImplementedException();
        }
    }
}
