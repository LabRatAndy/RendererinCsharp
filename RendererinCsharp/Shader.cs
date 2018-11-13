using OpenTK;
using System;

namespace RendererinCsharp
{
    class Shader : IDisposable
    {
        private int handle;
        private int vertexshader;
        private int fragmentshader;
        Shader(string vertexshaderfile, string fragmentshaderfile)
        {

        }
        public void Use()
        {

        }
        public int GetAttibuteHandle(string attributename)
        {

        }
        public int GetUniformHandle(string uniformname)
        {

        }
        public void Dispose()
        {

        }
        ~Shader()
        {

        }
        private void LoadShader(string filename,OpenTK.Graphics.OpenGL4.ShaderType shadertype)
        {

        }
    }

}