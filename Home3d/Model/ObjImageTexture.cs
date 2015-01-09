using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tao.OpenGl;

namespace Home3d.Model
{
    public class ObjImageTexture : IDisposable
    {
        public ObjImageTexture()
        {
            Texture = 0;
            ScaleU = 1;
            ScaleV = 1;
        }

        public uint Texture { get; private set; }
        public double ScaleU { get; set; }
        public double ScaleV { get; set; }

        public bool LoadImage(string path)
        {
            if (Texture == 0)
            {
                uint texture;
                Gl.glGenTextures(1, out texture);
                Texture = texture;
            }
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture);

            if (!File.Exists(path))
            {
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
                return false;
            }

            using (var bitmapImage = new Bitmap(path))
            {
                bitmapImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                bitmapImage.RotateFlip(RotateFlipType.Rotate180FlipY);
                var bitmapImageData = bitmapImage.LockBits(
                    new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppRgb);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, bitmapImage.Width, bitmapImage.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapImageData.Scan0);
                bitmapImage.UnlockBits(bitmapImageData);
            }

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);

            return true;
        }

        public void Dispose()
        {
            uint texture = Texture;
            if (texture == 0)
            {
                return;
            }
            Gl.glDeleteTextures(1, ref texture);
            Texture = 0;
            ScaleU = 1;
            ScaleV = 1;
        }
    }
}
