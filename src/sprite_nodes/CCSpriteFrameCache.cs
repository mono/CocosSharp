using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace CocosSharp
{
    public class CCSpriteFrameCache
    {
        Dictionary<string, CCSpriteFrame> spriteFrames;
        Dictionary<string, string> spriteFramesAliases;


        #region Properties

        // When false, an exception is thrown if an animation frame is overwritten.
        public bool AllowFrameOverwrite { get; set; }

        // Get the sprite frame for the given frame name, or as an alias for the sprite frame name.
        public CCSpriteFrame this[string name]
        {
            get
            {
                CCSpriteFrame frame;

                if (!spriteFrames.TryGetValue(name, out frame))
                {
                    // try alias dictionary
                    string key;
                    if (spriteFramesAliases.TryGetValue(name, out key))
                    {
                        if (!spriteFrames.TryGetValue(key, out frame))
                        {
                            CCLog.Log("CocosSharp: CCSpriteFrameCahce: Frame '{0}' not found", name);
                        }
                    }
                }

                return frame;
            }
        }

        #endregion Properties


        #region Constructors

        public CCSpriteFrameCache()
        {
            spriteFrames = new Dictionary<string, CCSpriteFrame>();
            spriteFramesAliases = new Dictionary<string, string>();
        }

        #endregion Constructors


        #region Adding frames

        internal void AddSpriteFrames(PlistDictionary pobDictionary, CCTexture2D pobTexture)
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
            if (format < 0 || format > 3)
            {
                throw (new NotSupportedException("PList format " + format + " is not supported."));
            }

            foreach (var pair in framesDict)
            {
                PlistDictionary frameDict = pair.Value.AsDictionary;
                CCSpriteFrame spriteFrame = null;

                if (format == 0)
                {
                    float x=0f, y=0f, w=0f, h=0f;
                    x = frameDict["x"].AsFloat;
                    y = frameDict["y"].AsFloat;
                    w = frameDict["width"].AsFloat;
                    h = frameDict["height"].AsFloat;
                    float ox = 0f, oy = 0f;
                    ox = frameDict["offsetX"].AsFloat;
                    oy = frameDict["offsetY"].AsFloat;
                    int ow = 0, oh = 0;
                    ow = frameDict["originalWidth"].AsInt;
                    oh = frameDict["originalHeight"].AsInt;
                    // check ow/oh
                    if (ow == 0 || oh == 0)
                    {
                        CCLog.Log(
                            "cocos2d: WARNING: originalWidth/Height not found on the CCSpriteFrame. AnchorPoint won't work as expected. Regenerate the .plist or check the 'format' metatag");
                    }
                    // abs ow/oh
                    ow = Math.Abs(ow);
                    oh = Math.Abs(oh);
                    // create frame
                    spriteFrame = new CCSpriteFrame(
                        new CCSize(ow, oh),
                        pobTexture,
                        new CCRect(x + ox, y + oy, w, h),
                        false
                    );
                }
                else if (format == 1 || format == 2)
                {
                    CCRect frame = CCRect.Parse(frameDict["frame"].AsString);
                    bool rotated = false;

                    // rotation
                    if (format == 2)
                    {
                        if (frameDict.ContainsKey("rotated"))
                        {
                            rotated = frameDict["rotated"].AsBool;
                        }
                    }

                    CCPoint offset = CCPoint.Parse(frameDict["offset"].AsString);
                    CCSize sourceSize = CCSize.Parse (frameDict["sourceSize"].AsString);

                    frame.Origin.X += offset.X;
                    frame.Origin.Y += offset.Y;

                    // create frame
                    spriteFrame = new CCSpriteFrame(
                        sourceSize, 
                        pobTexture,
                        frame,
                        rotated
                    );
                }
                else if (format == 3)
                {
                    // get values
                    CCSize spriteSize = CCSize.Parse (frameDict["spriteSize"].AsString);
                    CCPoint spriteOffset = CCPoint.Parse(frameDict["spriteOffset"].AsString);
                    CCSize spriteSourceSize = CCSize.Parse (frameDict["spriteSourceSize"].AsString);
                    CCRect textureRect = CCRect.Parse(frameDict["textureRect"].AsString);
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
                        if (spriteFramesAliases.ContainsKey(oneAlias))
                        {
                            if (spriteFramesAliases[oneAlias] != null)
                            {
                                CCLog.Log("CocosSharp: WARNING: an alias with name {0} already exists", oneAlias);
                            }
                        }
                        if (!spriteFramesAliases.ContainsKey(oneAlias))
                        {
                            spriteFramesAliases.Add(oneAlias, frameKey);
                        }
                    }

                    // create frame
                    spriteFrame = new CCSpriteFrame(
                        spriteSourceSize,
                        pobTexture,
                        new CCRect(textureRect.Origin.X + spriteOffset.X, textureRect.Origin.Y + spriteOffset.Y, spriteSize.Width, spriteSize.Height),
                        textureRotated
                    );
                }

                // add sprite frame
                string key = pair.Key;
                if (!AllowFrameOverwrite && spriteFrames.ContainsKey(key))
                {
                    CCLog.Log("Frame named " + key + " already exists in the animation cache. Not overwriting existing record.");
                }
                else if (AllowFrameOverwrite || !spriteFrames.ContainsKey(key))
                {
                    spriteFrames[key] = spriteFrame;
                }
            }
        }

        public void AddSpriteFrames(string plistFileName)
        {
            PlistDocument document = CCContentManager.SharedContentManager.Load<PlistDocument>(plistFileName);

            PlistDictionary dict = document.Root.AsDictionary;
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
                texturePath = CCFileUtils.FullPathFromRelativeFile(texturePath, plistFileName);
            }
            else
            {
                // build texture path by replacing file extension
                texturePath = plistFileName;

                // remove .xxx
                texturePath = CCFileUtils.RemoveExtension(texturePath);

                // append .png
                texturePath = texturePath + ".png";

                CCLog.Log("CocosSharp: CCSpriteFrameCache: Trying to use file {0} as texture", texturePath);
            }

            CCTexture2D pTexture = CCApplication.SharedApplication.TextureCache.AddImage(texturePath);

            if (pTexture != null)
            {
                AddSpriteFrames(dict, pTexture);
            }
            else
            {
                CCLog.Log("CocosSharp: CCSpriteFrameCache: Couldn't load texture");
            }
        }

        public void AddSpriteFrames(string plistFileName, string textureFileName)
        {
            Debug.Assert(textureFileName != null);

            CCTexture2D texture = CCApplication.SharedApplication.TextureCache.AddImage(textureFileName);

            if (texture != null)
            {
                AddSpriteFrames(plistFileName, texture);
            }
            else
            {
                CCLog.Log("CocosSharp: CCSpriteFrameCache: couldn't load texture file. File not found {0}", textureFileName);
            }
        }

        public void AddSpriteFrames(Stream plist, CCTexture2D pobTexture)
        {
            PlistDocument document = new PlistDocument();
            try
            {
                document.LoadFromXmlFile(plist);
            }
            catch (Exception)
            {
                throw (new Microsoft.Xna.Framework.Content.ContentLoadException("Failed to load the particle definition file from stream"));
            }

            PlistDictionary dict = document.Root.AsDictionary;

            AddSpriteFrames(dict, pobTexture);
        }

        public void AddSpriteFrames(string plistFileName, CCTexture2D pobTexture)
        {
            PlistDocument document = CCContentManager.SharedContentManager.Load<PlistDocument>(plistFileName);

            PlistDictionary dict = document.Root.AsDictionary;

            AddSpriteFrames(dict, pobTexture);
        }

        public void AddSpriteFrame(CCSpriteFrame frame, string frameName)
        {
            if (!AllowFrameOverwrite && spriteFrames.ContainsKey(frameName))
            {
                throw (new ArgumentException("The frame named " + frameName + " already exists."));
            }

            spriteFrames[frameName] = frame;
        }

        #endregion Adding frames


        #region Removing frames

        public void RemoveSpriteFrames()
        {
            spriteFrames.Clear();
            spriteFramesAliases.Clear();
        }

        public void RemoveUnusedSpriteFrames()
        {
            if (spriteFrames.Count > 0)
            {
                var tmp = new Dictionary<string, WeakReference>();

                foreach (var pair in spriteFrames)
                {
                    tmp.Add(pair.Key, new WeakReference(pair.Value));
                }

                spriteFrames.Clear();

                GC.Collect();

                foreach (var pair in tmp)
                {
                    if (pair.Value.IsAlive)
                    {
                        spriteFrames.Add(pair.Key, (CCSpriteFrame) pair.Value.Target);
                    }
                }
            }
        }

        public void RemoveSpriteFrame(string frameName)
        {
            // explicit nil handling
            if (string.IsNullOrEmpty(frameName))
            {
                return;
            }

            // Is this an alias ?
            string key = spriteFramesAliases[frameName];

            if (!string.IsNullOrEmpty(key))
            {
                spriteFrames.Remove(key);
                spriteFramesAliases.Remove(key);
            }
            else
            {
                spriteFrames.Remove(frameName);
            }
        }

        public void RemoveSpriteFrames(string plistFileName)
        {
            PlistDocument document = CCContentManager.SharedContentManager.Load<PlistDocument>(plistFileName);

            PlistDictionary dict = document.Root.AsDictionary;

            RemoveSpriteFrames(dict);
        }

        public void RemoveSpriteFrames(PlistDictionary dictionary)
        {
            PlistDictionary framesDict = dictionary["frames"].AsDictionary;
            var keysToRemove = new List<string>();

            foreach (var pair in framesDict)
            {
                if (spriteFrames.ContainsKey(pair.Key))
                {
                    keysToRemove.Remove(pair.Key);
                }
            }

            foreach (string key in keysToRemove)
            {
                spriteFrames.Remove(key);
            }
        }

        public void RemoveSpriteFrames(CCTexture2D texture)
        {
            var keysToRemove = new List<string>();

            foreach (string key in spriteFrames.Keys)
            {
                CCSpriteFrame frame = spriteFrames[key];
                if (frame != null && (frame.Texture.Name == texture.Name))
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (string key in keysToRemove)
            {
                spriteFrames.Remove(key);
            }
        }

        #endregion Removing frames
    }
}