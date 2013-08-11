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
using Microsoft.Xna.Framework.Content;

namespace Cocos2D
{
    /// <summary>
    /// Helper class to handle file operations
    /// </summary>
    public class CCFileUtils
    {
        protected static bool s_bPopupNotify = true;
        /// <summary>
        /// Set/Get whether pop-up a message box when the image load failed
        /// </summary>
        public static bool IsPopupNotify
        {
            get
            {
                return s_bPopupNotify;
            }
            set
            {
                s_bPopupNotify = value;
            }
        }

        public static Stream GetFileStream(string fileName)
        {
            fileName = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fileName);
            return TitleContainer.OpenStream(fileName);
        }

        /// <summary>
        /// @brief Get resource file data
        /// @param[in]  pszFileName The resource file name which contain the path
        /// @param[in]  pszMode The read mode of the file
        /// @param[out] pSize If get the file data succeed the it will be the data size,or it will be 0
        /// @return if success,the pointer of data will be returned,or NULL is returned
        /// @warning If you get the file data succeed,you must delete it after used.
        /// </summary>
        /// <param name="pszFileName"></param>
        /// <returns></returns>
        public static string GetFileData(string pszFileName)
        {
            return CCContentManager.SharedContentManager.Load<string>(pszFileName);
        }

        public static byte[] GetFileBytes(string pszFileName)
        {
            pszFileName = System.IO.Path.Combine(CCContentManager.SharedContentManager.RootDirectory, pszFileName);

            using (var stream = TitleContainer.OpenStream(pszFileName))
            {
                var buffer = new byte[1024];
                var ms = new MemoryStream();

                int readed = 0;

                readed = stream.Read(buffer, 0, 1024);
                while (readed > 0)
                {
                    ms.Write(buffer, 0, readed);
                    readed = stream.Read(buffer, 0, 1024);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// @brief Get resource file data from zip file
        /// @param[in]  pszFileName The resource file name which contain the relative path of zip file
        /// @param[out] pSize If get the file data succeed the it will be the data size,or it will be 0
        /// @return if success,the pointer of data will be returned,or NULL is returned
        /// @warning If you get the file data succeed,you must delete it after used.
        /// </summary>
        /// <param name="pszZipFilePath"></param>
        /// <param name="pszFileName"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public static char[] GetFileDataFromZip(string pszZipFilePath, string pszFileName, UInt64 pSize)
        {
            throw new NotImplementedException("Cannot load zip files for this method has not been realized !");
        }

        /// <summary>
        /// removes the HD suffix from a path
        /// @returns const char * without the HD suffix
        /// @since v0.99.5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CCRemoveHDSuffixFromFile(string path)
        {
            throw new NotImplementedException("Remove hd picture !");
        }

        /// <summary>
        /// @brief   Generate the absolute path of the file.
        /// @param   pszRelativePath     The relative path of the file.
        /// @return  The absolute path of the file.
        /// @warning We only add the ResourcePath before the relative path of the file.
        /// If you have not set the ResourcePath,the function add "/NEWPLUS/TDA_DATA/UserData/" as default.
        /// You can set ResourcePath by function void setResourcePath(const char *pszResourcePath);
        /// </summary>
        /// <param name="pszRelativePath"></param>
        /// <returns></returns>
        public static string FullPathFromRelativePath(string pszRelativePath)
        {
            // todo: return self now
            return pszRelativePath;
            // throw new NotImplementedException("win32 only definition does not realize !");
        }

        /// <summary>
        /// extracts the directory from the pszRelativeFile and uses that directory path as the
        /// path for the pszFilename.
        /// </summary>
        public static string FullPathFromRelativeFile(string pszFilename, string pszRelativeFile)
        {
            string path = Path.GetDirectoryName(pszRelativeFile);
            return Path.Combine(path, RemoveExtension(pszFilename));
        }

        public static string RemoveExtension(string fileName)
        {
            int len = fileName.LastIndexOf('.');
            if (len != -1)
            {
                return fileName.Substring(0, len);
            }
            return fileName;
        }

        /// <summary>
        /// @brief  Set the ResourcePath,we will find resource in this path
        /// @param pszResourcePath  The absolute resource path
        /// @warning Don't call this function in android and iOS, it has not effect.
        /// In android, if you want to read file other than apk, you shoud use invoke getFileData(), and pass the 
        /// absolute path.
        /// </summary>
        /// <param name="?"></param>
        public static void SetResourcePath(string pszResourcePath)
        {
            throw new NotSupportedException ("Not supported in XNA");
        }

        /// <summary>
        /// @brief   Generate a CCDictionary pointer by file
        /// @param   pFileName  The file name of *.plist file
        /// @return  The CCDictionary pointer generated from the file
        /// </summary>
        /// <typeparam name="?"></typeparam>
        /// <typeparam name="?"></typeparam>
        /// <param name="?"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DictionaryWithContentsOfFile(string pFileName)
        {
            CCDictMaker tMaker = new CCDictMaker();
            return tMaker.DictionaryWithContentsOfFile(pFileName);
        }

        /// <summary>
        /// @brief   Get the writeable path
        /// @return  The path that can write/read file
        /// </summary>
        /// <returns></returns>
        public static string GetWriteablePath()
        {
            throw new NotSupportedException("Use IsolatedStorage in XNA");
        }


        ///////////////////////////////////////////////////
        // interfaces on wophone
        ///////////////////////////////////////////////////
        /// <summary>
        /// @brief  Set the resource zip file name
        /// @param pszZipFileName The relative path of the .zip file
        /// </summary>
        /// <param name="pszZipFileName"></param>
        public static void SetResource(string pszZipFileName)
        {
            throw new NotImplementedException("win32 only definition does not realize !");
        }

        ///////////////////////////////////////////////////
        // interfaces on ios
        ///////////////////////////////////////////////////
        public static int CCLoadFileIntoMemory(string filename, out char[] file)
        {
            throw new NotImplementedException("win32 only definition does not realize !");
        }
    }
}
