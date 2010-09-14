using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace InfoControl.Multimedia
{
    public class Sound
    {
        private byte[] m_soundBytes;
        private string m_fileName;

        private enum Flags
        {
            Sync = 0x0000,  /* play synchronously (default) */
            Async = 0x0001,  /* play asynchronously */
            NoDefault = 0x0002,  /* silence (!default) if sound not found */
            Memory = 0x0004,  /* pszSound points to a memory file */
            Loop = 0x0008,  /* loop the sound until next sndPlaySound */
            NoStop = 0x0010,  /* don't stop any currently playing sound */
            NoWait = 0x00002000, /* don't wait if the driver is busy */
            Alias = 0x00010000, /* name is a registry alias */
            AliasId = 0x00110000, /* alias is a predefined ID */
            Filename = 0x00020000, /* name is file name */
            Resource = 0x00040004  /* name is resource name or atom */
        }

        [DllImport("CoreDll.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int WCE_PlaySound(string szSound, IntPtr hMod, int flags);

        [DllImport("CoreDll.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int WCE_PlaySoundBytes(byte[] szSound, IntPtr hMod, int flags);

        /// <summary>
        /// Construct the Sound object to play sound data from the specified file.
        /// </summary>
        public Sound(string fileName)
        {
            m_fileName = fileName;
        }

        /// <summary>
        /// Construct the Sound object to play sound data from the specified stream.
        /// </summary>
        public Sound(Stream stream)
        {
            // read the data from the stream
            m_soundBytes = new byte[stream.Length];
            stream.Read(m_soundBytes, 0, (int)stream.Length);
        }

        /// <summary>
        /// Play the sound
        /// </summary>
        public void Play()
        {
            // if a file name has been registered, call WCE_PlaySound, 
            //  otherwise call WCE_PlaySoundBytes
            if (m_fileName != null)
                WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.Async | Flags.Filename));
            else
                WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.Async | Flags.Memory));
        }
    }

}