using System;

namespace CocosSharp
{
    public class CCControlColourPicker : CCControl
    {
        private CCSprite _background;
        private CCControlSaturationBrightnessPicker _colourPicker;
        protected HSV _hsv;
        private CCControlHuePicker _huePicker;

        public CCControlColourPicker()
        {
            Init();
        }

        public CCControlSaturationBrightnessPicker ColourPicker
        {
            get { return _colourPicker; }
            set { _colourPicker = value; }
        }

        public CCControlHuePicker HuePicker
        {
            get { return _huePicker; }
            set { _huePicker = value; }
        }

        public CCSprite Background
        {
            get { return _background; }
            set { _background = value; }
        }

        public void SetColor(CCColor3B colorValue)
        {
            // XXX fixed me if not correct
            base.Color = colorValue;

            RGBA rgba;
            rgba.r = colorValue.R / 255.0f;
            rgba.g = colorValue.G / 255.0f;
            rgba.b = colorValue.B / 255.0f;
            rgba.a = 1.0f;

            _hsv = CCControlUtils.HSVfromRGB(rgba);
            UpdateHueAndControlPicker();
        }

        public void SetEnabled(bool bEnabled)
        {
            base.Enabled = bEnabled;
            if (_huePicker != null)
            {
                _huePicker.Enabled = bEnabled;
            }
            if (_colourPicker != null)
            {
                _colourPicker.Enabled = bEnabled;
            }
        }

        public static CCControlColourPicker Create()
        {
            var pRet = new CCControlColourPicker();
            pRet.Init();
            return pRet;
        }


        public override bool Init()
        {
            if (base.Init())
            {
                TouchEnabled = true;
                // Cache the sprites
                CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFramesWithFile(
                    "extensions/CCControlColourPickerSpriteSheet.plist");

                // Create the sprite batch node
                var spriteSheet = new CCSpriteBatchNode("extensions/CCControlColourPickerSpriteSheet.png");
                AddChild(spriteSheet);

                // MIPMAP
//        ccTexParams params  = {GL_LINEAR_MIPMAP_LINEAR, GL_LINEAR, GL_REPEAT, GL_REPEAT};
                /* Comment next line to avoid something like mosaic in 'ControlExtensionTest',
		   especially the display of 'huePickerBackground.png' when in 800*480 window size with 480*320 design resolution and hd(960*640) resources.
	    */
//        spriteSheet->getTexture()->setAliasTexParameters();
//         spriteSheet->getTexture()->setTexParameters(&params);
//         spriteSheet->getTexture()->generateMipmap();

                // Init default color
                _hsv.h = 0;
                _hsv.s = 0;
                _hsv.v = 0;

                // Add image
                _background = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("menuColourPanelBackground.png",
                                                                               spriteSheet, CCPoint.Zero,
                                                                               new CCPoint(0.5f, 0.5f));

                CCPoint backgroundPointZero = _background.Position -
                                              new CCPoint(_background.ContentSize.Width / 2,
                                                          _background.ContentSize.Height / 2);

                // Setup panels
                float hueShift = 8;
                float colourShift = 28;

                _huePicker = new CCControlHuePicker();
                _huePicker.InitWithTargetAndPos(spriteSheet,
                                                new CCPoint(backgroundPointZero.X + hueShift,
                                                            backgroundPointZero.Y + hueShift));
                _colourPicker = new CCControlSaturationBrightnessPicker();
                _colourPicker.InitWithTargetAndPos(spriteSheet,
                                                   new CCPoint(backgroundPointZero.X + colourShift,
                                                               backgroundPointZero.Y + colourShift));

                // Setup events
                _huePicker.AddTargetWithActionForControlEvents(this, HueSliderValueChanged,
                                                               CCControlEvent.ValueChanged);
                _colourPicker.AddTargetWithActionForControlEvents(this, ColourSliderValueChanged,
                                                                  CCControlEvent.ValueChanged);

                // Set defaults
                UpdateHueAndControlPicker();
                AddChild(_huePicker);
                AddChild(_colourPicker);

                // Set content size
                ContentSize = _background.ContentSize;
                return true;
            }
            return false;
        }

        //virtual ~ControlColourPicker();
        public void HueSliderValueChanged(Object sender, CCControlEvent controlEvent)
        {
            _hsv.h = ((CCControlHuePicker) sender).Hue;

            // Update the value
            RGBA rgb = CCControlUtils.RGBfromHSV(_hsv);
            // XXX fixed me if not correct
            base.Color = new CCColor3B((byte) (rgb.r * 255.0f), (byte) (rgb.g * 255.0f), (byte) (rgb.b * 255.0f));

            // Send Control callback
            SendActionsForControlEvents(CCControlEvent.ValueChanged);
            UpdateControlPicker();
        }

        public void ColourSliderValueChanged(Object sender, CCControlEvent controlEvent)
        {
            _hsv.s = ((CCControlSaturationBrightnessPicker) sender).Saturation;
            _hsv.v = ((CCControlSaturationBrightnessPicker) sender).Brightness;


            // Update the value
            RGBA rgb = CCControlUtils.RGBfromHSV(_hsv);
            // XXX fixed me if not correct
            base.Color = new CCColor3B((byte) (rgb.r * 255.0f), (byte) (rgb.g * 255.0f), (byte) (rgb.b * 255.0f));

            // Send Control callback
            SendActionsForControlEvents(CCControlEvent.ValueChanged);
        }

        protected void UpdateControlPicker()
        {
            _huePicker.Hue = _hsv.h;
            _colourPicker.UpdateWithHSV(_hsv);
        }

        protected void UpdateHueAndControlPicker()
        {
            _huePicker.Hue = _hsv.h;
            _colourPicker.UpdateWithHSV(_hsv);
            _colourPicker.UpdateDraggerWithHSV(_hsv);
        }

        public override bool TouchBegan(CCTouch touch)
        {
            return false;
        }
    }
}