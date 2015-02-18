using System.Collections.Generic;

namespace Home3d.Model
{
    public class ObjFace
    {
        public ObjFace() : this(string.Empty)
        {
        }

        public ObjFace(string materialName)
        {
            MaterialName = materialName;
            FaceItems = new List<ObjFaceItem>();
        }

        public string MaterialName { get; private set; }
        public List<ObjFaceItem> FaceItems { get; private set; }
    }
}
