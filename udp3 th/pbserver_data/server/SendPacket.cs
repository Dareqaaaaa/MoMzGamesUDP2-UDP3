/*
 * Arquivo: SendPacket.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 07/07/2017
 * Sintam inveja, não nos atinge
 */

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Core.server
{
    public abstract class SendPacket : IDisposable
    {
        public MemoryStream mstream = new MemoryStream();
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public byte[] GetBytes(string name)
        {
            try
            {
                write();
                return mstream.ToArray();
            }
            catch (Exception ex)
            {
                Logger.error("GetBytes problem at: " + name + "\r\n" + ex.ToString());
                return new byte[0];
            }
        }
        public byte[] GetCompleteBytes(string name)
        {
            try
            {
                write();
                byte[] data = mstream.ToArray();
                if (data.Length >= 2)
                {
                    ushort size = Convert.ToUInt16(data.Length - 2);
                    List<byte> list = new List<byte>(data.Length + 2);
                    list.AddRange(BitConverter.GetBytes(size));
                    list.AddRange(data);

                    return list.ToArray();
                }
                return new byte[0];
            }
            catch (Exception ex)
            {
                Logger.error("GetCompleteBytes problem at: " + name + "\r\n" + ex.ToString());
                return new byte[0];
            }
        }
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
        protected internal void writeIP(string address)
        {
            writeB(IPAddress.Parse(address).GetAddressBytes());
        }
        protected internal void writeIP(IPAddress address)
        {
            writeB(address.GetAddressBytes());
        }
        protected internal void writeB(byte[] value)
        {
            mstream.Write(value, 0, value.Length);
        }
        protected internal void writeB(byte[] value, int offset, int length)
        {
            mstream.Write(value, offset, length);
        }
        protected internal void writeD(bool value)
        {
            writeB(new byte[] { Convert.ToByte(value), 0, 0, 0 });
        }
        protected internal void writeD(uint valor)
        {
            writeB(BitConverter.GetBytes(valor));
        }
        protected internal void writeD(int value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeH(ushort valor)
        {
            writeB(BitConverter.GetBytes(valor));
        }
        protected internal void writeH(short val)
        {
            writeB(BitConverter.GetBytes(val));
        }
        protected internal void writeC(byte value)
        {
            mstream.WriteByte(value);
        }
        /// <summary>
        /// True = 1; False = 0.
        /// </summary>
        /// <param name="value"></param>
        protected internal void writeC(bool value)
        {
            mstream.WriteByte(Convert.ToByte(value));
        }
        protected internal void writeT(float value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeF(double value)
        {
            writeB(BitConverter.GetBytes(value));
        }
        protected internal void writeQ(ulong valor)
        {
            writeB(BitConverter.GetBytes(valor));
        }
        protected internal void writeQ(long valor)
        {
            writeB(BitConverter.GetBytes(valor));
        }
        /// <summary>
        /// Escreve um texto no formato Unicode.
        /// </summary>
        /// <param name="value">Texto</param>
        protected internal void writeS(string value)
        {
            if (value != null)
                writeB(Encoding.Unicode.GetBytes(value));
        }
        protected internal void writeS(string name, int count)
        {
            if (name == null)
                return;
            writeB(ConfigGB.EncodeText.GetBytes(name));
            writeB(new byte[count - name.Length]);
        }
        protected internal void writeS(string name, int count, int CodePage)
        {
            if (name == null)
                return;
            writeB(Encoding.GetEncoding(CodePage).GetBytes(name));
            writeB(new byte[count - name.Length]);
        }
        public abstract void write();
    }
}