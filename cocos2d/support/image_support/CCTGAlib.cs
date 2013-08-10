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
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    internal enum CCTGAEnum
    {
        TGA_OK,
        TGA_ERROR_FILE_OPEN,
        TGA_ERROR_READING_FILE,
        TGA_ERROR_INDEXED_COLOR,
        TGA_ERROR_MEMORY,
        TGA_ERROR_COMPRESSED_FILE,
    }

    internal struct TGAHeader
    {
        public byte idlength;
        public byte colourmaptype;
        public byte datatypecode;
        public short colourmaporigin;
        public short colourmaplength;
        public byte colourmapdepth;
        public short x_origin;
        public short y_origin;
        public short width;
        public short height;
        public byte bitsperpixel;
        public byte imagedescriptor;
    };

    public class CCImageTGA
    {
        //private TGAHeader header;

        public int status;
        public byte type, pixelDepth;

        public short width;

        public short height;

        /// <summary>
        /// raw data
        /// </summary>
        public Color[] imageData;

        public int flipped;

		public CCImageTGA()
		{
		}

        public CCImageTGA (string fileName)
		{
            var tex = CCContentManager.SharedContentManager.Load<Texture2D>(fileName);

			width = (short) tex.Width;
			height = (short) tex.Height;

			imageData = new Color[tex.Width * tex.Height];
			tex.GetData(imageData);

			var tmp = new Color[tex.Width];
			for (int i = 0; i < tex.Height / 2; i++)
			{
				Array.Copy(imageData, i * tex.Width, tmp, 0, tex.Width);
				Array.Copy(imageData, (tex.Height - i - 1) * tex.Width, imageData, i * tex.Width, tex.Width);
				Array.Copy(tmp, 0, imageData, (tex.Height - i - 1) * tex.Width, tex.Width);
			}

		}
    }

    internal class CCTGAlib
    {

        private static byte ReadByte(byte[] buffer, int position)
        {
            return buffer[position];
        }

        private static short ReadShort(byte[] buffer, int position)
        {
            return (short)(buffer[position] | buffer[position + 1] << 8);
        }

        /// <summary>
        /// load the image header fields. We only keep those that matter!
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool LoadHeader(byte[] buffer, int bufSize, CCImageTGA psInfo)
        {
            TGAHeader header;
            int pos = 0;

            header.idlength = ReadByte(buffer, pos);
            pos++;
            header.colourmaptype = ReadByte(buffer, pos);
            pos++;
            header.datatypecode = ReadByte(buffer, pos);
            pos++;
            header.colourmaporigin = ReadShort(buffer, pos);
            pos += 2;
            header.colourmaplength = ReadShort(buffer, pos);
            pos += 2;
            header.colourmapdepth = ReadByte(buffer, pos);
            pos++;
            header.x_origin = ReadShort(buffer, pos);
            pos += 2;
            header.y_origin = ReadShort(buffer, pos);
            pos += 2;
            header.width = ReadShort(buffer, pos);
            pos += 2;
            header.height = ReadShort(buffer, pos);
            pos += 2;
            header.bitsperpixel = ReadByte(buffer, pos);
            pos++;
            header.imagedescriptor = ReadByte(buffer, pos);
            pos++;

            /*

            bool bRet = false;

            do
            {
                int step = sizeof (byte) * 2;
                if ((step + sizeof (byte)) > bufSize)
                {
                    break;
                }

                psInfo.type = ReadByte(buffer, step);

                step += sizeof (byte) * 2;
                step += sizeof (short) * 4;
                if ((step + sizeof (short) * 2 + sizeof (char)) > bufSize)
                {
                    break;
                }

                psInfo.width = ReadShort(buffer, step);
                psInfo.height = ReadShort(buffer, step + sizeof (short));
                psInfo.pixelDepth = ReadByte(buffer, step + sizeof (short) * 2);

                step += sizeof (char);
                step += sizeof (short) * 2;
                if ((step + sizeof (byte)) > bufSize)
                {
                    break;
                }

                byte cGarbage = ReadByte(buffer, step);

                psInfo.flipped = 0;
                if ((cGarbage & 0x20) != 0)
                {
                    psInfo.flipped = 1;
                }
                bRet = true;
            } while (true);
            */

            return true;
        }

        /// <summary>
        /// loads the image pixels. You shouldn't call this function directly
        /// </summary>
        /// <param name="buffer">red,green,blue pixel values</param>
        /// <returns></returns>
        public static bool LoadImageData(byte[] buffer, int bufSize, CCImageTGA psInfo)
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
                for (int i = headerSkip; i < buffer.Length; i += mode)
                {
                    psInfo.imageData[cx].R = buffer[i + 2];
                    psInfo.imageData[cx].G = buffer[i + 1];
                    psInfo.imageData[cx].B = buffer[i];
                    if (mode == 4)
                    {
                        psInfo.imageData[cx].A = buffer[i + 3];
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
            return true;

        }

        public static CCImageTGA Load(byte[] buffer)
        {
            var info = new CCImageTGA();

            LoadHeader(buffer, buffer.Length, info);

            return info;
        }

    /// <summary>
        /// this is the function to call when we want to load an image
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static CCImageTGA Load(string pszFilename)
        {
            //int mode, total;
            CCImageTGA info = null;
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
        public static void RGBToGreyscale(CCImageTGA psInfo) 
        {
            throw new NotImplementedException();
        }

        bool LoadRLEImageData(byte[] Buffer, UInt64 bufSize, CCImageTGA psInfo) 
        {
            throw new NotImplementedException();
        }
    }
}
