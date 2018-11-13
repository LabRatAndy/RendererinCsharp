using System.IO;
using System;
using OpenTK.Graphics.OpenGL;

namespace RendererinCsharp
{
    class Texture : IDisposable
    {
        private int handle;
        private bool cubemapped;

        Texture(string filename)
        {

        }
        Texture(string top, string bottom, string left, string right, string front, string back)
        {

        }
        public void Dispose()
        {

        }
        ~Texture()
        {

        }
        public void Bind()
        {

        }
        public void Unbind()
        {

        }
        public void Activate(TextureUnit textureunit)
        {

        }
        public bool IsCubeMapped
        {
            get { return cubemapped; }
        }

    }

}
