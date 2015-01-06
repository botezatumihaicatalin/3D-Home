namespace Home3d.Model
{
    public class ObjRgba
    {
        public ObjRgba()
        {
            Red = 0;
            Blue = 0;
            Green = 0;
            Alpha = 1;
        }

        public ObjRgba(double red, double green, double blue, double alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Alpha { get; set; }

        public double[] ToArray()
        {
            return new [] {Red, Green, Blue, Alpha};
        }

        public float[] ToFloatArray()
        {
            return new[] { (float)Red, (float)Green, (float)Blue, (float)Alpha };
        }
    }
}
