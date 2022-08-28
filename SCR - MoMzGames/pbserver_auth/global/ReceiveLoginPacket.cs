/*
 * Arquivo: ReceiveLoginPacket.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 11/07/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using System;
using System.Text;

namespace Auth.global
{
    public abstract class ReceiveLoginPacket
    {
        private byte[] _buffer;
        private int _offset = 4;
        public LoginClient _client;
        protected internal void makeme(LoginClient client, byte[] buffer)
        {
            _client = client;
            _buffer = buffer;
            read();
        }
        protected internal int readD()
        {
            int num = BitConverter.ToInt32(_buffer, _offset);
            _offset += 4;
            return num;
        }
        protected internal byte readC()
        {
            byte num = _buffer[_offset];
            _offset++;
            return num;
        }
        protected internal byte[] readB(int Length)
        {
            byte[] result = new byte[Length];
            Array.Copy(_buffer, _offset, result, 0, Length);
            _offset += Length;
            return result;
        }
        protected internal short readH()
        {
            short num = BitConverter.ToInt16(_buffer, _offset);
            _offset += 2;
            return num;
        }
        protected internal double readF()
        {
            double num = BitConverter.ToDouble(_buffer, _offset);
            _offset += 8;
            return num;
        }
        protected internal long readQ()
        {
            long num = BitConverter.ToInt64(_buffer, _offset);
            _offset += 8;
            return num;
        }
        protected internal ulong readUQ()
        {
            ulong num = BitConverter.ToUInt64(_buffer, _offset);
            _offset += 8;
            return num;
        }
        protected internal string readS(int Length)
        {
            string str = "";
            try
            {
                str = ConfigGB.EncodeText.GetString(_buffer, _offset, Length);
                int length = str.IndexOf(char.MinValue);
                if (length != -1)
                    str = str.Substring(0, length);
                _offset += Length;
            }
            catch
            {
            }
            return str;
        }
        protected internal string readS(int Length, int CodePage)
        {
            string str = "";
            try
            {
                str = Encoding.GetEncoding(CodePage).GetString(_buffer, _offset, Length);
                int length = str.IndexOf(char.MinValue);
                if (length != -1)
                    str = str.Substring(0, length);
                _offset += Length;
            }
            catch
            {
            }
            return str;
        }
        /// <summary>
        /// Usa Unicode.
        /// </summary>
        /// <returns></returns>
        protected internal string readS()
        {
            string str = "";
            try
            {
                str = Encoding.Unicode.GetString(_buffer, _offset, _buffer.Length - _offset);
                int length = str.IndexOf(char.MinValue);
                if (length != -1)
                    str = str.Substring(0, length);
                _offset += str.Length + 1;
            }
            catch
            {
            }
            return str;
        }
        public abstract void read();
        public abstract void run();
    }
}