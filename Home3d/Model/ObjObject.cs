using System.Collections.Generic;
using Tao.OpenGl;

namespace Home3d.Model
{
    public class ObjObject
    {
        public ObjObject()
        {
            ListId = Gl.glGenLists(1);
            Faces = new List<ObjFace>();
        }
        public ObjObject(string name)
        {
            ListId = Gl.glGenLists(1);
            Name = name;
            Faces = new List<ObjFace>();
        }

        public string Name { get; set; }
        public List<ObjFace> Faces { get; set; }
        public int ListId { get; private set; }

        public void Render()
        {
            Gl.glCallList(ListId);
        }

        ~ObjObject()
        {
            //Gl.glDeleteLists(ListId, 1);
        }
    }
}
