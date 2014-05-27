using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCMenuLoader : CCLayerLoader
    {
        public override CCNode CreateCCNode()
        {
            return new CCMenu();
        }
    }
}