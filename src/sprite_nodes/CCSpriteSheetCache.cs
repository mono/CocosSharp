using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCSpriteSheetCache
    {
        static CCSpriteSheetCache instance;

        Dictionary<string, CCSpriteSheet> spriteSheets = new Dictionary<string, CCSpriteSheet>(); 


        #region Properties

        public static CCSpriteSheetCache Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CCSpriteSheetCache();
                }
                return instance;
            }
        }

        public CCSpriteSheet this[string name]
        {
            get
            {
                CCSpriteSheet result = null;
                if (!spriteSheets.TryGetValue(name, out result))
                {
                    CCLog.Log("SpriteSheet of key {0} is not exist.", name);
                }
                return result;
            }
        }

        #endregion Properties


        public static void DestroyInstance()
        {
            instance = null;
        }


        #region Adding sprite sheets

        public CCSpriteSheet AddSpriteSheet(string fileName)
        {
            CCSpriteSheet result;
            if (!spriteSheets.TryGetValue(fileName, out result))
            {
                result = new CCSpriteSheet(fileName);
                spriteSheets.Add(fileName, result);
            }
            return result;
        }

        public CCSpriteSheet AddSpriteSheet(string fileName, string textureFileName)
        {
            CCSpriteSheet result;
            if (!spriteSheets.TryGetValue(fileName, out result))
            {
                result = new CCSpriteSheet(fileName, textureFileName);
                spriteSheets.Add(fileName, result);
            }
            return result;
        }

        public CCSpriteSheet AddSpriteSheet(string fileName, CCTexture2D texture)
        {
            CCSpriteSheet result;
            if (!spriteSheets.TryGetValue(fileName, out result))
            {
                result = new CCSpriteSheet(fileName, texture);
                spriteSheets.Add(fileName, result);
            }
            return result;
        }

        #endregion Adding sprite sheets


        #region Removing sprite sheets

        public void RemoveAll()
        {
            spriteSheets.Clear();
        }

        public void RemoveUnused()
        {
            if (spriteSheets.Count > 0)
            {
                var tmp = new Dictionary<string, WeakReference>();

                foreach (var pair in spriteSheets)
                {
                    tmp.Add(pair.Key, new WeakReference(pair.Value));
                }

                spriteSheets.Clear();

                GC.Collect();

                foreach (var pair in tmp)
                {
                    if (pair.Value.IsAlive)
                    {
                        spriteSheets.Add(pair.Key, (CCSpriteSheet)pair.Value.Target);
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

            foreach (var pair in spriteSheets)
            {
                if (pair.Value == spriteSheet)
                {
                    key = pair.Key;
                    break;
                }
            }

            if (key != null)
            {
                spriteSheets.Remove(key);
            }
        }

        public void Remove(string name)
        {
            if (name == null)
            {
                return;
            }
            spriteSheets.Remove(name);
        }

        #endregion Removing sprite sheets
    }
}
