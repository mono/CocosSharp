using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace CocosSharp
{


    /// <summary>
    /// From http://stackoverflow.com/questions/10889743/cocos2d-2-0-3-numbers-on-the-bottom-left by Steffen Itterheim:
    /// 
    /// 82    -- number of draw calls
    /// 0.016 -- time it took to render the frame
    /// 60.0  -- frames per second
    /// 
    /// The first number (82) is the number of draw calls (which is fairly high). Typically each node that renders
    /// something on the screen (sprites, labels, particle fx, etc) increases that number by one. If you use
    /// a CCSpriteBatchNode and add 100 sprites to it, it will increase the draw call only by 1.
    /// 
    /// 82 is a pretty high draw call number - depending on the game's complexity and assuming it is well optimized to
    /// reduce draw calls, the number of draw calls should be around 10 to 40. Assuming all 82 draw calls are sprites,
    /// then creating a texture atlas out of the sprite images (use TexturePacker, Zwoptex, SpriteHelper) in order to
    /// use CCSpriteBatchNode you could reduce the number of draw calls to 1. Draw calls are expensive, so it is very
    /// important to keep that number down.
    /// 
    /// The time it took to render a frame is in milliseconds. Since you need to draw a new frame every 0.016666666
    /// seconds in order to achieve 60 frames per second (1/60 = 0,0166…) this number can tell you how close your game
    /// is to dropping below 60 fps. Yours is pretty close, you have practically no room left for additional game logic
    /// or visuals before the framerate will drop below 60 fps.
    /// 
    /// The last number is the number of frames per second. This value, like the previous one, is averaged over several
    /// frames so that it doesn't fluctuate as much (makes it hard to read).
    /// 
    /// PS: one other thing to keep in mind is that the bottom two values become misleading can not be compared for
    /// framerates below 15 fps. For example cocos2d might show 0.0 for the time it took to render a frame at such
    /// a low framerate.
    /// </summary>
    public class CCStats
    {


        int _GCCount;
        bool m_bEnabled;
        uint m_uTotalFrames = 0;
        float m_fAccumDt = 0.0f;
        uint m_uUpdateCount;
        float m_fAccumDraw;
        uint m_uDrawCount;
        float m_fAccumUpdate;
        float startTime;

        Stopwatch m_pStopwatch;

        WeakReference _wk = new WeakReference(new object());

        CCLabelAtlas m_pFPSLabel;
        CCLabelAtlas m_pUpdateTimeLabel;
        CCLabelAtlas m_pDrawTimeLabel;
        CCLabelAtlas m_pDrawsLabel;
        CCLabelAtlas m_pMemoryLabel;
        CCLabelAtlas m_pGCLabel;


        public void Prepare ()
        {
            if (m_pFPSLabel == null) {
                CCTexture2D texture;
                CCTextureCache textureCache = CCTextureCache.SharedTextureCache;

                m_pStopwatch = new Stopwatch();

                try {
                    texture = !textureCache.Contains ("cc_fps_images") ? textureCache.AddImage (CCFPSImage.PngData, "cc_fps_images", SurfaceFormat.Bgra4444) : textureCache.TextureForKey ("cc_fps_images");

                    if (texture == null || (texture.ContentSize.Width == 0 && texture.ContentSize.Height == 0)) {
                        m_bEnabled = false;
                        return;
                    }
                } catch (Exception) {
                    // MonoGame may not allow texture.fromstream, so catch this exception here
                    // and disable the stats
                    m_bEnabled = false;
                    return;
                }

                try {
                    texture.IsAntialiased = false; // disable antialiasing so the labels are always sharp

                    m_pFPSLabel = new CCLabelAtlas ("00.0", texture, 4, 8, '.');
                    m_pFPSLabel.IgnoreContentScaleFactor = false;

                    m_pUpdateTimeLabel = new CCLabelAtlas ("0.000", texture, 4, 8, '.');
                    m_pUpdateTimeLabel.IgnoreContentScaleFactor = false;

                    m_pDrawTimeLabel = new CCLabelAtlas ("0.000", texture, 4, 8, '.');
                    m_pDrawTimeLabel.IgnoreContentScaleFactor = false;

                    m_pDrawsLabel = new CCLabelAtlas ("000", texture, 4, 8, '.');
                    m_pDrawsLabel.IgnoreContentScaleFactor = false;

                    m_pMemoryLabel = new CCLabelAtlas ("0", texture, 4, 8, '.');
                    m_pMemoryLabel.IgnoreContentScaleFactor = false;
                    m_pMemoryLabel.Color = new CCColor3B (35, 185, 255);

                    m_pGCLabel = new CCLabelAtlas ("0", texture, 4, 8, '.');
                    m_pGCLabel.IgnoreContentScaleFactor = false;
                    m_pGCLabel.Color = new CCColor3B (255, 196, 54);
                } catch (Exception ex) {
                    m_pFPSLabel = null;
                    m_bEnabled = false;

                    CCLog.Log ("Failed to create the stats labels.");
                    CCLog.Log (ex.ToString ());

                    return;
                }
            }

            const float factor = 2.0f;
            var pos = CCDirector.SharedDirector.VisibleOrigin;

            m_pFPSLabel.Scale = factor;
            m_pUpdateTimeLabel.Scale = factor;
            m_pDrawTimeLabel.Scale = factor;
            m_pDrawsLabel.Scale = factor;
            m_pMemoryLabel.Scale = factor;
            m_pGCLabel.Scale = factor;

            m_pMemoryLabel.Position = new CCPoint (2 * factor, 31 * factor) + pos;
            m_pGCLabel.Position = new CCPoint (2 * factor, 25 * factor) + pos;
            m_pDrawsLabel.Position = new CCPoint (2 * factor, 19 * factor) + pos;
            m_pUpdateTimeLabel.Position = new CCPoint (2 * factor, 13 * factor) + pos;
            m_pDrawTimeLabel.Position = new CCPoint (2 * factor, 7 * factor) + pos;
            m_pFPSLabel.Position = new CCPoint (2 * factor, 1 * factor) + pos;
        }


        public bool Enabled {
            get { return m_bEnabled; }
            set {
                m_bEnabled = value;

                if (value) {
                    m_pStopwatch.Reset ();
                    m_pStopwatch.Start ();
                }
            }
        }


        public void UpdateStart ()
        {
            if (m_bEnabled)
                startTime = (float)m_pStopwatch.Elapsed.TotalMilliseconds;
        }


        public void UpdateEnd (float delta)
        {
            if (m_bEnabled) {
                m_fAccumDt += delta;

                if (m_bEnabled) {
                    m_uUpdateCount++;
                    m_fAccumUpdate += (float)m_pStopwatch.Elapsed.TotalMilliseconds - startTime;
                }
            }
        }


        public void Draw ()
        {
            if (m_bEnabled) {
                m_uTotalFrames++;
                m_uDrawCount++;
                m_fAccumDraw += (float)m_pStopwatch.Elapsed.TotalMilliseconds - startTime;

                if (!_wk.IsAlive) {
                    _GCCount++;
                    _wk = new WeakReference (new object ());
                }

                if (m_pFPSLabel != null && m_pUpdateTimeLabel != null && m_pDrawsLabel != null) {
                    if (m_fAccumDt > CCMacros.CCDirectorStatsUpdateIntervalInSeconds) {
                        m_pFPSLabel.Text = (String.Format ("{0:00.0}", m_uDrawCount / m_fAccumDt));

                        m_pUpdateTimeLabel.Text = (String.Format ("{0:0.000}", m_fAccumUpdate / m_uUpdateCount));
                        m_pDrawTimeLabel.Text = (String.Format ("{0:0.000}", m_fAccumDraw / m_uDrawCount));
                        m_pDrawsLabel.Text = (String.Format ("{0:000}", CCDrawManager.DrawCount));

                        m_fAccumDt = m_fAccumDraw = m_fAccumUpdate = 0;
                        m_uDrawCount = m_uUpdateCount = 0;

                        m_pMemoryLabel.Text = String.Format ("{0}", GC.GetTotalMemory (false));
                        m_pGCLabel.Text = String.Format ("{0}", _GCCount);
                    }

                    m_pDrawsLabel.Visit ();
                    m_pFPSLabel.Visit ();
                    m_pUpdateTimeLabel.Visit ();
                    m_pDrawTimeLabel.Visit ();
                    m_pMemoryLabel.Visit ();
                    m_pGCLabel.Visit ();
                }
            }    
        }


    }


    public static class CCFPSImage
    {


        public static byte[] PngData = {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52, 0x00, 0x00,
                0x00, 0x40, 0x00, 0x00, 0x00, 0x08, 0x08, 0x04, 0x00, 0x00, 0x00, 0xE3, 0x6B, 0x31, 0x54, 0x00, 0x00, 0x00,
                0xA4, 0x49, 0x44, 0x41, 0x54, 0x78, 0x01, 0xED, 0x8F, 0x31, 0xAE, 0xC6, 0x20, 0x0C, 0x83, 0x7D, 0x8F, 0xEC,
                0xB9, 0xBF, 0x72, 0xA1, 0x8E, 0x9D, 0x19, 0xFF, 0xC9, 0x0F, 0x03, 0x96, 0x5A, 0x16, 0x7A, 0x80, 0x97, 0xC8,
                0xF9, 0x64, 0xE3, 0x05, 0x3C, 0xE7, 0x7F, 0x48, 0x82, 0x45, 0x76, 0x61, 0x5D, 0xD1, 0xDE, 0xB4, 0xE8, 0xFE,
                0xA0, 0xDF, 0x6B, 0xE3, 0x61, 0x5F, 0xE6, 0x47, 0x64, 0x57, 0x48, 0xC0, 0xB8, 0x9A, 0x91, 0x99, 0xEA, 0x58,
                0x5A, 0x65, 0xA2, 0xDF, 0x77, 0xE1, 0xFB, 0x34, 0x36, 0x20, 0x1B, 0xA7, 0x80, 0x75, 0xAB, 0xE7, 0x35, 0xBD,
                0xD8, 0xA8, 0x5D, 0xAC, 0x06, 0x53, 0xAB, 0xDC, 0x7D, 0x51, 0x1E, 0x5F, 0xE7, 0xE6, 0x0D, 0xA4, 0x88, 0x94,
                0x00, 0x5F, 0x44, 0x67, 0xCC, 0x8E, 0xE8, 0xF7, 0xC1, 0x58, 0x7D, 0xBF, 0x3B, 0x93, 0x17, 0x13, 0xA7, 0xB9,
                0x78, 0x61, 0xEC, 0x2A, 0x5F, 0xD5, 0x93, 0x42, 0xF6, 0xAB, 0xC5, 0x8B, 0x35, 0x3B, 0xEA, 0xDA, 0xBB, 0xEF,
                0xDC, 0xFE, 0xD9, 0x3F, 0x4D, 0x6E, 0xF4, 0x8F, 0x53, 0xBB, 0x31, 0x1E, 0x5D, 0xFB, 0x78, 0xE5, 0xF6, 0x7E,
                0x3F, 0xCC, 0x1F, 0xBE, 0x5D, 0x06, 0xEE, 0x39, 0x9F, 0xA8, 0xC7, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E,
                0x44, 0xAE, 0x42, 0x60, 0x82
            };


    }


}