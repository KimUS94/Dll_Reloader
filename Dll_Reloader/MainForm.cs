using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dll_Reloader
{
    /*
     * Dot Net DLL re-load test code
     */
    public partial class MainForm : Form
    {
        private string m_path;

        public MainForm()
        {
            InitializeComponent();

            m_path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "ClassLibrary.dll");
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            object result;

            result = MashallLoad();

            MessageBox.Show(result.ToString());
        }

        private object MashallLoad()
        {
            try
            {
                //Create 'New AppDomain' to load 'Assembly'
                AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
                AppDomain newDomain = AppDomain.CreateDomain("NewDom", AppDomain.CurrentDomain.Evidence, setup);

                //Create instance of 'AssemblyLoader' class in new appdomain  
                System.Runtime.Remoting.ObjectHandle obj = newDomain.CreateInstance(typeof(AssemblyLoader).Assembly.FullName, typeof(AssemblyLoader).FullName);

                //The instance we created came from a different AppDomain
                //We will get that instance in wrapped format
                //So we have to unwrap it
                AssemblyLoader loader = (AssemblyLoader)obj.Unwrap();

                //Call loadassembly method 
                //so that the assembly will be loaded into the new appdomain and the object will also remain in the new appdomain only.  
                loader.Load(m_path);

                //Call exceuteMethod and pass the name of the method from assembly and the parameters.  
                object result = loader.ExecuteMethod("Class", "TestMethod", null);

                //After the method has been executed call the unload method of the appdomain.  
                AppDomain.Unload(newDomain);

                return result;
            }
            catch(Exception ex)
            {
                //
                return null;
            }
        }


    }
}
