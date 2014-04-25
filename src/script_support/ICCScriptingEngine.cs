/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
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

using System.Collections.Generic;

namespace CocosSharp
{
    public class ICCScriptingEngine
    {
        // functions for excute touch event
        public virtual bool ExecuteTouchEvent(string funcName, CCTouch touch)
        {
            return false;
        }

        public virtual bool ExecuteTouchesEvent(string funcName, List<CCTouch> touches)
        {
            return false;
        }

        // functions for CCCallFuncX
        public virtual bool ExecuteCallFunc(string funcName)
        {
            return false;
        }

        public virtual bool ExecuteCallFuncN(string funcName, CCNode node)
        {
            return false;
        }

        public virtual bool ExecuteCallFuncNd(string funcName, CCNode node, object pData)
        {
            return false;
        }

        public virtual bool ExecuteCallFunc0(string funcName, object pObject)
        {
            return false;
        }

        // excute a script function without params
        public virtual int ExecuteFuction(string funcName)
        {
            return 0;
        }

        // excute a script file
        public virtual bool ExecuteScriptFile(string filename)
        {
            return false;
        }

        // excute script from string
        public virtual bool ExecuteString(string codes)
        {
            return false;
        }

        // execute a schedule function
        public virtual bool ExecuteSchedule(string funcName, float t)
        {
            return false;
        }

        // add a search path  
        public virtual bool AddSearchPath(string path)
        {
            return false;
        }
    }
}