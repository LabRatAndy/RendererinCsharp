using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace RendererinCsharp
{
    class RenderWindow : GameWindow
    {
        //cube
        private Shader cubeshader = null;
        private VertexBufferObject cubeVBO = null;
        private VertexArrayObject cubeVAO = null;
        private ElementBufferObject cubeIBO = null;
        private Texture cubeTexture = null;
        private Attribute cubePosition = null;
        private Attribute cubeTexCoords = null;
        int uniform_cubemodel;
        int uniform_cubeview;
        int uniform_cube_projection;
        int uniform_cubeTexture;
        int attribute_cube;
        int attribute_cube_texcoords;
        //skybox
        private Shader skyboxShader = null;
        private VertexBufferObject skyboxVBO = null;
        private VertexArrayObject skyboxVAO = null;
        private Texture skyboxTexture = null;
        private int uniform_skyboxview;
        private int uniform_skyboxprojection;
        private int uniform_skyboxtexture;
        private int attribute_skybox;
        private Attribute skyboxPosition = null;
        //viewpoint stuff
        private Vector3 camrapos;
        private Vector3 lookatpos;
        private Matrix4 view;
        private Matrix4 model;
        private Matrix4 skyboxview;
        private Matrix4 projection;
        //functions

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //cube shader
            cubeshader = new Shader("C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\cube.vert",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\cube.frag");
            //skybox shader
            skyboxShader = new Shader("C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\skybox.vert",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\skybox.frag");
            //get attributes and uniforms
            attribute_cube = cubeshader.GetAttibuteHandle("position");
            attribute_cube_texcoords = cubeshader.GetAttibuteHandle("texcoords");
            attribute_skybox = cubeshader.GetAttibuteHandle("position");
            uniform_cubemodel = cubeshader.GetUniformHandle("model");
            uniform_cubeview = cubeshader.GetUniformHandle("view");
            uniform_cube_projection = cubeshader.GetUniformHandle("projection");
            uniform_cubeTexture = cubeshader.GetUniformHandle("cubetexture");
            uniform_skyboxview = skyboxShader.GetUniformHandle("view");
            uniform_skyboxprojection = skyboxShader.GetUniformHandle("projection");
            uniform_skyboxtexture = skyboxShader.GetUniformHandle("skybox");
            if (attribute_cube == -1 || attribute_cube_texcoords == -1 || attribute_skybox == -1 || uniform_cubemodel == -1 || uniform_cubeTexture == -1 ||
                uniform_cubeview == -1 || uniform_cube_projection == -1 || uniform_skyboxprojection == -1 || uniform_skyboxtexture == -1 || uniform_skyboxview == -1)
            {
                Console.WriteLine("Error cannot get uniform or attribute handles");
            }
            //create cube buffers
            float[] cube;
            CreateCube(0.5f, out cube);
            int[] ibo;
            CreateEBO(out ibo);
            cubeVBO = new VertexBufferObject(cube, 0);
            cubeIBO = new ElementBufferObject(ibo);
            cubeVAO = new VertexArrayObject();
            cubeVAO.Bind();
            cubeVBO.BufferData(OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
            cubeIBO.BufferData(OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
            cubeVAO.SetVBO(cubeVBO);
            cubeVAO.SetIBO(cubeIBO);
            cubePosition = new Attribute("position", 3, 0, 5 * sizeof(float), 0,VertexAttribPointerType.Float, false);
            cubeVAO.AddAttribute(0, cubePosition);
            cubeTexCoords = new Attribute("texcoords", 2, 3 * sizeof(float), 5 * sizeof(float), 1, VertexAttribPointerType.Float, false);
            cubeVAO.AddAttribute(1, cubeTexCoords);
            cubeVAO.SetAttributes();
            cubeVAO.UnBind();
            //create sky box buffers
            float[] skybox;
            CreateSkybox(10.0f, out skybox);
            skyboxVBO = new VertexBufferObject(skybox, 108);
            skyboxVAO = new VertexArrayObject();
            skyboxVAO.Bind();
            skyboxVBO.BufferData(BufferUsageHint.StaticDraw);
            skyboxVAO.SetVBO(skyboxVBO);
            skyboxPosition = new Attribute("position", 3 * sizeof(float), 0, 3, 0, VertexAttribPointerType.Float, false);
            skyboxVAO.AddAttribute(0, skyboxPosition);
            skyboxVAO.SetAttributes();
            skyboxVAO.UnBind();
            //cube texture
            cubeTexture = new Texture("C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\Container2.png");
            cubeTexture.SetTexParameterI(TextureParameterName.TextureWrapS, All.Repeat);
            cubeTexture.SetTexParameterI(TextureParameterName.TextureWrapT, All.Repeat);
            cubeTexture.SetTexParameterI(TextureParameterName.TextureMagFilter, All.Linear);
            cubeTexture.SetTexParameterI(TextureParameterName.TextureMinFilter, All.LinearMipmapLinear);
            //skybox texture
            skyboxTexture = new Texture("C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\top.png",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\bottom.png",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\left.png",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\right.png",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\front.png",
                "C:\\Users\\me\\Documents\\Visual Studio 2015\\Projects\\Project1\\Debug\\back.png");
            skyboxTexture.SetTexParameterI(TextureParameterName.TextureMinFilter, All.Linear);
            skyboxTexture.SetTexParameterI(TextureParameterName.TextureMagFilter, All.Linear);
            skyboxTexture.SetTexParameterI(TextureParameterName.TextureWrapR, All.ClampToEdge);
            skyboxTexture.SetTexParameterI(TextureParameterName.TextureWrapS, All.ClampToEdge);
            skyboxTexture.SetTexParameterI(TextureParameterName.TextureWrapT, All.ClampToEdge);
            //viewpoint
            camrapos = new Vector3(-7.0f, 7.0f, -7.0f);
            lookatpos = new Vector3(0.0f, 0.0f, 0.0f);
            model = Matrix4.Identity;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            //clear scene
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            //draw cube
            GL.DepthFunc(DepthFunction.Less);
            cubeshader.Use();
            cubeTexture.Activate(TextureUnit.Texture0);
            cubeTexture.Bind();
            GL.Uniform1(uniform_cubeTexture, 0);
            GL.UniformMatrix4(uniform_cubemodel, false, ref model);
            GL.UniformMatrix4(uniform_cubeview, false, ref view);
            GL.UniformMatrix4(uniform_cube_projection, false, ref projection);
            cubeVAO.Bind();
            cubeVAO.Draw();
            cubeVAO.UnBind();
            cubeTexture.Unbind();
            //draw skybox
            GL.DepthFunc(DepthFunction.Lequal);
            skyboxShader.Use();
            skyboxTexture.Activate(TextureUnit.Texture1);
            skyboxTexture.Bind();
            GL.Uniform1(uniform_skyboxtexture, 1);
            GL.UniformMatrix4(uniform_skyboxview, false, ref skyboxview);
            GL.UniformMatrix4(uniform_skyboxprojection, false, ref projection);
            skyboxVAO.Bind();
            skyboxVAO.Draw(0, 36);
            skyboxVAO.UnBind();
            skyboxTexture.Unbind();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            view = Matrix4.LookAt(camrapos, lookatpos, new Vector3(0.0f, 1.0f, 0.0f));
            projection = Matrix4.CreatePerspectiveFieldOfView(90.0f, (800.0f / 600.0f), 0.1f, 100.0f);
            skyboxview = Matrix4.LookAt(camrapos, lookatpos, new Vector3(0.0f, 1.0f, 0.0f));
            skyboxview = skyboxview.ClearTranslation();
        }
        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
        }
        private void CreateCube(float size, out float[] cube)
        {
            cube = new float[]
            {
                -size,size,-size,1.0f,0.0f,
                -size,size,size,1.0f,1.0f,
                -size,-size,-size,0.0f,0.0f,
                -size,-size,size,0.0f,1.0f,
                size,size,-size,1.0f,0.0f,
                size,size,size,1.0f,1.0f,
                size,-size,-size,0.0f,0.0f,
                size,-size,size,0.0f,1.0f
            };
        }
        private void CreateEBO(out int[] cube)
        {
            cube = new int[]
            {
                0,1,3,
                3,2,0,
		        //right
		        4,6,7,
                6,5,4,
		        //top
		        5,1,0,
                0,4,5,
		        //bottom
		        7,3,2,
                2,6,7,
		        //front
		        0,2,6,
                6,4,0,
		        //back
		        5,1,3,
                3,7,5
            };
        }
        private void CreateSkybox(float size, out float[] skybox)
        {
            skybox = new float[]
            {
                -size,  size, -size,
                -size, -size, -size,
                size, -size, -size,
                size, -size, -size,
                size,  size, -size,
                -size,  size, -size,

                -size, -size,  size,
                -size, -size, -size,
                -size,  size, -size,
                -size,  size, -size,
                -size,  size,  size,
                -size, -size,  size,

                size, -size, -size,
                size, -size,  size,
                size,  size,  size,
                size,  size,  size,
                size,  size, -size,
                size, -size, -size,

                -size, -size,  size,
                -size,  size,  size,
                size,  size,  size,
                size,  size,  size,
                size, -size,  size,
                -size, -size,  size,

                -size,  size, -size,
                size,  size, -size,
                size,  size,  size,
                size,  size,  size,
                -size,  size,  size,
                -size,  size, -size,

                -size, -size, -size,
                -size, -size,  size,
                size, -size, -size,
                size, -size, -size,
                -size, -size,  size,
                size, -size, size
            };
        }



    }
}
