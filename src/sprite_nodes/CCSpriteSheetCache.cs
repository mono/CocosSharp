using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCSpriteSheetCache
    {
        private static CCSpriteSheetCache _instance;

        private Dictionary<string, CCSpriteSheet> _spriteSheets = new Dictionary<string, CCSpriteSheet>(); 

        public static CCSpriteSheetCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CCSpriteSheetCache();
                }
                return _instance;
            }
        }

        public static void DestroyInstance()
        {
            _instance = null;
        }


        public CCSpriteSheet AddSpriteSheet(string fileName)
        {
            CCSpriteSheet result;
            if (!_spriteSheets.TryGetValue(fileName, out result))
            {
                result = new CCSpriteSheet(fileName);
                _spriteSheets.Add(fileName, result);
            }
            return result;
        }

        public CCSpriteSheet AddSpriteSheet(string fileName, string textureFileName)
        {
            CCSpriteSheet result;
            if (!_spriteSheets.TryGetValue(fileName, out result))
            {
                result = new CCSpriteSheet(fileName, textureFileName);
                _spriteSheets.Add(fileName, result);
            }
            return result;
        }

        public CCSpriteSheet AddSpriteSheet(string fileName, CCTexture2D texture)
        {
            CCSpriteSheet result;
            if (!_spriteSheets.TryGetValue(fileName, out result))
            {
                result = new CCSpriteSheet(fileName, texture);
                _spriteSheets.Add(fileName, result);
            }
            return result;
        }

        public CCSpriteSheet AddSpriteSheet(Stream stream, CCTexture2D texture, string name)
        {
            CCSpriteSheet result;
            if (!_spriteSheets.TryGetValue(name, out result))
            {
                result = new CCSpriteSheet(name, texture);
                _spriteSheets.Add(name, result);
            }
            return result;
        }

        public CCSpriteSheet AddSpriteSheet(PlistDictionary dictionary, CCTexture2D texture, string name)
        {
            CCSpriteSheet result;
            if (!_spriteSheets.TryGetValue(name, out result))
            {
                result = new CCSpriteSheet(name, texture);
                _spriteSheets.Add(name, result);
            }
            return result;
        }

        public CCSpriteSheet SpriteSheetForKey(string name)
        {
            CCSpriteSheet result = null;
            if (!_spriteSheets.TryGetValue(name, out result))
            {
                CCLog.Log("SpriteSheet of key {0} is not exist.", name);
            }
            return result;
        }

        public void RemoveAll()
        {
            _spriteSheets.Clear();
        }

        public void RemoveUnused()
        {
            if (_spriteSheets.Count > 0)
            {
                var tmp = new Dictionary<string, WeakReference>();

                foreach (var pair in _spriteSheets)
                {
                    tmp.Add(pair.Key, new WeakReference(pair.Value));
                }

                _spriteSheets.Clear();

                GC.Collect();

                foreach (var pair in tmp)
                {
                    if (pair.Value.IsAlive)
                    {
                        _spriteSheets.Add(pair.Key, (CCSpriteSheet)pair.Value.Target);
                    }
                }
            }
        }

        public void Remove(CCSpriteSheet spriteSheet)
        {
            if (spriteSheet == null)
            {
                return;
            }

            string key = null;

            foreach (var pair in _spriteSheets)
            {
                if (pair.Value == spriteSheet)
                {
                    key = pair.Key;
                    break;
                }
            }

            if (key != null)
            {
                _spriteSheets.Remove(key);
            }
        }

        public void Remove(string name)
        {
            if (name == null)
            {
                return;
            }
            _spriteSheets.Remove(name);
        }

    }
}
