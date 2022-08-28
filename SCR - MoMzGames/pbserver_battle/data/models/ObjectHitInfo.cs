/*
 * Arquivo: ObjectHitInfo.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 20/10/2017
 * Sinta inveja, não nos atinge
 */

using Battle.data.enums;
using SharpDX;

namespace Battle.data.models
{
    public class ObjectHitInfo
    {
        public int syncType, objSyncId, objId, objLife, weaponId, killerId, _animId1, _animId2, _destroyState;
        public CHARA_DEATH deathType = CHARA_DEATH.DEFAULT;
        public int hitPart;
        public Half3 Position;
        public float _specialUse;
        public ObjectHitInfo(int type)
        {
            syncType = type;
        }
    }
}