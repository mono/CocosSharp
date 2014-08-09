using System.Collections.Generic;
using System.Text;
using System;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace ChipmunkSharp
{
	public static class cpExtension
	{

		public static cpVect ToCpVect(this CCPoint vec)
		{


			return new cpVect(vec.X, vec.Y);
		}

		public static CCPoint ToCCPoint(this cpVect vec)
		{
			return new CCPoint((float)vec.x, (float)vec.y);
		}



		public static CCColor4F ToCCColor4F(this cpColor color)
		{
			return new CCColor4F(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
		}

		public static CCColor4B ToCCColor4B(this cpColor color)
		{
			return new CCColor4B(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
		}

		//public static Vector2 ToVector(this CCPoint sender)
		//{
		//	return new Vector2(sender.X, sender.Y);
		//}
		//public static Vector2 ToVector(this cpVect sender)
		//{
		//	return new Vector2((float)sender.x, (float)sender.y);
		//}

		//public static CCPoint ToCCPoint(this Vector2 sender)
		//{
		//	return new CCPoint(sender.X, sender.Y);
		//}

		public static CCVector2 ToCCVector2(this cpVect vec)
		{
			return new CCVector2((float)vec.x, (float)vec.y);
		}

		public static Vector2 ToVector(this CCPoint sender)
		{
			return new Vector2(sender.X, sender.Y);
		}

		public static CCPoint ToCCPoint(this Vector2 sender)
		{
			return new CCPoint(sender.X, sender.Y);
		}

	}


}