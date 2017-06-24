using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Anti_AntiAliasing
{
    public static class BusinessLayer
    {
        //Not my code
        //Credits to Francois Zard on StackOverFlow

        public static void GetListOfFiles()
        {
            try
            {
                Data.ListOfFiles.AddRange(Directory.GetDirectories(Data.FilePath));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
        }

        public static void ProcessImages()
        {
            foreach (var files in Data.ListOfFiles)
            {
                using (Bitmap b = (Bitmap)Image.FromFile(files))
                {
                    try
                    {
                        Resample(b);
                        //What happens are the image is resampled?
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.InnerException);
                    }
                }
            }
        }

        private static List<Point> GetNeighbors(Bitmap bmp, int x, int y)
        {
            List<Point> neighbors = new List<Point>();

            for (int i = x - 1; i > 0 && i <= x + 1 && i < bmp.Width; i++)
                for (int j = y - 1; j > 0 && j <= y + 1 && j < bmp.Height; j++)
                    neighbors.Add(new Point(i, j));
            return neighbors;
        }

        private static bool GrowSelected(Bitmap bmp, List<int> selectedColors)
        {
            bool flag = false;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color px = bmp.GetPixel(x, y);
                    if (px.A == 0)
                        continue;

                    Color pxS = Color.FromArgb(255, px);

                    if (selectedColors.Contains(pxS.ToArgb()))
                    {
                        if (!isBackedByNeighbors(bmp, x, y))
                            continue;

                        List<Point> neighbors = GetNeighbors(bmp, x, y);
                        foreach (Point p in neighbors)
                        {
                            Color n = bmp.GetPixel(p.X, p.Y);
                            if (!isBackedByNeighbors(bmp, p.X, p.Y))
                                bmp.SetPixel(p.X, p.Y, Color.FromArgb(n.A, pxS));
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }

            return flag;
        }

        private static bool isBackedByNeighbors(Bitmap bmp, int x, int y)
        {
            List<Point> neighbors = GetNeighbors(bmp, x, y);
            Color px = bmp.GetPixel(x, y);
            int similar = 0;
            foreach (Point p in neighbors)
            {
                Color n = bmp.GetPixel(p.X, p.Y);
                if (Color.FromArgb(255, px).ToArgb() == Color.FromArgb(255, n).ToArgb())
                    similar++;
            }

            return (similar > 2);
        }

        private static void Resample(Bitmap bmp)
        {
            // First we look for the most prominent colors i.e. They make up at least 1% of the image
            Hashtable stats = new Hashtable();

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color px = bmp.GetPixel(x, y);
                    if (px.A == 0)
                        continue;

                    Color pxS = Color.FromArgb(255, px);
                    if (stats.ContainsKey(pxS.ToArgb()))
                        stats[pxS.ToArgb()] = (int)stats[pxS.ToArgb()] + 1;
                    else
                        stats.Add(pxS.ToArgb(), 1);
                }
            }

            float totalSize = bmp.Width * bmp.Height;
            float minAccepted = 0.01f;
            List<int> selectedColors = new List<int>();

            // Make up a list with the selected colors
            foreach (int key in stats.Keys)
            {
                int total = (int)stats[key];
                if (((float)total / totalSize) > minAccepted)
                    selectedColors.Add(key);
            }

            // Keep growing the zones with the selected colors to cover the invalid colors created by
            // the anti-aliasing
            while (GrowSelected(bmp, selectedColors)) ;
        }
    }
}