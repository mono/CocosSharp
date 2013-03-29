using System;

namespace cocos2d
{
	public interface ICopyable
	{
		Object Copy(ICopyable zone);
	}
}

