using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PodPuppy
{
    class InterprocessCommunication
    {
        private const int WM_COPYDATA = 0x004a;

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static int SendMessage(System.IntPtr hWnd,
            int Msg, int wParam, ref CopyDataStruct lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct CopyDataStruct
        {
            public string ID;
            public int Length;
            public string Data;
        }

        public static void SendString(IntPtr destination, string message)
        {
            CopyDataStruct dataStruct = new CopyDataStruct();
            dataStruct.Data = message;
            dataStruct.Length = dataStruct.Data.Length;
            SendMessage(destination, WM_COPYDATA, 0, ref dataStruct);
        }

        public static string ProcessReceivedMessage(System.Windows.Forms.Message message)
        {
            if (message.Msg != WM_COPYDATA)
                return null;

            try
            {
                CopyDataStruct data = (CopyDataStruct)Marshal.PtrToStructure(message.LParam, typeof(CopyDataStruct));
                return data.Data.Substring(0, data.Length);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
