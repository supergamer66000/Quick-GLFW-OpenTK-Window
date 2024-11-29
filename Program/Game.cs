﻿using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK;

namespace Program
{
    internal class Game
    {
        public unsafe Window* _window;

        public int width { get; set; }
        public int height { get; set; }
        public string title;

        // Loop Vars
        public double deltaTime;
        public double fps;
        private Stopwatch _stopwatch;
        private double _lastTime;
        private double _accumulator;
        private const double Timestep = 1.0 / 60.0; // Fixed timestep (60 FPS)

        public Game(int width, int height, string title)
        {
            this.width = width; this.height = height; this.title = title;

            InitializeGLFW();
            InitializeGL();
        }

        private void InitializeGLFW()
        {
            if (!GLFW.Init())
            {
                throw new Exception("Failed to initialize GLFW");
            }

            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 3);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core); // Compatibility profile for fixed pipeline
            GLFW.WindowHint(WindowHintBool.Resizable, true);

            unsafe
            {
                _window = GLFW.CreateWindow(this.width, this.height, this.title, null, null);
                if (_window == null)
                {
                    throw new Exception("Failed to create GLFW window");
                }
                GLFW.SetFramebufferSizeCallback(_window, (window, newWidth, newHeight) =>
                {
                    GL.Viewport(0, 0, newWidth, newHeight);
                });
                GLFW.MakeContextCurrent(_window);

                // Load OpenGL function bindings
                GL.LoadBindings(new GLFWBindingsContext());

                //GL.Viewport(0, 0, this.width, this.height);
            }
        }

        public void Start()
        {
            try
            {
                StartGameLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Starting Application: " + ex.ToString());
            }
        }

        public void Dispose()
        {
            unsafe
            {
                if (_window != null)
                {
                    GLFW.DestroyWindow(_window);
                    _window = null;
                }
            }
            GLFW.Terminate();
            GC.SuppressFinalize(this);
        }

        private void StartGameLoop()
        {
            _stopwatch = Stopwatch.StartNew();
            _lastTime = _stopwatch.Elapsed.TotalSeconds;

            // FPS stuff
            double fpsTimeElapsed = 0;
            int frameCount = 0;

            bool isRunning = true;
            while (isRunning)
            {
                unsafe { isRunning = !GLFW.WindowShouldClose(_window); }
                double currentTime = _stopwatch.Elapsed.TotalSeconds;
                deltaTime = currentTime - _lastTime;
                _lastTime = currentTime;

                /// FPS Logic ///
                //frameCount++;                     // Increment frame count
                //fpsTimeElapsed += deltaTime;      // Accumulate elapsed time
                //if (fpsTimeElapsed >= 1)
                //{
                //    fps = frameCount / fpsTimeElapsed;
                //    Console.WriteLine($"FPS: {fps}");
                //    frameCount = 0;
                //    fpsTimeElapsed = 0.0;
                //}
                ///

                _accumulator += deltaTime;
                while (_accumulator >= Timestep)
                {
                    Update(deltaTime);
                    _accumulator -= Timestep;
                }

                Render();
                unsafe { GLFW.SwapBuffers(_window); }
                GLFW.PollEvents();
            }
            Dispose();
        }

        private void Update(double deltaTime)
        {
            // GameUpdates goes here, e.g Physics, AI, ect.
        }

        private void InitializeGL()
        {
            // OpenGL initialize Code Goes Here
        }

        private void Render()
        {
            // Rendering Code goes here
        }
    }
}