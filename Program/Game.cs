using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Drawing;

namespace Program
{
    internal class Game : IDisposable
    {
        public unsafe Window* _window;

        public int Width { get; set; }
        public int Height { get; set; }
        public string Title;

        // Loop Vars
        public double _deltaTime;
        public double _fps;
        private Stopwatch _stopwatch;
        private double _lastTime;
        private double _accumulator;
        private const double Timestep = 1.0 / 60.0; // Fixed timestep (60 FPS)

        public Game(int width, int height, string title)
        {
            this.Width = width; this.Height = height; this.Title = title;

            InitializeGLFW();
            InitializeGL();

            var error = GL.GetError();
            if (error != 0)
            {
                throw new Exception("Error with OpenGL" + error);
            }
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
                _window = GLFW.CreateWindow(this.Width, this.Height, this.Title, null, null);
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
                GL.LoadBindings(new GLFWBindingsContext()); // Load binding for OpenGL
            }
        }

        public void Start()
        {
            try
            {
                RunGameLoop();
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

        private void RunGameLoop()
        {
            _stopwatch = Stopwatch.StartNew();
            _lastTime = _stopwatch.Elapsed.TotalSeconds;

            double fpsElapsedTime = 0;
            int frameCount = 0;

            bool isRunning = true;
            try
            {
                while (isRunning)
                {
                    unsafe { isRunning = !GLFW.WindowShouldClose(_window); }

                    double currentTime = _stopwatch.Elapsed.TotalSeconds;
                    _deltaTime = currentTime - _lastTime;
                    _lastTime = currentTime;

                    // Calculate FPS
                    frameCount++;
                    fpsElapsedTime += _deltaTime;
                    if (fpsElapsedTime >= 1.0)
                    {
                        _fps = frameCount / fpsElapsedTime;
                        Console.WriteLine($"FPS: {_fps}");
                        frameCount = 0;
                        fpsElapsedTime = 0.0;
                    }

                    // Update and render as fast as possible
                    Update(_deltaTime);
                    Render();

                    // Swap buffers and poll events
                    unsafe { GLFW.SwapBuffers(_window); }
                    GLFW.PollEvents();

                    // Optional: Yield CPU if too fast (comment out if not needed)
                    Thread.Sleep(0); // Gives time to other threads, reduces CPU load slightly
                }
            }
            finally
            {
                Dispose(); // Deletes all unnessary stuff after the program Exits
            }
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
            unsafe { if ( _window == null) return; } // Additional Error Checking

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.AliceBlue);

            // Rendering Code goes here
        }
    }
}