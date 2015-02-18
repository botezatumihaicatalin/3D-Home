using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using DotNetPixelFormat = System.Drawing.Imaging.PixelFormat;
using OpenTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Home3d.Model
{
    /// <summary>
    /// A class which contains OPEN GL texture binding information
    /// </summary>
    public class ObjImageTexture : IDisposable
    {
        private static readonly ICollection<string> AllowedFormats = new List<string>(new [] { ".png" });

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
        /// <throws>
        /// ArgumentException
        /// </throws>
        public void LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File does not exist!", path);
            }

            if (!Path.HasExtension(path))
            {
                throw new ArgumentException("File has no extension!", path);
            }

            if (!AllowedFormats.Contains(Path.GetExtension(path)))
            {
                throw new ArgumentException("The extension of the file is not the following : png", path);
            }

            // If the texture wasn't generated we generate it now.
            if (Texture == 0)
            {
                uint texture;
                GL.GenTextures(1, out texture);
                Texture = texture;
            }

            // We need to preserve the state of the binding so we dont mess up.
            // So we have to get the previous binding and after we are done we rebind it.
            int previousTexture;
            GL.GetInteger(GetPName.TextureBinding2D, out previousTexture);

            // Bind the current texture.
            GL.BindTexture(TextureTarget.Texture2D, Texture);

            using (var bitmapImage = new Bitmap(path))
            {
                bitmapImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                var bitmapImageData = bitmapImage.LockBits(
                    new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height),
                    ImageLockMode.ReadOnly,
                    DotNetPixelFormat.Format32bppArgb);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapImage.Width, bitmapImage.Height, 0, OpenTKPixelFormat.Bgra, PixelType.UnsignedByte, bitmapImageData.Scan0);
                bitmapImage.UnlockBits(bitmapImageData);
            }

            // Rebind last texture.
            GL.BindTexture(TextureTarget.Texture2D, previousTexture);
        }

        public void Dispose()
        {
            var texture = Texture;
            if (texture == 0)
            {
                return;
            }
            GL.DeleteTextures(1, ref texture);
            Texture = 0;
            ScaleU = 1;
            ScaleV = 1;
        }
    }
}
