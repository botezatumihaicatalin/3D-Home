namespace Home3d.Model
{
    public class ObjRgb
    {
        public ObjRgb()
        {
            Red = 0;
            Blue = 0;
            Green = 0;
        }

        public ObjRgb(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        public double[] ToArray()
        {
            return new [] {Red, Green, Blue};
        }

        public float[] ToFloatArray()
        {
            return new[] { (float)Red, (float)Green, (float)Blue };
        }
    }
}
