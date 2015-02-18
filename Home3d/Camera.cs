using OpenTK;

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

        public Matrix4 MakeLookingMatrix()
        {
            return Matrix4.LookAt((float)EyePoint.X, (float)EyePoint.Y, (float)EyePoint.Z, (float)LookingPoint.X, (float)LookingPoint.Y, (float)LookingPoint.Z,
                (float)Tilt.X, (float)Tilt.Y, (float)Tilt.Z);
        }
    }
}
