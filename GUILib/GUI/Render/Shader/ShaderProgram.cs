using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using GuiLib.Logger;
using GuiLib.Util;

namespace GuiLib.GUI.Render.Shader
{
    abstract class ShaderProgram
    {
        private readonly int programID, vertexShaderID, fragmentShaderID;

        private Dictionary<string, int> uniforms = new Dictionary<string, int>();

        public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            programID = GL.CreateProgram();

            if (programID == 0)
                ALogger.defaultLogger.Log("Could not create Shader program!", LogLevel.Error);

            vertexShaderID = CreateShader(Loader.ReadFile(vertexShaderPath), ShaderType.VertexShader);
            fragmentShaderID = CreateShader(Loader.ReadFile(fragmentShaderPath), ShaderType.FragmentShader);

            GL.LinkProgram(programID);

            string programLog = GL.GetProgramInfoLog(programID);

            if(programLog != "")
                ALogger.defaultLogger.Log("Could not link shader program. Info log: " + programLog, LogLevel.Error);

            if (vertexShaderID != 0)
                GL.DetachShader(programID, vertexShaderID);

            if (fragmentShaderID != 0)
                GL.DetachShader(programID, fragmentShaderID);

            GL.ValidateProgram(programID);

            programLog = GL.GetProgramInfoLog(programID);

            if (programLog != "Validation successful.\n")
                ALogger.defaultLogger.Log("Could not validate shader program. Info log: " + programLog, LogLevel.Error);

            ALogger.defaultLogger.Log("Successfully loaded shaders: " + vertexShaderPath + " and " + fragmentShaderPath, LogLevel.Info);

            LoadUniforms();
        }

        public abstract void LoadUniforms();


        public void Start()
        {
            GL.UseProgram(programID);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            Stop();
            if (programID != 0)
                GL.DeleteProgram(programID);
        }

        #region Uniforms

        public void CreateUniform(string uniformName)
        {
            int uniformLocation = GL.GetUniformLocation(programID, uniformName);

            if (uniformLocation < 0)
                Console.WriteLine("WARNING: Could not load uniform: " + uniformName);


            uniforms.Add(uniformName, uniformLocation);
        }

        public void SetUniform(string uniformName, Matrix4 value)
        {
            GL.UniformMatrix4(uniforms[uniformName], false, ref value);
        }

        public void SetUniform(string uniformName, bool value)
        {
            GL.Uniform1(uniforms[uniformName], value ? 1 : 0);
        }

        public void SetUniform(string uniformName, float value)
        {
            GL.Uniform1(uniforms[uniformName], value);
        }

        public void SetUniform(string uniformName, int value)
        {
            GL.Uniform1(uniforms[uniformName], value);
        }

        public void SetUniform(string uniformName, Vector4 value)
        {
            GL.Uniform4(uniforms[uniformName], value);
        }

        public void SetUniform(string uniformName, Vector3 value)
        {
            GL.Uniform3(uniforms[uniformName], value);
        }

        public void SetUniform(string uniformName, Vector2 value)
        {
            GL.Uniform2(uniforms[uniformName], value);
        }

        #endregion



        public int CreateShader(string code, ShaderType type)
        {
            int shaderID = GL.CreateShader(type);

            if (shaderID == 0)
                ALogger.defaultLogger.Log("Could not create shader of type " + type + "!", LogLevel.Error);

            GL.ShaderSource(shaderID, code);
            GL.CompileShader(shaderID);

            string shaderLog = GL.GetShaderInfoLog(shaderID);

            if (shaderLog != "")
                ALogger.defaultLogger.Log("Could not compile shader of type " + type + ". Info log:" + shaderLog, LogLevel.Error);

            GL.AttachShader(programID, shaderID);

            return shaderID;
        }
    }
}
