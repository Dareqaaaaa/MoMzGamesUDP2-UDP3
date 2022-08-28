﻿/*
 * Arquivo: SendPacket.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 03/08/2017
 * Sinta inveja, não nos atinge
 */

using Microsoft.Win32.SafeHandles;
using SharpDX;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Battle.network
{
    public class SendPacket : IDisposable
    {
        public MemoryStream mstream = new MemoryStream();
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            mstream.Dispose();
            if (disposing)
                handle.Dispose();
            disposed = true;
        }
        public byte[] GetArray()
        {
            if (mstream == null)
                return new byte[0];
            return mstream.ToArray();
        }
        protected internal void writeB(byte[] value)
        {
            mstream.Write(value, 0, value.Length);
        }
        protected internal void writeD(int value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeD(uint value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeH(short val)
        {
            writeB(BitConverter.GetBytes(val));
        }
        protected internal void writeH(ushort val)
        {
            writeB(BitConverter.GetBytes(val));
        }
        /// <summary>
        /// True = 1; False = 0.
        /// </summary>
        /// <param name="value"></param>
        protected internal void writeC(bool value)
        {
            mstream.WriteByte(Convert.ToByte(value));
        }
        protected internal void writeC(byte value)
        {
            mstream.WriteByte(value);
        }
        protected internal void writeF(double value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeT(float value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeQ(long value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeHVector(Half3 half)
        {
            writeH(half.X.RawValue);
            writeH(half.Y.RawValue);
            writeH(half.Z.RawValue);
        }
        protected internal void writeTVector(Half3 half)
        {
            writeT(half.X);
            writeT(half.Y);
            writeT(half.Z);
        }
        protected internal void writeS(string value)
        {
            if (value != null)
                writeB(Encoding.Unicode.GetBytes(value));
            writeH(0);
        }
        protected internal void writeS(string name, int count)
        {
            if (name == null)
                return;
            writeB(Encoding.GetEncoding(1251).GetBytes(name));
            writeB(new byte[count - name.Length]);
        }
        /// <summary>
        /// Volta uma determinada quantia de bytes do MemoryStream.
        /// </summary>
        /// <param name="value"></param>
        protected internal void GoBack(int value)
        {
            mstream.Position -= value;
        }
    }
}