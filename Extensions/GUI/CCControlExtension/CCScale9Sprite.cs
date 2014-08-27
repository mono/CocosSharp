using System.Diagnostics;

namespace CocosSharp
{
	/// <summary>
	/// This is a special sprite container that represents a 9 point sprite region, where 8 of hte
	/// points are along the perimeter, and the 9th is the center area. This special sprite is capable of resizing
	/// itself to arbitrary scales.
	/// </summary>
	public class CCScale9Sprite : CCNode
	{
		#region Enums

		private enum Positions
		{
			Centre = 0,
			Top,
			Left,
			Right,
			Bottom,
			TopRight,
			TopLeft,
			BottomRight,
			BottomLeft
		};

		#endregion Enums

		bool positionsAreDirty;
		bool opacityModifyRGB;
		bool spriteFrameRotated;
		bool spritesGenerated;

		float insetBottom;
		float insetLeft;
		float insetRight;
		float insetTop;

		/** 
        * The end-cap insets. 
        * On a non-resizeable sprite, this property is set to CGRectZero; the sprite 
        * does not use end caps and the entire sprite is subject to stretching. 
        */
		CCRect capInsets;
		CCRect capInsetsInternal;
		CCRect spriteRect;

		CCSize originalSize;
		CCSize preferredSize;

		CCSpriteBatchNode scale9Image;
		CCSprite top;
		CCSprite topLeft;
		CCSprite topRight;
		CCSprite bottom;
		CCSprite bottomLeft;
		CCSprite bottomRight;
		CCSprite centre;
		CCSprite left;
		CCSprite right;


		#region Properties

		public override CCSize ContentSize
		{
			get { return base.ContentSize; }
			set
			{
				base.ContentSize = value;
				positionsAreDirty = true;
			}
		}

		public CCSize PreferredSize
		{
			get { return preferredSize; }
			set
			{
				ContentSize = value;
				preferredSize = value;
			}
		}

		public CCRect CapInsets
		{
			get { return capInsets; }
			set
			{
				CCSize contentSize = ContentSize;
				UpdateWithBatchNode(scale9Image, spriteRect, spriteFrameRotated, value);
				ContentSize = contentSize;
			}
		}

		public float InsetLeft
		{
			set
			{
				insetLeft = value;
				UpdateCapInset();
			}
			get { return insetLeft; }
		}

		public float InsetTop
		{
			set
			{
				insetTop = value;
				UpdateCapInset();
			}
			get { return insetTop; }
		}

		public float InsetRight
		{
			set
			{
				insetRight = value;
				UpdateCapInset();
			}
			get { return insetRight; }
		}

		public float InsetBottom
		{
			set
			{
				insetBottom = value;
				UpdateCapInset();
			}
			get { return insetBottom; }
		}

		public override CCColor3B Color
		{
			get { return RealColor; }
			set
			{
				base.Color = value;
				if (scale9Image != null && scale9Image.Children != null)
				{
					foreach(CCNode child in Children)
					{
						var node = child;
						if (node != null)
						{
							node.Color = value;
						}
					}
				}
			}
		}

		public override byte Opacity
		{
			get { return RealOpacity; }
			set
			{
				base.Opacity = value;
				if (scale9Image != null && scale9Image.Children != null)
				{
					foreach(CCNode child in Children)
					{
						var node = child;
						if (node != null)
						{
							node.Opacity = value;
						}
					}
				}
			}
		}

		public override bool IsColorModifiedByOpacity
		{
			get { return opacityModifyRGB; }
			set
			{
				opacityModifyRGB = value;
				if (scale9Image != null && scale9Image.Children != null)
				{
					foreach(CCNode child in Children)
					{
						var node = child;
						if (node != null)
						{
							node.IsColorModifiedByOpacity = value;
						}
					}
				}
			}
		}

		public CCSpriteFrame SpriteFrame
		{
			set 
			{
				CCSpriteBatchNode batchnode = new CCSpriteBatchNode (value.Texture, 9);
                UpdateWithBatchNode (batchnode, value.TextureRectInPixels, value.IsRotated, CCRect.Zero);

				// Reset insets
				insetLeft = 0;
				insetTop = 0;
				insetRight = 0;
				insetBottom = 0;
			}
		}

		#endregion Properties


		#region Constructors

		public CCScale9Sprite(CCSpriteBatchNode batchnode, CCRect rect, bool rotated, CCRect capInsets)
		{
			InitCCScale9Sprite(batchnode, rect, rotated, capInsets);
		}

		public CCScale9Sprite(CCSpriteBatchNode batchnode, CCRect rect, CCRect capInsets) 
			: this(batchnode, rect, false, capInsets)
		{
		}

		public CCScale9Sprite() : this((CCSpriteBatchNode)null, CCRect.Zero, CCRect.Zero)
		{
		}

		public CCScale9Sprite(CCRect capInsets) : this((CCSpriteBatchNode)null, CCRect.Zero, capInsets)
		{
		}

		// File

		public CCScale9Sprite(string file, CCRect rect, CCRect capInsets) : this(new CCSpriteBatchNode(file, 9), rect, capInsets)
		{
		}

		public CCScale9Sprite(string file, CCRect rect) : this(file, rect, CCRect.Zero)
		{
		}

		public CCScale9Sprite(string file) : this(file, CCRect.Zero)
		{
		}

		// Sprite frame

		public CCScale9Sprite(CCSpriteFrame spriteFrame, CCRect capInsets) 
            : this(new CCSpriteBatchNode(spriteFrame.Texture, 9), spriteFrame.TextureRectInPixels, spriteFrame.IsRotated, capInsets)
		{
		}

		public CCScale9Sprite(CCSpriteFrame spriteFrame) : this(spriteFrame, CCRect.Zero)
		{
		}

		// Sprite frame name

		// A constructor with argument string already exists (filename), so create this factory method instead
		public static CCScale9Sprite SpriteWithFrameName(string spriteFrameName, CCRect capInsets)
		{
			CCScale9Sprite sprite = new CCScale9Sprite();
			sprite.InitWithSpriteFrameName(spriteFrameName, capInsets);

			return sprite;
		}

		public static CCScale9Sprite SpriteWithFrameName(string spriteFrameName)
		{
			return CCScale9Sprite.SpriteWithFrameName(spriteFrameName, CCRect.Zero);
		}

		void InitCCScale9Sprite(CCSpriteBatchNode batchnode, CCRect rect, bool rotated, CCRect capInsets)
		{
			if (batchnode != null)
			{
				UpdateWithBatchNode(batchnode, rect, rotated, capInsets);
			}

			AnchorPoint = new CCPoint(0.5f, 0.5f);
			positionsAreDirty = true;
		}

		// Init calls that are called externally for objects that are already instantiated

		internal void InitWithSpriteFrame(CCSpriteFrame spriteFrame)
		{
            InitCCScale9Sprite(new CCSpriteBatchNode(spriteFrame.Texture, 9), spriteFrame.TextureRectInPixels, spriteFrame.IsRotated, CCRect.Zero);
		}

		internal void InitWithSpriteFrameName(string spriteFrameName)
		{
			InitWithSpriteFrameName(spriteFrameName, CCRect.Zero);
		}

		internal void InitWithSpriteFrameName(string spriteFrameName, CCRect capInsets)
		{
            CCSpriteFrame spriteFrame = CCSpriteFrameCache.SharedSpriteFrameCache[spriteFrameName];

            InitCCScale9Sprite(new CCSpriteBatchNode(spriteFrame.Texture, 9), spriteFrame.TextureRectInPixels, spriteFrame.IsRotated, capInsets);
		}

		#endregion Constructors


		public override void Visit()
		{
			if (positionsAreDirty)
			{
				UpdatePositions();
				positionsAreDirty = false;
			}
			base.Visit();
		}

		public override void UpdateDisplayedColor(CCColor3B parentColor)
		{
			base.UpdateDisplayedColor(parentColor);
			if (scale9Image != null && scale9Image.Children != null)
			{
				foreach(CCNode child in Children)
				{
					var node = child;
					if (node != null)
					{
						node.UpdateDisplayedColor(parentColor);
					}
				}
			}

		}

		public bool UpdateWithBatchNode(CCSpriteBatchNode batchnode, CCRect rect, bool rotated, CCRect capInsets)
		{
			var opacity = Opacity;
			var color = Color;

			// Release old sprites
			RemoveAllChildren(true);

			scale9Image = batchnode;
			scale9Image.RemoveAllChildren(true);

			this.capInsets = capInsets;
			spriteFrameRotated = rotated;

			// If there is no given rect
			if (rect.Equals(CCRect.Zero))
			{
				// Get the texture size as original
                CCSize textureSize = scale9Image.TextureAtlas.Texture.ContentSizeInPixels;

				rect = new CCRect(0, 0, textureSize.Width, textureSize.Height);
			}

			// Set the given rect's size as original size
			spriteRect = rect;
			originalSize = rect.Size;
			preferredSize = originalSize;
			capInsetsInternal = capInsets;

			float h = rect.Size.Height;
			float w = rect.Size.Width;

			// If there is no specified center region
			if (capInsetsInternal.Equals(CCRect.Zero))
			{
				capInsetsInternal = new CCRect(w / 3, h / 3, w / 3, h / 3);
			}

			float left_w = capInsetsInternal.Origin.X;
			float center_w = capInsetsInternal.Size.Width;
			float right_w = rect.Size.Width - (left_w + center_w);

			float top_h = capInsetsInternal.Origin.Y;
			float center_h = capInsetsInternal.Size.Height;
			float bottom_h = rect.Size.Height - (top_h + center_h);

			// calculate rects

			// ... top row
			float x = 0.0f;
			float y = 0.0f;

			// top left
			CCRect lefttopbounds = new CCRect(x, y, left_w, top_h);

			// top center
			x += left_w;
			CCRect centertopbounds = new CCRect(x, y, center_w, top_h);

			// top right
			x += center_w;
			CCRect righttopbounds = new CCRect(x, y, right_w, top_h);

			// ... center row
			x = 0.0f;
			y = 0.0f;
			y += top_h;

			// center left
			CCRect leftcenterbounds = new CCRect(x, y, left_w, center_h);

			// center center
			x += left_w;
			CCRect centerbounds = new CCRect(x, y, center_w, center_h);

			// center right
			x += center_w;
			CCRect rightcenterbounds = new CCRect(x, y, right_w, center_h);

			// ... bottom row
			x = 0.0f;
			y = 0.0f;
			y += top_h;
			y += center_h;

			// bottom left
			CCRect leftbottombounds = new CCRect(x, y, left_w, bottom_h);

			// bottom center
			x += left_w;
			CCRect centerbottombounds = new CCRect(x, y, center_w, bottom_h);

			// bottom right
			x += center_w;
			CCRect rightbottombounds = new CCRect(x, y, right_w, bottom_h);

			if (!rotated)
			{
				CCAffineTransform t = CCAffineTransform.Identity;
				t = CCAffineTransform.Translate(t, rect.Origin.X, rect.Origin.Y);

				centerbounds = CCAffineTransform.Transform(centerbounds, t);
				rightbottombounds = CCAffineTransform.Transform(rightbottombounds, t);
				leftbottombounds = CCAffineTransform.Transform(leftbottombounds, t);
				righttopbounds = CCAffineTransform.Transform(righttopbounds, t);
				lefttopbounds = CCAffineTransform.Transform(lefttopbounds, t);
				rightcenterbounds = CCAffineTransform.Transform(rightcenterbounds, t);
				leftcenterbounds = CCAffineTransform.Transform(leftcenterbounds, t);
				centerbottombounds = CCAffineTransform.Transform(centerbottombounds, t);
				centertopbounds = CCAffineTransform.Transform(centertopbounds, t);

				// Centre
				centre = new CCSprite(scale9Image.Texture, centerbounds);
				scale9Image.AddChild(centre, 0, (int)Positions.Centre);

				// Top
				top = new CCSprite(scale9Image.Texture, centertopbounds);
				scale9Image.AddChild(top, 1, (int)Positions.Top);

				// Bottom
				bottom = new CCSprite(scale9Image.Texture, centerbottombounds);
				scale9Image.AddChild(bottom, 1, (int)Positions.Bottom);

				// Left
				left = new CCSprite(scale9Image.Texture, leftcenterbounds);
				scale9Image.AddChild(left, 1, (int)Positions.Left);

				// Right
				right = new CCSprite(scale9Image.Texture, rightcenterbounds);
				scale9Image.AddChild(right, 1, (int)Positions.Right);

				// Top left
				topLeft = new CCSprite(scale9Image.Texture, lefttopbounds);
				scale9Image.AddChild(topLeft, 2, (int)Positions.TopLeft);

				// Top right
				topRight = new CCSprite(scale9Image.Texture, righttopbounds);
				scale9Image.AddChild(topRight, 2, (int)Positions.TopRight);

				// Bottom left
				bottomLeft = new CCSprite(scale9Image.Texture, leftbottombounds);
				scale9Image.AddChild(bottomLeft, 2, (int)Positions.BottomLeft);

				// Bottom right
				bottomRight = new CCSprite(scale9Image.Texture, rightbottombounds);
				scale9Image.AddChild(bottomRight, 2, (int)Positions.BottomRight);
			}
			else
			{
				// set up transformation of coordinates
				// to handle the case where the sprite is stored rotated
				// in the spritesheet
				// CCLog("rotated");

				CCAffineTransform t = CCAffineTransform.Identity;

				CCRect rotatedcenterbounds = centerbounds;
				CCRect rotatedrightbottombounds = rightbottombounds;
				CCRect rotatedleftbottombounds = leftbottombounds;
				CCRect rotatedrighttopbounds = righttopbounds;
				CCRect rotatedlefttopbounds = lefttopbounds;
				CCRect rotatedrightcenterbounds = rightcenterbounds;
				CCRect rotatedleftcenterbounds = leftcenterbounds;
				CCRect rotatedcenterbottombounds = centerbottombounds;
				CCRect rotatedcentertopbounds = centertopbounds;

				t = CCAffineTransform.Translate(t, rect.Size.Height + rect.Origin.X, rect.Origin.Y);
				t = CCAffineTransform.Rotate(t, 1.57079633f);

				centerbounds = CCAffineTransform.Transform(centerbounds, t);
				rightbottombounds = CCAffineTransform.Transform(rightbottombounds, t);
				leftbottombounds = CCAffineTransform.Transform(leftbottombounds, t);
				righttopbounds = CCAffineTransform.Transform(righttopbounds, t);
				lefttopbounds = CCAffineTransform.Transform(lefttopbounds, t);
				rightcenterbounds = CCAffineTransform.Transform(rightcenterbounds, t);
				leftcenterbounds = CCAffineTransform.Transform(leftcenterbounds, t);
				centerbottombounds = CCAffineTransform.Transform(centerbottombounds, t);
				centertopbounds = CCAffineTransform.Transform(centertopbounds, t);

				rotatedcenterbounds.Origin = centerbounds.Origin;
				rotatedrightbottombounds.Origin = rightbottombounds.Origin;
				rotatedleftbottombounds.Origin = leftbottombounds.Origin;
				rotatedrighttopbounds.Origin = righttopbounds.Origin;
				rotatedlefttopbounds.Origin = lefttopbounds.Origin;
				rotatedrightcenterbounds.Origin = rightcenterbounds.Origin;
				rotatedleftcenterbounds.Origin = leftcenterbounds.Origin;
				rotatedcenterbottombounds.Origin = centerbottombounds.Origin;
				rotatedcentertopbounds.Origin = centertopbounds.Origin;

				// Centre
					centre = new CCSprite(scale9Image.Texture, rotatedcenterbounds, true);
					//centre.InitWithTexture(scale9Image.Texture, rotatedcenterbounds, true);
				scale9Image.AddChild(centre, 0, (int)Positions.Centre);

				// Top
					top = new CCSprite(scale9Image.Texture, rotatedcentertopbounds, true);
					//top.InitWithTexture(scale9Image.Texture, rotatedcentertopbounds, true);
				scale9Image.AddChild(top, 1, (int)Positions.Top);

				// Bottom
					bottom = new CCSprite(scale9Image.Texture, rotatedcenterbottombounds, true);
					//bottom.InitWithTexture(scale9Image.Texture, rotatedcenterbottombounds, true);
				scale9Image.AddChild(bottom, 1, (int)Positions.Bottom);

				// Left
					left = new CCSprite(scale9Image.Texture, rotatedleftcenterbounds, true);
					//left.InitWithTexture(scale9Image.Texture, rotatedleftcenterbounds, true);
				scale9Image.AddChild(left, 1, (int)Positions.Left);

				// Right
					right = new CCSprite(scale9Image.Texture, rotatedrightcenterbounds, true);
					//right.InitWithTexture(scale9Image.Texture, rotatedrightcenterbounds, true);
				scale9Image.AddChild(right, 1, (int)Positions.Right);

				// Top left
					topLeft = new CCSprite(scale9Image.Texture, rotatedlefttopbounds, true);
					//topLeft.InitWithTexture(scale9Image.Texture, rotatedlefttopbounds, true);
				scale9Image.AddChild(topLeft, 2, (int)Positions.TopLeft);

				// Top right
					topRight = new CCSprite(scale9Image.Texture, rotatedrighttopbounds, true);
					//topRight.InitWithTexture(scale9Image.Texture, rotatedrighttopbounds, true);
				scale9Image.AddChild(topRight, 2, (int)Positions.TopRight);

				// Bottom left
					bottomLeft = new CCSprite(scale9Image.Texture, rotatedleftbottombounds, true);
					//bottomLeft.InitWithTexture(scale9Image.Texture, rotatedleftbottombounds, true);
				scale9Image.AddChild(bottomLeft, 2, (int)Positions.BottomLeft);

				// Bottom right
					bottomRight = new CCSprite(scale9Image.Texture, rotatedrightbottombounds, true);
					//bottomRight.InitWithTexture(scale9Image.Texture, rotatedrightbottombounds, true);
				scale9Image.AddChild(bottomRight, 2, (int)Positions.BottomRight);
			}

			ContentSize = rect.Size;
			AddChild(scale9Image);

			if (spritesGenerated)
			{
				// Restore color and opacity
				Opacity = opacity;
				Color = color;
			}
			spritesGenerated = true;

			return true;
		}

		protected void UpdatePositions()
		{
			// Check that instances are non-NULL
			if (!((topLeft != null) &&
				(topRight != null) &&
				(bottomRight != null) &&
				(bottomLeft != null) &&
				(centre != null)))
			{
				// if any of the above sprites are NULL, return
				return;
			}

			CCSize size = ContentSize;

			float sizableWidth = size.Width - topLeft.ContentSize.Width - topRight.ContentSize.Width;
			float sizableHeight = size.Height - topLeft.ContentSize.Height - bottomRight.ContentSize.Height;

			float horizontalScale = sizableWidth / centre.ContentSize.Width;
			float verticalScale = sizableHeight / centre.ContentSize.Height;

			centre.ScaleX = horizontalScale;
			centre.ScaleY = verticalScale;

			float rescaledWidth = centre.ContentSize.Width * horizontalScale;
			float rescaledHeight = centre.ContentSize.Height * verticalScale;

			float leftWidth = bottomLeft.ContentSize.Width;
			float bottomHeight = bottomLeft.ContentSize.Height;

			bottomLeft.AnchorPoint = CCPoint.Zero;
			bottomRight.AnchorPoint = CCPoint.Zero;
			topLeft.AnchorPoint = CCPoint.Zero;
			topRight.AnchorPoint = CCPoint.Zero;
			left.AnchorPoint = CCPoint.Zero;
			right.AnchorPoint = CCPoint.Zero;
			top.AnchorPoint = CCPoint.Zero;
			bottom.AnchorPoint = CCPoint.Zero;
			centre.AnchorPoint = CCPoint.Zero;

			// Position corners
			bottomLeft.Position = CCPoint.Zero;
			bottomRight.Position = new CCPoint(leftWidth + rescaledWidth, 0);
			topLeft.Position = new CCPoint(0, bottomHeight + rescaledHeight);
			topRight.Position = new CCPoint(leftWidth + rescaledWidth, bottomHeight + rescaledHeight);

			// Scale and position borders
			left.Position = new CCPoint(0, bottomHeight);
			left.ScaleY = verticalScale;
			right.Position = new CCPoint(leftWidth + rescaledWidth, bottomHeight);
			right.ScaleY = verticalScale;
			bottom.Position = new CCPoint(leftWidth, 0);
			bottom.ScaleX = horizontalScale;
			top.Position = new CCPoint(leftWidth, bottomHeight + rescaledHeight);
			top.ScaleX = horizontalScale;

			// Position centre
			centre.Position = new CCPoint(leftWidth, bottomHeight);
		}

		protected void UpdateCapInset()
		{
			CCRect insets;
			if (insetLeft == 0 && insetTop == 0 && insetRight == 0 && insetBottom == 0)
			{
				insets = CCRect.Zero;
			}
			else
			{
				insets = new CCRect(insetLeft,
					insetTop,
					spriteRect.Size.Width - insetLeft - insetRight,
					spriteRect.Size.Height - insetTop - insetBottom);
			}
			CapInsets = insets;
		}
	}
}