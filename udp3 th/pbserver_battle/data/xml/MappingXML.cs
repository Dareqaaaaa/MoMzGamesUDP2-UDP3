using Battle.data.models;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Battle.data.xml
{
    public class MappingXML
    {
        public static List<MapModel> _maps = new List<MapModel>();
        public static MapModel getMapById(int mapId)
        {
            for (int i = 0; i < _maps.Count; i++)
            {
                MapModel map = _maps[i];
                if (map._id == mapId)
                    return map;
            }
            return null;
        }
        public static void SetObjectives(ObjModel obj, Room room)
        {
            if (obj._ultraSYNC == 0)
                return;
            if (obj._ultraSYNC == 1 || obj._ultraSYNC == 3)
            {
                room._bar1 = obj._life;
                room._default1 = room._bar1;
            }
            else if (obj._ultraSYNC == 2 || obj._ultraSYNC == 4)
            {
                room._bar2 = obj._life;
                room._default2 = room._bar2;
            }
        }
        public static void Load()
        {
            string path = "data/battle/maps.xml";
            if (File.Exists(path))
                parse(path);
            else
                Logger.warning("[MappingXML] Não existe o arquivo: " + path);
        }
        private static void parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length > 0)
                {
                    try
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("Map".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        MapModel map = new MapModel
                                        {
                                            _id = int.Parse(xml.GetNamedItem("Id").Value)
                                        };
                                        BombsXML(xmlNode2, map);
                                        ObjectsXML(xmlNode2, map);
                                        _maps.Add(map);
                                    }
                                }
                            }
                        }
                    }
                    catch (XmlException ex)
                    {
                        Logger.warning(ex.ToString());
                    }
                }
                fileStream.Dispose();
                fileStream.Close();
                Logger.warning("[Aviso] Loaded " + _maps.Count + " maps");
            }
        }
        private static void BombsXML(XmlNode xmlNode, MapModel map)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("BombPositions".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Bomb".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            BombPosition bomb = new BombPosition
                            {
                                X = float.Parse(xml4.GetNamedItem("X").Value),
                                Y = float.Parse(xml4.GetNamedItem("Y").Value),
                                Z = float.Parse(xml4.GetNamedItem("Z").Value)
                            };
                            bomb.Position = new Half3(bomb.X, bomb.Y, bomb.Z);
                            if (bomb.X == 0 && bomb.Y == 0 && bomb.Z == 0)
                                bomb.Everywhere = true;
                            map._bombs.Add(bomb);
                        }
                    }
                }
            }
        }
        private static void ObjectsXML(XmlNode xmlNode, MapModel map)
        {
            for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
            {
                if ("objects".Equals(xmlNode3.Name))
                {
                    for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
                    {
                        if ("Obj".Equals(xmlNode4.Name))
                        {
                            XmlNamedNodeMap xml4 = xmlNode4.Attributes;
                            ObjModel obj = new ObjModel(bool.Parse(xml4.GetNamedItem("NeedSync").Value))
                            {
                                _id = int.Parse(xml4.GetNamedItem("Id").Value),
                                _life = int.Parse(xml4.GetNamedItem("Life").Value),
                                _anim1 = int.Parse(xml4.GetNamedItem("Anim1").Value)
                            };
                            if (obj._life > -1)
                                obj.isDestroyable = true;
                            if (obj._anim1 > 255)
                            {
                                if (obj._anim1 == 256)
                                    obj._ultraSYNC = 1;
                                else if (obj._anim1 == 257)
                                    obj._ultraSYNC = 2;
                                else if (obj._anim1 == 258)
                                    obj._ultraSYNC = 3;
                                else if (obj._anim1 == 259)
                                    obj._ultraSYNC = 4;
                                obj._anim1 = 255;
                            }
                            AnimsXML(xmlNode4, obj);
                            DEffectsXML(xmlNode4, obj);
                            map._objects.Add(obj);
                        }
                    }
                }
            }
        }
        private static void AnimsXML(XmlNode xmlNode, ObjModel obj)
        {
            for (XmlNode xmlNode5 = xmlNode.FirstChild; xmlNode5 != null; xmlNode5 = xmlNode5.NextSibling)
            {
                if ("Anims".Equals(xmlNode5.Name))
                {
                    for (XmlNode xmlNode6 = xmlNode5.FirstChild; xmlNode6 != null; xmlNode6 = xmlNode6.NextSibling)
                    {
                        if ("Sync".Equals(xmlNode6.Name))
                        {
                            XmlNamedNodeMap xml6 = xmlNode6.Attributes;
                            AnimModel anim = new AnimModel
                            {
                                _id = int.Parse(xml6.GetNamedItem("Id").Value),
                                _duration = float.Parse(xml6.GetNamedItem("Date").Value),
                                _nextAnim = int.Parse(xml6.GetNamedItem("Next").Value),
                                _otherObj = int.Parse(xml6.GetNamedItem("OtherOBJ").Value),
                                _otherAnim = int.Parse(xml6.GetNamedItem("OtherANIM").Value)
                            };
                            if (anim._id == 0)
                                obj._noInstaSync = true;
                            if (anim._id != 255)
                                obj._updateId = 3;
                            obj._anims.Add(anim);
                        }
                    }
                }
            }
        }
        private static void DEffectsXML(XmlNode xmlNode, ObjModel obj)
        {
            for (XmlNode xmlNode5 = xmlNode.FirstChild; xmlNode5 != null; xmlNode5 = xmlNode5.NextSibling)
            {
                if ("DestroyEffects".Equals(xmlNode5.Name))
                {
                    for (XmlNode xmlNode6 = xmlNode5.FirstChild; xmlNode6 != null; xmlNode6 = xmlNode6.NextSibling)
                    {
                        if ("Effect".Equals(xmlNode6.Name))
                        {
                            XmlNamedNodeMap xml6 = xmlNode6.Attributes;
                            DEffectModel anim = new DEffectModel
                            {
                                _id = int.Parse(xml6.GetNamedItem("Id").Value),
                                _life = int.Parse(xml6.GetNamedItem("Percent").Value)
                            };
                            obj._effects.Add(anim);
                        }
                    }
                }
            }
        }
    }
    public class MapModel
    {
        public int _id;
        public List<ObjModel> _objects = new List<ObjModel>();
        public List<BombPosition> _bombs = new List<BombPosition>();
        public BombPosition GetBomb(int bombId)
        {
            try
            {
                return _bombs[bombId];
            }
            catch
            {
                return null;
            }
        }
    }
    public class BombPosition
    {
        public float X, Y, Z;
        public Half3 Position;
        public bool Everywhere;
    }
    public class ObjModel
    {
        public int _id, _life, _anim1, _ultraSYNC, _updateId = 1;
        public bool _needSync, isDestroyable, _noInstaSync;
        public List<AnimModel> _anims;
        public List<DEffectModel> _effects;
        public ObjModel(bool needSYNC)
        {
            _needSync = needSYNC;
            if (needSYNC)
                _anims = new List<AnimModel>();
            _effects = new List<DEffectModel>();
        }
        public int CheckDestroyState(int life)
        {
            for (int i = _effects.Count - 1; i > -1; i--)
            {
                DEffectModel eff = _effects[i];
                if (eff._life >= life)
                    return eff._id;
            }
            return 0;
        }
        public int GetARandomAnim(Room room, ObjectInfo obj)
        {
            if (_anims != null && _anims.Count > 0)
            {
                int idx = new Random().Next(_anims.Count);
                AnimModel anim = _anims[idx];
                obj._anim = anim;
                obj._useDate = DateTime.Now;
                if (anim._otherObj > 0)
                {
                    ObjectInfo obj2 = room._objects[anim._otherObj];
                    GetAnim(anim._otherAnim, 0, 0, obj2);
                }
                //Logger.warning("Animação escolhida: " + anim._id);
                return anim._id;
            }
            obj._anim = null;
            return 255;
        }
        public void GetAnim(int animId, float time, float duration, ObjectInfo obj)
        {
            if (animId == 255 || obj == null || obj._model == null || obj._model._anims == null || obj._model._anims.Count == 0)
                return;
            ObjModel objModel = obj._model;
            for (int i = 0; i < objModel._anims.Count; i++)
            {
                AnimModel anim = objModel._anims[i];
                if (anim._id == animId)
                {
                    obj._anim = anim;
                    time -= duration;
                    obj._useDate = DateTime.Now.AddSeconds(time * -1);
                    break;
                }
            }
        }
    }
    public class AnimModel
    {
        public int _id, _nextAnim, _otherObj, _otherAnim;
        public float _duration;
    }
    public class DEffectModel
    {
        public int _id, _life;
    }
}