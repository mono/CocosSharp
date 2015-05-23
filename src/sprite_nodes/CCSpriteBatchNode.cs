using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    [Obsolete("This class is now obsolete and has been made redundant by CCRenderer.")]
    public class CCSpriteBatchNode : CCNode, ICCTexture
    {
        const int defaultSpriteBatchCapacity = 29;


        #region Properties

        public CCTextureAtlas TextureAtlas { get ; private set; }
        public CCRawList<CCSprite> Descendants { get; private set; }
        public CCBlendFunc BlendFunc { get; set; }

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }
            set { Texture.IsAntialiased = value; }
        }

        public virtual CCTexture2D Texture
        {
            get { return TextureAtlas.Texture; }
            set
            {
                TextureAtlas.Texture = value;
            }
        }

//        // Size of batch node in world space makes no sense
//        public override CCSize ContentSize
//        {
//            get { return CCSize.Zero; }
//            set
//            {
//            }
//        }
//
//        public override CCAffineTransform AffineLocalTransform
//        {
//            get { return CCAffineTransform.Identity; }
//        }
//
//        protected internal override Matrix XnaLocalMatrix 
//        { 
//            get { return Matrix.Identity; }
//            protected set 
//            {
//            }
//        }
//
        #endregion Properties


        #region Constructors

        // We need this constructor for all the subclasses that initialise by directly calling InitCCSpriteBatchNode
        public CCSpriteBatchNode()
        {
        }

        public CCSpriteBatchNode(CCTexture2D tex, int capacity=defaultSpriteBatchCapacity)
        {
            BlendFunc = CCBlendFunc.AlphaBlend;

            if (capacity == 0)
            {
                capacity = defaultSpriteBatchCapacity;
            }

            TextureAtlas = new CCTextureAtlas(tex, capacity);

            // no lazy alloc in this node
            Children = new CCRawList<CCNode>(capacity);
            Descendants = new CCRawList<CCSprite>(capacity);
        }

        public CCSpriteBatchNode(string fileImage, int capacity=defaultSpriteBatchCapacity) 
            : this(CCTextureCache.SharedTextureCache.AddImage(fileImage), capacity)
        {
        }

        #endregion Constructors


        protected override void AddedToScene ()
        {
            base.AddedToScene ();

            if(ContentSize == CCSize.Zero)
                ContentSize = Layer.VisibleBoundsWorldspace.Size;
        }

        public void AppendChild(CCSprite sprite)
        {
            AddChild(sprite);
        }

        public void RemoveSpriteFromAtlas(CCSprite sprite)
        {
            RemoveChild(sprite);
        }

        protected CCSpriteBatchNode AddSpriteWithoutQuad(CCSprite child, int z, int aTag)
        {
            base.AddChild(child, z, aTag);

            return this;
        }
    }
}