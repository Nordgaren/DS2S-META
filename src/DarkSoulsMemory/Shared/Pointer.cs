using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace DarkSoulsMemory.Shared
{
    public class Pointer
    {
        public Pointer(bool is64Bit, Process process, long baseAddress, params long[] offsets)
        {
            Process     = process;
            BaseAddress = baseAddress;
            Offsets     = offsets.ToList();
            Is64Bit     = is64Bit;
            //Kernel32.IsWow64Process(Process.Handle, out Is64Bit);
        }

        public Process Process;
        public long BaseAddress;
        public List<long> Offsets;
        public bool Is64Bit;

        private long ResolveOffsets(StringBuilder debugStringBuilder = null)
        {
            debugStringBuilder?.Append($" 0x{BaseAddress:x}");

            long ptr = BaseAddress;
            for (int i = 0; i < Offsets.Count; i++)
            {
                var offset = Offsets[i];

                //Create a copy for debug output
                var debugCopy = ptr;

                //Resolve an offset
                var address = ptr + offset;

                //Not the last offset = resolve as pointer
                int unused = 0;
                if (i + 1 < Offsets.Count)
                {
                    if (Is64Bit)
                    {
                        var buffer = new byte[8];
                        Kernel32.ReadProcessMemory(Process.Handle, (IntPtr)address, buffer, buffer.Length, ref unused);
                        ptr = BitConverter.ToInt64(buffer, 0);
                    }
                    else
                    {
                        var buffer = new byte[4];
                        Kernel32.ReadProcessMemory(Process.Handle, (IntPtr)address, buffer, buffer.Length, ref unused);
                        ptr = BitConverter.ToInt32(buffer, 0);
                    }
                    debugStringBuilder?.Append($"\r\n[0x{debugCopy:x} + 0x{offset:x}]: 0x{ptr:x}");
                }
                //Last offset = resolve as simple increment
                else
                {
                    ptr = address;
                    debugStringBuilder?.Append($"\r\n 0x{debugCopy:x} + 0x{offset:x}: 0x{ptr:x}");
                }
            }

            return ptr;
        }


        //Debug representation, shows in IDE
        public string Path { get; private set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            ResolveOffsets(sb);
            Path = sb.ToString();
            return Path;
        }

        #region Read/write memory

        private byte[] ReadMemory(long offset, int length)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[length];
            Kernel32.ReadProcessMemory(Process.Handle, (IntPtr)(ResolveOffsets() + offset), buffer, length, ref bytesRead);
            return buffer;
        }

        private void WriteMemory(long offset, byte[] bytes)
        {
            Kernel32.WriteProcessMemory(Process.Handle, (IntPtr)(ResolveOffsets() + offset), bytes, (uint)bytes.Length, 0);
        }

        #region Read
        public int ReadInt32(long offset = 0)
        {
            return BitConverter.ToInt32(ReadMemory(offset, 4), 0);
        }

        public long ReadInt64(long offset = 0)
        {
            return BitConverter.ToInt64(ReadMemory(offset, 8), 0);
        }

        public bool ReadBool(long offset = 0)
        {
            return BitConverter.ToBoolean(ReadMemory(offset, 1), 0);
        }
        public byte ReadByte(long offset = 0)
        {
            return ReadMemory(offset, 1)[0];
        }

        public byte[] ReadBytes(long offset, int size)
        {
            return ReadMemory(offset, size);
        }

        public float ReadFloat(long offset = 0)
        {
            return BitConverter.ToSingle(ReadMemory(offset, 4), 0);
        }

        #endregion

        #region Write

        public void WriteInt32(int value)
        {
            WriteInt32(0, value);
        }

        public void WriteInt32(long offset, int value)
        {
            WriteMemory(offset, BitConverter.GetBytes(value));
        }

        public void WriteInt64(long value)
        {
            WriteInt64(0, value);
        }
        public void WriteInt64(long offset, long value)
        {
            WriteMemory(offset, BitConverter.GetBytes(value));
        }

        public void WriteBool(bool value)
        {
            WriteBool(0, value);
        }
        public void WriteBool(long offset, bool value)
        {
            WriteMemory(offset, BitConverter.GetBytes(value));
        }

        public void WriteByte(byte value)
        {
            WriteByte(0, value);
        }

        public void WriteByte( long offset, byte value)
        {
            WriteMemory(offset, BitConverter.GetBytes(value));
        }

        public void WriteBytes(byte[] value)
        {
            WriteBytes(0, value);
        }

        public void WriteBytes(long offset, byte[] value)
        {
            WriteMemory(offset, value);
        }

        public void WriteFloat(float value)
        {
            WriteFloat(0, value);
        }

        public void WriteFloat(long offset, float value)
        {
            WriteMemory(offset, BitConverter.GetBytes(value));
        }

        #endregion

        #endregion
    }
}
