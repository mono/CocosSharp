using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    /// <summary>
    /// This is a sprite with a collision mask assigned to it. The sprite collision works by testing
    /// for overlap on the active pixels.
    /// </summary>
    public class CCMaskedSprite : CCSprite
    {
        #region Constructors

        public CCMaskedSprite (CCTexture2D texture, byte[] mask) : base(texture)
        {
            _MyMask = mask;
        }

        public CCMaskedSprite(CCTexture2D texture, CCRect rect, byte[] mask)
            : base(texture, rect)
        {
            _MyMask = mask;
        }

        public CCMaskedSprite(string fileName, byte[] mask)
            : base(fileName)
        {
            _MyMask = mask;
        }

        public CCMaskedSprite(string fileName, CCRect rect, byte[] mask)
            : base(fileName, rect)
        {
            _MyMask = mask;
        }

        public CCMaskedSprite(CCSpriteFrame pSpriteFrame, byte[] mask)
            : base(pSpriteFrame)
        {
            _MyMask = mask;
        }

        public CCMaskedSprite()
        {
        }

        #endregion

            #region Sprite Collission

        public virtual bool CollidesWith(CCMaskedSprite other, out CCPoint hitPoint)
        {
            // Default behavior uses per-pixel collision detection
            return CollidesWith(other, out hitPoint, true);
        }

        public virtual bool CollidesWith(CCMaskedSprite other, out CCPoint hitPoint, bool calcPerPixel)
        {
            hitPoint = CCPoint.Zero;
            if (other.Texture == null || other.Texture.XNATexture == null)
            {
                return (false);
            }
            if (Texture == null || Texture.XNATexture == null)
            {
                return (false);
            }
            // Get dimensions of texture
            int widthOther = other.Texture.XNATexture.Width;
            int heightOther = other.Texture.XNATexture.Height;
            int widthMe = Texture.XNATexture.Width;
            int heightMe = Texture.XNATexture.Height;

            return BoundingBox.IntersectsRect(other.BoundingBox) // If simple intersection fails, don't even bother with per-pixel
                && MaskCollision(this, other, out hitPoint);
        }

        /// <summary>
        /// This is the feature mask for the sprite.
        /// </summary>
        private byte[] _MyMask;

        public virtual byte[] CollisionMask
        {
            get
            {
                return (_MyMask);
            }
            set
            {
                _MyMask = value;
                // Check the size?
            }
        }
#if DEBUG
        public bool DebugCollision = false;
#endif
        /// <summary>
        /// Tests the collision mask for the two sprites and returns true if there is a collision. The hit point of the
        /// collision is returned.
        /// </summary>
        /// <param name="a">Sprite A</param>
        /// <param name="b">Sprite B</param>
        /// <param name="hitPoint">The place, in real space, where they collide</param>
        /// <returns>True upon collision and false if not.</returns>
        static bool MaskCollision(CCMaskedSprite a, CCMaskedSprite b, out CCPoint hitPoint)
        {
            byte[] maskA = a.CollisionMask; // bitfield mask of sprite A
            byte[] maskB = b.CollisionMask; // bitfield mask of sprite B
            int aWidth = (int)a.Texture.ContentSize.Width; // bitwise stride
            int bWidth = (int)b.Texture.ContentSize.Width; // bitwise stride
            int aHeight = (int)a.Texture.ContentSize.Height;
            int bHeight = (int)b.Texture.ContentSize.Height;
            // Calculate the intersecting rectangle
            Rectangle aBounds = a.CollisionBounds;
            Rectangle bBounds = b.CollisionBounds;
            int x1 = Math.Max(aBounds.X, bBounds.X);
            int x2 = Math.Min(aBounds.X + aBounds.Width, bBounds.X + bBounds.Width);

            int y1 = Math.Max(aBounds.Y, bBounds.Y);
            int y2 = Math.Min(aBounds.Y + aBounds.Height, bBounds.Y + bBounds.Height);
            // Next extract the bitfields for the intersection rectangles
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; x++)
                {
                    // Coordinates in the respective sprites
                    // Invert the Y because screen coords are opposite of mask coordinates!
                    int xA = x - aBounds.X;
                    int yA = aHeight - (y - aBounds.Y);
                    if (yA < 0)
                    {
                        yA = 0;
                    }
                    else if (yA >= aHeight)
                    {
                        yA = aHeight - 1;
                    }
                    int xB = x - bBounds.X;
                    int yB = bHeight - (y - bBounds.Y);
                    if (yB < 0)
                    {
                        yB = 0;
                    }
                    else if (yB >= bHeight)
                    {
                        yB = bHeight - 1;
                    }
                    // Get the color from each texture
                    int iA = xA + yA * aWidth;
                    int iB = xB + yB * bWidth;
                    if (iA >= maskA.Length || iB >= maskB.Length)
                    {
                        continue;
                    }
                    byte ca = maskA[iA];
                    byte cb = maskB[iB];
#if DEBUG
                    if (a.DebugCollision && b.DebugCollision)
                    {
                        CCLog.Log("Collision test[{0},{1}] = A{2} == B{3} {4}", x, y, ca, cb, (ca == cb && ca > 0) ? "BOOM" : "");
                    }
#endif

                    if (ca > 0 && cb > 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        // Find the hit point, where on the sprite in real space the collision occurs.
                        hitPoint = new CCPoint(x,y);
                        return (true);
                    }
                }
            }
            hitPoint = new CCPoint(0, 0);
            return (false);
        }
        protected virtual Rectangle CollisionBounds
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - Texture.ContentSize.Width / 2f),
                    (int)(Position.Y - Texture.ContentSize.Height / 2f),
                    (int)Texture.ContentSize.Width,
                    (int)Texture.ContentSize.Height);
            }

        }
        #endregion
    }
}
