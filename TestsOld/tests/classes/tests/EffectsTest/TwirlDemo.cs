using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TwirlDemo : CCTwirl
    {

		public TwirlDemo (float t, CCPoint position) : base (t, new CCGridSize(12, 8), position, 1, 2.5f)  
        {
			
        }
    }
}
