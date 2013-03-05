/*
 * CCControlSwitch.h
 *
 * Copyright 2012 Yannick Loriot. All rights reserved.
 * http://yannickloriot.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 */


using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    /** @class CCControlSwitch Switch control for Cocos2D. */

    public class CCControlSwitch : CCControl
    {
        /** Initializes a switch with a mask sprite, on/off sprites for on/off states and a thumb sprite. */

        protected bool m_bMoved;
        /** A Boolean value that determines the off/on state of the switch. */
        protected bool m_bOn;
        protected float m_fInitialTouchXPosition;
        protected CCControlSwitchSprite m_pSwitchSprite;

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                m_bEnabled = value;
                if (m_pSwitchSprite != null)
                {
                    m_pSwitchSprite.Opacity = (byte) (value ? 255 : 128);
                }
            }
        }

        public bool InitWithMaskSprite(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite)
        {
            return InitWithMaskSprite(maskSprite, onSprite, offSprite, thumbSprite, null, null);
        }

        /** Creates a switch with a mask sprite, on/off sprites for on/off states and a thumb sprite. */

        public static CCControlSwitch Create(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite)
        {
            var pRet = new CCControlSwitch();
            pRet.InitWithMaskSprite(maskSprite, onSprite, offSprite, thumbSprite, null, null);
            return pRet;
        }

        /** Initializes a switch with a mask sprite, on/off sprites for on/off states, a thumb sprite and an on/off labels. */

        public bool InitWithMaskSprite(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite, CCLabelTTF onLabel,
                                       CCLabelTTF offLabel)
        {
            if (base.Init())
            {
                Debug.Assert(maskSprite != null, "Mask must not be nil.");
                Debug.Assert(onSprite != null, "onSprite must not be nil.");
                Debug.Assert(offSprite != null, "offSprite must not be nil.");
                Debug.Assert(thumbSprite != null, "thumbSprite must not be nil.");

                TouchEnabled = true;
                m_bOn = true;

                m_pSwitchSprite = new CCControlSwitchSprite();
                m_pSwitchSprite.InitWithMaskSprite(maskSprite, onSprite, offSprite, thumbSprite,
                                                   onLabel, offLabel);
                m_pSwitchSprite.Position = new CCPoint(m_pSwitchSprite.ContentSize.Width / 2, m_pSwitchSprite.ContentSize.Height / 2);
                AddChild(m_pSwitchSprite);

                IgnoreAnchorPointForPosition = false;
                AnchorPoint = new CCPoint(0.5f, 0.5f);
                ContentSize = m_pSwitchSprite.ContentSize;
                return true;
            }
            return false;
        }

        /** Creates a switch with a mask sprite, on/off sprites for on/off states, a thumb sprite and an on/off labels. */

        public static CCControlSwitch Create(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite, CCLabelTTF onLabel,
                                             CCLabelTTF offLabel)
        {
            var pRet = new CCControlSwitch();
            pRet.InitWithMaskSprite(maskSprite, onSprite, offSprite, thumbSprite, onLabel, offLabel);
            return pRet;
        }

        /**
		 * Set the state of the switch to On or Off, optionally animating the transition.
		 *
		 * @param isOn YES if the switch should be turned to the On position; NO if it 
		 * should be turned to the Off position. If the switch is already in the 
		 * designated position, nothing happens.
		 * @param animated YES to animate the "flipping" of the switch; otherwise NO.
		 */

        public void SetOn(bool isOn)
        {
            SetOn(isOn, false);
        }

        public void SetOn(bool isOn, bool animated)
        {
            m_bOn = isOn;

            m_pSwitchSprite.RunAction(
                CCActionTween.Create(
                    0.2f,
                    "sliderXPosition",
                    m_pSwitchSprite.SliderXPosition,
                    (m_bOn) ? m_pSwitchSprite.OnPosition : m_pSwitchSprite.OffPosition
                    )
                );

            SendActionsForControlEvents(CCControlEvent.ValueChanged);
        }

        public bool IsOn()
        {
            return m_bOn;
        }

        public bool HasMoved()
        {
            return m_bMoved;
        }


        public CCPoint LocationFromTouch(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location; // Get the touch position
            touchLocation = ConvertToNodeSpace(touchLocation); // Convert to the node space of this class

            return touchLocation;
        }

        //events
        public override bool TouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            if (!IsTouchInside(pTouch) || !Enabled)
            {
                return false;
            }

            m_bMoved = false;

            CCPoint location = LocationFromTouch(pTouch);

            m_fInitialTouchXPosition = location.x - m_pSwitchSprite.SliderXPosition;

            m_pSwitchSprite.ThumbSprite.Color = new ccColor3B(166, 166, 166);
            m_pSwitchSprite.NeedsLayout();

            return true;
        }

        public override void TouchMoved(CCTouch pTouch, CCEvent pEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);
            location = new CCPoint(location.x - m_fInitialTouchXPosition, 0);

            m_bMoved = true;

            m_pSwitchSprite.SliderXPosition = location.x;
        }

        public override void TouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);

            m_pSwitchSprite.ThumbSprite.Color = new ccColor3B(255, 255, 255);

            if (HasMoved())
            {
                SetOn(!(location.x < m_pSwitchSprite.ContentSize.Width / 2), true);
            }
            else
            {
                SetOn(!m_bOn, true);
            }
        }

        public override void TouchCancelled(CCTouch pTouch, CCEvent pEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);

            m_pSwitchSprite.ThumbSprite.Color = new ccColor3B(255, 255, 255);

            if (HasMoved())
            {
                SetOn(!(location.x < m_pSwitchSprite.ContentSize.Width / 2), true);
            }
            else
            {
                SetOn(!m_bOn, true);
            }
        }

        /** Sprite which represents the view. */
    }

    public class CCControlSwitchSprite : CCSprite, CCActionTweenDelegate
    {
        private CCSprite m_ThumbSprite;
        private float m_fOffPosition;
        private float m_fOnPosition;
        private float m_fSliderXPosition;
        private CCSprite m_pMaskSprite;
        private CCTexture2D m_pMaskTexture;
        private CCLabelTTF m_pOffLabel;
        private CCSprite m_pOffSprite;
        private CCLabelTTF m_pOnLabel;
        private CCSprite m_pOnSprite;

        public CCControlSwitchSprite()
        {
            m_fSliderXPosition = 0.0f;
            m_fOnPosition = 0.0f;
            m_fOffPosition = 0.0f;
            m_pMaskTexture = null;
            TextureLocation = 0;
            MaskLocation = 0;
            m_pOnSprite = null;
            m_pOffSprite = null;
            m_ThumbSprite = null;
            m_pOnLabel = null;
            m_pOffLabel = null;
        }

        public float OnPosition
        {
            get { return m_fOnPosition; }
            set { m_fOnPosition = value; }
        }

        public float OffPosition
        {
            get { return m_fOffPosition; }
            set { m_fOffPosition = value; }
        }

        public CCTexture2D MaskTexture
        {
            get { return m_pMaskTexture; }
            set { m_pMaskTexture = value; }
        }

        public uint TextureLocation { get; set; }

        public uint MaskLocation { get; set; }

        public CCSprite OnSprite
        {
            get { return m_pOnSprite; }
            set { m_pOnSprite = value; }
        }

        public CCSprite OffSprite
        {
            get { return m_pOffSprite; }
            set { m_pOffSprite = value; }
        }

        public CCSprite ThumbSprite
        {
            get { return m_ThumbSprite; }
            set { m_ThumbSprite = value; }
        }


        public CCLabelTTF OnLabel
        {
            get { return m_pOnLabel; }
            set { m_pOnLabel = value; }
        }

        public CCLabelTTF OffLabel
        {
            get { return m_pOffLabel; }
            set { m_pOffLabel = value; }
        }

        public float SliderXPosition
        {
            get { return m_fSliderXPosition; }
            set
            {
                if (value <= m_fOffPosition)
                {
                    // Off
                    value = m_fOffPosition;
                }
                else if (value >= m_fOnPosition)
                {
                    // On
                    value = m_fOnPosition;
                }

                m_fSliderXPosition = value;

                NeedsLayout();
            }
        }

        public float OnSideWidth
        {
            get { return m_pOnSprite.ContentSize.Width; }
        }

        public float OffSideWidth
        {
            get { return m_pOffSprite.ContentSize.Height; }
        }

        #region CCActionTweenDelegate Members

        public virtual void UpdateTweenAction(float value, string key)
        {
            //CCLog.Log("key = {0}, value = {1}", key, value);
            SliderXPosition = value;
        }

        #endregion

        public bool InitWithMaskSprite(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite,
                                       CCSprite thumbSprite, CCLabelTTF onLabel, CCLabelTTF offLabel)
        {
            if (base.InitWithTexture(maskSprite.Texture))
            {
                // Sets the default values
                m_fOnPosition = 0;
                m_fOffPosition = -onSprite.ContentSize.Width + thumbSprite.ContentSize.Width / 2;
                m_fSliderXPosition = m_fOnPosition;

                OnSprite = onSprite;
                OffSprite = offSprite;
                ThumbSprite = thumbSprite;
                OnLabel = onLabel;
                OffLabel = offLabel;

                AddChild(m_ThumbSprite);

                // Set up the mask with the Mask shader
                MaskTexture = maskSprite.Texture;

                /*
				CCGLProgram* pProgram = new CCGLProgram();
				pProgram->initWithVertexShaderByteArray(ccPositionTextureColor_vert, ccExSwitchMask_frag);
				setShaderProgram(pProgram);
				pProgram->release();

				CHECK_GL_ERROR_DEBUG();

				getShaderProgram()->addAttribute(kCCAttributeNamePosition, kCCVertexAttrib_Position);
				getShaderProgram()->addAttribute(kCCAttributeNameColor, kCCVertexAttrib_Color);
				getShaderProgram()->addAttribute(kCCAttributeNameTexCoord, kCCVertexAttrib_TexCoords);
				CHECK_GL_ERROR_DEBUG();

				getShaderProgram()->link();
				CHECK_GL_ERROR_DEBUG();

				getShaderProgram()->updateUniforms();
				CHECK_GL_ERROR_DEBUG();                

				m_uTextureLocation    = glGetUniformLocation( getShaderProgram()->getProgram(), "u_texture");
				m_uMaskLocation       = glGetUniformLocation( getShaderProgram()->getProgram(), "u_mask");
				CHECK_GL_ERROR_DEBUG();
				*/
                ContentSize = m_pMaskTexture.ContentSize;

                NeedsLayout();
                return true;
            }
            return false;
        }

        public override void Draw()
        {
            DrawManager.BindTexture(Texture);
            DrawManager.BlendFunc(new ccBlendFunc(ccMacros.CC_BLEND_SRC, ccMacros.CC_BLEND_DST)); // OGLES.GL_SRC_ALPHA, OGLES.GL_ONE_MINUS_SRC_ALPHA));
            DrawManager.DrawQuad(ref m_sQuad);

            //    /*
            //    CC_NODE_DRAW_SETUP();

            //    ccGLEnableVertexAttribs(kCCVertexAttribFlag_PosColorTex);
            //    ccGLBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
            //    getShaderProgram()->setUniformForModelViewProjectionMatrix();

            //    glActiveTexture(GL_TEXTURE0);
            //    glBindTexture( GL_TEXTURE_2D, getTexture()->getName());
            //    glUniform1i(m_uTextureLocation, 0);

            //    glActiveTexture(GL_TEXTURE1);
            //    glBindTexture( GL_TEXTURE_2D, m_pMaskTexture->getName() );
            //    glUniform1i(m_uMaskLocation, 1);

            //#define kQuadSize sizeof(m_sQuad.bl)
            //    long offset = (long)&m_sQuad;

            //    // vertex
            //    int diff = offsetof( ccV3F_C4B_T2F, vertices);
            //    glVertexAttribPointer(kCCVertexAttrib_Position, 3, GL_FLOAT, GL_FALSE, kQuadSize, (void*) (offset + diff));

            //    // texCoods
            //    diff = offsetof( ccV3F_C4B_T2F, texCoords);
            //    glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, kQuadSize, (void*)(offset + diff));

            //    // color
            //    diff = offsetof( ccV3F_C4B_T2F, colors);
            //    glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, kQuadSize, (void*)(offset + diff));

            //    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);    
            //    glActiveTexture(GL_TEXTURE0);
            //    */
        }


        public void NeedsLayout()
        {
            m_pOnSprite.Position = new CCPoint(m_pOnSprite.ContentSize.Width / 2 + m_fSliderXPosition,
                                               m_pOnSprite.ContentSize.Height / 2);
            m_pOffSprite.Position = new CCPoint(m_pOnSprite.ContentSize.Width + m_pOffSprite.ContentSize.Width / 2 + m_fSliderXPosition,
                                                m_pOffSprite.ContentSize.Height / 2);
            m_ThumbSprite.Position = new CCPoint(m_pOnSprite.ContentSize.Width + m_fSliderXPosition,
                                                 m_pMaskTexture.ContentSize.Height / 2);

            if (m_pOnLabel != null)
            {
                m_pOnLabel.Position = new CCPoint(m_pOnSprite.Position.x - m_ThumbSprite.ContentSize.Width / 6,
                                                  m_pOnSprite.ContentSize.Height / 2);
            }
            if (m_pOffLabel != null)
            {
                m_pOffLabel.Position = new CCPoint(m_pOffSprite.Position.x + m_ThumbSprite.ContentSize.Width / 6,
                                                   m_pOffSprite.ContentSize.Height / 2);
            }

            CCRenderTexture rt = CCRenderTexture.Create((int) m_pMaskTexture.ContentSize.Width, (int) m_pMaskTexture.ContentSize.Height,
                                                        SurfaceFormat.Color, DepthFormat.None, RenderTargetUsage.DiscardContents);

            rt.BeginWithClear(0, 0, 0, 0);

            m_pOnSprite.Visit();
            m_pOffSprite.Visit();

            if (m_pOnLabel != null)
            {
                m_pOnLabel.Visit();
            }
            if (m_pOffLabel != null)
            {
                m_pOffLabel.Visit();
            }

            if (m_pMaskSprite == null)
            {
                m_pMaskSprite = Create(m_pMaskTexture);
                m_pMaskSprite.AnchorPoint = new CCPoint(0, 0);
                m_pMaskSprite.BlendFunc = new ccBlendFunc(OGLES.GL_ZERO, OGLES.GL_SRC_ALPHA);
            }
            else
            {
                m_pMaskSprite.Texture = m_pMaskTexture;
            }

            m_pMaskSprite.Visit();

            rt.End();

            Texture = rt.Sprite.Texture;
            //IsFlipY = true;
        }
    }
}