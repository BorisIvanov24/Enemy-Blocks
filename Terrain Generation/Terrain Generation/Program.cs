using Raylib_cs;

namespace HelloWorld
{
    static class Program
    {

        static Image world;

        public static void Main()
        {
            Raylib.InitWindow(800, 480, "Hello World");
            
            
            world = Raylib.GenImageColor(900, 900, Color.WHITE);
            
            unsafe
            {
                fixed (Image *vp = &world)
                {
                    Raylib.ImageDrawRectangle(vp, 0, 0, 900, 100, Color.BROWN);
                    Raylib.ImageDrawRectangle(vp, 0, 100, 900, 800, Color.GRAY);
                }
             }

            Raylib.ExportImage(world, "world.png");
            

            Raylib.CloseWindow();
        }
    }
}