namespace Home3d.Model
{
    public class ObjFaceItem
    {
        public ObjFaceItem()
        {
            VertexIndex = -1;
            TextureIndex = -1;
            NormalIndex = -1;
        }

        public ObjFaceItem(int vertexIndex, int textureIndex, int normalIndex)
        {
            VertexIndex = vertexIndex;
            TextureIndex = textureIndex;
            NormalIndex = normalIndex;
        }
        public int VertexIndex { get; set; }
        public int TextureIndex { get; set; }
        public int NormalIndex { get; set; }
    }
}
