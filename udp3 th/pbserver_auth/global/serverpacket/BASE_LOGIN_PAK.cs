/*
 * Arquivo: BASE_LOGIN_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 10/07/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.enums.errors;
using Core.server;

namespace Auth.global.serverpacket
{
    public class BASE_LOGIN_PAK : SendPacket
    {
        private uint _result;
        private string _login;
        private long _pId;
        public BASE_LOGIN_PAK(EventErrorEnum result, string login, long pId)
        {
            _result = (uint)result;
            _login = login;
            _pId = pId;
        }
        public BASE_LOGIN_PAK(uint result, string login, long pId)
        {
            _result = result;
            _login = login;
            _pId = pId;
        }
        public BASE_LOGIN_PAK(int result, string login, long pId)
        {
            _result = (uint)result;
            _login = login;
            _pId = pId;
        }
        public override void write()
        {
            writeH(2564);
            writeD(_result);
            writeC(0);
            writeQ(_pId);
            writeC((byte)_login.Length);
            writeS(_login, _login.Length);
            writeC(0); //(Max = 127/128)
            writeC(0); //(Max = 49/50)
        }
    }
}