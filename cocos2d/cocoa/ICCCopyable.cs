using System;

namespace CocosSharp
{
    public interface ICCCopyable
	{
		Object Copy(ICCCopyable zone);
	}

    public interface ICCCopyable<T>
    {
        T DeepCopy();
    }
}

