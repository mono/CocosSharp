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

        public virtual bool CollidesWith(CCMaskedSprite target, out CCPoint pt)
        {
            pt = CCPoint.Zero;
            CCAffineTransform m1 = NodeToWorldTransform();
            CCAffineTransform m2 = target.NodeToWorldTransform();
            CCRect myBBInWorld = WorldBoundingBox;
            CCRect targetBBInWorld = target.WorldBoundingBox;
            if (!myBBInWorld.IntersectsRect(targetBBInWorld))
            {
                return (false);
            }
            // Based upon http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Putting_CD_into_practice.php
            Matrix mat1 = m1.XnaMatrix;
            Matrix mat2 = m2.XnaMatrix;
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width2 = (int)target.ContentSize.Width;
            int height2 = (int)target.ContentSize.Height;
            int width1 = (int)ContentSize.Width;
            int height1 = (int)ContentSize.Height;
            byte[] maskA = CollisionMask;
            byte[] maskB = target.CollisionMask;
            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            int iA = x1 + y1 * width1;
                            int iB = x2 + y2 * width2;
                            if (iA >= maskA.Length || iB >= maskB.Length)
                            {
                                continue;
                            }
                            byte ca = maskA[iA];
                            byte cb = maskB[iB];

                            if (maskA[iA] > 0)
                            {
                                if (maskB[iB] > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, mat1);
                                    pt = new CCPoint(screenPos);
                                    return (true);
                                }
                            }
                        }
                    }
                }
            }
            return (false);
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
        #endregion
    }
}
