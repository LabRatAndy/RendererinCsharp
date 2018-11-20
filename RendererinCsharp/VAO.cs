using OpenTK.Graphics.OpenGL;
using System;

namespace RendererinCsharp
{
    public class VertexArrayObject : IDisposable
    {
        private int handle;
        private Attribute[] attributeList;
        private VertexBufferObject vbo = null;
        private ElementBufferObject ibo = null;
        private int attributecount = 0;

        public VertexArrayObject()
        {
            GL.GenVertexArrays(1, out handle);
        }
        public void AddAttribute(int index, Attribute attribute)
        {
#if DEBUG
            if (index < 0) throw new IndexOutOfRangeException("cannot have an attribute with a negative index");
            if (index != attributecount) throw new IndexOutOfRangeException("Index not the next available one");
#endif
            attributeList[index] = attribute;
            attributecount++;
        }
        public void SetVBO(VertexBufferObject VBO)
        {
            if (vbo == null) vbo = VBO;
            else
            {
                vbo.UnBind();
                vbo.Dispose();
                vbo = VBO;
            }
            vbo.Bind();
        }
        public void SetIBO(ElementBufferObject IBO)
        {
            if (ibo == null) ibo = IBO;
            else
            {
                ibo.UnBind();
                ibo.Dispose();
                ibo = IBO;
            }
            ibo.Bind();
        }
        public void Bind()
        {
            GL.BindVertexArray(handle);
        }
        public void UnBind()
        {
            GL.BindVertexArray(0);
        }
        public void SetAttributes()
        {
            for (int n = 0; n < attributecount; n++)
            {
                attributeList[n].SetAttribute();
                attributeList[n].EnableAttribute();
            }

        }
        public void Draw()
        {
            ibo.Draw();
        }
        public void Draw(int first, int count)
        {
            vbo.Draw(first, count);
        }
        public void Dispose()
        {
            ibo.Dispose();
            vbo.Dispose();
            attributeList = null;
            GL.DeleteVertexArray(handle);
            GC.SuppressFinalize(this);
        }
        ~VertexArrayObject()
        {
            ibo.Dispose();
            vbo.Dispose();
            attributeList = null;
            GL.DeleteVertexArray(handle);
        }
    }

}
