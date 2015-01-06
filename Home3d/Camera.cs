using Tao.OpenGl;

namespace Home3d
{
    public class Camera
    {
        public Camera()
        {
            EyePoint = new Vertex3();
            LookingPoint = new Vertex3();
            Tilt = new Vertex3(0 , 1 , 0);
        }
        public Vertex3 EyePoint { get; set; }
        public Vertex3 LookingPoint { get; set; }
        public Vertex3 Tilt { get; set; }

        public void PositionCamera()
        {
            Glu.gluLookAt(EyePoint.X, EyePoint.Y, EyePoint.Z, LookingPoint.X, LookingPoint.Y, LookingPoint.Z, Tilt.X, Tilt.Y, Tilt.Z);
        }
    }
}
