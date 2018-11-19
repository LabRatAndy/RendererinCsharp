using System;
using OpenTK.Graphics.OpenGL4;

namespace RendererinCsharp
{
    public class Shader : IDisposable
    {
        private int handle;
        private int vertexshader;
        private int fragmentshader;
        Shader(string vertexshaderfile, string fragmentshaderfile)
        {
            handle = GL.CreateProgram();
            LoadShader(vertexshaderfile, ShaderType.VertexShader);
            LoadShader(fragmentshaderfile, ShaderType.FragmentShader);
            GL.AttachShader(handle, vertexshader);
            GL.AttachShader(handle, fragmentshader);
            GL.LinkProgram(handle);
            string error;
            error = GL.GetProgramInfoLog(handle);
            Console.WriteLine(error);
            GL.DetachShader(handle,vertexshader);
            GL.DetachShader(handle, fragmentshader);
            GL.DeleteShader(vertexshader);
            GL.DeleteShader(fragmentshader);
        }
        public void Use()
        {
            GL.UseProgram(handle);
        }
        public int GetAttibuteHandle(string attributename)
        {
            return GL.GetAttribLocation(handle, attributename);
        }
        public int GetUniformHandle(string uniformname)
        {
            return GL.GetUniformLocation(handle, uniformname);
        }
        public void Dispose()
        {
            GL.DeleteProgram(handle);
            GC.SuppressFinalize(this);
        }
        ~Shader()
        {
            GL.DeleteProgram(handle);
        }
        private void LoadShader(string filename,ShaderType shadertype)
        {
            string shadersource = System.IO.File.ReadAllText(filename);
            if(shadertype == ShaderType.VertexShader)
            {
                vertexshader = GL.CreateShader(shadertype);
                GL.ShaderSource(vertexshader, shadersource);
                GL.CompileShader(vertexshader);
                string error = null;
                error = GL.GetShaderInfoLog(vertexshader);
                Console.WriteLine(error);
            }
            if(shadertype == ShaderType.FragmentShader)
            {
                fragmentshader = GL.CreateShader(shadertype);
                GL.ShaderSource(fragmentshader, shadersource);
                GL.CompileShader(fragmentshader);
                string error = null;
                error = GL.GetShaderInfoLog(fragmentshader);
                Console.WriteLine(error);
            }
        }
    }

}