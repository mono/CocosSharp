using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCData : CCObject
    {
        public CCData() { }

        public byte[] Bytes() 
        {
            return m_pData;
        }

        public static CCData DataWithBytes(byte[] pBytes, int size) 
        {
            return null;
        }

        public static CCData DataWithContentsOfFile(string strPath)
        {
            CCFileData data = new CCFileData(strPath, "rb");
            ulong nSize = data.Size;
            byte[] pBuffer = data.Buffer;

            if (pBuffer == null)
            {
                return null;
            }

            CCData pRet = new CCData();
            pRet.m_pData = pBuffer;
            //memcpy(pRet->m_pData, pBuffer, nSize);

            return pRet;
        }

        private byte[] m_pData;
    }
}
