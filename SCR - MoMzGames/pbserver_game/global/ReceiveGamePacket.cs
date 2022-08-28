﻿/*
 * Arquivo: ReceiveGamePacket.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 27/10/2016
 * Sintam inveja, não nos atinge
 */

using Core;
using System;
using System.Text;

namespace Game.global
{
    public abstract class ReceiveGamePacket
    {
        private byte[] _buffer;
        private int _offset = 4;
        public GameClient _client;
        protected internal void makeme(GameClient client, byte[] buffer)
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
        protected internal uint readUD()
        {
            uint num = BitConverter.ToUInt32(_buffer, _offset);
            _offset += 4;
            return num;
        }
        protected internal byte readC()
        {
            byte num = _buffer[_offset++];
            return num;
        }
        protected internal byte readC2()
        {
            try
            {
                byte num = _buffer[_offset++];
                return num;
            }
            catch { return 255; }
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
        protected internal ushort readUH()
        {
            ushort num = BitConverter.ToUInt16(_buffer, _offset);
            _offset += 2;
            return num;
        }
        protected internal double readF()
        {
            double num = BitConverter.ToDouble(_buffer, _offset);
            _offset += 8;
            return num;
        }
        protected internal float readT()
        {
            float num = BitConverter.ToSingle(_buffer, _offset);
            _offset += 4;
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
            string result = "";
            try
            {
                int count = (_buffer.Length - _offset);
                result = Encoding.Unicode.GetString(_buffer, _offset, count);
                int idx = result.IndexOf(char.MinValue);
                if (idx != -1)
                    result = result.Substring(0, idx);
                _offset += result.Length + 1;
            }
            catch
            {
            }
            return result;
        }
        public abstract void read();
        public abstract void run();
    }
}