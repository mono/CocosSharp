using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCAnimationCache 
    {
        private static CCAnimationCache s_pSharedAnimationCache;
        private Dictionary<string, CCAnimation> m_pAnimations;

        public static CCAnimationCache SharedAnimationCache
        {
            get
            {
                if (s_pSharedAnimationCache == null)
                {
                    s_pSharedAnimationCache = new CCAnimationCache();
                    s_pSharedAnimationCache.Init();
                }

                return s_pSharedAnimationCache;
            }
        }

        public static void PurgeSharedAnimationCache()
        {
            s_pSharedAnimationCache = null;
        }

        public void AddAnimation(CCAnimation animation, string name)
        {
            if (!m_pAnimations.ContainsKey(name))
            {
                m_pAnimations.Add(name, animation);
            }
        }

        public void RemoveAnimationByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            m_pAnimations.Remove(name);
        }

        public CCAnimation this[string index]
        {
            get
            {
                return (AnimationByName(index));
            }
            set
            {
                m_pAnimations[index] = value;
            }
        }

        public CCAnimation AnimationByName(string name)
        {
            CCAnimation retValue;
            m_pAnimations.TryGetValue(name, out retValue);
            return retValue;
        }

        public void AddAnimationsWithDictionary(PlistDictionary dictionary)
        {
            PlistDictionary animations = dictionary["animations"].AsDictionary;

            if (animations == null)
            {
                CCLog.Log("cocos2d: CCAnimationCache: No animations were found in provided dictionary.");
                return;
            }

            PlistDictionary properties = dictionary["properties"].AsDictionary;
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

        public void AddAnimationsWithFile(string plist)
        {
            Debug.Assert(!string.IsNullOrEmpty(plist), "Invalid texture file name");

            PlistDocument document = CCContentManager.SharedContentManager.Load<PlistDocument>(plist);

            PlistDictionary dict = document.Root.AsDictionary;

            Debug.Assert(dict != null, "CCAnimationCache: File could not be found");

            AddAnimationsWithDictionary(dict);
        }

        public bool Init()
        {
            m_pAnimations = new Dictionary<string, CCAnimation>();
            return true;
        }

        private void ParseVersion1(PlistDictionary animations)
        {
            CCSpriteFrameCache frameCache = CCSpriteFrameCache.SharedSpriteFrameCache;

            foreach (var pElement in animations)
            {
                PlistDictionary animationDict = pElement.Value.AsDictionary;

                PlistArray frameNames = animationDict["frames"].AsArray;
                float delay = animationDict["delay"].AsFloat;
                //CCAnimation* animation = NULL;

                if (frameNames == null)
                {
                    CCLog.Log(
                        "cocos2d: CCAnimationCache: Animation '{0}' found in dictionary without any frames - cannot add to animation cache.",
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

                    var animFrame = new CCAnimationFrame();
                    animFrame.InitWithSpriteFrame(spriteFrame, 1, null);
                    frames.Add(animFrame);
                }

                if (frames.Count == 0)
                {
                    CCLog.Log(
                        "cocos2d: CCAnimationCache: None of the frames for animation '{0}' were found in the CCSpriteFrameCache. Animation is not being added to the Animation Cache.",
                        pElement.Key);
                    continue;
                }
                else if (frames.Count != frameNames.Count)
                {
                    CCLog.Log(
                        "cocos2d: CCAnimationCache: An animation in your dictionary refers to a frame which is not in the CCSpriteFrameCache. Some or all of the frames for the animation '{0}' may be missing.",
                        pElement.Key);
                }

                CCAnimation animation = new CCAnimation(frames, delay, 1);

                SharedAnimationCache.AddAnimation(animation, pElement.Key);
            }
        }

        private void ParseVersion2(PlistDictionary animations)
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
                        "cocos2d: CCAnimationCache: Animation '{0}' found in dictionary without any frames - cannot add to animation cache.",
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

                    var animFrame = new CCAnimationFrame();
                    animFrame.InitWithSpriteFrame(spriteFrame, delayUnits, userInfo);

                    array.Add(animFrame);
                }

                float delayPerUnit = animationDict["delayPerUnit"].AsFloat;
                var animation = new CCAnimation(array, delayPerUnit, (uint) loops);

                animation.RestoreOriginalFrame = restoreOriginalFrame;

                SharedAnimationCache.AddAnimation(animation, name);
            }
        }
    }
}