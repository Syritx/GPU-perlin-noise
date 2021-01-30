using perlin_noise_visualization.src.shaders;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

using System;

namespace perlin_noise_visualization.src.rendering {

    class NoiseMap : Shader {

        int Resolution = 1000;
        float[] vertices;
        float seed = new Random().Next(0,100000);

        public NoiseMap(string v, string f) : base(v,f) {

            CreateVertices();

            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));
            
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
        }

        void CreateVertices() {
            int R = Resolution/2;
            List<float> vertices_list = new List<float>();

            for (int x = -R; x <= R; x++) {
                for (int y = -R; y <= R; y++) {
                    Vector2 toScreenCoords = new Vector2((float)x/(float)R, (float)y/(float)R);
                    //float multiplier = CreateNoiseLayer(3, 2, 0.5f, toScreenCoords.X+R, toScreenCoords.Y+R, seed);
                    vertices_list.Add(toScreenCoords.X);
                    vertices_list.Add(toScreenCoords.Y);
                    vertices_list.Add(1);
                    vertices_list.Add(1);
                    vertices_list.Add(1);
                }   
            }

            vertices = new float[vertices_list.Count];
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i] = vertices_list[i];
            }
        }

        public override void Render() {
            base.Render();
            GL.Enable(EnableCap.ProgramPointSize);

            seed += .04f;
            int seedLoc = GL.GetUniformLocation(Program, "s");
            Console.WriteLine(seedLoc);
            GL.Uniform1(seedLoc, seed);

            Use();
            int pointSizeLocation = GL.GetUniformLocation(Program, "pointSize");
            GL.Uniform1(pointSizeLocation, (float)Scene.ScreenSize.X/Resolution);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length);
        }
    }
}