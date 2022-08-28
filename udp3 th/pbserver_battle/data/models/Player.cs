/*
 * Arquivo: Player.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 30/11/2017
 * Sinta inveja, não nos atinge
 */

using Battle.config;
using Battle.data.enums;
using Battle.data.enums.weapon;
using SharpDX;
using System;
using System.Net;

namespace Battle.data.models
{
    public class Player
    {
        public int _slot = -1, _team, _life = 100, _maxLife = 100,
            _playerIdByUser = -2, _playerIdByServer = -1, WeaponSlot,
            _respawnByUser = -2, _respawnByLogic, _respawnByServer = -1;
        public float _plantDuration, _defuseDuration, _C4FTime;
        public Half3 Position;
        public IPEndPoint _client;
        public DateTime _date, LastPing, LastDie, _C4First;
        public ClassType WeaponClass;
        public CHARACTER_RES_ID _character;
        public bool isDead = true, _neverRespawn = true, Integrity = true, Immortal;
        public Player(int slot)
        {
            _slot = slot;
            _team = (slot % 2);
        }
        public void LogPlayerPos(Half3 EndBullet)
        {
            Logger.warning("[Player position] X: " + Position.X + "; Y: " + Position.Y + "; Z: " + Position.Z);
            Logger.warning("[End Bullet position] X: " + EndBullet.X + "; Y: " + EndBullet.Y + "; Z: " + EndBullet.Z);
        }
        public bool CompareIP(IPEndPoint ip)
        {
            return _client != null && ip != null && _client.Address.Equals(ip.Address) && _client.Port == ip.Port;
        }
        public bool RespawnIsValid()
        {
            //Logger.warning("[Server: " + _respawnByServer + "; User: " + _respawnByUser + "]");
            return _respawnByServer == _respawnByUser;
        }
        public bool RespawnLogicIsValid()
        {
            //Logger.warning("[Server: " + _respawnByServer + "; User: " + _respawnByLogic + "]");
            return _respawnByServer == _respawnByLogic;
        }
        public bool AccountIdIsValid()
        {
            return (_playerIdByServer == _playerIdByUser);
        }
        public bool AccountIdIsValid(int number)
        {
            return (_playerIdByServer == number);
        }
        public void CheckLifeValue()
        {
            if (_life > _maxLife)
                _life = _maxLife;
        }
        public void ResetAllInfos()
        {
            _client = null;
            _date = new DateTime();
            _playerIdByUser = -2;
            _playerIdByServer = -1;
            Integrity = true;
            ResetBattleInfos();
        }
        public void ResetBattleInfos()
        {
            _respawnByServer = -1;
            _respawnByUser = -2;
            _respawnByLogic = 0;
            Immortal = false;
            isDead = true;
            _neverRespawn = true;
            WeaponClass = ClassType.Unknown;
            WeaponSlot = 0;
            LastPing = new DateTime();
            LastDie = new DateTime();
            _C4First = new DateTime();
            _C4FTime = 0;
            Position = new Half3();
            _life = 100;
            _maxLife = 100;
            _plantDuration = Config.plantDuration;
            _defuseDuration = Config.defuseDuration;
        }
        public void ResetLife()
        {
            _life = _maxLife;
        }
    }
}