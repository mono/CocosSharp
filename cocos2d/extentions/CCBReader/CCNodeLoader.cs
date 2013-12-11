using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class BlockData
    {
		public Action<object> mSELMenuHandler;
        public object mTarget;
    }

    public class BlockCCControlData
    {
        public CCControlEvent mControlEvents;
		public Action<object, CCControlEvent> mSELCCControlHandler;
        public object mTarget;
    }


    public class CCNodeLoader 
    {
        protected const string PROPERTY_POSITION = "position";
        protected const string PROPERTY_CONTENTSIZE = "contentSize";
        protected const string PROPERTY_SKEW = "skew";
        protected const string PROPERTY_ANCHORPOINT = "anchorPoint";
        protected const string PROPERTY_SCALE = "scale";
        protected const string PROPERTY_ROTATION = "rotation";
        protected const string PROPERTY_ROTATIONX = "rotationX";
        protected const string PROPERTY_ROTATIONY = "rotationY";
        protected const string PROPERTY_TAG = "tag";
        protected const string PROPERTY_IGNOREANCHORPOINTFORPOSITION = "ignoreAnchorPointForPosition";
        protected const string PROPERTY_VISIBLE = "visible";

        protected Dictionary<string, CCBValue> _customProperties; 

        public CCNodeLoader()
        {
            _customProperties = new Dictionary<string, CCBValue>();
        }

        public virtual CCNode CreateCCNode()
        {
            return new CCNode ();
        }

        public virtual CCNode LoadCCNode(CCNode parent, CCBReader reader)
        {
            CCNode node = CreateCCNode();
            return node;
        }

        public virtual Dictionary<string, CCBValue> CustomProperties
        {
            get { return _customProperties; }
        }

        public virtual void ParseProperties(CCNode node, CCNode parent, CCBReader reader)
        {
            int numRegularProps = reader.ReadInt(false);
            int numExturaProps = reader.ReadInt(false);
            int propertyCount = numRegularProps + numExturaProps;

            for (int i = 0; i < propertyCount; i++)
            {
                bool isExtraProp = (i >= numRegularProps);
                int type = reader.ReadInt(false);
                string propertyName = reader.ReadCachedString();

                // Check if the property can be set for this platform
                bool setProp = false;

                var platform = (PlatformType) reader.ReadByte();
                if (platform == PlatformType.All)
                {
                    setProp = true;
                }
#if __CC_PLATFORM_IOS
        if(platform == kCCBPlatform.kCCBPlatformIOS) 
        {
            setProp = true;
        }
#elif __CC_PLATFORM_MAC
        if(platform == kCCBPlatform.kCCBPlatformMac) 
        {
            setProp = true;
        }
#endif

                // Forward properties for sub ccb files
                if (node is CCBFile)
                {
                    var ccbNode = (CCBFile) node;
                    if (ccbNode.FileNode != null && isExtraProp)
                    {
                        node = ccbNode.FileNode;

                        // Skip properties that doesn't have a value to override
                        var extraPropsNames = (List<string>) node.UserObject;
                        bool bFound = false;
                        foreach (var pObj in extraPropsNames)
                        {
                            if (pObj == propertyName)
                            {
                                bFound = true;
                                break;
                            }
                        }
                        setProp &= bFound;
                    }
                }
                else if (isExtraProp && node == reader.AnimationManager.RootNode)
                {
                    var extraPropsNames = (List<string>) node.UserObject;
                    if (extraPropsNames == null)
                    {
                        extraPropsNames = new List<string>();
                        node.UserObject = extraPropsNames;
                    }

                    extraPropsNames.Add(propertyName);
                }

                switch ((CCBPropertyType) type)
                {
                    case CCBPropertyType.Position:
                        {
                            CCPoint position = ParsePropTypePosition(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypePosition(node, parent, propertyName, position, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Point:
                        {
                            CCPoint point = ParsePropTypePoint(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypePoint(node, parent, propertyName, point, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.PointLock:
                        {
                            CCPoint pointLock = ParsePropTypePointLock(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypePointLock(node, parent, propertyName, pointLock, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Size:
                        {
                            CCSize size = ParsePropTypeSize(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeSize(node, parent, propertyName, size, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.ScaleLock:
                        {
                            float[] scaleLock = ParsePropTypeScaleLock(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypeScaleLock(node, parent, propertyName, scaleLock, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Float:
                        {
                            float f = ParsePropTypeFloat(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFloat(node, parent, propertyName, f, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.FloatXY:
                        {
                            var xy = ParsePropTypeFloatXY(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFloatXY(node, parent, propertyName, xy, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Degrees:
                        {
                            float degrees = ParsePropTypeDegrees(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypeDegrees(node, parent, propertyName, degrees, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.FloatScale:
                        {
                            float floatScale = ParsePropTypeFloatScale(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFloatScale(node, parent, propertyName, floatScale, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Integer:
                        {
                            int integer = ParsePropTypeInteger(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeInteger(node, parent, propertyName, integer, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.IntegerLabeled:
                        {
                            int integerLabeled = ParsePropTypeIntegerLabeled(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeIntegerLabeled(node, parent, propertyName, integerLabeled, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.FloatVar:
                        {
                            float[] floatVar = ParsePropTypeFloatVar(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFloatVar(node, parent, propertyName, floatVar, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Check:
                        {
                            bool check = ParsePropTypeCheck(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypeCheck(node, parent, propertyName, check, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.SpriteFrame:
                        {
                            CCSpriteFrame ccSpriteFrame = ParsePropTypeSpriteFrame(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypeSpriteFrame(node, parent, propertyName, ccSpriteFrame, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Animation:
                        {
                            CCAnimation ccAnimation = ParsePropTypeAnimation(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeAnimation(node, parent, propertyName, ccAnimation, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Texture:
                        {
                            CCTexture2D ccTexture2D = ParsePropTypeTexture(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeTexture(node, parent, propertyName, ccTexture2D, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Byte:
                        {
                            byte b = ParsePropTypeByte(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypeByte(node, parent, propertyName, b, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Color3:
                        {
                            CCColor3B color3B = ParsePropTypeColor3(node, parent, reader, propertyName);
                            if (setProp)
                            {
                                OnHandlePropTypeColor3(node, parent, propertyName, color3B, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Color4FVar:
                        {
                            CCColor4F[] color4FVar = ParsePropTypeColor4FVar(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeColor4FVar(node, parent, propertyName, color4FVar, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Flip:
                        {
                            bool[] flip = ParsePropTypeFlip(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFlip(node, parent, propertyName, flip, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Blendmode:
                        {
                            CCBlendFunc blendFunc = ParsePropTypeBlendFunc(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.FntFile:
                        {
                            string fntFile = ParsePropTypeFntFile(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFntFile(node, parent, propertyName, fntFile, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.FontTTF:
                        {
                            string fontTTF = ParsePropTypeFontTTF(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeFontTTF(node, parent, propertyName, fontTTF, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.String:
                        {
                            string s = ParsePropTypeString(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeString(node, parent, propertyName, s, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Text:
                        {
                            string text = ParsePropTypeText(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeText(node, parent, propertyName, text, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.Block:
                        {
                            BlockData blockData = ParsePropTypeBlock(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeBlock(node, parent, propertyName, blockData, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.BlockCCControl:
                        {
                            BlockCCControlData blockCCControlData = ParsePropTypeBlockCcControl(node, parent, reader);
                            if (setProp && blockCCControlData != null)
                            {
                                OnHandlePropTypeBlockCcControl(node, parent, propertyName, blockCCControlData, reader);
                            }
                            break;
                        }
                    case CCBPropertyType.CCBFile:
                        {
                            CCNode ccbFileNode = ParsePropTypeCcbFile(node, parent, reader);
                            if (setProp)
                            {
                                OnHandlePropTypeCCBFile(node, parent, propertyName, ccbFileNode, reader);
                            }
                            break;
                        }
                    default:
                        //ASSERT_FAIL_UNEXPECTED_PROPERTYTYPE(type);
                        break;
                }
            }
        }

        protected virtual CCPoint ParsePropTypePosition(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            float x = reader.ReadFloat();
            float y = reader.ReadFloat();

            var type = (CCBPositionType) reader.ReadInt(false);

            CCSize containerSize = reader.AnimationManager.GetContainerSize(parent);

            CCPoint pt = CCBHelper.GetAbsolutePosition(new CCPoint(x, y), type, containerSize, propertyName);
            node.Position = CCBHelper.GetAbsolutePosition(pt, type, containerSize, propertyName);

            if (reader.AnimatedProperties.Contains(propertyName))
            {
                var baseValue = new List<CCBValue>
                    {
                        new CCBValue(x),
                        new CCBValue(y),
                        new CCBValue((int) type)
                    };
                reader.AnimationManager.SetBaseValue(baseValue, node, propertyName);
            }

            return pt;
        }

        protected virtual CCPoint ParsePropTypePoint(CCNode node, CCNode parent, CCBReader reader)
        {
            float x = reader.ReadFloat();
            float y = reader.ReadFloat();

            return new CCPoint(x, y);
        }

        protected virtual CCPoint ParsePropTypePointLock(CCNode node, CCNode parent, CCBReader reader)
        {
            float x = reader.ReadFloat();
            float y = reader.ReadFloat();

            return new CCPoint(x, y);
        }

        protected virtual CCSize ParsePropTypeSize(CCNode node, CCNode parent, CCBReader reader)
        {
            float width = reader.ReadFloat();
            float height = reader.ReadFloat();

            int type = reader.ReadInt(false);

            CCSize containerSize = reader.AnimationManager.GetContainerSize(parent);

            switch ((SizeType) type)
            {
                case SizeType.Absolute:
                    {
                        /* Nothing. */
                        break;
                    }
                case SizeType.RelativeContainer:
                    {
                        width = containerSize.Width - width;
                        height = containerSize.Height - height;
                        break;
                    }
                case SizeType.Percent:
                    {
                        width = (int) (containerSize.Width * width / 100.0f);
                        height = (int) (containerSize.Height * height / 100.0f);
                        break;
                    }
                case SizeType.HorizontalPercent:
                    {
                        width = (int) (containerSize.Width * width / 100.0f);
                        break;
                    }
                case SizeType.VerticalPercent:
                    {
                        height = (int) (containerSize.Height * height / 100.0f);
                        break;
                    }
                case SizeType.MultiplyResolution:
                    {
                        float resolutionScale = CCBReader.ResolutionScale;

                        width *= resolutionScale;
                        height *= resolutionScale;
                        break;
                    }
                default:
                    {
                        CCLog.Log("Unknown CCB type.");
                    }
                    break;
            }

            return new CCSize(width, height);
        }

        protected virtual float[] ParsePropTypeScaleLock(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            float x = reader.ReadFloat();
            float y = reader.ReadFloat();

            var type = (CCBScaleType) reader.ReadInt(false);

            CCBHelper.SetRelativeScale(node, x, y, type, propertyName);

            if (reader.AnimatedProperties.Contains(propertyName))
            {
                var baseValue = new List<CCBValue>
                    {
                        new CCBValue(x),
                        new CCBValue(y),
                        new CCBValue((int) type)
                    };
                reader.AnimationManager.SetBaseValue(baseValue, node, propertyName);
            }

            if (type == CCBScaleType.MultiplyResolution)
            {
                x *= CCBReader.ResolutionScale;
                y *= CCBReader.ResolutionScale;
            }

            var scaleLock = new float[2];
            scaleLock[0] = x;
            scaleLock[1] = y;

            return scaleLock;
        }

        protected virtual float ParsePropTypeFloat(CCNode node, CCNode parent, CCBReader reader)
        {
            return reader.ReadFloat();
        }

        protected virtual float ParsePropTypeDegrees(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            float ret = reader.ReadFloat();
            if (reader.AnimatedProperties.Contains(propertyName))
            {
                CCBValue value = new CCBValue(ret);
                reader.AnimationManager.SetBaseValue(value, node, propertyName);
            }

            return ret;
        }

        protected virtual float ParsePropTypeFloatScale(CCNode node, CCNode parent, CCBReader reader)
        {
            float f = reader.ReadFloat();

            int type = reader.ReadInt(false);

            if ((CCBScaleType) type == CCBScaleType.MultiplyResolution)
            {
                f *= CCBReader.ResolutionScale;
            }

            return f;
        }

        protected virtual int ParsePropTypeInteger(CCNode node, CCNode parent, CCBReader reader)
        {
            return reader.ReadInt(true);
        }

        protected virtual int ParsePropTypeIntegerLabeled(CCNode node, CCNode parent, CCBReader reader)
        {
            return reader.ReadInt(true);
        }

        protected virtual float[] ParsePropTypeFloatVar(CCNode node, CCNode parent, CCBReader reader)
        {
            float f = reader.ReadFloat();
            float fVar = reader.ReadFloat();

            var arr = new float[2];
            arr[0] = f;
            arr[1] = fVar;

            return arr;
        }

        protected virtual bool ParsePropTypeCheck(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            bool ret = reader.ReadBool();

            if (reader.AnimatedProperties.Contains(propertyName))
            {
                CCBValue value = new CCBValue(ret);
                reader.AnimationManager.SetBaseValue(value, node, propertyName);
            }

            return ret;
        }

        protected virtual CCSpriteFrame ParsePropTypeSpriteFrame(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            string spriteSheet = reader.ReadCachedString();
            string spriteFile = reader.ReadCachedString();

            CCSpriteFrame spriteFrame = null;
            if (spriteFile.Length != 0)
            {
                if (spriteSheet.Length == 0)
                {
                    spriteFile = reader.CCBRootPath + spriteFile;
                    CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(CCFileUtils.RemoveExtension(spriteFile));
                    var bounds = new CCRect(0, 0, texture.ContentSize.Width, texture.ContentSize.Height);
                    spriteFrame = new CCSpriteFrame(texture, bounds);
                }
                else
                {
                    CCSpriteFrameCache frameCache = CCSpriteFrameCache.SharedSpriteFrameCache;
                    spriteSheet = reader.CCBRootPath + spriteSheet;

                    // Load the sprite sheet only if it is not loaded
                    if (!reader.LoadedSpriteSheet.Contains(spriteSheet))
                    {
                        frameCache.AddSpriteFramesWithFile(spriteSheet);
                        reader.LoadedSpriteSheet.Add(spriteSheet);
                    }

                    spriteFrame = frameCache.SpriteFrameByName(spriteFile);
                }

                if (reader.AnimatedProperties.Contains(propertyName))
                {
                    reader.AnimationManager.SetBaseValue(spriteFrame, node, propertyName);
                }
            }

            return spriteFrame;
        }

        protected virtual CCAnimation ParsePropTypeAnimation(CCNode node, CCNode parent, CCBReader reader)
        {
            string animationFile = reader.ReadCachedString();
            string animation = reader.ReadCachedString();

            CCAnimation ccAnimation = null;

            // Support for stripping relative file paths, since ios doesn't currently
            // know what to do with them, since its pulling from bundle.
            // Eventually this should be handled by a client side asset manager
            // interface which figured out what resources to load.
            // TODO Does this problem exist in C++?
            animation = CCBReader.LastPathComponent(animation);
            animationFile = CCBReader.LastPathComponent(animationFile);

            if (!String.IsNullOrEmpty(animation))
            {
                CCAnimationCache animationCache = CCAnimationCache.SharedAnimationCache;
                animationCache.AddAnimationsWithFile(animationFile);

                ccAnimation = animationCache.AnimationByName(animation);
            }
            return ccAnimation;
        }

        protected virtual CCTexture2D ParsePropTypeTexture(CCNode node, CCNode parent, CCBReader reader)
        {
            string spriteFile = reader.ReadCachedString();

            if (!String.IsNullOrEmpty(spriteFile))
            {
                return CCTextureCache.SharedTextureCache.AddImage(CCFileUtils.RemoveExtension(spriteFile));
            }
            return null;
        }

        protected virtual byte ParsePropTypeByte(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            byte ret = reader.ReadByte();

            if (reader.AnimatedProperties.Contains(propertyName))
            {
                reader.AnimationManager.SetBaseValue(new CCBValue(ret), node, propertyName);
            }

            return ret;
        }

        protected virtual CCColor3B ParsePropTypeColor3(CCNode node, CCNode parent, CCBReader reader, string propertyName)
        {
            byte red = reader.ReadByte();
            byte green = reader.ReadByte();
            byte blue = reader.ReadByte();

            var color = new CCColor3B(red, green, blue);
            if (reader.AnimatedProperties.Contains(propertyName))
            {
                CCColor3BWapper value = new CCColor3BWapper(color);
                reader.AnimationManager.SetBaseValue(value, node, propertyName);
            }
            return color;
        }

        protected virtual CCColor4F[] ParsePropTypeColor4FVar(CCNode node, CCNode parent, CCBReader reader)
        {
            float red = reader.ReadFloat();
            float green = reader.ReadFloat();
            float blue = reader.ReadFloat();
            float alpha = reader.ReadFloat();
            float redVar = reader.ReadFloat();
            float greenVar = reader.ReadFloat();
            float blueVar = reader.ReadFloat();
            float alphaVar = reader.ReadFloat();

            var colors = new CCColor4F[2];
            colors[0].R = red;
            colors[0].G = green;
            colors[0].B = blue;
            colors[0].A = alpha;

            colors[1].R = redVar;
            colors[1].G = greenVar;
            colors[1].B = blueVar;
            colors[1].A = alphaVar;

            return colors;
        }

        protected virtual bool[] ParsePropTypeFlip(CCNode node, CCNode parent, CCBReader reader)
        {
            bool flipX = reader.ReadBool();
            bool flipY = reader.ReadBool();

            var arr = new bool[2];
            arr[0] = flipX;
            arr[1] = flipY;

            return arr;
        }

        protected virtual CCBlendFunc ParsePropTypeBlendFunc(CCNode node, CCNode parent, CCBReader reader)
        {
            int source = reader.ReadInt(false);
            int destination = reader.ReadInt(false);

            CCBlendFunc blendFunc;
            blendFunc.Source = source;
            blendFunc.Destination = destination;

            return blendFunc;
        }

        protected virtual string ParsePropTypeFntFile(CCNode node, CCNode parent, CCBReader reader)
        {
            return reader.ReadCachedString();
        }

        protected virtual string ParsePropTypeString(CCNode node, CCNode parent, CCBReader reader)
        {
            return reader.ReadCachedString();
        }

        protected virtual string ParsePropTypeText(CCNode node, CCNode parent, CCBReader reader)
        {
            return reader.ReadCachedString();
        }

        protected virtual string ParsePropTypeFontTTF(CCNode node, CCNode parent, CCBReader reader)
        {
            string fontTTF = reader.ReadCachedString();

            // CCString * ttfEnding = ".ttf";

            // TODO Fix me if it is wrong
            /* If the fontTTF comes with the ".ttf" extension, prepend the absolute path. 
             * System fonts come without the ".ttf" extension and do not need the path prepended. */
            /*
            if(CCBReader.endsWith(CCBReader.toLowerCase(fontTTF), ttfEnding)){
                fontTTF = CCBReader.concat(reader.getCCBRootPath(), fontTTF);
            }
             */

            return fontTTF;
        }

        protected virtual BlockData ParsePropTypeBlock(CCNode node, CCNode parent, CCBReader reader)
        {
            string selectorName = reader.ReadCachedString();
            var selectorTarget = (CCBTargetType) reader.ReadInt(false);

            if (selectorTarget != CCBTargetType.None)
            {
                object target = null;
                if (!reader.IsJSControlled())
                {
                    if (selectorTarget == CCBTargetType.DocumentRoot)
                    {
                        target = reader.AnimationManager.RootNode;
                    }
                    else if (selectorTarget == CCBTargetType.Owner)
                    {
                        target = reader.Owner;

                        /* Scripting specific code because selector function is common for all callbacks.
                         * So if we had 1 target and 1 selector function, the context (callback function name)
                         * would get lost. Hence the need for a new target for each callback.
                         */
                        if (reader._hasScriptingOwner)
                        {
                            var proxy = (ICCBScriptOwnerProtocol) reader.Owner;
                            if (proxy != null)
                            {
                                target = proxy.CreateNew() as object;
                            }
                        }
                    }

                    if (target != null)
                    {
                        if (selectorName.Length > 0)
                        {
                            Action<object> selMenuHandler = null;

                            var targetAsCCBSelectorResolver = target as ICCBSelectorResolver;

                            if (targetAsCCBSelectorResolver != null)
                            {
                                selMenuHandler = targetAsCCBSelectorResolver.OnResolveCCBCCMenuItemSelector(target,
                                                                                                            selectorName);
                            }
                            if (selMenuHandler == null)
                            {
                                ICCBSelectorResolver ccbSelectorResolver = reader.SelectorResolver;
                                if (ccbSelectorResolver != null)
                                {
                                    selMenuHandler = ccbSelectorResolver.OnResolveCCBCCMenuItemSelector(target,
                                                                                                        selectorName);
                                }
                            }

                            if (selMenuHandler == null)
                            {
                                CCLog.Log("Skipping selector '{0}' since no CCBSelectorResolver is present.",
                                          selectorName);
                            }
                            else
                            {
                                var blockData = new BlockData();
                                blockData.mSELMenuHandler = selMenuHandler;

                                blockData.mTarget = target;

                                return blockData;
                            }
                        }
                        else
                        {
                            CCLog.Log("Unexpected empty selector.");
                        }
                    }
                    else
                    {
                        CCLog.Log("Unexpected NULL target for selector.");
                    }
                }
                else
                {
                    if (selectorTarget == CCBTargetType.DocumentRoot)
                    {
                        reader.AddDocumentCallbackNode(node);
                        reader.AddDocumentCallbackName(selectorName);

                    }
                    else
                    {
                        reader.AddOwnerCallbackNode(node);
                        reader.AddOwnerCallbackName(selectorName);
                    }
                }
            }

            return null;
        }

        protected virtual BlockCCControlData ParsePropTypeBlockCcControl(CCNode node, CCNode parent, CCBReader reader)
        {
            string selectorName = reader.ReadCachedString();
            var selectorTarget = (CCBTargetType) reader.ReadInt(false);
            var controlEvents = (CCControlEvent) reader.ReadInt(false);

            if (selectorTarget != CCBTargetType.None)
            {
                if (!reader.IsJSControlled())
                {
                    object target = null;
                    if (selectorTarget == CCBTargetType.DocumentRoot)
                    {
                        target = reader.AnimationManager.RootNode;
                    }
                    else if (selectorTarget == CCBTargetType.Owner)
                    {
                        target = reader.Owner;
                    }

                    if (target != null)
                    {
                        if (selectorName.Length > 0)
                        {
                            Action<object, CCControlEvent> selCCControlHandler = null;

                            var targetAsCCBSelectorResolver = target as ICCBSelectorResolver;
                            if (targetAsCCBSelectorResolver != null)
                            {
                                selCCControlHandler = targetAsCCBSelectorResolver.OnResolveCCBCCControlSelector(target,
                                                                                                                selectorName);
                            }
                            if (selCCControlHandler == null)
                            {
                                ICCBSelectorResolver ccbSelectorResolver = reader.SelectorResolver;
                                if (ccbSelectorResolver != null)
                                {
                                    selCCControlHandler = ccbSelectorResolver.OnResolveCCBCCControlSelector(target,
                                                                                                            selectorName);
                                }
                            }

                            if (selCCControlHandler == null)
                            {
                                CCLog.Log("Skipping selector '{0}' since no CCBSelectorResolver is present.",
                                          selectorName);
                            }
                            else
                            {
                                var blockCCControlData = new BlockCCControlData();
                                blockCCControlData.mSELCCControlHandler = selCCControlHandler;

                                blockCCControlData.mTarget = target;
                                blockCCControlData.mControlEvents = controlEvents;

                                return blockCCControlData;
                            }
                        }
                        else
                        {
                            CCLog.Log("Unexpected empty selector.");
                        }
                    }
                    else
                    {
                        CCLog.Log("Unexpected NULL target for selector.");
                    }
                }
                else
                {
                    if (selectorTarget == CCBTargetType.DocumentRoot)
                    {
                        reader.AddDocumentCallbackNode(node);
                        reader.AddDocumentCallbackName(selectorName);

                    }
                    else
                    {
                        reader.AddOwnerCallbackNode(node);
                        reader.AddOwnerCallbackName(selectorName);
                    }
                }
            }

            return null;
        }

        protected virtual CCNode ParsePropTypeCcbFile(CCNode node, CCNode parent, CCBReader pCCBReader)
        {
            string ccbFileName = pCCBReader.ReadCachedString();

            /* Change path extension to .ccbi. */
            string ccbFileWithoutPathExtension = CCBReader.DeletePathExtension(ccbFileName);
            ccbFileName = ccbFileWithoutPathExtension + ".ccbi";

            // Load sub file
            string path = CCFileUtils.FullPathFromRelativePath(ccbFileName);
            //long size = 0;
            byte[] pBytes = CCFileUtils.GetFileBytes(path);
            var reader = new CCBReader(pCCBReader);

            reader.AnimationManager.RootContainerSize = parent.ContentSize;

            reader._bytes = pBytes;
            reader._currentByte = 0;
            reader._currentBit = 0;
            reader._owner = pCCBReader._owner;

            reader.AnimationManager._owner = reader._owner;

            CCNode ccbFileNode = reader.ReadFileWithCleanUp(false, pCCBReader.AnimationManagers);

            if (ccbFileNode != null && reader.AnimationManager.AutoPlaySequenceId != -1)
            {
                // Auto play animations
                reader.AnimationManager.RunAnimationsForSequenceIdTweenDuration(reader.AnimationManager.AutoPlaySequenceId, 0);
            }

            if (reader.IsJSControlled() && pCCBReader.IsJSControlled() && null != reader._owner)
            {
                //set variables and callback to owner
                //set callback
                var ownerCallbackNames = reader.OwnerCallbackNames;
                var ownerCallbackNodes = reader.OwnerCallbackNodes;
                if (null != ownerCallbackNames && ownerCallbackNames.Count > 0 &&
                    null != ownerCallbackNodes && ownerCallbackNodes.Count > 0)
                {
                    Debug.Assert(ownerCallbackNames.Count == ownerCallbackNodes.Count);
                    int nCount = ownerCallbackNames.Count;
                    for (int i = 0; i < nCount; i++)
                    {
                        pCCBReader.AddOwnerCallbackName(ownerCallbackNames[i]);
                        pCCBReader.AddOwnerCallbackNode(ownerCallbackNodes[i]);
                    }
                }
                //set variables
                var ownerOutletNames = reader.OwnerOutletNames;
                var ownerOutletNodes = reader.OwnerOutletNodes;
                if (null != ownerOutletNames && ownerOutletNames.Count > 0 &&
                    null != ownerOutletNodes && ownerOutletNodes.Count > 0)
                {
                    Debug.Assert(ownerOutletNames.Count == ownerOutletNodes.Count);
                    int nCount = ownerOutletNames.Count;
                    for (int i = 0; i < nCount; i++)
                    {
                        pCCBReader.AddOwnerOutletName(ownerOutletNames[i]);
                        pCCBReader.AddOwnerOutletNode(ownerOutletNodes[i]);
                    }
                }
            }

            return ccbFileNode;
        }

        protected virtual float[] ParsePropTypeFloatXY(CCNode pNode, CCNode pParent, CCBReader ccbReader)
        {
            float x = ccbReader.ReadFloat();
            float y = ccbReader.ReadFloat();

            var floatXY = new float[2];
            floatXY[0] = x;
            floatXY[1] = y;

            return floatXY;
        }


        protected virtual void OnHandlePropTypePosition(CCNode node, CCNode parent, string propertyName, CCPoint pPosition, CCBReader reader)
        {
            if (propertyName == PROPERTY_POSITION)
            {
                node.Position = pPosition;
            }
            else
            {
                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
                Debug.Assert(false);
            }
        }

        protected virtual void OnHandlePropTypePoint(CCNode node, CCNode parent, string propertyName, CCPoint point, CCBReader reader)
        {
            if (propertyName == PROPERTY_ANCHORPOINT)
            {
                node.AnchorPoint = point;
            }
            else
            {
                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
                Debug.Assert(false);
            }
        }

        protected virtual void OnHandlePropTypePointLock(CCNode node, CCNode parent, string propertyName, CCPoint pPointLock, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeSize(CCNode node, CCNode parent, string propertyName, CCSize pSize, CCBReader reader)
        {
            if (propertyName == PROPERTY_CONTENTSIZE)
            {
                node.ContentSize = pSize;
            }
            else
            {
                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
                Debug.Assert(false);
            }
        }

        protected virtual void OnHandlePropTypeScaleLock(CCNode node, CCNode parent, string propertyName, float[] pScaleLock, CCBReader reader)
        {
            if (propertyName == PROPERTY_SCALE)
            {
                node.ScaleX = pScaleLock[0];
                node.ScaleY = pScaleLock[1];
            }
            else
            {
                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
                Debug.Assert(false);
            }
        }

        protected virtual void OnHandlePropTypeFloat(CCNode node, CCNode parent, string propertyName, float pFloat, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
             // It may be a custom property, add it to custom property dictionary.
            _customProperties.Add(propertyName, new CCBValue(pFloat));
        }

        protected virtual void OnHandlePropTypeDegrees(CCNode node, CCNode parent, string propertyName, float pDegrees, CCBReader reader)
        {
            if (propertyName == PROPERTY_ROTATION)
            {
                node.Rotation = pDegrees;
            }
            else if (propertyName == PROPERTY_ROTATIONX)
            {
                node.RotationX = pDegrees;
            }
            else if (propertyName == PROPERTY_ROTATIONY)
            {
                node.RotationY = pDegrees;
            }
            else
            {
                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
                Debug.Assert(false);
            }
        }

        protected virtual void OnHandlePropTypeFloatScale(CCNode node, CCNode parent, string propertyName, float floatScale, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeInteger(CCNode node, CCNode parent, string propertyName, int pInteger, CCBReader reader)
        {
            if (propertyName == PROPERTY_TAG)
            {
                node.Tag = pInteger;
            }
            else
            {
//                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
//                Debug.Assert(false);
                // It may be a custom property, add it to custom property dictionary.
                _customProperties.Add(propertyName, new CCBValue(pInteger));
            }
        }

        protected virtual void OnHandlePropTypeIntegerLabeled(CCNode node, CCNode parent, string propertyName, int pIntegerLabeled,
                                                              CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeFloatVar(CCNode node, CCNode parent, string propertyName, float[] pFoatVar, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeFloatXY(CCNode pNode, CCNode pParent, string pPropertyName,
                                                       float[] pFoatVar, CCBReader ccbReader)
        {
            if (pPropertyName == PROPERTY_SKEW)
            {
                pNode.SkewX = pFoatVar[0];
                pNode.SkewY = pFoatVar[1];
            }
            else
            {
                CCLog.Log("Unexpected property type: '{0}'!", pPropertyName);
                Debug.Assert(false);
            }
        }

        protected virtual void OnHandlePropTypeCheck(CCNode node, CCNode parent, string propertyName, bool pCheck, CCBReader reader)
        {
            if (propertyName == PROPERTY_VISIBLE)
            {
                node.Visible = pCheck;
            }
            else if (propertyName == PROPERTY_IGNOREANCHORPOINTFORPOSITION)
            {
                node.IgnoreAnchorPointForPosition = pCheck;
            }
            else
            {
//                CCLog.Log("Unexpected property type: '{0}'!", propertyName);
//                Debug.Assert(false);
                // It may be a custom property, add it to custom property dictionary.
                _customProperties.Add(propertyName, new CCBValue(pCheck));
            }
        }

        protected virtual void OnHandlePropTypeSpriteFrame(CCNode node, CCNode parent, string propertyName, CCSpriteFrame spriteFrame,
                                                           CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeAnimation(CCNode node, CCNode parent, string propertyName, CCAnimation animation,
                                                         CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeTexture(CCNode node, CCNode parent, string propertyName, CCTexture2D texture,
                                                       CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeByte(CCNode node, CCNode parent, string propertyName, byte pByte, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeColor3(CCNode node, CCNode parent, string propertyName, CCColor3B color, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }


        protected virtual void OnHandlePropTypeColor4FVar(CCNode node, CCNode parent, string propertyName, CCColor4F[] colorVar,
                                                          CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }


        protected virtual void OnHandlePropTypeFlip(CCNode node, CCNode parent, string propertyName, bool[] pFlip, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }


        protected virtual void OnHandlePropTypeBlendFunc(CCNode node, CCNode parent, string propertyName, CCBlendFunc blendFunc,
                                                         CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }


        protected virtual void OnHandlePropTypeFntFile(CCNode node, CCNode parent, string propertyName, string pFntFile, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeString(CCNode node, CCNode parent, string propertyName, string pString, CCBReader reader)
        {
//            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
//            Debug.Assert(false);
            // It may be a custom property, add it to custom property dictionary.
            _customProperties.Add(propertyName, new CCBValue(pString));
        }

        protected virtual void OnHandlePropTypeText(CCNode node, CCNode parent, string propertyName, string pText, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeFontTTF(CCNode node, CCNode parent, string propertyName, string fontTTF, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeBlock(CCNode node, CCNode parent, string propertyName, BlockData pBlockData, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeBlockCcControl(CCNode node, CCNode parent, string propertyName,
                                                              BlockCCControlData blockControlData, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }

        protected virtual void OnHandlePropTypeCCBFile(CCNode node, CCNode parent, string propertyName, CCNode fileNode, CCBReader reader)
        {
            CCLog.Log("Unexpected property type: '{0}'!", propertyName);
            Debug.Assert(false);
        }
    }
}