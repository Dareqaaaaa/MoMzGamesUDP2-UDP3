namespace Battle.network.actions.user
{
    public class a4_PositionSync
    {
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _rotationX = p.readUH(),
                _rotationY = p.readUH(),
                _rotationZ = p.readUH(),
                _cameraX = p.readUH(),
                _cameraY = p.readUH(),
                _area = p.readUH()
            };
            if (genLog)
            {
               // Logger.warning("Slot " + aM._slot + " is now in: rX,rY,rZ,cX,cY,A (" + info._rotationX + ";" + info._rotationY + ";" + info._rotationZ + ";" + info._cameraX + ";" + info._cameraY + ";" + info._area + ")");
            }
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeH(info._rotationX);
            s.writeH(info._rotationY);
            s.writeH(info._rotationZ);
            s.writeH(info._cameraX);
            s.writeH(info._cameraY);
            s.writeH(info._area);
        }
        public class Struct
        {
            public ushort _rotationX, _rotationY, _rotationZ, _cameraX, _cameraY, _area;
        }
    }
}