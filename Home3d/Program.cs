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
        private static readonly Camera Camera = new Camera();
        private static int lastX = -1, lastY = -1;

        static void InitGraphics()
        {
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glClearColor(1, 1, 1, 1);

            bool result = false;
            result = SofaModel.Load("3dmodels/sofa.obj", "3dmodels/sofa.mtl");
            Console.WriteLine("{0} loaded with result : {1}", "sofa.obj", result);
            result = ChairModel.Load("3dmodels/chair1.obj", "3dmodels/chair1.mtl");
            Console.WriteLine("{0} loaded with result : {1}", "chair1.obj", result);
            result = SonyTvModel.Load("3dmodels/sonyTV.obj", "3dmodels/sonyTV.mtl");
            Console.WriteLine("{0} loaded with result : {1}", "sonyTV.obj", result);
            result = TvTableModel.Load("3dmodels/cofeeTable.obj", "3dmodels/cofeeTable.mtl");
            Console.WriteLine("{0} loaded with result : {1}", "cofeeTable.obj", result);
            result = DoorModel.Load("3dmodels/door.obj", "3dmodels/door.mtl");
            Console.WriteLine("{0} loaded with result : {1}", "door.obj", result);

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

            Gl.glPushMatrix();
                
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
            Gl.glTranslated(0, 1.5 * (TvTableModel.MaximumVertex.Y - TvTableModel.MinimumVertex.Y), -10);
            Gl.glScaled(3, 3, 2);

            foreach (var obj in TvTableModel.Objects.Values)
            {
                obj.Render();
            }
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, 3 * (TvTableModel.MaximumVertex.Y - TvTableModel.MinimumVertex.Y) + (SonyTvModel.MaximumVertex.Y - SonyTvModel.MinimumVertex.Y) / 2.0, -10);
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
            Glut.glutSwapBuffers();
            //Glut.glutPostRedisplay();
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