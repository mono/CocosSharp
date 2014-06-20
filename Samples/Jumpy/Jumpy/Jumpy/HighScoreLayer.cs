// HighScoreLayer.cs
//
// Author(s)
//	Stephane Delcroix <stephane@delcroix.org>
//
// Copyright (C) 2012 s. Delcroix
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//		
// 		The above copyright notice and this permission notice shall be included in all copies or 
//		substantial portions of the Software.
//		
//		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
//		BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//		NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//		DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
//		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace Jumpy
{
	public class HighScoreLayer : MainLayer
	{ 

		int currentScore;

		public static CCScene Scene (int score) {
			var scene = new CCScene (); 
			var layer = new HighScoreLayer (score);
			scene.AddChild(layer);
			return scene;
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			Schedule(Step);
		}

		public HighScoreLayer (int score): base()
		{
			currentScore = score;


			var batchnode = GetChildByTag ((int)Tags.SpriteManager) as CCSpriteBatchNode;
			var title = new CCSprite(batchnode.Texture,new CCRect(608,192,225,57));
            title.Position=new CCPoint(160,240);
			batchnode.AddChild (title);



			var button1 = new CCMenuItemImage("Images/playAgainButton", "Images/playAgainButton",
                new Action<object>(delegate(object o) {
			 	 Director.ReplaceScene(new CCTransitionFade(.5f, GameLayer.Scene, new CCColor3B(255,255,255)));
                }));
			var button2 = new CCMenuItemImage("Images/changePlayerButton", "Images/changePlayerButton", new Action<object>(delegate (object sender) {
                // do nothing
			}));
			var menu = new CCMenu(button1,button2);
            menu.Position=new CCPoint(160,58);
			menu.AlignItemsVertically(9);

			AddChild (menu);
			

		}
	}
}

