using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCAnimationCache 
    {
        static CCAnimationCache sharedAnimationCache;
        Dictionary<string, CCAnimation> animations;


        #region Properties

        public static CCAnimationCache SharedAnimationCache
        {
            get
            {
                if (sharedAnimationCache == null)
                {
                    sharedAnimationCache = new CCAnimationCache();
                }

                return sharedAnimationCache;
            }
        }

        public CCAnimation this[string index]
        {
            get
            {
                CCAnimation retValue;
                animations.TryGetValue(index, out retValue);
                return retValue;
            }
            set
            {
                animations[index] = value;
            }
        }

        public static void PurgeSharedAnimationCache()
        {
            sharedAnimationCache = null;
        }

        #endregion Properties


        #region Constructors

        // Singleton, so ensure users only call SharedAnimationCache to get instance
        protected CCAnimationCache()
        {
            animations = new Dictionary<string, CCAnimation>();
        }

        #endregion Constructors


        public void AddAnimation(CCAnimation animation, string name)
        {
            if (!animations.ContainsKey(name))
            {
                animations.Add(name, animation);
            }
        }

        public void RemoveAnimation(string animationName)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                return;
            }

            animations.Remove(animationName);
        }

        public void AddAnimations(PlistDictionary animationDict)
        {
            PlistDictionary animations = animationDict["animations"].AsDictionary;

            if (animations == null)
            {
                CCLog.Log("CocosSharp: CCAnimationCache: No animations were found in provided dictionary.");
                return;
            }

            PlistDictionary properties = animationDict["properties"].AsDictionary;
            if (properties != null)
            {
                int version = properties["format"].AsInt;
                PlistArray spritesheets = properties["spritesheets"].AsArray;

                foreach (PlistObjectBase pObj in spritesheets)
                {
                    string name = pObj.AsString;
                    CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile(name);
                }

                switch (version)
                {
                    case 1:
                        ParseVersion1(animations);
                        break;

                    case 2:
                        ParseVersion2(animations);
                        break;

                    default:
                        Debug.Assert(false, "Invalid animation format");
                        break;
                }
            }
        }

        public void AddAnimations(string plistFilename)
        {
            Debug.Assert(!string.IsNullOrEmpty(plistFilename), "Invalid texture file name");

            PlistDocument document = CCContentManager.SharedContentManager.Load<PlistDocument>(plistFilename);

            PlistDictionary dict = document.Root.AsDictionary;

            Debug.Assert(dict != null, "CCAnimationCache: File could not be found");

            this.AddAnimations(dict);
        }

        #region Parsing plist animation dict

        void ParseVersion1(PlistDictionary animations)
        {
            CCSpriteFrameCache frameCache = CCSpriteFrameCache.SharedSpriteFrameCache;

            foreach (var pElement in animations)
            {
                PlistDictionary animationDict = pElement.Value.AsDictionary;

                PlistArray frameNames = animationDict["frames"].AsArray;
                float delay = animationDict["delay"].AsFloat;

                if (frameNames == null)
                {
                    CCLog.Log(
                        "CocosSharp: CCAnimationCache: Animation '{0}' found in dictionary without any frames - cannot add to animation cache.",
                        pElement.Key);
                    continue;
                }

                var frames = new List<CCAnimationFrame>(frameNames.Count);

                foreach (PlistObjectBase pObj in frameNames)
                {
                    string frameName = pObj.AsString;
                    CCSpriteFrame spriteFrame = frameCache.SpriteFrameByName(frameName);

                    if (spriteFrame == null)
                    {
                        CCLog.Log(
                            "cocos2d: CCAnimationCache: Animation '{0}' refers to frame '%s' which is not currently in the CCSpriteFrameCache. This frame will not be added to the animation.",
                            pElement.Key, frameName);
                        continue;
                    }

                    var animFrame = new CCAnimationFrame(spriteFrame, 1, null);
                    frames.Add(animFrame);
                }

                if (frames.Count == 0)
                {
                    CCLog.Log(
                        "CocosSharp: CCAnimationCache: None of the frames for animation '{0}' were found in the CCSpriteFrameCache. Animation is not being added to the Animation Cache.",
                        pElement.Key);
                    continue;
                }
                else if (frames.Count != frameNames.Count)
                {
                    CCLog.Log(
                        "CocosSharp: CCAnimationCache: An animation in your dictionary refers to a frame which is not in the CCSpriteFrameCache. Some or all of the frames for the animation '{0}' may be missing.",
                        pElement.Key);
                }

                CCAnimation animation = new CCAnimation(frames, delay, 1);

                SharedAnimationCache.AddAnimation(animation, pElement.Key);
            }
        }

        void ParseVersion2(PlistDictionary animations)
        {
            CCSpriteFrameCache frameCache = CCSpriteFrameCache.SharedSpriteFrameCache;

            foreach (var pElement in animations)
            {
                string name = pElement.Key;
                PlistDictionary animationDict = pElement.Value.AsDictionary;

                int loops = animationDict["loops"].AsInt;
                bool restoreOriginalFrame = animationDict["restoreOriginalFrame"].AsBool;

                PlistArray frameArray = animationDict["frames"].AsArray;

                if (frameArray == null)
                {
                    CCLog.Log(
                        "CocosSharp: CCAnimationCache: Animation '{0}' found in dictionary without any frames - cannot add to animation cache.",
                        name);
                    continue;
                }

                // Array of AnimationFrames
                var array = new List<CCAnimationFrame>(frameArray.Count);

                foreach (PlistObjectBase pObj in frameArray)
                {
                    PlistDictionary entry = pObj.AsDictionary;

                    string spriteFrameName = entry["spriteframe"].AsString;
                    CCSpriteFrame spriteFrame = frameCache.SpriteFrameByName(spriteFrameName);

                    if (spriteFrame == null)
                    {
                        CCLog.Log(
                            "cocos2d: CCAnimationCache: Animation '{0}' refers to frame '{1}' which is not currently in the CCSpriteFrameCache. This frame will not be added to the animation.",
                            name, spriteFrameName);

                        continue;
                    }

                    float delayUnits = entry["delayUnits"].AsFloat;
                    PlistDictionary userInfo = entry["notification"].AsDictionary;

                    var animFrame = new CCAnimationFrame(spriteFrame, delayUnits, userInfo);

                    array.Add(animFrame);
                }

                float delayPerUnit = animationDict["delayPerUnit"].AsFloat;
                var animation = new CCAnimation(array, delayPerUnit, (uint) loops);

                animation.RestoreOriginalFrame = restoreOriginalFrame;

                SharedAnimationCache.AddAnimation(animation, name);
            }
        }

        #endregion Parsing plist animation dict
    }
}