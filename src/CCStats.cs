using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
#if IOS
using MonoTouch.ObjCRuntime;
#endif

namespace CocosSharp
{
    /// <summary>
    /// Modified from http://stackoverflow.com/questions/10889743/cocos2d-2-0-3-numbers-on-the-bottom-left
    /// by Steffen Itterheim:
    /// 
    /// 5623459 -- memory consumption in bytes
    /// 3       -- garbage collection counter (always 0 when in iOS simulator, see below)
    /// 082     -- number of draw calls
    /// 0.023   -- time it took to update the frame
    /// 0.016   -- time it took to draw the frame
    /// 60.0    -- frames per second
    /// 
    /// The Draw Call number is the number of draw calls (which is fairly high). Typically each node that renders
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
    /// seconds in order to achieve 60 frames per second (1/60 = 0,0166...) this number can tell you how close your game
    /// is to dropping below 60 fps. Yours is pretty close, you have practically no room left for additional game logic
    /// or visuals before the framerate will drop below 60 fps.
    /// 
    /// The last number is the number of frames per second. This value, like the previous one, is averaged over several
    /// frames so that it doesn't fluctuate as much (makes it hard to read).
    /// 
    /// PS: one other thing to keep in mind is that the bottom two values become misleading can not be compared for
    /// framerates below 15 fps. For example cocos2d might show 0.0 for the time it took to render a frame at such
    /// a low framerate.
    /// 
    /// There is a special case for Xamarin iOS monotouch on emulator where they aggresively call 
    /// garbage collection themselves on the simulator. This should not affect the devices though.
    /// So we check if we are running on a Device and only update the counters if we are.
    /// </summary>
    public class CCStats
    {
		bool isInitialized, isEnabled;
		bool isCheckGC = true;
		uint totalFrames = 0;
		uint updateCount;
		uint totalDrawCount;
		int gcCounter;
        float deltaAll = 0.0f;
        float totalDrawTime;
        float totalUpdateTime;
        float startTime;

        Stopwatch stopwatch;

        WeakReference gcWeakRef = new WeakReference(new object());

        CCLabelAtlas fpsLabel;
        CCLabelAtlas updateTimeLabel;
        CCLabelAtlas drawTimeLabel;
        CCLabelAtlas drawCallLabel;
        CCLabelAtlas memoryLabel;
        CCLabelAtlas gcLabel;


		#region Properties

        public bool IsInitialized 
		{
            get { return isInitialized; }
        }
			
        public bool IsEnabled 
		{
            get { return isEnabled; }
            set 
			{
                isEnabled = value;

                if (value) 
				{
                    stopwatch.Reset();
                    stopwatch.Start();
                }
            }
        }

		#endregion Properties


        // Initialize the stats display.
        public void Initialize()
        {
            if (!isInitialized) 
			{
                // There is a special case for Xamarin iOS monotouch on emulator where they aggresively call 
                // garbage collection themselves on the simulator. This should not affect the devices though.
                // So we check if we are running on a Device and only update the counters if we are.
                #if IOS
                if (Runtime.Arch != Arch.DEVICE)
                    isCheckGC = false;
                #endif

                CCTexture2D texture;
                CCTextureCache textureCache = CCTextureCache.Instance;

                stopwatch = new Stopwatch();

                try {
                    texture = !textureCache.Contains ("cc_fps_images") ? textureCache.AddImage (CCFPSImage.PngData, "cc_fps_images", CCSurfaceFormat.Bgra4444) : textureCache["cc_fps_images"];

                    if (texture == null || (texture.ContentSize.Width == 0 && texture.ContentSize.Height == 0)) {
                        CCLog.Log ("CCStats: Failed to create stats texture");

                        return;
                    }
                } catch (Exception ex) {
                    // MonoGame may not allow texture.fromstream,
                    // so catch this exception here and disable the stats
                    CCLog.Log ("CCStats: Failed to create stats texture:");
					if(ex!=null)
						CCLog.Log (ex.ToString ());

                    return;
                }

                try {
                    texture.IsAntialiased = false; // disable antialiasing so the labels are always sharp

                    fpsLabel = new CCLabelAtlas ("00.0", texture, 4, 8, '.');
                    fpsLabel.IgnoreContentScaleFactor = true;

                    updateTimeLabel = new CCLabelAtlas ("0.000", texture, 4, 8, '.');
                    updateTimeLabel.IgnoreContentScaleFactor = true;

                    drawTimeLabel = new CCLabelAtlas ("0.000", texture, 4, 8, '.');
                    drawTimeLabel.IgnoreContentScaleFactor = true;

                    drawCallLabel = new CCLabelAtlas ("000", texture, 4, 8, '.');
                    drawCallLabel.IgnoreContentScaleFactor = true;

                    memoryLabel = new CCLabelAtlas ("0", texture, 4, 8, '.');
                    memoryLabel.IgnoreContentScaleFactor = true;
                    memoryLabel.Color = new CCColor3B (35, 185, 255);

                    gcLabel = new CCLabelAtlas ("0", texture, 4, 8, '.');
                    gcLabel.IgnoreContentScaleFactor = true;
                    gcLabel.Color = new CCColor3B (255, 196, 54);
                } catch (Exception ex) {
                    CCLog.Log ("CCStats: Failed to create stats labels:");
					if(ex !=null)
						CCLog.Log (ex.ToString ());

                    return;
                }
            }

			var factor = CCDirector.SharedDirector.ContentScaleFactor;
			var pos = CCDirector.SharedDirector.VisibleOrigin;

            fpsLabel.Scale = factor;
            updateTimeLabel.Scale = factor;
            drawTimeLabel.Scale = factor;
            drawCallLabel.Scale = factor;
            memoryLabel.Scale = factor;
            gcLabel.Scale = factor;

            memoryLabel.Position = new CCPoint (4 * factor, 44 * factor) + pos;
            gcLabel.Position = new CCPoint (4 * factor, 36 * factor) + pos;
            drawCallLabel.Position = new CCPoint (4 * factor, 28 * factor) + pos;
            updateTimeLabel.Position = new CCPoint (4 * factor, 20 * factor) + pos;
            drawTimeLabel.Position = new CCPoint (4 * factor, 12 * factor) + pos;
            fpsLabel.Position = new CCPoint (4 * factor, 4 * factor) + pos;

            isInitialized = true;
        }

        public void UpdateStart ()
        {
            if (isEnabled)
                startTime = (float)stopwatch.Elapsed.TotalMilliseconds;
        }

        public void UpdateEnd (float delta)
        {
            if (isEnabled) {
                deltaAll += delta;

                if (isEnabled) {
                    updateCount++;
                    totalUpdateTime += (float)stopwatch.Elapsed.TotalMilliseconds - startTime;
                }
            }
        }

        public void Draw ()
        {
            if (isEnabled) {
                totalFrames++;
                totalDrawCount++;
                totalDrawTime += (float)stopwatch.Elapsed.TotalMilliseconds - startTime;

                if (isCheckGC && !gcWeakRef.IsAlive) {
                    gcCounter++;
                    gcWeakRef = new WeakReference (new object ());
                }

                if (isInitialized) {
                    if (deltaAll > CCMacros.CCDirectorStatsUpdateIntervalInSeconds) {
                        fpsLabel.Text = (String.Format ("{0:00.0}", totalDrawCount / deltaAll));
                        updateTimeLabel.Text = (String.Format ("{0:0.000}", totalUpdateTime / updateCount));
                        drawTimeLabel.Text = (String.Format ("{0:0.000}", totalDrawTime / totalDrawCount));
                        drawCallLabel.Text = (String.Format ("{0:000}", CCDrawManager.DrawCount));
                        
                        deltaAll = totalDrawTime = totalUpdateTime = 0;
                        totalDrawCount = updateCount = 0;
                        
                        memoryLabel.Text = String.Format ("{0}", GC.GetTotalMemory (false));
                        gcLabel.Text = String.Format ("{0}", gcCounter);
                    }

                    drawCallLabel.Visit ();
                    fpsLabel.Visit ();
                    updateTimeLabel.Visit ();
                    drawTimeLabel.Visit ();
                    memoryLabel.Visit ();
                    gcLabel.Visit ();
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
