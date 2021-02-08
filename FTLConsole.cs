using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Brender_0_5
{
    [Serializable]
    public struct Pixel
    {
        public float zBuffering;
        public CharInfo charInfo;
    }
    
    [StructLayout(LayoutKind.Explicit)]
    [Serializable()]
    public struct CharUnion
    {
        [FieldOffset(0)] public char UnicodeChar;
        [FieldOffset(0)] public byte AsciiChar;

        public static bool operator ==(CharUnion cu0, CharUnion cu1)
        {
            return cu0.UnicodeChar == cu1.UnicodeChar && cu0.AsciiChar == cu1.AsciiChar;
        }
        
        public static bool operator !=(CharUnion cu0, CharUnion cu1)
        {
            return !(cu0 == cu1);
        }
    }
    
    [StructLayout(LayoutKind.Explicit)]
    [Serializable()]
    public struct CharInfo
    {
        [FieldOffset(0)] public CharUnion Char;
        [FieldOffset(2)] public short Attributes;
        
        public static bool operator ==(CharInfo c0, CharInfo c1)
        {
            return c0.Char == c1.Char && c0.Attributes == c1.Attributes;
        }

        public static bool operator !=(CharInfo c0, CharInfo c1)
        {
            return !(c0 == c1);
        }

        public CharInfo(char c, short foreground, short background)
        {
            Char = new CharUnion();
            Char.AsciiChar = (byte)c;
            Attributes = (short)(foreground | background << 4);
        }
    }

    public static class FTLConsole
    {

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        static SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

        [STAThread]
        public static void Render(CharInfo[,] canvas)
        {
            //SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            System.Diagnostics.Debug.WriteLine("Ended calculations: " + HolderClass.sw.Elapsed);
            HolderClass.sw.Restart();

            if (!h.IsInvalid)
            {
                short width = (short)canvas.GetLength(0);
                short height = (short)canvas.GetLength(1);

                /*for (int i = 0; i < height; i++)
                {
                    string line = "";

                    for (int j = 0; j < width; j++)
                    {
                        line += (char)canvas[j, i].Char.AsciiChar;
                    }

                    Console.WriteLine(line);
                }
                Console.Read();*/

                CharInfo[] buf = new CharInfo[width * height];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        buf[i * width + j] = canvas[j, i];
                    }
                }

                SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = width, Bottom = height };

                bool b = WriteConsoleOutput(h, buf,
                  new Coord() { X = width, Y = height },
                  new Coord() { X = 0, Y = 0 },
                  ref rect);
            }
        }
    }
}