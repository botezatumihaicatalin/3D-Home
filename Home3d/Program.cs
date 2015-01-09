using System;
using Home3d.Model;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Home3d
{
    class Program
    {
        private static readonly ObjModel SofaModel = new ObjModel();
        private static readonly ObjModel ChairModel = new ObjModel();
        private static readonly ObjModel SonyTvModel = new ObjModel();
        private static readonly ObjModel TvTableModel = new ObjModel();
        private static readonly ObjModel DoorModel = new ObjModel();
        private static readonly ObjModel LampModel = new ObjModel();
        private static readonly ObjModel PaintingModel = new ObjModel();
        private static readonly ObjModel PianoModel = new ObjModel();
        private static readonly ObjModel TexturedModel = new ObjModel();
        private static readonly Camera Camera = new Camera();
        private static int lastX = -1, lastY = -1;

        static void InitGraphics()
        {
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glClearColor(1, 1, 1, 1);

            bool result = false;
            result = SofaModel.Load("3dmodels/sofa.obj");
            Console.WriteLine("{0} loaded with result : {1}", "sofa.obj", result);
            result = ChairModel.Load("3dmodels/chair1.obj");
            Console.WriteLine("{0} loaded with result : {1}", "chair1.obj", result);
            result = SonyTvModel.Load("3dmodels/sonyTV.obj");
            Console.WriteLine("{0} loaded with result : {1}", "sonyTV.obj", result);
            result = TvTableModel.Load("3dmodels/cofeeTable.obj");
            Console.WriteLine("{0} loaded with result : {1}", "cofeeTable.obj", result);
            result = DoorModel.Load("3dmodels/door.obj");
            Console.WriteLine("{0} loaded with result : {1}", "door.obj", result);
            result = LampModel.Load("3dmodels/lamp.obj");
            Console.WriteLine("{0} loaded with result : {1}", "lamp.obj", result);
            result = PaintingModel.Load("3dmodels/painting.obj");
            Console.WriteLine("{0} loaded with result : {1}", "painting.obj", result);
            result = PianoModel.Load("3dmodels/piano.obj");
            Console.WriteLine("{0} loaded with result : {1}", "piano.obj", result);
            result = TexturedModel.Load(@"C:\Users\Botezatu\Desktop\untitled.obj");
            Console.WriteLine("{0} loaded with result : {1}", "untitled.obj", result);

            Camera.EyePoint = new Vertex3(0, 4, 0);
            Camera.LookingPoint = new Vertex3(0, 4, -50);
        }

        static void OnDisplay()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            
            Camera.PositionCamera();

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { 1 , 10 , -20 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT_AND_DIFFUSE, new float[] { 1, 1, 1 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 1, 1, 1 });

            Gl.glPushMatrix();
            Gl.glTranslated(1, 10 , -20);
            Glut.glutSolidCube(0.1);
            Gl.glPopMatrix();

            var minVertex = new Vertex3(-12.0, 0.0, 0.0);
            var maxVertex = new Vertex3(12.0, 10.0, -23.0);
            var centerVertex = new Vertex3();
            centerVertex.Add(minVertex);
            centerVertex.Add(maxVertex);
            centerVertex.Divide(2);

            Gl.glPushMatrix();
            Gl.glMaterialfv(Gl.GL_BACK, Gl.GL_AMBIENT, new[] { 1f, 1f, 1f });
            Gl.glMaterialfv(Gl.GL_BACK, Gl.GL_DIFFUSE, new[] { 0.5f, 0.5f, 0.5f });
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, new[] { 0.5f, 0.5f, 0.5f });
            Gl.glMaterialfv(Gl.GL_BACK, Gl.GL_SPECULAR, new[] { 0.5f, 0.5f, 0.5f });
            Gl.glTranslated(centerVertex.X, centerVertex.Y, centerVertex.Z);
            Gl.glScaled(maxVertex.X - minVertex.X, maxVertex.Y - minVertex.Y, maxVertex.Z - minVertex.Z);
            Glut.glutSolidCube(1);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, (SofaModel.MaximumVertex.Y - SofaModel.MinimumVertex.Y) / 2.0, -20);
            Gl.glRotated(90, 0, 1, 0);
            foreach (var obj in SofaModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-(SofaModel.MaximumVertex.X - SofaModel.MinimumVertex.X) - (ChairModel.MaximumVertex.X - ChairModel.MinimumVertex.X), (ChairModel.MaximumVertex.Y - ChairModel.MinimumVertex.Y) / 2.0, -20);

            foreach (var obj in ChairModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated((SofaModel.MaximumVertex.X - SofaModel.MinimumVertex.X) + (ChairModel.MaximumVertex.X - ChairModel.MinimumVertex.X), (ChairModel.MaximumVertex.Y - ChairModel.MinimumVertex.Y) / 2.0, -20);
            foreach (var obj in ChairModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, 1.5 * (TvTableModel.MaximumVertex.Y - TvTableModel.MinimumVertex.Y), -5);
            Gl.glScaled(3, 3, 2);

            foreach (var obj in TvTableModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, 3 * (TvTableModel.MaximumVertex.Y - TvTableModel.MinimumVertex.Y) + (SonyTvModel.MaximumVertex.Y - SonyTvModel.MinimumVertex.Y) / 2.0, -5);
            Gl.glRotated(90, 0, 1, 0);

            foreach (var obj in SonyTvModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-12, (DoorModel.MaximumVertex.Y - DoorModel.MinimumVertex.Y) / 2.0, -15);
            foreach (var obj in DoorModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glScaled(1, 2, 1);
            Gl.glTranslated(-10, (LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y) / 2.0, -21);
            foreach (var obj in LampModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glScaled(1, 2, 1);
            Gl.glTranslated(10, (LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y) / 2.0, -21);
            foreach (var obj in LampModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(12, 0, -11);
            Gl.glScaled(2, 1.8, 2);
            Gl.glTranslated(-(PianoModel.MaximumVertex.X - PianoModel.MinimumVertex.X), (PianoModel.MaximumVertex.Y - PianoModel.MinimumVertex.Y) / 2.0, 0);
            Gl.glRotated(180, 0, 1, 0);
            foreach (var obj in PianoModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, (LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y) / 2.0, -22);
            Gl.glTranslated(0, (SofaModel.MaximumVertex.Y - SofaModel.MinimumVertex.Y), 0);
            Gl.glTranslated(0, 2, 0);
            Gl.glRotated(-90 , 0 , 1 , 0);
            foreach (var obj in PaintingModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Glut.glutSwapBuffers();
        }

        static void OnReshape(int width, int height)
        {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, width, height);
            Glu.gluPerspective(45, width / height, 0.1, 1000);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
        }

        static void MouseMove(int x, int y) {
	        
	        y = Glut.glutGet(Glut.GLUT_WINDOW_HEIGHT) - y;

	        if (lastX != -1 && lastY != -1) {
		        Camera.LookingPoint.X += ((x - lastX) / 10.0);
		        Camera.LookingPoint.Y += ((y - lastY) / 10.0);
		        lastX = x;
		        lastY = y;
	        } else {
		        lastX = x;
		        lastY = y;
	        }

	        Glut.glutPostRedisplay();
        }

        static void KeyPressed(byte key, int x, int y)
        {
            var lookingDirection = new Vertex3(Camera.LookingPoint.X - Camera.EyePoint.X, Camera.LookingPoint.Y - Camera.EyePoint.Y, Camera.LookingPoint.Z - Camera.EyePoint.Z);
            lookingDirection = lookingDirection.Normalize();

            if (key == 'w')
            {
                Camera.EyePoint.Z += lookingDirection.Z;
                Camera.LookingPoint.Z += lookingDirection.Z;
                Camera.EyePoint.X += lookingDirection.X;
                Camera.LookingPoint.X += lookingDirection.X;
            }
            else if (key == 's')
            {
                Camera.EyePoint.Z -= lookingDirection.Z;
                Camera.LookingPoint.Z -= lookingDirection.Z;
                Camera.EyePoint.X -= lookingDirection.X;
                Camera.LookingPoint.X -= lookingDirection.X;
            }
            else if (key == 'a')
            {
                Camera.EyePoint.X -= 1;
                Camera.LookingPoint.X -= 1;
            }
            else if (key == 'd')
            {
                Camera.EyePoint.X += 1;
                Camera.LookingPoint.X += 1;
            }

            Glut.glutPostRedisplay();
        }

        static void Main()
        {
            Glut.glutInit();
            Glut.glutInitWindowSize(1300, 768);
            Glut.glutCreateWindow("Tao Example");
            InitGraphics();
            Glut.glutDisplayFunc(OnDisplay);
            Glut.glutReshapeFunc(OnReshape);
            Glut.glutPassiveMotionFunc(MouseMove);
            Glut.glutKeyboardFunc(KeyPressed);
            Glut.glutMainLoop();
        }
    }
}