using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntiDBG
{
    /*
     *    | Author : Arsium 
     *    | Sources : 
     *    
     *    https://www.geoffchappell.com/studies/windows/km/ntoskrnl/api/ps/psquery/class.htm
     *    https://www.pinvoke.net/default.aspx/ntdll/PROCESSINFOCLASS.html
     *    http://undocumented.ntinternals.net/index.html?page=UserMode%2FUndocumented%20Functions%2FNT%20Objects%2FThread%2FTHREAD_INFORMATION_CLASS.html
     *    https://ntquery.wordpress.com/2014/03/29/anti-debug-ntsetinformationthread/
     *    
     *    | Note : The value are in hexadecimal , I've translated to decimal.
     *    
     */

    public class AntiDebug
    {
        [DllImport("kernel32.dll",SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr ProcHHandle, out bool dwReason);
        
        [DllImport("Ntdll.dll",SetLastError =true)]
        private static extern uint NtSetInformationThread(IntPtr hThread, int ThreadInformationClass, IntPtr ThreadInformation, uint ThreadInformationLength);
        
        [DllImport("Kernel32.dll",SetLastError = true)]
        private static extern  IntPtr GetCurrentThread();

        public static void firsTech()
        {
            bool checkDebug;

            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, out checkDebug);

            if (checkDebug)
                    //MessageBox.Show("Stop Debugging !");
                return;
                
        }
        public static uint secondTech()
        {
            uint Status;

            Status = NtSetInformationThread(GetCurrentThread(), 17, IntPtr.Zero, 0);

            if (Status != 0)
            {

                string errorMsg = String.Format("Error with NtSetInformationThread : 0x{0:x} n", Status);
                //MessageBox.Show(errorMsg);
                return 0;
            }

            //MessageBox.Show("Hide from Debug is activated !");  //NTStatus = 0 : Success

            return 0;
        }

    }
}
