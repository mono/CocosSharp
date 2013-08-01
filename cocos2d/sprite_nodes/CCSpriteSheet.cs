using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace Cocos2D
{
    public class CCSpriteSheet
    {
        private readonly Dictionary<string, CCSpriteFrame> _spriteFrames = new Dictionary<string, CCSpriteFrame>();
        private readonly Dictionary<string, string> _spriteFramesAliases = new Dictionary<string, string>();

        public CCSpriteSheet(string fileName)
        {
            InitWithFile(fileName);
        }

        public CCSpriteSheet(string fileName, string textureFileName)
        {
            InitWithFile(fileName, textureFileName);
        }

        public CCSpriteSheet(string fileName, CCTexture2D texture)
        {
            InitWithFile(fileName, texture);
        }

        public CCSpriteSheet(Stream stream, CCTexture2D texture)
        {
            InitWitchStream(stream, texture);
        }

        public CCSpriteSheet(PlistDictionary dictionary, CCTexture2D texture)
        {
            InitWithDictionary(dictionary, texture);
        }

        public void InitWithFile(string fileName)
        {
            string path = CCFileUtils.FullPathFromRelativePath(fileName);

            PlistDocument document = null;
            try
            {
                document = CCApplication.SharedApplication.Content.Load<PlistDocument>(path);
            }
            catch (System.Exception)
            {
                string xml = CCContent.LoadContentFile(path);
                if (xml != null)
                {
                    document = new PlistDocument(xml);
                }
            }

            if (document == null)
            {
                throw (new Microsoft.Xna.Framework.Content.ContentLoadException("Failed to load the particle definition file from " + path));
            }

            var dict = document.Root.AsDictionary;
            var texturePath = "";
            var metadataDict = dict.ContainsKey("metadata") ? dict["metadata"].AsDictionary : null;

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
                texturePath = CCFileUtils.FullPathFromRelativeFile(texturePath, path);
            }
            else
            {
                // build texture path by replacing file extension
                texturePath = path;

                // remove .xxx
                texturePath = CCFileUtils.RemoveExtension(texturePath);

                CCLog.Log("cocos2d: CCSpriteFrameCache: Trying to use file {0} as texture", texturePath);
            }

            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(texturePath);

            if (pTexture != null)
            {
                InitWithDictionary(dict, pTexture);
            }
            else
            {
                CCLog.Log("CCSpriteSheet: Couldn't load texture");
            }
        }

        public void InitWithFile(string fileName, string textureFileName)
        {
            Debug.Assert(textureFileName != null);
            
            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(textureFileName);

            if (texture != null)
            {
                InitWithFile(fileName, texture);
            }
            else
            {
                CCLog.Log("CCSpriteSheet: couldn't load texture file. File not found {0}", textureFileName);
            }
        }

        public void InitWithFile(string fileName, CCTexture2D texture)
        {
            PlistDocument document = null;
            try
            {
                document = CCApplication.SharedApplication.Content.Load<PlistDocument>(fileName);
            }
            catch (System.Exception)
            {
                string xml = CCContent.LoadContentFile(fileName);
                if (xml != null)
                {
                    document = new PlistDocument(xml);
                }
            }
            if (document == null)
            {
                throw (new Microsoft.Xna.Framework.Content.ContentLoadException("Failed to load the particle definition file from " + fileName));
            }

            PlistDictionary dict = document.Root.AsDictionary;

            InitWithDictionary(dict, texture);
        }

        public void InitWitchStream(Stream stream, CCTexture2D texture)
        {
            var document = new PlistDocument();
            try
            {
                document.LoadFromXmlFile(stream);
            }
            catch (Exception)
            {
                throw (new Microsoft.Xna.Framework.Content.ContentLoadException("Failed to load the particle definition file from stream"));
            }

            PlistDictionary dict = document.Root.AsDictionary;

            InitWithDictionary(dict, texture);
        }

        public void InitWithDictionary(PlistDictionary dict, CCTexture2D texture)
        {
            _spriteFrames.Clear();
            _spriteFramesAliases.Clear();

            PlistDictionary metadataDict = null;
            
            if (dict.ContainsKey("metadata"))
            {
                metadataDict = dict["metadata"].AsDictionary;
            }

            PlistDictionary framesDict = null;
            if (dict.ContainsKey("frames"))
            {
                framesDict = dict["frames"].AsDictionary;
            }

            // get the format
            int format = 0;
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
                    float x = 0f, y = 0f, w = 0f, h = 0f;
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
                        texture,
                        new CCRect(x, y, w, h),
                        false,
                        new CCPoint(ox, oy),
                        new CCSize(ow, oh)
                        );
                }
                else if (format == 1 || format == 2)
                {
                    var frame = CCRect.Parse(frameDict["frame"].AsString);
                    bool rotated = false;

                    // rotation
                    if (format == 2)
                    {
                        if (frameDict.ContainsKey("rotated"))
                        {
                            rotated = frameDict["rotated"].AsBool;
                        }
                    }

                    var offset = CCPoint.Parse(frameDict["offset"].AsString);
                    var sourceSize = CCSize.Parse(frameDict["sourceSize"].AsString);

                    // create frame
                    spriteFrame = new CCSpriteFrame(texture, frame, rotated, offset, sourceSize);
                }
                else if (format == 3)
                {
                    var spriteSize = CCSize.Parse(frameDict["spriteSize"].AsString);
                    var spriteOffset = CCPoint.Parse(frameDict["spriteOffset"].AsString);
                    var spriteSourceSize = CCSize.Parse(frameDict["spriteSourceSize"].AsString);
                    var textureRect = CCRect.Parse(frameDict["textureRect"].AsString);

                    bool textureRotated = false;
                    
                    if (frameDict.ContainsKey("textureRotated"))
                    {
                        textureRotated = frameDict["textureRotated"].AsBool;
                    }

                    // get aliases
                    var aliases = frameDict["aliases"].AsArray;

                    for (int i = 0; i < aliases.Count; i++ )
                    {
                        string oneAlias = aliases[i].AsString;

                        if (_spriteFramesAliases.ContainsKey(oneAlias))
                        {
                            if (_spriteFramesAliases[oneAlias] != null)
                            {
                                CCLog.Log("cocos2d: WARNING: an alias with name {0} already exists", oneAlias);
                            }
                        }

                        if (!_spriteFramesAliases.ContainsKey(oneAlias))
                        {
                            _spriteFramesAliases.Add(oneAlias, pair.Key);
                        }
                    }

                    // create frame
                    spriteFrame = new CCSpriteFrame(
                        texture,
                        new CCRect(textureRect.Origin.X, textureRect.Origin.Y, spriteSize.Width, spriteSize.Height),
                        textureRotated,
                        spriteOffset,
                        spriteSourceSize
                        );
                }

                _spriteFrames[pair.Key] = spriteFrame;
            }
        }

        public CCSpriteFrame SpriteFrameByName(string name)
        {
            CCSpriteFrame frame;

            if (!_spriteFrames.TryGetValue(name, out frame))
            {
                string key;
                
                if (_spriteFramesAliases.TryGetValue(name, out key))
                {
                    if (!_spriteFrames.TryGetValue(key, out frame))
                    {
                        CCLog.Log("cocos2d: CCSpriteFrameCahce: Frame '{0}' not found", name);
                    }
                }
            }

            if (frame != null)
            {
                CCLog.Log("cocos2d: {0} frame {1}", name, frame.Rect.ToString());
            }
            
            return frame;
        }
    }
}