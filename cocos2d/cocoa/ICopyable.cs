using System;

namespace Cocos2D
{
	public interface ICopyable
	{
		Object Copy(ICopyable zone);
	}
}

