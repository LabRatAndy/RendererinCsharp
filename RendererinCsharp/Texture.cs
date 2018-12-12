using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace RendererinCsharp
{
    class Texture : IDisposable
    {
        private int handle;
        private bool cubemapped;

        public Texture(string filename)
        {
            cubemapped = false;
            GL.GenTextures(1, out handle);
            GL.BindTexture(TextureTarget.Texture2D, handle);
            Image image = Image.FromFile(filename);
            Bitmap bitmap = new Bitmap(image);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb); 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, data.Width, data.Height,0,
                 OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
        }
        public Texture(string top, string bottom, string left, string right, string front, string back)
        {
            cubemapped = true;
            GL.GenTextures(1, out handle);
            GL.BindTexture(TextureTarget.TextureCubeMap, handle);
            Image image = Image.FromFile(top);
            Bitmap bitmap = new Bitmap(image);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
            image = Image.FromFile(bottom);
            bitmap = new Bitmap(image);
            data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                 OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
            image = Image.FromFile(left);
            bitmap = new Bitmap(image);
            data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                 OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
            image = Image.FromFile(right);
            bitmap = new Bitmap(image);
            data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                 OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
            image = Image.FromFile(front);
            bitmap = new Bitmap(image);
            data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                 OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
            image = Image.FromFile(back);
            bitmap = new Bitmap(image);
            data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
                 OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            data = null;
            bitmap.Dispose();
            bitmap = null;
            image.Dispose();
            image = null;
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
        public void Dispose()
        {
            GL.DeleteTexture(handle);
            GC.SuppressFinalize(this);
        }
        ~Texture()
        {
            GL.DeleteTexture(handle);
        }
        public void Bind()
        {
            if(cubemapped == true)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap, handle);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, handle);
            }
        }
        public void Unbind()
        {
            if (cubemapped == true)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }
        public void Activate(TextureUnit textureunit)
        {
            GL.ActiveTexture(textureunit);
        }
        public bool IsCubeMapped
        {
            get { return cubemapped; }
        }
        public void SetTexParameterI(TextureParameterName parametername, All parameter)
        {
            if(cubemapped == true)
            {
                GL.BindTexture(TextureTarget.TextureCubeMap, handle);
                GL.TexParameterI(TextureTarget.TextureCubeMap, parametername, new int[] { (int)parameter });
                GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, handle);
                GL.TexParameterI(TextureTarget.Texture2D, parametername, new int[] { (int)parameter });
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

    }

}
