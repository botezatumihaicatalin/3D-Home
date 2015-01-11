using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tao.OpenGl;

namespace Home3d.Model
{
    /// <summary>
    /// A class which contains OPEN GL texture binding information
    /// </summary>
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

        /// <summary>
        /// Loads a texture from a path.
        /// Only supports PNG images.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <returns>True is the load succedeed either way false.</returns>
        public bool LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            if (!Path.HasExtension(path))
            {
                return false;
            }

            if (Path.GetExtension(path) != ".png")
            {
                return false;
            }

            // If the texture wasn't generated we generate it now.
            if (Texture == 0)
            {
                uint texture;
                Gl.glGenTextures(1, out texture);
                Texture = texture;
            }

            // We need to preserve the state of the binding so we dont mess up.
            // So we have to get the previous binding and after we are done we rebind it.
            int previousTexture;
            Gl.glGetIntegerv(Gl.GL_TEXTURE_BINDING_2D, out previousTexture);

            // Bind the current texture.
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture);

            using (var bitmapImage = new Bitmap(path))
            {
                bitmapImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                var bitmapImageData = bitmapImage.LockBits(
                    new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppRgb);
                Gl.glTexParameteri(Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE);
                Gl.glTexParameteri(Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE);
                Gl.glTexParameteri(Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_R, Gl.GL_CLAMP_TO_EDGE);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, bitmapImage.Width, bitmapImage.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapImageData.Scan0);
                bitmapImage.UnlockBits(bitmapImageData);
            }

            // Rebind last texture.
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, previousTexture);

            return true;
        }

        public void Dispose()
        {
            var texture = Texture;
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
