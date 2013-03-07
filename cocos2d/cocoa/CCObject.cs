/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org


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

using System.Diagnostics;
using System.IO;
using System;

namespace cocos2d
{
    public interface CCCopying
    {
        CCObject CopyWithZone(CCZone zone);
    }

    /// <summary>
    /// Add functions if needed.
    /// </summary>
    public class CCObject : CCCopying
    {
        public virtual CCObject Copy()
        {
            return CopyWithZone(null);
        }

        public virtual CCObject CopyWithZone(CCZone zone)
        {
            Debug.Assert(false);
            return null;
        }
        #region Raw Serializers
        protected void SerializeData(CCPoint pt, StreamWriter sw)
        {
            sw.WriteLine("{0} {1}", pt.x, pt.y);
        }
        protected void SerializeData(CCSize pt, StreamWriter sw)
        {
            sw.WriteLine("{0} {1}", pt.Width, pt.Height);
        }
        protected void SerializeData(float f, StreamWriter sw)
        {
            sw.WriteLine(f.ToString());
        }
        protected void SerializeData(int f, StreamWriter sw)
        {
            sw.WriteLine(f.ToString());
        }
        protected void SerializeData(bool f, StreamWriter sw)
        {
            sw.WriteLine(f.ToString());
        }
        protected void SerializeData(CCRect r, StreamWriter sw)
        {
            SerializeData(r.origin, sw);
            SerializeData(r.size, sw);
        }
        protected bool DeSerializeBool(StreamReader sr)
        {
            string s = sr.ReadLine();
            if (s == null)
            {
                CCLog.Log("DeSerializeBool: null");
                return (false);
            }
            return (bool.Parse(s));
        }
        protected float DeSerializeFloat(StreamReader sr)
        {
            string s = sr.ReadLine();
            if (s == null)
            {
                CCLog.Log("DeSerializeFloat: null");
                return (0f);
        }
            return (CCUtils.CCParseFloat(s));
        }
        protected int DeSerializeInt(StreamReader sr)
        {
            string s = sr.ReadLine();
            if (s == null)
            {
                CCLog.Log("DeSerializeInt: null");
                return (0);
            }
            return (CCUtils.CCParseInt(s));
        }
        protected CCRect DeSerializeRect(StreamReader sr)
        {
            CCPoint pt = DeSerializePoint(sr);
            CCSize sz = DeSerializeSize(sr);
            return (new CCRect(pt.x, pt.y, sz.Width, sz.Height));
        }
        protected CCSize DeSerializeSize(StreamReader sr)
        {
            string x = sr.ReadLine();
            if(x == null ){
                CCLog.Log("DeSerializeSize: null");
                return(CCSize.Zero);
            }
            CCSize pt = new CCSize();
            string[] s = x.Split(' ');
            pt.Width = CCUtils.CCParseFloat(s[0]);
            pt.Height = CCUtils.CCParseFloat(s[1]);
            return (pt);
        }
        protected CCPoint DeSerializePoint(StreamReader sr)
        {
            string x = sr.ReadLine();
            if (x == null)
            {
                CCLog.Log("DeSerializePoint: null");
                return (CCPoint.Zero);
            }
            CCPoint pt = new CCPoint();
            string[] s = x.Split(' ');
            pt.x = CCUtils.CCParseFloat(s[0]);
            pt.y = CCUtils.CCParseFloat(s[1]);
            return (pt);
        }
        #endregion
    }
}
