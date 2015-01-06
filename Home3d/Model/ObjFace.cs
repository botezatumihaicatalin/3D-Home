using System.Collections.Generic;

namespace Home3d.Model
{
    public class ObjFace
    {
        public ObjFace()
        {
            MaterialName = string.Empty;
            FaceItems = new List<ObjFaceItem>();
        }
        public ObjFace(string materialName)
        {
            MaterialName = materialName;
            FaceItems = new List<ObjFaceItem>();
        }

        public string MaterialName { get; set; }
        public List<ObjFaceItem> FaceItems { get; set; }
    }
}
