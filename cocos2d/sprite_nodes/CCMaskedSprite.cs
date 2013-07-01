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
        }

        public CCMaskedSprite(CCTexture2D texture, CCRect rect, byte[] mask)
            : base(texture, rect)
        {
        }

        public CCMaskedSprite(string fileName, byte[] mask)
            : base(fileName)
        {
        }

        public CCMaskedSprite(string fileName, CCRect rect, byte[] mask)
            : base(fileName, rect)
        {
        }

        public CCMaskedSprite(CCSpriteFrame pSpriteFrame, byte[] mask)
            : base(pSpriteFrame)
        {
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

            return Bounds.Intersects(other.Bounds) // If simple intersection fails, don't even bother with per-pixel
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
            int x1 = Math.Max(a.Bounds.X, b.Bounds.X);
            int x2 = Math.Min(a.Bounds.X + a.Bounds.Width, b.Bounds.X + b.Bounds.Width);

            int y1 = Math.Max(a.Bounds.Y, b.Bounds.Y);
            int y2 = Math.Min(a.Bounds.Y + a.Bounds.Height, b.Bounds.Y + b.Bounds.Height);
            // Next extract the bitfields for the intersection rectangles
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; x++)
                {
                    // Coordinates in the respective sprites
                    // Invert the Y because screen coords are opposite of mask coordinates!
                    int xA = x - a.Bounds.X;
                    int yA = aHeight - (y - a.Bounds.Y);
                    if (yA < 0)
                    {
                        yA = 0;
                    }
                    else if (yA >= aHeight)
                    {
                        yA = aHeight - 1;
                    }
                    int xB = x - b.Bounds.X;
                    int yB = bHeight - (y - b.Bounds.Y);
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
                        hitPoint = new CCPoint(a.Position.X - a.AnchorPoint.X * a.ContentSizeInPixels.Width + x, a.Position.Y - a.AnchorPoint.Y * a.ContentSizeInPixels.Height + y);
                        return (true);
                    }
                }
            }
            hitPoint = new CCPoint(0, 0);
            return (false);
        }
        private Rectangle bounds = Rectangle.Empty;
        public virtual Rectangle Bounds
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
