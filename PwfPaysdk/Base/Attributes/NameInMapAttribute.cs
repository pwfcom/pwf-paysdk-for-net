using System;
namespace Pwf.PaySDK.Base.Attributes
{
	public class NameInMapAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public NameInMapAttribute(string name)
		{
			Name = name;
		}
	}
}

