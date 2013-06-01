/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public enum TGAEnum
    {
        TGA_OK,
        TGA_ERROR_FILE_OPEN,
        TGA_ERROR_READING_FILE,
        TGA_ERROR_INDEXED_COLOR,
        TGA_ERROR_MEMORY,
        TGA_ERROR_COMPRESSED_FILE,
    }

    public class ImageTGA
    {
        public int status;
        public char type, pixelDepth;

        /// <summary>
        /// map width
        /// </summary>
        public short width;

        /// <summary>
        /// map height
        /// </summary>
        public short height;

        /// <summary>
        /// raw data
        /// </summary>
        public Color[] imageData;
        public int flipped;

        public static ImageTGA Load(string filename)
        {
            var tex = CCApplication.SharedApplication.Content.Load<Texture2D>(filename);

            var image = new ImageTGA();

            image.width = (short) tex.Width;
            image.height = (short) tex.Height;

            image.imageData = new Color[tex.Width * tex.Height];
            tex.GetData(image.imageData);

            var tmp = new Color[tex.Width];
            for (int i = 0; i < tex.Height / 2; i++)
            {
                Array.Copy(image.imageData, i * tex.Width, tmp, 0, tex.Width);
                Array.Copy(image.imageData, (tex.Height - i - 1) * tex.Width, image.imageData, i * tex.Width, tex.Width);
                Array.Copy(tmp, 0, image.imageData, (tex.Height - i - 1) * tex.Width, tex.Width);
            }
            
            return image;
        }
    }

    public class TGAlib
    {
        /// <summary>
        /// load the image header fields. We only keep those that matter!
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool LoadHeader(byte[] Buffer, UInt64 bufSize, ImageTGA psInfo)
        {
            bool bRet = false;

            //do
            //{
            //    UInt64 step = sizeof(char) * 2;
            //    if ((step + sizeof(char)) > bufSize)
            //    {
            //        break;
            //    }

            //    memcpy(psInfo.type, Buffer + step, sizeof(char));

            //    step += sizeof(char) * 2;
            //    step += sizeof(short) * 4;
            //    if ((step + sizeof(short) * 2 + sizeof(char)) > bufSize)
            //    {
            //        break;
            //    }
            //    memcpy(psInfo.width, Buffer + step, sizeof(short));
            //    memcpy(psInfo.height, Buffer + step + sizeof(short), sizeof(short));
            //    memcpy(psInfo.pixelDepth, Buffer + step + sizeof(short) * 2, sizeof(char));

            //    step += sizeof(char);
            //    step += sizeof(short) * 2;
            //    if ((step + sizeof(char)) > bufSize)
            //    {
            //        break;
            //    }
            //    char cGarbage;
            //    memcpy(cGarbage, Buffer + step, sizeof(char));

            //    psInfo.flipped = 0;
            //    if (cGarbage & 0x20)
            //    {
            //        psInfo.flipped = 1;
            //    }
            //    bRet = true;
            //} while (0);

            return bRet;
        }

        /// <summary>
        /// loads the image pixels. You shouldn't call this function directly
        /// </summary>
        /// <param name="Buffer">red,green,blue pixel values</param>
        /// <returns></returns>
        public static bool LoadImageData(byte[] Buffer, int bufSize, ImageTGA psInfo)
        {
            int mode;
            int headerSkip = (1 + 2) * 6; // sizeof(char) + sizeof(short) = size of the header

            // mode equal the number of components for each pixel
            mode = psInfo.pixelDepth / 8;

            // mode=3 or 4 implies that the image is RGB(A). However TGA
            // stores it as BGR(A) so we'll have to swap R and B.
            if (mode >= 3)
            {
                int cx = 0;
                for (int i = headerSkip; i < Buffer.Length; i += mode)
                {
                    psInfo.imageData[cx].R = Buffer[i + 2];
                    psInfo.imageData[cx].G = Buffer[i + 1];
                    psInfo.imageData[cx].B = Buffer[i];
                    if (mode == 4)
                    {
                        psInfo.imageData[cx].A = Buffer[i + 3];
                    }
                    else
                    {
                        psInfo.imageData[cx].A = 255;
                    }
                }
            }
            else
            {
                return (false);
            }
            return(true);
          
        }

        /// <summary>
        /// this is the function to call when we want to load an image
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static ImageTGA Load(string pszFilename)
        {
            //int mode, total;
            ImageTGA info = null;
            //CCFileData data = new CCFileData(pszFilename, "rb");
            //UInt64 nSize = data.Size;
            //byte[] pBuffer = data.Buffer;

            //do
            //{
            //    if (pBuffer == null)
            //    {
            //        break;
            //    }
            //    //info = malloc(sizeof(tImageTGA)) as tImageTGA;

            //    // get the file header info
            //    if (tgaLoadHeader(pBuffer, nSize, info) == null)
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_MEMORY;
            //        break;
            //    }

            //    // check if the image is color indexed
            //    if (info.type == 1)
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_INDEXED_COLOR;
            //        break;
            //    }

            //    // check for other types (compressed images)
            //    if ((info.type != 2) && (info.type != 3) && (info.type != 10))
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_COMPRESSED_FILE;
            //        break;
            //    }

            //    // mode equals the number of image components
            //    mode = info.pixelDepth / 8;
            //    // total is the number of unsigned chars to read
            //    total = info.height * info.width * mode;
            //    // allocate memory for image pixels
            //    // info.imageData = (char[])malloc(sizeof(unsigned char) * total);

            //    // check to make sure we have the memory required
            //    if (info.imageData == null)
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_MEMORY;
            //        break;
            //    }

            //    bool bLoadImage = false;
            //    // finally load the image pixels
            //    if (info.type == 10)
            //    {
            //        bLoadImage = tgaLoadRLEImageData(pBuffer,nSize, info);
            //    }
            //    else
            //    {
            //        bLoadImage = tgaLoadImageData(pBuffer, nSize, info);
            //    }

            //    // check for errors when reading the pixels
            //    if (!bLoadImage)
            //    {
            //        info.status = TGAEnum.TGA_ERROR_READING_FILE;
            //        break;
            //    }
            //    info->status = TGA_OK;

            //    if (info->flipped)
            //    {
            //        tgaFlipImage(info);
            //        if (info->flipped)
            //        {
            //            info->status = TGA_ERROR_MEMORY;
            //        }
            //    }
            //} while (0);

            return info;
        }

        /// <summary>
        /// converts RGB to greyscale
        /// </summary>
        /// <param name="psInfo"></param>
        public static void RGBToGreyscale(ImageTGA psInfo) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// releases the memory used for the image
        /// </summary>
        /// <param name="psInfo"></param>
        public static void Destroy(ImageTGA psInfo)
        {
            if (psInfo != null)
            {
                if (psInfo.imageData != null)
                {
                    psInfo.imageData = null;
                    //free();
                }

                psInfo = null;
            }
        }

        bool LoadRLEImageData(byte[] Buffer, UInt64 bufSize, ImageTGA psInfo) 
        {
            throw new NotImplementedException();
        }
    }
}
