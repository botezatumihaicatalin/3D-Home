using System;
using Home3d.Model;

namespace Home3d
{
    class Program
    {
        /*private static readonly ObjModel SofaModel = new ObjModel();
        private static readonly ObjModel ChairModel = new ObjModel();
        private static readonly ObjModel SonyTvModel = new ObjModel();
        private static readonly ObjModel TvTableModel = new ObjModel();
        private static readonly ObjModel DoorModel = new ObjModel();
        private static readonly ObjModel LampModel = new ObjModel();
        private static readonly ObjModel PaintingModel = new ObjModel();
        private static readonly ObjModel PianoModel = new ObjModel();
        private static readonly Camera Camera = new Camera();
        private static int _lastX = -1, _lastY = -1;
        private static double _angleX = 0.0;
        private static double _angleY = 0.0;
        private static bool _fogEnabled = true;
        private static bool _light0Enabled = true;
        private static bool _light1Enabled = true;
        private static bool _light2Enabled = true;

        static void InitGraphics()
        {
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_LIGHT1);
            Gl.glEnable(Gl.GL_LIGHT2);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glEnable(Gl.GL_FOG);
            Gl.glClearColor(1, 1, 1, 1);

            var result = false;
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

            Camera.EyePoint = new Vertex3(0, 4, 0);
            Camera.LookingPoint = new Vertex3(0, 4, -50);
        }

        static void InitLights()
        {
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { 1, 9, -20 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT_AND_DIFFUSE, new float[] { 1, 1, 1 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 1, 1, 1 });
           
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, new[] { -10.0f, (float)(LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y), -21.0f });
            Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_SPOT_CUTOFF, 180.0f);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT_AND_DIFFUSE, new float[] { 1, 1, 1 });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPECULAR, new float[] { 1, 1, 1 });

            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, new[] { 10.0f, (float)(LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y), -21.0f });
            Gl.glLightf(Gl.GL_LIGHT2, Gl.GL_SPOT_CUTOFF, 180.0f);
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_AMBIENT_AND_DIFFUSE, new float[] { 1, 1, 1 });
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_SPECULAR, new float[] { 1, 1, 1 });
        }

        static void InitFog()
        {
            var fogColor = new[] { 0.7f, 0.7f, 0.7f, 0.7f };
            Gl.glFogf(Gl.GL_FOG_MODE, Gl.GL_LINEAR);
            Gl.glFogf(Gl.GL_FOG_START, 0);
            Gl.glFogf(Gl.GL_FOG_END, 20);
            Gl.glFogfv(Gl.GL_FOG_COLOR, fogColor);
        }

        static void OnDisplay()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();

            Camera.PositionCamera();

            InitLights();
            InitFog();

            var minVertex = new Vertex3(-12.0, 0.0, 0.0);
            var maxVertex = new Vertex3(12.0, 10.0, -23.0);
            var centerVertex = new Vertex3();
            centerVertex.Add(minVertex);
            centerVertex.Add(maxVertex);
            centerVertex.Divide(2);

            Gl.glPushMatrix();
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, new[] { 201 / 255.0f, 173 / 255.0f, 48 / 255.0f });
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, new[] { 222 / 255.0f, 191 / 255.0f, 53 / 255.0f });
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, new[] { 0.1f , 0.1f, 0.1f });
            
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
            Gl.glRotated(-90, 0, 1, 0);
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

        static void MouseMove(int x, int y)
        {
            y = Glut.glutGet(Glut.GLUT_WINDOW_HEIGHT) - y;

            if (_lastX != -1 && _lastY != -1)
            {
                Camera.LookingPoint.X = Camera.EyePoint.X + Math.Sin(_angleX);
                Camera.LookingPoint.Z = Camera.EyePoint.Z + Math.Cos(_angleX);
                Camera.LookingPoint.Y = Camera.EyePoint.Y + Math.Sin(_angleY);
                _angleX += (_lastX - x) / 100.0;
                _angleY -= (_lastY - y) / 100.0;
                _lastX = x;
                _lastY = y;
            }
            else
            {
                _lastX = x;
                _lastY = y;
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
            else if (key == 'f')
            {
                if (_fogEnabled)
                {
                    Gl.glDisable(Gl.GL_FOG);
                    _fogEnabled = false;
                }
                else
                {
                    Gl.glEnable(Gl.GL_FOG);
                    _fogEnabled = true;
                }
            } 
            else if (key == 'm')
            {
                if (_light0Enabled)
                {
                    Gl.glDisable(Gl.GL_LIGHT0);
                    _light0Enabled = false;
                }
                else
                {
                    Gl.glEnable(Gl.GL_LIGHT0);
                    _light0Enabled = true;
                }
            } 
            else if (key == 'k')
            {
                if (_light1Enabled)
                {
                    Gl.glDisable(Gl.GL_LIGHT1);
                    _light1Enabled = false;
                }
                else
                {
                    Gl.glEnable(Gl.GL_LIGHT1);
                    _light1Enabled = true;
                }
            }
            else if (key == 'l')
            {
                if (_light2Enabled)
                {
                    Gl.glDisable(Gl.GL_LIGHT2);
                    _light2Enabled = false;
                }
                else
                {
                    Gl.glEnable(Gl.GL_LIGHT2);
                    _light2Enabled = true;
                }
            }

            Glut.glutPostRedisplay();
        }

        static void Main()
        {
            Glut.glutInit();
            Glut.glutInitWindowSize(1300, 768);
            Glut.glutCreateWindow("Living room");
            InitGraphics();
            Glut.glutDisplayFunc(OnDisplay);
            Glut.glutReshapeFunc(OnReshape);
            Glut.glutPassiveMotionFunc(MouseMove);
            Glut.glutKeyboardFunc(KeyPressed);
            Glut.glutMainLoop();
        }*/

        static void Main(string[] args)
        {
            using (var newScene = new Scene())
            {
                newScene.Run(30.0, 0.0);
            }
        }

    }
}