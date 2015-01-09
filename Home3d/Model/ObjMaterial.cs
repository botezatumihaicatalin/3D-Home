using System.Drawing;

namespace Home3d.Model
{
    public class ObjMaterial
    {
        public ObjMaterial()
        {
            Name = "";
            AmbientColor = new ObjRgba(1 , 1 , 1 , 1);
            DiffuseColor = new ObjRgba(1, 1, 1, 1);
            SpecularColor = new ObjRgba(1, 1, 1, 1);

            Transparency = 1.0;
            Illumination = 2;
            SpecularExponent = 1.0;

            DiffuseTexture = new ObjImageTexture();
            AmbientTexture = new ObjImageTexture();
            SpecularTexture = new ObjImageTexture();
        }

        public ObjMaterial(string name)
        {
            Name = name;
            AmbientColor = new ObjRgba(1, 1, 1, 1);
            DiffuseColor = new ObjRgba(1, 1, 1, 1);
            SpecularColor = new ObjRgba(1, 1, 1, 1);

            Transparency = 1.0;
            Illumination = 2;
            SpecularExponent = 1.0;

            DiffuseTexture = new ObjImageTexture();
            AmbientTexture = new ObjImageTexture();
            SpecularTexture = new ObjImageTexture();
        }

        public string Name { get; set; }
        public ObjRgba AmbientColor { get; set; }
        public ObjRgba DiffuseColor { get; set; }
        public ObjRgba SpecularColor { get; set; }

        public double Transparency { get; set; }
        public int Illumination { get; set; }
        public double SpecularExponent { get; set; }
    
        public ObjImageTexture DiffuseTexture { get; set; }
        public ObjImageTexture AmbientTexture { get; set; }
        public ObjImageTexture SpecularTexture { get; set; }
    }
}
