using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CollisionMaskGenerator
{
    struct FileToProcess {
        public string Source;
        public string Output;
    }

    class Program
    {
        static bool bitwise = false;
        static List<FileToProcess> files = new List<FileToProcess>();
        static bool bAlphaMask = true;
        static Color colorMask = Color.Black;
        static int xFrames = 0;
        static int yFrames = 0;
        static int totalFrames = 0;
        static int frameStride = 1;
        static int frameWidth = 0;
        static int frameHeight = 0;

        static void ProcessFrames(Bitmap bmp, string outfile)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            xFrames = w / frameWidth;
            yFrames = h / frameHeight;
            int dx = bmp.Width / xFrames;
            int dy = bmp.Height / yFrames;

            StreamWriter sw = null;
            for (int iFrame = 1; iFrame <= totalFrames; iFrame++)
            {
                int x1 = (iFrame - 1) % xFrames * dx;
                int y1 = (iFrame - 1) / yFrames * dy;
                int x2 = x1 + dx;
                int y2 = y1 + dy;
                using (sw = new StreamWriter(string.Format(outfile, iFrame)))
                {
                    Console.WriteLine("Writing mask file to {0}", string.Format(outfile, iFrame));
                    for (int y = y1; y < y2; y++)
                    {
                        for (int x = x1; x < x2; x++)
                        {
                            byte bTest = 0;
                            if (bAlphaMask)
                            {
                                bTest = bmp.GetPixel(x, y).A;
                            }
                            else if (colorMask.R > 0)
                            {
                                bTest = bmp.GetPixel(x, y).R;
                            }
                            else if (colorMask.G > 0)
                            {
                                bTest = bmp.GetPixel(x, y).G;
                            }
                            else if (colorMask.B > 0)
                            {
                                bTest = bmp.GetPixel(x, y).B;
                            }
                            if (bTest != 0)
                            {
                                Console.Write("1");
                                sw.Write("1");
                            }
                            else
                            {
                                sw.Write("0");
                                Console.Write("0");
                            }
                        }
                        Console.WriteLine();
                        sw.WriteLine();
                    }
                }
            }
        }

        static void Main(string[] args)
        {

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("/bitwise", StringComparison.OrdinalIgnoreCase))
                {
                    bitwise = true;
                }
                else if (args[i].Equals("/red", StringComparison.OrdinalIgnoreCase))
                {
                    bAlphaMask = false;
                    colorMask = Color.Red;
                }
                else if (args[i].Equals("/green", StringComparison.OrdinalIgnoreCase))
                {
                    bAlphaMask = false;
                    colorMask = Color.Green;
                }
                else if (args[i].Equals("/blue", StringComparison.OrdinalIgnoreCase))
                {
                    bAlphaMask = false;
                    colorMask = Color.Blue;
                }
                else if (args[i].Equals("/frames", StringComparison.OrdinalIgnoreCase) && (i+1 < args.Length))
                {
                    int ix = args[i + 1].IndexOf('x');
                    frameWidth = int.Parse(args[i+1].Substring(0,ix));
                    frameHeight = int.Parse(args[i + 1].Substring(ix + 1));
                    i++;
                }
                else if (args[i].Equals("/framecount", StringComparison.OrdinalIgnoreCase) && (i + 1 < args.Length))
                {
                    totalFrames = int.Parse(args[i + 1]);
                    i++;
                }
                else if (args[i].Equals("/skipframes", StringComparison.OrdinalIgnoreCase) && (i + 1 < args.Length))
                {
                    frameStride = int.Parse(args[i + 1]);
                    i++;
                }
                else
                {
                    if (args[i].ToLower().EndsWith(".txt"))
                    {
                        try
                        {
                            StreamReader reader = new StreamReader(args[i]);
                            while (true)
                            {
                                string s = reader.ReadLine();
                                if (s.Length == 0 || s[0] == '#')
                                    continue;
                                FileToProcess fp = new FileToProcess();
                                fp.Source = s;
                                fp.Output = reader.ReadLine();
                                files.Add(fp);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to process {0}", args[i]);
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                    {
                        FileToProcess fp = new FileToProcess();
                        fp.Source = args[i];
                        files.Add(fp);
                    }
                }
            }
            foreach (FileToProcess fp in files)
            {
                try
                {
                    Image img = Image.FromFile(fp.Source);
                    Bitmap bmp = (Bitmap)img;
                    int h = img.Height;
                    int w = img.Width;
                    Console.WriteLine("{0} : {1} x {2} as {3}", args[0], w, h, img.PixelFormat);
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Bmp);
                    byte[] data = ms.ToArray();
                    int byteOffset = 0;
                    byte bb = 0;
                    FileInfo fi = new FileInfo(fp.Source);
                    string outname = fp.Output;
                    if (outname == null)
                    {
                        outname = fi.Name.Replace(fi.Extension, ".mask");
                    }
                    if (frameHeight > 0)
                    {
                        ProcessFrames(bmp, outname);
                        continue;
                    }
                    if (!bitwise)
                    {
                        int dx = (xFrames > 0 ? w / xFrames : 0);
                        int dy = (yFrames > 0 ? h / yFrames : 0);

                        using (StreamWriter sw = new StreamWriter(outname))
                        {
                            Console.WriteLine("Writing mask file to {0}", outname);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    byte bTest = 0;
                                    if (bAlphaMask)
                                    {
                                        bTest = bmp.GetPixel(x, y).A;
                                    }
                                    else if(colorMask.R > 0) 
                                    {
                                        bTest = bmp.GetPixel(x, y).R;
                                    }
                                    else if (colorMask.G > 0)
                                    {
                                        bTest = bmp.GetPixel(x, y).G;
                                    }
                                    else if (colorMask.B > 0)
                                    {
                                        bTest = bmp.GetPixel(x, y).B;
                                    }
                                    if (bTest != 0)
                                    {
                                        Console.Write("1");
                                        sw.Write("1");
                                    }
                                    else
                                    {
                                        sw.Write("0");
                                        Console.Write("0");
                                    }
                                }
                                Console.WriteLine();
                                sw.WriteLine();
                            }
                        }
                    }
                    if (bitwise)
                    {
                        using (StreamWriter sw = new StreamWriter(outname))
                        {
                            Console.WriteLine("Writing mask file to {0}", outname);
                            sw.Write("{0} {1} ", w, h);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    byte bTest = bmp.GetPixel(x, y).A;
                                    if (bTest != 0)
                                    {
                                        Console.Write("1");
                                        bb |= (byte)((1 << (7 - byteOffset)) & 0xff);
                                    }
                                    else
                                    {
                                        Console.Write("0");
                                    }
                                    byteOffset++;
                                    if (byteOffset == 8)
                                    {
                                        sw.Write("{0} ", bb);
                                        byteOffset = 0;
                                        Console.Write("|");
                                        bb = 0;
                                    }
                                }
                                // Final padding to ensure 8-bit alignment
                                if (byteOffset > 0)
                                {
                                    // Fill in the final bits
                                    bb = (byte)(bb << (7 - byteOffset));
                                    sw.Write("{0}", bb);
                                    while (byteOffset < 8)
                                    {
                                        Console.Write("0");
                                        byteOffset++;
                                    }
                                    byteOffset = 0;
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
