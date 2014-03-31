using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace CocosSharp
{
#if !NETFX_CORE
	public class CCPointConverter : TypeConverter
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
				return CCPointFromString(value as String);
			}
			return base.ConvertFrom(context, culture, value);
		}
		
		// Overrides the ConvertTo method of TypeConverter.
		public override object ConvertTo(ITypeDescriptorContext context, 
		                                 CultureInfo culture, object value, Type destinationType) {  
			if (destinationType == typeof(string)) {
				return "{" + ((CCPoint)value).X + "," + ((CCPoint)value).Y + "}";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public static CCPoint CCPointFromString(string pszContent)
		{
			CCPoint ret = CCPoint.Zero;
			
			do
			{
				List<string> strs = new List<string>();
				if (!CCUtils.SplitWithForm(pszContent, strs)) break;
				
				float x = CCUtils.CCParseFloat(strs[0]);
				float y = CCUtils.CCParseFloat(strs[1]);
				
				ret.X = x;
				ret.Y = y;

			} while (false);
			
			return ret;
		}

	}
#else
    public class CCPointConverter
    {
        public static CCPoint CCPointFromString(string pszContent)
        {
            CCPoint ret = CCPoint.Zero;

            do
            {
                List<string> strs = new List<string>();
                if (!CCUtils.SplitWithForm(pszContent, strs)) break;

                float x = CCUtils.CCParseFloat(strs[0]);
                float y = CCUtils.CCParseFloat(strs[1]);

                ret.X = x;
                ret.Y = y;

            } while (false);

            return ret;
        }

    }
#endif
}

