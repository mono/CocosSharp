using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace tests
{
    public class LabelFNTColorAndOpacity : AtlasDemoNew
    {
        float m_time;

		CCLabel label1, label2, label3;

        public LabelFNTColorAndOpacity()
        {
            m_time = 0;

            Color = new CCColor3B(128, 128, 128);
            Opacity = 255;

			label1 = new CCLabel("Label1", "fonts/bitmapFontTest2.fnt");

            // testing anchors
			label1.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

			var fade = new CCFadeOut  (1.0f);
			var fade_in = fade.Reverse();
			label1.RepeatForever ( fade, fade_in);


            // VERY IMPORTANT
            // color and opacity work OK because bitmapFontAltas2 loads a BMP image (not a PNG image)
            // If you want to use both opacity and color, it is recommended to use NON premultiplied images like BMP images
            // Of course, you can also tell XCode not to compress PNG images, but I think it doesn't work as expected
			label2 = new CCLabel("Label2", "fonts/bitmapFontTest2.fnt");
            // testing anchors
			label2.AnchorPoint = CCPoint.AnchorMiddle;
            label2.Color = CCColor3B.Red;
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

			label2.RepeatForever( new CCTintTo (1, 255, 0, 0), new CCTintTo (1, 0, 255, 0), new CCTintTo (1, 0, 0, 255));

			label3 = new CCLabel("Label3", "fonts/bitmapFontTest2.fnt");
            // testing anchors
			label3.AnchorPoint = CCPoint.AnchorUpperRight;
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            base.Schedule(step);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            label1.Position = visibleRect.LeftBottom();
            label2.Position = visibleRect.Center();
            label3.Position = visibleRect.RightTop();
		}


        public virtual void step(float dt)
        {
            m_time += dt;
            string stepString;
            stepString = string.Format("{0,2:f2} Test j", m_time);

            label1.Text = stepString;

            label2.Text = stepString;

            label3.Text = stepString;
        }

        public override string Title
        {
            get {
                return "New Label + .FNT file";
            }
        }

        public override string Subtitle
        {
            get {
                return "Testing opacity + tint";
            }
        }

    }

    public class LabelFNTFromHTTP : AtlasDemoNew
    {
        float m_time;

        CCLabel label1, label2, label3;

        LoadingLabel loading;

        public LabelFNTFromHTTP()
        {
            m_time = 0;

            Color = new CCColor3B(128, 128, 128);
            Opacity = 255;

            loading = new LoadingLabel();
            AddChild(loading);

            Schedule(step);

            ScheduleOnce((dt) =>
            {
                if (!loading.Visible)
                    loading.RemoveFromParent();
                else
                {
                    loading.StopAllActions();
                    loading.Scale = 1;
                    loading.Text = "Loading Timeout";    
                }
            },
            5  // delay 5 seconds

                );
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            loading.Position = visibleRect.Center;

            LoadLabel((fntFNT) =>
                {
                    
                    label1 = new CCLabel(fntFNT, "Label1", CCSize.Zero, CCLabelFormat.BitMapFont);
                    AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
                    //// testing anchors
                    label1.AnchorPoint = CCPoint.AnchorLowerLeft;
                    label1.Position = visibleRect.LeftBottom();

                    var fade = new CCFadeOut(1.0f);
                    var fade_in = fade.Reverse();
                    label1.RepeatForever(fade, fade_in);

                    // VERY IMPORTANT
                    // color and opacity work OK because bitmapFontAltas2 loads a BMP image (not a PNG image)
                    // If you want to use both opacity and color, it is recommended to use NON premultiplied images like BMP images
                    // Of course, you can also tell XCode not to compress PNG images, but I think it doesn't work as expected
                    label2 = new CCLabel(fntFNT, "Label2", CCSize.Zero, CCLabelFormat.BitMapFont);
                    // testing anchors
                    label2.AnchorPoint = CCPoint.AnchorMiddle;
                    label2.Color = CCColor3B.Red;
                    label2.Position = visibleRect.Center();
                    AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

                    label2.RepeatForever(new CCTintTo(1, 255, 0, 0), new CCTintTo(1, 0, 255, 0), new CCTintTo(1, 0, 0, 255));

                    label3 = new CCLabel(fntFNT, "Label3", CCSize.Zero, CCLabelFormat.BitMapFont);
                    // testing anchors
                    label3.AnchorPoint = CCPoint.AnchorUpperRight;
                    label3.Position = visibleRect.RightTop();
                    AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

                    loading.Visible = false;
                }
            );

        }

        Task<Stream> GetStreamAsync(string address)
        {

            //IWebProxy webProxy = WebRequest.DefaultWebProxy;
            //webProxy.Credentials = new NetworkCredential("domain\\user", "password");
            //WebRequest.DefaultWebProxy = webProxy;

            var httpClient = new HttpClient();
            return httpClient.GetStreamAsync(address);

        }

        async Task LoadLabel(Action<CCFontFNT> actionOnLoad )
        {
            try
            {
                using (var fntStream = await GetStreamAsync(@"https://rawgit.com/mono/CocosSharp/master/tests/testsContent/fonts/bitmapFontTest2.fnt"))
                {
                    using (var fntImageStream = await GetStreamAsync(@"https://rawgit.com/mono/CocosSharp/master/tests/testsContent/fonts/bitmapFontTest2.png"))
                    {
                        using (var reader = new StreamReader(fntStream, Encoding.UTF8))
                        {
                            string value = await reader.ReadToEndAsync();

                            try
                            {
                                var fnt = new CCFontFNT(value, fntImageStream, null);
                                actionOnLoad(fnt);
                            }
                            catch (Exception exc)
                            {
                                CCLog.Log(exc.Message);
                            }


                        }
                    }
                }
            }
            catch (Exception exc)
            {
                CCLog.Log("error loading: " + exc.Message);
            }

        }


        public virtual void step(float dt)
        {
            m_time += dt;
            string stepString;
            stepString = string.Format("{0,2:f2} Test j", m_time);

            if (label1 != null)
                label1.Text = stepString;

            if (label2 != null)
                label2.Text = stepString;

            if (label3 != null)
                label3.Text = stepString;
        }

        public override string Title
        {
            get
            {
                return "New Label + .FNT file";
            }
        }

        public override string Subtitle
        {
            get
            {
                return "Testing create from http";
            }
        }

    }

}
