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
            int radiusLen = 5;
            List<Vector2> points = new List<Vector2>(); //list that consists all points that collide with the las circle
            int circlesInVain = 15;  //how many circles are in a vain

            if (CompareColors(col, Color.GRAY))          //Stone
            posRadius.Y = Raylib.GetRandomValue(100, 200);
            else posRadius.Y = Raylib.GetRandomValue(200, 900); //Dirt
            //Add on what level can a vain of blocks spawn here
            
            //All vains can generate on x level between 0 and 900
            posRadius.X = Raylib.GetRandomValue(1, 900);
            
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

        

        static void GenerateCaves()
        {
            bool CheckSquare(int posX, int posY, int length)
            {
                int i, j;
                for(i=posY;i<posY+length;i++)
                    for(j=posX;j<posX+length;j++)
                    {
                        if (!CompareColors(Raylib.GetImageColor(world, j, i), Color.LIGHTGRAY))
                            return false;
                    }
                return true;
            }

            int numberOfCaves = 60, i, j, circleRad = 5, p;
            string side;

            unsafe
            {
                fixed (Image* img = &world)
                {
                    for (i = 1; i <= numberOfCaves; i++)
                    {

                        
                        circleRad = Raylib.GetRandomValue(5, 10);
                        Vector2 position = new Vector2(Raylib.GetRandomValue(20, 880), 
                            Raylib.GetRandomValue((i / 10) * 100+100, (i/10)*100+250));
                        
                        if (i%2==0) side = "left";
                        else side = "right";

                        while ((position.X < 899 && position.X > 1) && (position.Y < 899)&&
                            !CheckSquare((int)position.X-circleRad, (int)position.Y-circleRad, 2*circleRad))
                        //(!CompareColors(Raylib.GetImageColor(world,(int)position.X, (int)position.Y), Color.WHITE)))
                        {
                            Raylib.ImageDrawCircle(img, (int)position.X, (int)position.Y, circleRad, Color.LIGHTGRAY);
                            for (j = (int)position.Y - circleRad; j <= (int)position.Y + circleRad; j++)
                            {
                                for (p = (int)position.X - circleRad; p <= (int)position.X + circleRad; p++) //in which interval to search for points that
                                {                   //collide with the circle [p,j]
                                    Vector2 point;
                                    point.X = p;
                                    point.Y = j;

                                    if (Raylib.CheckCollisionPointCircle(point, position, circleRad))
                                    {
                                        //if point collide add it to the List 

                                        //points.Add(point);
                                        Raylib.ImageDrawPixelV(img, point, Color.LIGHTGRAY);

                                    }
                                }

                            }
                            if (side == "left")
                            {
                                position.X = Raylib.GetRandomValue((int)position.X-circleRad, (int)position.X);
                                position.Y = Raylib.GetRandomValue((int)position.Y, (int)position.Y+circleRad/2);
                            }
                            else
                            {
                                position.X = Raylib.GetRandomValue((int)position.X, (int)position.X + circleRad);
                                position.Y = Raylib.GetRandomValue((int)position.Y, (int)position.Y + circleRad/2);
                            }
                        }
                    }
                }
            }
            }
        

        //Generates the surface 
        static void GenerateGround()
        {
            int i, j, rand;
            //starting coordinates of the mountain
            float mountainPosX = Raylib.GetRandomValue(0, 600), mountainStartY=0;
            //Maximum and minimum level for the surface
            float minY = 50, maxY = 200; 
            //current position                      peak of the mountain coordinates
            Vector2 position = new Vector2(0, 150), mountainPeak = new Vector2(0, minY);
            

            unsafe
            {
                fixed (Image* img = &world)
                {
                    for (i = 0; i <= 900; i++)
                    {
                        
                        //Draw mountain
                        //Detect if the construction of the mountain has started
                        if (position.X == mountainPosX) mountainStartY = position.Y;
                        position.X = i;
                        
                        //if the current position is in the range of the mountain
                        if (position.X >= mountainPosX&& position.Y - 1<mountainStartY)
                        {
                            //Detect if the current position is on the Peak
                            if (position.Y <= minY)
                            {
                                mountainPeak.X = position.X; //Initialise mountainPeak
                                mountainPeak.Y = position.Y;
                            }
                                //mountain before the peak - generate only upwards or same level
                                if(mountainPeak.X==0)
                                {
                                 rand = Raylib.GetRandomValue(1, 2);
                                }
                                //mountain after the peak - generate only downwards or same level
                                else
                                    {
                                     rand = Raylib.GetRandomValue(2, 3);
                                    }

                            if (rand == 1) position.Y -= 2; //upwards
                            else if (rand == 3) position.Y += 2; //downwards
                            else i++;                        //same level
                            Raylib.ImageDrawRectangle(img, (int)position.X, 0, 2, (int)position.Y, Color.SKYBLUE);
                            continue;
                        }
                        
                        //Draw flat ground
                        Raylib.ImageDrawRectangle(img, (int)position.X, 0, 1, (int)position.Y, Color.SKYBLUE);
                        
                        //if current position is in the limits of Y
                        if (position.Y == minY)
                        {
                         rand = Raylib.GetRandomValue(2, 4);
                        }
                        else if (position.Y == maxY)
                            {
                             rand = Raylib.GetRandomValue(0, 2);
                            }
                            else
                                {
                                 rand = Raylib.GetRandomValue(1, 5);
                                }

                        if (rand == 1) position.Y--;     //upwards
                        else if (rand == 3) position.Y++;//downwards
                             
                    }
                    
                 
                    //Draw snow on mountain Peak
                    for(i=(int)mountainPeak.Y;i<(int)mountainPeak.Y+20;i++)
                    {
                        for(j=0;j<900;j++)
                            if(CompareColors(Raylib.GetImageColor(world, j, i), Color.BROWN))
                            {
                                Raylib.ImageDrawPixel(img, j, i, Color.WHITE);
                            }
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

            int dirtVains = 200, stoneVains = 30;
            for(int i=0;i<stoneVains;i++) //Generates all stoneVains
            GenerateVain(Color.GRAY);
            for (int i = 0; i < dirtVains; i++) //Generates all dirtVains
            GenerateVain(Color.BROWN);
            //Add new block vains here
            GenerateCaves();
            GenerateGround();
            
            Raylib.ExportImage(world, "world.png");
            

            Raylib.CloseWindow();
        }
    }
}