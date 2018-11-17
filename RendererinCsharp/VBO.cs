using OpenTK.Graphics.OpenGL;
using System;

namespace RendererinCsharp
{
    public class VertexBufferObject : IDisposable
    {
        private int handle;
        private float[] vertexData = null;
        private int dataSize;

        VertexBufferObject(float[] vertexdata, int size)
        {
            GL.GenBuffers(1, out handle);
            this.vertexData = vertexdata;
            dataSize = size;
        }
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
        }
        public void BufferData(BufferUsageHint drawtype)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, drawtype);
        }
        public void Draw(int first, int count)
        {
            GL.DrawArrays(PrimitiveType.Triangles, first, count);
        }
        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void Dispose()
        {
            GL.DeleteBuffer(handle);
            GC.SuppressFinalize(this);
        }
        ~VertexBufferObject()
        {
            GL.DeleteBuffer(handle);
        }
    }

}
