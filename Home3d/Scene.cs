using System;
using Home3d.Model;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Home3d
{
    public class Scene : GameWindow
    {
        private static readonly ObjModel SofaModel = new ObjModel();
        private static readonly ObjModel ChairModel = new ObjModel();
        private static readonly ObjModel SonyTvModel = new ObjModel();
        private static readonly ObjModel TvTableModel = new ObjModel();
        private static readonly ObjModel DoorModel = new ObjModel();
        private static readonly ObjModel LampModel = new ObjModel();
        private static readonly ObjModel PaintingModel = new ObjModel();
        private static readonly ObjModel PianoModel = new ObjModel();
        private static readonly Camera Camera = new Camera();
        private static double _angleX = 0.0;
        private static double _angleY = 0.0;
        private static bool _fogEnabled = true;
        private static bool _light0Enabled = true;
        private static bool _light1Enabled = true;
        private static bool _light2Enabled = true;

        public Scene() : base(1300, 768, GraphicsMode.Default)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            VSync = VSyncMode.Off;

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Light1);
            GL.Enable(EnableCap.Light2);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Fog);
            GL.ClearColor(1, 1, 1, 1);

            SofaModel.Load("3dmodels/sofa.obj");
            ChairModel.Load("3dmodels/chair1.obj");
            SonyTvModel.Load("3dmodels/sonyTV.obj");
            TvTableModel.Load("3dmodels/cofeeTable.obj");
            DoorModel.Load("3dmodels/door.obj");
            LampModel.Load("3dmodels/lamp.obj");
            PaintingModel.Load("3dmodels/painting.obj");
            PianoModel.Load("3dmodels/piano.obj");

            Camera.EyePoint = new Vertex3(0, 4, 0);
            Camera.LookingPoint = new Vertex3(0, 4, -50);
        }

        private void InitLights()
        {
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 1, 9, -20 });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 1, 1, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1, 1, 1 });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1, 1, 1 });

            GL.Light(LightName.Light1, LightParameter.Position, new[] { -10.0f, (float)(LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y), -21.0f });
            GL.Light(LightName.Light1, LightParameter.SpotCutoff, 180.0f);
            GL.Light(LightName.Light1, LightParameter.Ambient, new float[] { 1, 1, 1 });
            GL.Light(LightName.Light1, LightParameter.Diffuse, new float[] { 1, 1, 1 });
            GL.Light(LightName.Light1, LightParameter.Specular, new float[] { 1, 1, 1 });

            GL.Light(LightName.Light2, LightParameter.Position, new[] { 10.0f, (float)(LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y), -21.0f });
            GL.Light(LightName.Light2, LightParameter.SpotCutoff, 180.0f);
            GL.Light(LightName.Light2, LightParameter.Ambient, new float[] { 1, 1, 1 });
            GL.Light(LightName.Light2, LightParameter.Diffuse, new float[] { 1, 1, 1 });
            GL.Light(LightName.Light2, LightParameter.Specular, new float[] { 1, 1, 1 });
        }

        private void InitFog()
        {
            var fogColor = new[] { 0.7f, 0.7f, 0.7f, 0.7f };
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Fog(FogParameter.FogStart, 0);
            GL.Fog(FogParameter.FogEnd, 20);
            GL.Fog(FogParameter.FogColor, fogColor);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 0.1f, 1000.0f);
            GL.LoadMatrix(ref perpective);
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            
            InitLights();
            InitFog();

            var lookat = Camera.MakeLookingMatrix();
            GL.LoadMatrix(ref lookat);
            GL.PushMatrix();

            var minVertex = new Vertex3(-12.0, 0.0, 0.0);
            var maxVertex = new Vertex3(12.0, 10.0, -23.0);
            var centerVertex = new Vertex3();
            centerVertex.Add(minVertex);
            centerVertex.Add(maxVertex);
            centerVertex.Divide(2);

            GL.PushMatrix();
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new[] { 201 / 255.0f, 173 / 255.0f, 48 / 255.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new[] { 222 / 255.0f, 191 / 255.0f, 53 / 255.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, new[] { 0.1f, 0.1f, 0.1f });

            GL.Translate(centerVertex.X, centerVertex.Y, centerVertex.Z);
            GL.Scale(maxVertex.X - minVertex.X, maxVertex.Y - minVertex.Y, maxVertex.Z - minVertex.Z);
            
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, (SofaModel.MaximumVertex.Y - SofaModel.MinimumVertex.Y) / 2.0, -20);
            GL.Rotate(90, 0, 1, 0);
            foreach (var obj in SofaModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(-(SofaModel.MaximumVertex.X - SofaModel.MinimumVertex.X) - (ChairModel.MaximumVertex.X - ChairModel.MinimumVertex.X), (ChairModel.MaximumVertex.Y - ChairModel.MinimumVertex.Y) / 2.0, -20);

            foreach (var obj in ChairModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate((SofaModel.MaximumVertex.X - SofaModel.MinimumVertex.X) + (ChairModel.MaximumVertex.X - ChairModel.MinimumVertex.X), (ChairModel.MaximumVertex.Y - ChairModel.MinimumVertex.Y) / 2.0, -20);
            foreach (var obj in ChairModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 1.5 * (TvTableModel.MaximumVertex.Y - TvTableModel.MinimumVertex.Y), -5);
            GL.Scale(3, 3, 2);

            foreach (var obj in TvTableModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 3 * (TvTableModel.MaximumVertex.Y - TvTableModel.MinimumVertex.Y) + (SonyTvModel.MaximumVertex.Y - SonyTvModel.MinimumVertex.Y) / 2.0, -5);
            GL.Rotate(90, 0, 1, 0);

            foreach (var obj in SonyTvModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(-12, (DoorModel.MaximumVertex.Y - DoorModel.MinimumVertex.Y) / 2.0, -15);
            foreach (var obj in DoorModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Scale(1, 2, 1);
            GL.Translate(-10, (LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y) / 2.0, -21);
            foreach (var obj in LampModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Scale(1, 2, 1);
            GL.Translate(10, (LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y) / 2.0, -21);
            foreach (var obj in LampModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(12, 0, -11);
            GL.Scale(2, 1.8, 2);
            GL.Translate(-(PianoModel.MaximumVertex.X - PianoModel.MinimumVertex.X), (PianoModel.MaximumVertex.Y - PianoModel.MinimumVertex.Y) / 2.0, 0);
            GL.Rotate(180, 0, 1, 0);
            foreach (var obj in PianoModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, (LampModel.MaximumVertex.Y - LampModel.MinimumVertex.Y) / 2.0, -22);
            GL.Translate(0, (SofaModel.MaximumVertex.Y - SofaModel.MinimumVertex.Y), 0);
            GL.Translate(0, 2, 0);
            GL.Rotate(-90, 0, 1, 0);
            foreach (var obj in PaintingModel.Objects.Values)
            {
                obj.Render();
            }
            GL.PopMatrix();

            SwapBuffers();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            Camera.LookingPoint.X = Camera.EyePoint.X + Math.Sin(_angleX);
            Camera.LookingPoint.Z = Camera.EyePoint.Z + Math.Cos(_angleX);
            Camera.LookingPoint.Y = Camera.EyePoint.Y + Math.Sin(_angleY);
            _angleX -= e.XDelta/100.0;
            _angleY += e.YDelta/100.0;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            var lookingDirection = new Vertex3(Camera.LookingPoint.X - Camera.EyePoint.X, Camera.LookingPoint.Y - Camera.EyePoint.Y, Camera.LookingPoint.Z - Camera.EyePoint.Z);
            lookingDirection = lookingDirection.Normalize();

            if (e.KeyChar == 'w')
            {
                Camera.EyePoint.Z += lookingDirection.Z;
                Camera.LookingPoint.Z += lookingDirection.Z;
                Camera.EyePoint.X += lookingDirection.X;
                Camera.LookingPoint.X += lookingDirection.X;
            }
            else if (e.KeyChar == 's')
            {
                Camera.EyePoint.Z -= lookingDirection.Z;
                Camera.LookingPoint.Z -= lookingDirection.Z;
                Camera.EyePoint.X -= lookingDirection.X;
                Camera.LookingPoint.X -= lookingDirection.X;
            }
            else if (e.KeyChar == 'a')
            {
                Camera.EyePoint.X -= 1;
                Camera.LookingPoint.X -= 1;
            }
            else if (e.KeyChar == 'd')
            {
                Camera.EyePoint.X += 1;
                Camera.LookingPoint.X += 1;
            }
            else if (e.KeyChar == 'f')
            {
                if (_fogEnabled)
                {
                    GL.Disable(EnableCap.Fog);
                    _fogEnabled = false;
                }
                else
                {
                    GL.Enable(EnableCap.Fog);
                    _fogEnabled = true;
                }
            }
            else if (e.KeyChar == 'm')
            {
                if (_light0Enabled)
                {
                    GL.Disable(EnableCap.Light0);
                    _light0Enabled = false;
                }
                else
                {
                    GL.Enable(EnableCap.Light0);
                    _light0Enabled = true;
                }
            }
            else if (e.KeyChar == 'k')
            {
                if (_light1Enabled)
                {
                    GL.Disable(EnableCap.Light1);
                    _light1Enabled = false;
                }
                else
                {
                    GL.Enable(EnableCap.Light1);
                    _light1Enabled = true;
                }
            }
            else if (e.KeyChar == 'l')
            {
                if (_light2Enabled)
                {
                    GL.Disable(EnableCap.Light2);
                    _light2Enabled = false;
                }
                else
                {
                    GL.Enable(EnableCap.Light2);
                    _light2Enabled = true;
                }
            }
        }
    }
}
