using OpenTK.Graphics.OpenGL;
using System;
namespace RendererinCsharp
{
    public class ElementBufferObject : IDisposable
    {
        private int handle;
        private int[] ibo = null;

        public ElementBufferObject(int[] ibo)
        {
            GL.GenBuffers(1, out handle);
            this.ibo = ibo;
        }
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
        }
        public void BufferData(BufferUsageHint drawtype)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, ibo.Length * sizeof(int), ibo, drawtype);
        }
        public void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, ibo.Length, DrawElementsType.UnsignedInt, 0);
        }
        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public void Dispose()
        {
            GL.DeleteBuffer(handle);
            GC.SuppressFinalize(this);
        }
        ~ElementBufferObject()
        {
            GL.DeleteBuffer(handle);
        }
    }

}
