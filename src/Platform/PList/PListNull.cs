using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class PlistNull : PlistObject<int?>
    {
		#region Properties

		public override byte[] AsBinary
		{
			get { return null; }
		}

		public override int AsInt
		{
			get { return 0; }
		}

		public override float AsFloat
		{
			get { return 0f; }
		}

		public override string AsString
		{
			get { return null; }
		}

		public override DateTime AsDate
		{
			get { throw new NotImplementedException(); }
		}

		public override bool AsBool
		{
			get {return false; }
		}

		public override PlistArray AsArray
		{
			get { return null; }
		}

		public override PlistDictionary AsDictionary
		{
			get { return null; }
		}

		#endregion Properties


		#region Constructors

		public PlistNull() : base(null)
        {
        }

		#endregion Constructors


        public override void Write(System.Xml.XmlWriter writer)
        {
        }
    }
}
