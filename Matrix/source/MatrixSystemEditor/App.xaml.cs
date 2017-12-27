using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace MatrixSystemEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)  //for mutex as single application
        {
            base.OnStartup(e);
            string mutexName = "singleInstanceApplication_williamxia2017";
            bool createNew=false;
            _mutex = new Mutex(true, mutexName, out createNew);
            if(!createNew)
            {
                MessageBox.Show("is already running!");
                Shutdown();
            }
        }

        
  

    }

  


}
