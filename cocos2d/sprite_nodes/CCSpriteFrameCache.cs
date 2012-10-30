using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace cocos2d
{
    public class CCSpriteFrameCache
    {
        public static CCSpriteFrameCache pSharedSpriteFrameCache = null;
        protected Dictionary<string, CCSpriteFrame> m_pSpriteFrames;
        protected Dictionary<string, string> m_pSpriteFramesAliases;

        public bool Init()
        {
            m_pSpriteFrames = new Dictionary<string, CCSpriteFrame>();
            m_pSpriteFramesAliases = new Dictionary<string, string>();
            return true;
        }

        public void AddSpriteFramesWithDictionary(PlistDictionary pobDictionary, CCTexture2D pobTexture)
        {
            /*
            Supported Zwoptex Formats:

            ZWTCoordinatesFormatOptionXMLLegacy = 0, // Flash Version
            ZWTCoordinatesFormatOptionXML1_0 = 1, // Desktop Version 0.0 - 0.4b
            ZWTCoordinatesFormatOptionXML1_1 = 2, // Desktop Version 1.0.0 - 1.0.1
            ZWTCoordinatesFormatOptionXML1_2 = 3, // Desktop Version 1.0.2+
            */

            PlistDictionary metadataDict = null;
            if (pobDictionary.ContainsKey("metadata"))
            {
                metadataDict = pobDictionary["metadata"].AsDictionary;
            }

            PlistDictionary framesDict = null;
            if (pobDictionary.ContainsKey("frames"))
            {
                framesDict = pobDictionary["frames"].AsDictionary;
            }

            int format = 0;

            // get the format
            if (metadataDict != null)
            {
                format = metadataDict["format"].AsInt;
            }

            // check the format
            Debug.Assert(format >= 0 && format <= 3);

            foreach (var pair in framesDict)
            {
                PlistDictionary frameDict = pair.Value.AsDictionary;
                var spriteFrame = new CCSpriteFrame();

                if (format == 0)
                {
                    float x = frameDict["x"].AsFloat;
                    float y = frameDict["y"].AsFloat;
                    float w = frameDict["width"].AsFloat;
                    float h = frameDict["height"].AsFloat;
                    float ox = frameDict["offsetX"].AsFloat;
                    float oy = frameDict["offsetY"].AsFloat;
                    int ow = frameDict["originalWidth"].AsInt;
                    int oh = frameDict["originalHeight"].AsInt;
                    // check ow/oh
                    if (ow == 0 || oh == 0)
                    {
                        CCLog.Log(
                            "cocos2d: WARNING: originalWidth/Height not found on the CCSpriteFrame. AnchorPoint won't work as expected. Regenrate the .plist");
                    }
                    // abs ow/oh
                    ow = Math.Abs(ow);
                    oh = Math.Abs(oh);
                    // create frame
                    spriteFrame = new CCSpriteFrame();
                    spriteFrame.InitWithTexture(pobTexture,
                                                new CCRect(x, y, w, h),
                                                false,
                                                new CCPoint(ox, oy),
                                                new CCSize(ow, oh)
                        );
                }
                else if (format == 1 || format == 2)
                {
                    CCRect frame = CCNS.CCRectFromString(frameDict["frame"].AsString);
                    bool rotated = false;

                    // rotation
                    if (format == 2)
                    {
                        if (frameDict.ContainsKey("rotated"))
                        {
                            rotated = frameDict["rotated"].AsBool;
                        }
                    }

                    CCPoint offset = CCNS.CCPointFromString(frameDict["offset"].AsString);
                    CCSize sourceSize = CCNS.CCSizeFromString(frameDict["sourceSize"].AsString);

                    // create frame
                    spriteFrame = new CCSpriteFrame();
                    spriteFrame.InitWithTexture(pobTexture,
                                                frame,
                                                rotated,
                                                offset,
                                                sourceSize
                        );
                }
                else if (format == 3)
                {
                    // get values
                    CCSize spriteSize = CCNS.CCSizeFromString(frameDict["spriteSize"].AsString);
                    CCPoint spriteOffset = CCNS.CCPointFromString(frameDict["spriteOffset"].AsString);
                    CCSize spriteSourceSize = CCNS.CCSizeFromString(frameDict["spriteSourceSize"].AsString);
                    CCRect textureRect = CCNS.CCRectFromString(frameDict["textureRect"].AsString);
                    bool textureRotated = false;
                    if (frameDict.ContainsKey("textureRotated"))
                    {
                        textureRotated = frameDict["textureRotated"].AsBool;
                    }

                    // get aliases
                    PlistArray aliases = frameDict["aliases"].AsArray;
                    string frameKey = pair.Key;

                    foreach (PlistObjectBase item2 in aliases)
                    {
                        string oneAlias = item2.AsString;
                        if (m_pSpriteFramesAliases.Keys.Contains(oneAlias))
                        {
                            if (m_pSpriteFramesAliases[oneAlias] != null)
                            {
                                CCLog.Log("cocos2d: WARNING: an alias with name {0} already exists", oneAlias);
                            }
                        }
                        if (!m_pSpriteFramesAliases.Keys.Contains(oneAlias))
                        {
                            m_pSpriteFramesAliases.Add(oneAlias, frameKey);
                        }
                    }

                    // create frame
                    spriteFrame = new CCSpriteFrame();
                    spriteFrame.InitWithTexture(pobTexture,
                                                new CCRect(textureRect.origin.x, textureRect.origin.y, spriteSize.width, spriteSize.height),
                                                textureRotated,
                                                spriteOffset,
                                                spriteSourceSize);
                }

                // add sprite frame
                if (!m_pSpriteFrames.ContainsKey(pair.Key))
                {
                    m_pSpriteFrames.Add(pair.Key, spriteFrame);
                }
            }
        }

        public void AddSpriteFramesWithFile(string pszPlist)
        {
            string pszPath = CCFileUtils.fullPathFromRelativePath(pszPlist);
            //Dictionary<string, Object> dict = CCFileUtils.dictionaryWithContentsOfFile(pszPath);
            PlistDictionary dict = CCApplication.SharedApplication.Content.Load<PlistDocument>(pszPlist).Root.AsDictionary;

            string texturePath = "";
            PlistDictionary metadataDict = dict.ContainsKey("metadata") ? dict["metadata"].AsDictionary : null;

            if (metadataDict != null)
            {
                // try to read  texture file name from meta data
                if (metadataDict.ContainsKey("textureFileName"))
                {
                    texturePath = metadataDict["textureFileName"].AsString;
                }
            }

            if (!string.IsNullOrEmpty(texturePath))
            {
                // build texture path relative to plist file
                texturePath = CCFileUtils.fullPathFromRelativeFile(texturePath, pszPath);
            }
            else
            {
                // build texture path by replacing file extension
                texturePath = pszPath;

                // remove .xxx
                texturePath = CCFileUtils.removeExtention(texturePath);

                CCLog.Log("cocos2d: CCSpriteFrameCache: Trying to use file {0} as texture", texturePath);
            }

            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(texturePath);

            if (pTexture != null)
            {
                AddSpriteFramesWithDictionary(dict, pTexture);
            }
            else
            {
                CCLog.Log("cocos2d: CCSpriteFrameCache: Couldn't load texture");
            }
        }

        public void AddSpriteFramesWithFile(string plist, string textureFileName)
        {
            Debug.Assert(textureFileName != null);
            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(textureFileName);

            if (texture != null)
            {
                AddSpriteFramesWithFile(plist, texture);
            }
            else
            {
                CCLog.Log("cocos2d: CCSpriteFrameCache: couldn't load texture file. File not found {0}", textureFileName);
            }
        }

        public void AddSpriteFramesWithFile(string pszPlist, CCTexture2D pobTexture)
        {
            //string pszPath = CCFileUtils.fullPathFromRelativePath(pszPlist);
            //Dictionary<string, Object> dict = CCFileUtils.dictionaryWithContentsOfFile(pszPath);
            PlistDictionary dict = CCApplication.SharedApplication.Content.Load<PlistDocument>(pszPlist).Root.AsDictionary;

            AddSpriteFramesWithDictionary(dict, pobTexture);
        }

        public void AddSpriteFrame(CCSpriteFrame pobFrame, string pszFrameName)
        {
            m_pSpriteFrames.Add(pszFrameName, pobFrame);
        }

        public void RemoveSpriteFrames()
        {
            m_pSpriteFrames.Clear();
            m_pSpriteFramesAliases.Clear();
        }

        public void RemoveUnusedSpriteFrames()
        {
            if (m_pSpriteFrames.Count > 0)
            {
                var tmp = new Dictionary<string, WeakReference>();

                foreach (var pair in m_pSpriteFrames)
                {
                    tmp.Add(pair.Key, new WeakReference(pair.Value));
                }

                m_pSpriteFrames.Clear();

                GC.Collect();

                foreach (var pair in tmp)
                {
                    if (pair.Value.IsAlive)
                    {
                        m_pSpriteFrames.Add(pair.Key, (CCSpriteFrame) pair.Value.Target);
                    }
                }
            }
        }

        public void RemoveSpriteFrameByName(string pszName)
        {
            // explicit nil handling
            if (string.IsNullOrEmpty(pszName))
            {
                return;
            }

            // Is this an alias ?
            string key = m_pSpriteFramesAliases[pszName];

            if (!string.IsNullOrEmpty(key))
            {
                m_pSpriteFrames.Remove(key);
                m_pSpriteFramesAliases.Remove(key);
            }
            else
            {
                m_pSpriteFrames.Remove(pszName);
            }
        }

        public void RemoveSpriteFramesFromFile(string plist)
        {
            //string path = CCFileUtils.fullPathFromRelativePath(plist);
            //Dictionary<string, object> dict = CCFileUtils.dictionaryWithContentsOfFile(path);
            PlistDictionary dict = CCApplication.SharedApplication.Content.Load<PlistDocument>(plist).Root.AsDictionary;

            RemoveSpriteFramesFromDictionary(dict);
        }

        public void RemoveSpriteFramesFromDictionary(PlistDictionary dictionary)
        {
            PlistDictionary framesDict = dictionary["frames"].AsDictionary;
            var keysToRemove = new List<string>();

            foreach (var pair in framesDict)
            {
                if (m_pSpriteFrames.ContainsKey(pair.Key))
                {
                    keysToRemove.Remove(pair.Key);
                }
            }

            foreach (string key in keysToRemove)
            {
                m_pSpriteFrames.Remove(key);
            }
        }

        public void RemoveSpriteFramesFromTexture(CCTexture2D texture)
        {
            var keysToRemove = new List<string>();

            foreach (string key in m_pSpriteFrames.Keys)
            {
                CCSpriteFrame frame = m_pSpriteFrames[key];
                if (frame != null && (frame.Texture.Name == texture.Name))
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (string key in keysToRemove)
            {
                m_pSpriteFrames.Remove(key);
            }
        }

        public CCSpriteFrame SpriteFrameByName(string pszName)
        {
            CCSpriteFrame frame;

            if (!m_pSpriteFrames.TryGetValue(pszName, out frame))
            {
                // try alias dictionary
                string key;
                if (m_pSpriteFramesAliases.TryGetValue(pszName, out key))
                {
                    if (!m_pSpriteFrames.TryGetValue(key, out frame))
                    {
                        CCLog.Log("cocos2d: CCSpriteFrameCahce: Frame '{0}' not found", pszName);
                    }
                }
            }
            return frame;
        }

        public static CCSpriteFrameCache SharedSpriteFrameCache
        {
            get
            {
                if (pSharedSpriteFrameCache == null)
                {
                    pSharedSpriteFrameCache = new CCSpriteFrameCache();
                    pSharedSpriteFrameCache.Init();
                }

                return pSharedSpriteFrameCache;
            }
        }

        public static void PurgeSharedSpriteFrameCache()
        {
            pSharedSpriteFrameCache = null;
        }
    }
}