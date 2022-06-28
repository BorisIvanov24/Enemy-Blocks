using Raylib_cs;
using System.Numerics;

namespace HelloWorld
{
    static class Program
    {

        static Image world;

        //returns true if two colors are equal
        static bool CompareColors(Color col1, Color col2)
        {
            if (col1.a == col2.a && col1.r == col2.r && col1.g == col2.g && col1.b == col2.b)
                return true;
            return false;
        }

        //Generates one vain (dirt,stone)
        static void GenerateVain(Color col)
        {
            Vector2 posRadius;
            int radiusLen = 10;
            List<Vector2> points = new List<Vector2>(); //list that consists all points that collide with the las circle
            int circlesInVain = 10;  //how many circles are in a vain

            if (CompareColors(col, Color.GRAY))          //Stone
            posRadius.Y = Raylib.GetRandomValue(50, 200);
            else posRadius.Y = Raylib.GetRandomValue(200, 900); //Dirt
            //Add on what level can a vain of blocks spawn here
            
            //All vains can generate on x level between 0 and 900
            posRadius.X = Raylib.GetRandomValue(0, 900);
            
            unsafe
            {
                fixed (Image* img = &world)
                {
                    int i, j, p;
                    for(i=1;i<=circlesInVain;i++)  //how many circles are in a vain
                    {
                        Raylib.ImageDrawCircleV(img, posRadius, radiusLen, col);
                        for(j=(int)posRadius.Y- radiusLen; j<= (int)posRadius.Y+ radiusLen; j++) 
                        {
                            for(p=1;p<=900;p++) //in which interval to search for points that
                            {                   //collide with the circle [p,j]
                                Vector2 point;
                                point.X = p;
                                point.Y = j;
                                
                                if(Raylib.CheckCollisionPointCircle(point, posRadius, radiusLen))
                                {
                                    //if point collide add it to the List 
                                    
                                    points.Add(point);
                                    Raylib.ImageDrawPixelV(img, point, col);
                                    
                                }
                            }
                            
                        }
                        //Choose new posRadius by random index on List<> points
                        int rand = Raylib.GetRandomValue(0, points.Count - 1);
                        posRadius = points[rand];
                        points.Clear();
                    }
                   
                }
            }
        }

        public static void Main()
        {
            Raylib.InitWindow(800, 480, "Hello World");
            
            
            world = Raylib.GenImageColor(900, 900, Color.WHITE);
            
            unsafe
            {
                fixed (Image *vp = &world)
                {
                    Raylib.ImageDrawRectangle(vp, 0, 0, 900, 200, Color.BROWN);
                    Raylib.ImageDrawRectangle(vp, 0, 200, 900, 700, Color.GRAY);
                }
             }

            int dirtVains = 80, stoneVains = 20;
            for(int i=0;i<stoneVains;i++) //Generates all stoneVains
            GenerateVain(Color.GRAY);
            for (int i = 0; i < dirtVains; i++) //Generates all dirtVains
            GenerateVain(Color.BROWN);
            //Add new block vains here

            Raylib.ExportImage(world, "world.png");
            

            Raylib.CloseWindow();
        }
    }
}