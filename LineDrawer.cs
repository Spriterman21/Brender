using System;


namespace Brender_0_5
{
    //https://cse.taylor.edu/~btoll/s99/424/res/ucdavis/GraphicsNotes/Bresenhams-Algorithm/Bresenhams-Algorithm.html
    public static class LineDrawer
    {
        static CharInfo[,] canvas;
        static float[,] zBuffering; 
        static CharInfo pixel;

        static CharInfo[][] canvasOverflowPixels = new CharInfo[2][];

        static int debugPixel = 0;

        #region Plot Line 1
        //public static void PlotLine(float[] point0, float[] point1)
        //{
        //    if (point0 == null || point1 == null)
        //    {
        //        return;
        //    }
            
        //    float x0 = point0[0];
        //    float y0 = point0[1];

        //    float x1 = point1[0];
        //    float y1 = point1[1];

        //    if (MathF.Abs(x1 - x0) > MathF.Abs(y1 - y0))
        //    {
        //        if (x0 > x1)
        //        {
        //            PlotLineLow(x1, y1, x0, y0);
        //        }
        //        else
        //        {
        //            PlotLineLow(x0, y0, x1, y1);
        //        }
        //    }
        //    else
        //    {
        //        if (y0 > y1)
        //        {
        //            PlotLineHigh(x1, y1, x0, y0);
        //        }
        //        else
        //        {
        //            PlotLineHigh(x0, y0, x1, y1);
        //        }
        //    }

        //    if (HolderClass.debug)
        //    {
        //        Console.SetCursorPosition(0, 0);
        //        Console.WriteLine("{0}, {1}, {2}, {3}\t\t\t", x0, y0, x1, y1);
        //        Console.ReadKey();
        //    }
        //}


        //static void PlotLineLow(float x0, float y0, float x1, float y1)
        //{
        //    int width = canvas.GetLength(0);
        //    int height = canvas.GetLength(1);

        //    float dx = x1 - x0;
        //    float dy = y1 - y0;
        //    int yi = 1;

        //    if (dy < 0)
        //    {
        //        yi = -1;
        //        dy = -dy;
        //    }

        //    pixel.Char.AsciiChar = (byte)'l';

        //    if (pixel.Char.AsciiChar == (byte)'\0')
        //    {
        //        pixel.Char.AsciiChar = (byte)'=';

        //        if (dx != 0)
        //        {
        //            if (dy / dx > 0.4)
        //            {
        //                if (yi == 1)
        //                {
        //                    pixel.Char.AsciiChar = (byte)'\\';
        //                }
        //                else
        //                {
        //                    pixel.Char.AsciiChar = (byte)'/';
        //                }
        //            }
        //        }

        //    }

        //    int i0 = (int)x0;
        //    int y = (int)y0;
        //    int i1 = (int)x1;

        //    float e = -(1 - (y0 - y) - (dy * (1 - (x0 - i0))) / dx);
            
        //    for (int x = i0; x <= i1; x++)
        //    {
        //        Iluminate(x, y);

        //        if (e >= 0)
        //        {
        //            y += yi;
        //            e -= 1;
        //        }

        //        e += dy / dx;
        //    }

        //    pixel.Char.AsciiChar = (byte)'L';
        //    Iluminate(i1, (int)y1);
        //}

        //static void PlotLineHigh(float x0, float y0, float x1, float y1)
        //{
        //    int width = canvas.GetLength(0);
        //    int height = canvas.GetLength(1);

        //    float dx = x1 - x0;
        //    float dy = y1 - y0;
        //    int xi = 1;

        //    if (dx < 0)
        //    {
        //        xi = -1;
        //        dx = -dx;
        //    }

        //    pixel.Char.AsciiChar = (byte)'h';

        //    if (pixel.Char.AsciiChar == (byte)'\0')
        //    {
        //        pixel.Char.AsciiChar = (byte)'|';

        //        if (dx != 0)
        //        {
        //            if (dy / dx * xi < 2.2)
        //            {
        //                pixel.Char.AsciiChar = xi == 1 ? (byte)'\\' : (byte)'/';
        //            }
        //        }
        //    }

        //    int i0 = (int)y0;
        //    int x = (int)x0;
        //    int i1 = (int)y1;

        //    float e = -(1 - (x0 - x) - (dx * (1 - (y0 - i0))) / dy);

        //    for (int y = i0; y <= i1; y++)
        //    {
        //        Iluminate(x, y);

        //        if (e >= 0)
        //        {
        //            x += xi;
        //            e -= 1;
        //        }

        //        e += dx / dy;
        //    }

        //    Iluminate((int)x1, i1);

        //}

        #endregion

        #region Plot Line UPDOWN version
        public static void PlotLineUpDown(float[] point0, float[] point1)
        {
            if (point0 == null || point1 == null)
            {
                return;
            }
            
            float x0 = point0[0];
            float y0 = point0[1];

            float x1 = point1[0];
            float y1 = point1[1];
            
            if (MathF.Abs(x1 - x0) > MathF.Abs(y1 - y0))
            {
                if (x0 > x1)
                {
                    float holder = x0;
                    x0 = x1;
                    x1 = holder;
                    holder = y0;
                    y0 = y1;
                    y1 = holder;
                }
                if (y1 < y0)
                {
                    PlotLineLowDown(x0, y0, x1, y1);
                }
                else
                {
                    PlotLineLowUp(x0, y0, x1, y1);
                }
            }
            else
            {
                if (x0 > x1)
                {
                    float holder = x0;
                    x0 = x1;
                    x1 = holder;
                    holder = y0;
                    y0 = y1;
                    y1 = holder;
                }
                if (y1 < y0)
                {
                    PlotLineHighDown(x0, y0, x1, y1);
                }
                else
                {
                    PlotLineHighUp(x0, y0, x1, y1);
                }
            }
        }


        static void PlotLineLowUp(float x0, float y0, float x1, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            float m = dy / dx;

            short i0 = (short)x0;
            short j = (short)y0;
            short i1 = (short)x1;

            float e = -(1 - (y0 - j) - m * (1 - (x0 - i0)));

            for (short i = i0; i <= i1; i++)
            {
                Iluminate(i, j);

                if (e >= 0)
                {
                    j++;
                    e--;
                }

                e += m;
            }

            Iluminate(i1, (short)y1);
        }

        static void PlotLineLowDown(float x0, float y0, float x1, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            float m = dy / dx;

            short i0 = (short)x0;
            short j = (short)y0;
            short i1 = (short)x1;

            float e = -(y0 - j + m * (1 - x0 + i0));

            for (short i = i0; i <= i1; i++)
            {
                Iluminate(i, j);

                if (e >= 0)
                {
                    j--;
                    e--;
                }

                e -= m;
            }

            Iluminate(i1, (short)y1);
        }
        static void PlotLineHighUp(float x0, float y0, float x1, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            float m = dx / dy;

            short j0 = (short)y0;
            short i = (short)x0;
            short j1 = (short)y1;

            float e = -(1 - x0 + i - m * (1 - y0 + j0));

            for (short j = j0; j <= j1; j++)
            {
                Iluminate(i, j);

                while (e >= 0)
                {
                    i++;
                    e--;
                }

                e += m;
            }

            Iluminate((short)x1, j1);
        }

        static void PlotLineHighDown(float x0, float y0, float x1, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            float m = dx / dy;

            short j0 = (short)y0;
            short i = (short)x0;
            short j1 = (short)y1;

            float e = -(1 - x0 + i + m * (y0 - j0));

            for (short j = j0; j >= j1; j--)
            {
                Iluminate(i, j);

                while (e >= 0)
                {
                    i++;
                    e--;
                }

                e -= m;
            }

            Iluminate((short)x1, j1);
        }
        #endregion

        #region Plot Line 2
        //static void PlotLine2(float[] point0, float[] point1)
        //{
        //    float x0 = point0[0];
        //    float y0 = point0[1];

        //    float x1 = point1[0];
        //    float y1 = point1[1];

        //    if (MathF.Abs(y1 - y0) <= MathF.Abs(x1 - x0))
        //    {
        //        PlotLineLow2(x0, y0, x1, y1);
        //    }
        //    else
        //    {
        //        PlotLineHigh2(x0, y0, x1, y1);
        //    }
        //}

        //static void PlotLineLow2(float x0, float y0, float x1, float y1)
        //{
        //    int width = canvas.GetLength(0);
        //    int height = canvas.GetLength(1);

        //    float dx = x1 - x0;
        //    float dy = y1 - y0;
        //    float m = MathF.Abs(dy / dx);

        //    int i0 = (int)x0;
        //    int i1 = (int)x1;
        //    int j = (int)y0;

        //    int Iinc = dx > 0 ? 1 : -1;
        //    int Jinc = dy > 0 ? 1 : -1;

        //    float e = -(1 - (y0 - j) - (1 - (x0 - i0))) * m;
        //    if (pixel.Char.AsciiChar == (byte)'\0')
        //    {
        //        pixel.Char.AsciiChar = (byte)'=';

        //        if (dx != 0)
        //        {
        //            if (dy / dx > 0.4)
        //            {
        //                pixel.Char.AsciiChar = dy > 0 ? (byte)'/' : (byte)'\\';
        //            }
        //        }
        //    }

        //    for (int i = i0; (Iinc > 0 ? i <= i1 : i >= i1); i += Iinc, e += m)
        //    {
        //        //Iluminate(i, j);

        //        if (e >= 0)
        //        {
        //            j += Jinc;
        //            e -= 1;
        //        }
        //    }
        //}

        //static void PlotLineHigh2(float x0, float y0, float x1, float y1)
        //{
        //    int width = canvas.GetLength(0);
        //    int height = canvas.GetLength(1);

        //    float dx = x1 - x0;
        //    float dy = y1 - y0;
        //    float m = MathF.Abs(dx / dy);

        //    int i = (int)x0;
        //    int j0 = (int)y0;
        //    int j1 = (int)y1;

        //    if (pixel.Char.AsciiChar == (byte)'\0')
        //    {
        //        pixel.Char.AsciiChar = (byte)'|';

        //        if (dx != 0)
        //        {
        //            if (m < 2.2)
        //            {
        //                pixel.Char.AsciiChar = dy < 0 ? (byte)'\\' : (byte)'/';
        //            }
        //        }
        //    }

        //    float e = 0;
        //    int Jinc = dy > 0 ? 1 : -1;

        //    if (dx > 0)
        //    {
        //        e = -(1 - (x0 - i) - (y0 - j0) * m);
        //    }
        //    else
        //    {
        //        e = -((x0 - i) - (y0 - j0) * m);
        //    }

        //    for (int j = j0; (Jinc > 0 ? j <= j1 : j >= j1); j += Jinc, e += m)
        //    {
        //        while (e >= 0)
        //        {
        //            i += dx * dy > 0 ? 1 : -1;
        //            e -= 1;
        //        }

        //        Iluminate(i, j);
        //    }
        //}
        #endregion

        #region Plot Line 3
        //static int n = 10;

        //public static void PlotLine3(float[] point0, float[] point1)
        //{
        //    if (point0 == null || point1 == null)
        //    {
        //        return;
        //    }

        //    float x0 = point0[0];
        //    float y0 = point0[1];

        //    float x1 = point1[0];
        //    float y1 = point1[1];

        //    if (MathF.Abs(x1 - x0) > MathF.Abs(y1 - y0))
        //    {
        //        if (x0 > x1)
        //        {
        //            PlotLineBitbyBit(x1, y1, x0, y0);
        //        }
        //        else
        //        {
        //            PlotLineBitbyBit(x0, y0, x1, y1);
        //        }
        //    }
        //    else
        //    {
        //        if (y0 > y1)
        //        {
        //            PlotLineBitbyBit(y1, x1, y0, x0);
        //        }
        //        else
        //        {
        //            PlotLineBitbyBit(y0, x0, y1, x1);
        //        }
        //    }
        //}


        //static void PlotLineBitbyBit(float x0, float y0, float x1, float y1)
        //{
        //    int X0 = (int)(x0 * MathF.Pow(2, n));
        //    int Y0 = (int)(y0 * MathF.Pow(2, n));
        //    int X1 = (int)(x1 * MathF.Pow(2, n));
        //    int Y1 = (int)(y1 * MathF.Pow(2, n));

        //    int dx = X1 - X0;
        //    int dy = Y1 - Y0;

        //    int m = (1 << n) - 1;
        //    float e = (2 * (Y0 & m) - (2 << n)) * dx - (2 * (X0 & m) - (1 << n)) * dy;

        //    int x = X0 >> n;
        //    int y = Y0 >> n;

        //    while (x < (X1 >> n))
        //    {
        //        if (e > 0)
        //        {
        //            y += 1;
        //            e -= 2 * dx << n;
        //        }

        //        Iluminate(x, y);

        //        x += 1;
        //        e += 2 * (dy << n);
        //    }
        //}
        #endregion

        static void Iluminate(short x, short y)
        {
            short width = (short)canvas.GetLength(0);
            short height = (short)canvas.GetLength(1);

            if (y >= 0 && y < height)
            {
                // Side overflow for rasterization
                if (x < 0)
                {
                    canvasOverflowPixels[0][y] = pixel;
                }
                else if (x >= width)
                {
                    canvasOverflowPixels[1][y] = pixel;
                }
                //painting pixel
                else
                {
                    if (HolderClass.debug)
                    {
                        if (x < Console.BufferWidth && y < Console.BufferHeight)
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write((char)(debugPixel % 26 + 65));
                        }
                    }
                    canvas[x, y] = pixel;
                }
            }

        }


        /// <summary>
        /// Fills area of polygon with pixels
        /// </summary>
        public static void Rasterize()
        {
            for (int y = 0; y < canvas.GetLength(1); y++)
            {
                int startX = 0;
                int endX = 0;

                bool onLine = false;
                bool offLine = false;
                CharInfo temp = HolderClass.background;
                
                /*
                for(int x=0;x<canvas.GetLength(0); x++)
                {
                    if (canvas[x, y] != temp)
                    {
                        onLine = true;
                        continue;
                    }

                    if(canvas[x, y] != temp && onLine)
                    {
                        startX = x;
                    }
                }
                *//*
                if (canvasOverflowPixels[0][y] != temp && canvas[0, y] == temp)
                {
                    startX = 0;
                }
                else
                {
                    for (int x = 0; x < canvas.GetLength(0) - 1; x++)
                    {
                        if (canvas[x, y] != temp && canvas[x + 1, y] == temp)
                        {
                            startX = x;
                            break;
                        }
                    }
                }

                if (startX == -1)
                {
                    continue;
                }

                for (int x = (startX + 2); x < canvas.GetLength(0); x++)
                {
                    if (canvas[x, y] != temp)
                    {
                        endX = x;
                        break;
                    }
                }
                if (endX == -1 && startX != -1 && canvasOverflowPixels[1][y] != temp)
                {
                    endX = (canvas.GetLength(0) - 1);
                }

                if (endX == -1)
                {
                    continue;
                }
                */
                
                if (canvasOverflowPixels[0][y] != temp)
                {
                    onLine = true;
                }


                for (int x = 0; x < canvas.GetLength(0); x++)
                {
                    if (canvas[x, y] == temp)
                    {
                        if (onLine && !offLine)
                        {
                            offLine = true;
                            startX = x;
                        }
                        continue;
                    }

                    if (offLine)
                    {
                        endX = x;
                        break;
                    }
                    onLine = true;
                }

                if (canvasOverflowPixels[1][y] != HolderClass.background && endX == 0 && startX != 0)
                {
                    endX = (int)canvas.GetLength(0);
                }
                
                for (int x = startX; x < endX; x++)
                {
                    canvas[x, y] = pixel;
                }
            }
        }


        public static CharInfo[,] DrawPolygon(Polygon polygon, int canvasX, int canvasY)
        {
            canvas = new CharInfo[canvasX, canvasY];
            canvasOverflowPixels[0] = new CharInfo[canvasY];
            canvasOverflowPixels[1] = new CharInfo[canvasY];

            pixel = polygon.edges;
            for (int i = 0; i < polygon.canvasPoints.Length - 1; i++)
            {
                PlotLineUpDown(polygon.canvasPoints[i], polygon.canvasPoints[i + 1]);
            }
            PlotLineUpDown(polygon.canvasPoints[polygon.canvasPoints.Length - 1], polygon.canvasPoints[0]);

            if (polygon.fill != HolderClass.background)
            {
                pixel = polygon.fill;
                Rasterize();
            }

            if (HolderClass.debug)
            {
                debugPixel++;
                Console.ReadKey();
            }

            return canvas;
        }
    }
}