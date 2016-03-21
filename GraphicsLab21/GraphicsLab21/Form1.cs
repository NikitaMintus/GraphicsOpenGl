using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Lighting;



namespace GraphicsLab21
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private float angx = 0;
        private float angy = 0;

        private Vector calculateLight(Vector colorVecM, Vector colorVecL, Vector vecLight, Vector vecNormal, Vector vecReflected, Vector vecViewer, float shining)
        { 
            float r = colorVecM.X * colorVecL.X + VectorOperations.ScalarMultiplication(vecLight, vecNormal) + (float)Math.Pow((VectorOperations.ScalarMultiplication(vecReflected, vecViewer)), shining);
            float g = colorVecM.Y * colorVecL.Y + VectorOperations.ScalarMultiplication(vecLight, vecNormal) + (float)Math.Pow((VectorOperations.ScalarMultiplication(vecReflected, vecViewer)), shining);
            float b = colorVecM.Z * colorVecL.Z + VectorOperations.ScalarMultiplication(vecLight, vecNormal) + (float)Math.Pow((VectorOperations.ScalarMultiplication(vecReflected, vecViewer)), shining);

            r = (r > 1) ? 1.0f : r;
            g = (g > 1) ? 1.0f : g;
            b = (b > 1) ? 1.0f : b;

            return new Vector(r, g, b);
        }

        private Vector calculateLightCos(Vector colorVecM, Vector colorVecL, Vector vecLight, Vector vecNormal, Vector vecReflected, Vector vecViewer, float shining)
        {
            float r = colorVecM.X * colorVecL.X + VectorOperations.CalculateCos(vecLight, vecNormal) + (float)Math.Pow((VectorOperations.CalculateCos(vecReflected, vecViewer)), shining);
            float g = colorVecM.Y * colorVecL.Y + VectorOperations.CalculateCos(vecLight, vecNormal) + (float)Math.Pow((VectorOperations.CalculateCos(vecReflected, vecViewer)), shining);
            float b = colorVecM.Z * colorVecL.Z + VectorOperations.CalculateCos(vecLight, vecNormal) + (float)Math.Pow((VectorOperations.CalculateCos(vecReflected, vecViewer)), shining);

            r = (r > 1) ? 1.0f : r;
            g = (g > 1) ? 1.0f : g;
            b = (b > 1) ? 1.0f : b;

            return new Vector(r, g, b);
        }

        private Vector calculateReflectedVec(Vector light, Vector normal)
        {
            float up = VectorOperations.ScalarMultiplication(light, normal);
            float bottom = VectorOperations.ScalarMultiplication(normal, normal);
            float division = up / bottom;

            Vector right = new Vector(2 * division * normal.X, 2 * division * normal.Y, 2 * division * normal.Z);
            Vector reflectedVec = VectorOperations.Diff(light, right);
            return reflectedVec;
        }

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            float u = 0;
            float v = 0;
            float deltaU = 0.01f;
            float deltaV = 0.01f;
            float step = 0.1f;
            float endU = 1.0f;
            float endV = 1.0f;
            float c = 0.2f;
            float x1 = 0, y1 = 0, z1 = 0;
            float x2 = 0, y2 = 0, z2 = 0;
            float b = 0.5f, a = 1.0f, p = 3.0f;

            float[] light_specular_color = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] light_ambient_color = { 0.0f, 0.39f, 0.0f, 1.0f };
            float[] light_diffuse_color = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] light_position = {100.0f, 2.0f, -100.0f, 0.0f};

            Vector vecColorM = new Vector(1.0f, 0.0f, 0.0f); //blue
            Vector vecColorL = new Vector(1.0f, 1.0f, 1.0f); //white
            Vector vecLight = new Vector(50.0f, 1.0f, 0.0f);
            Vector vecViewer = new Vector(-20.0f, 4.0f, -10.0f);
            Vector normal;
            Vector light;
            Vector vecReflected;

            float shining = 1.0f;
        

            OpenGL gl = this.openGLControl1.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Color(0.0, 0.0, 1.0);
            gl.Translate(0, 2.0, -20);

            gl.Rotate(angx, 0, angx);
            gl.PolygonMode(SharpGL.Enumerations.FaceMode.FrontAndBack, SharpGL.Enumerations.PolygonMode.Filled);
            
            angx += 0.5f;

            //gl.Enable(OpenGL.GL_LIGHTING);
            //gl.Enable(OpenGL.GL_LIGHT0);
            //gl.Enable(OpenGL.GL_NORMALIZE);
            //gl.Light(SharpGL.Enumerations.LightName.Light0, SharpGL.Enumerations.LightParameter.Ambient, light_ambient_color);
            //gl.Light(SharpGL.Enumerations.LightName.Light0, SharpGL.Enumerations.LightParameter.Diffuse, light_diffuse_color);
            //gl.Light(SharpGL.Enumerations.LightName.Light0, SharpGL.Enumerations.LightParameter.Specular, light_specular_color);
            //gl.Light(SharpGL.Enumerations.LightName.Light0, SharpGL.Enumerations.LightParameter.Position, light_position);

               
            for (u = 0; u <= 4.0 * Math.PI; u += step)
            {
                gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
                float f = 0;
                for (v = 5.0f; v <= 7.0f; v += step)
                {
                    x2 = (v * (float)Math.Cos(u));
                    y2 = (v * (float)Math.Sin(u));
                    z2 = b * u + a * (float)Math.Sin(p * u);
                    normal = VectorOperations.GetNormal(u, v, deltaU, deltaV, a, b, p);

                    //gl.Normal(normal.X, normal.Y, normal.Z);
                    vecReflected = calculateReflectedVec(vecLight, normal);
                    light = calculateLightCos(vecColorM, vecColorL, vecLight, normal, vecReflected, vecViewer, shining);
                    gl.Color(light.X, light.Y, light.Z);
                    gl.Vertex(x2, y2, z2);

                   
                    x2 = (v * (float)Math.Cos(u + step));
                    y2 = (v * (float)Math.Sin(u + step));
                    z2 = b * (u + step) + a * (float)Math.Sin(p * (u + step));
                    normal = VectorOperations.GetNormal((u + step), v, deltaU, deltaV, a, b, p);
                    //gl.Normal(normal.X, normal.Y, normal.Z);
                    vecReflected = calculateReflectedVec(vecLight, normal);
                    light = calculateLightCos(vecColorM, vecColorL, vecLight, normal, vecReflected, vecViewer, shining);
                    gl.Color(light.X, light.Y, light.Z);
                    gl.Vertex(x2, y2, z2);
                   
                }
                gl.End();
            }

            Axies ax = new Axies();
            ax.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Design);
            gl.Flush();
        }
    }
}
