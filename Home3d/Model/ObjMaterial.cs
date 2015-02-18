namespace Home3d.Model
{
    /// <summary>
    /// A class which contains information about material colours and textures. 
    /// </summary>
    public class ObjMaterial
    {
        public ObjMaterial() : this(string.Empty) { }

        public ObjMaterial(string name)
        {
            Name = name;
            AmbientColor = new ObjRgb(1, 1, 1);
            DiffuseColor = new ObjRgb(1, 1, 1);
            SpecularColor = new ObjRgb(1, 1, 1);

            Transparency = 1.0;
            Illumination = 2;
            Shininess = 100.0;

            DiffuseTexture = new ObjImageTexture();
            AmbientTexture = new ObjImageTexture();
            SpecularTexture = new ObjImageTexture();
        }

        public string Name { get; private set; }
        public ObjRgb AmbientColor { get; private set; }
        public ObjRgb DiffuseColor { get; private set; }
        public ObjRgb SpecularColor { get; private set; }

        public double Transparency { get; set; }
        public int Illumination { get; set; }
        public double Shininess { get; set; }
    
        public ObjImageTexture DiffuseTexture { get; private set; }
        public ObjImageTexture AmbientTexture { get; private set; }
        public ObjImageTexture SpecularTexture { get; private set; }
    }
}
