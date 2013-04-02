using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace cocos2d
{
	public class CCRectConverter : TypeConverter
	{
		// Overrides the CanConvertFrom method of TypeConverter.
		// The ITypeDescriptorContext interface provides the context for the
		// conversion. Typically, this interface is used at design time to 
		// provide information about the design-time container.
		public override bool CanConvertFrom(ITypeDescriptorContext context, 
		                                   Type sourceType) {
		
			if (sourceType == typeof(string)) {
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}
		// Overrides the ConvertFrom method of TypeConverter.
		public override object ConvertFrom(ITypeDescriptorContext context, 
		                                   CultureInfo culture, object value) {
			if (value is string) {
				return CCRectFromString(value as String);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Overrides the ConvertTo method of TypeConverter.
		public override object ConvertTo(ITypeDescriptorContext context, 
		                                 CultureInfo culture, object value, Type destinationType) {  
			if (destinationType == typeof(string)) {
				return "{" + ((CCRect)value).Origin.X + "," + ((CCRect)value).Origin.Y + "," + 
					((CCRect)value).Size.Width + "," + ((CCRect)value).Size.Height + "}";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public static CCRect CCRectFromString(string pszContent)
		{
			CCRect result = CCRect.Zero;
			
			do
			{
				if (pszContent == null)
				{
					break;
				}
				
				string content = pszContent;
				
				// find the first '{' and the third '}'
				int nPosLeft = content.IndexOf('{');
				int nPosRight = content.IndexOf('}');
				for (int i = 1; i < 3; ++i)
				{
					if (nPosRight == -1)
					{
						break;
					}
					nPosRight = content.IndexOf('}', nPosRight + 1);
				}
				if (nPosLeft == -1 || nPosRight == -1)
				{
					break;
				}
				content = content.Substring(nPosLeft + 1, nPosRight - nPosLeft - 1);
				int nPointEnd = content.IndexOf('}');
				if (nPointEnd == -1)
				{
					break;
				}
				nPointEnd = content.IndexOf(',', nPointEnd);
				if (nPointEnd == -1)
				{
					break;
				}
				
				// get the point string and size string
				string pointStr = content.Substring(0, nPointEnd);
				string sizeStr = content.Substring(nPointEnd + 1);
				//, content.Length - nPointEnd
				// split the string with ','
				List<string> pointInfo = new List<string>();
				
				if (!CCUtils.SplitWithForm(pointStr, pointInfo))
				{
					break;
				}
				List<string> sizeInfo = new List<string>();
				if (!CCUtils.SplitWithForm(sizeStr, sizeInfo))
				{
					break;
				}
				
				float x = CCUtils.CCParseFloat(pointInfo[0]);
				float y = CCUtils.CCParseFloat(pointInfo[1]);
				float width = CCUtils.CCParseFloat(sizeInfo[0]);
				float height = CCUtils.CCParseFloat(sizeInfo[1]);
				
				result = new CCRect(x, y, width, height);
			} while (false);
			
			return result;
		}
	}
}

