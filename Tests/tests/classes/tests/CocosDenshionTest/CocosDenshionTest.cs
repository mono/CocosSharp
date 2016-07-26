using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using CocosSharp;
using CocosDenshion;
using System.Diagnostics;

namespace tests
{
    public class CocosDenshionTest : CCLayer
    {
        static readonly string EFFECT_FILE = "Sounds/effect1";
        static readonly string MUSIC_FILE = "Sounds/background";

        int soundId;


        class Button : CCNode
        {

            CCNode child;

            public event TriggeredHandler Triggered;
            // A delegate type for hooking up button triggered events
            public delegate void TriggeredHandler(object sender,EventArgs e);


            private Button()
            {
                AttachListener();
            }

            public Button(CCSprite sprite)
                : this()
            {
                child = sprite;
                AddChild(sprite);
            }


            public Button(string text)
                : this()
            {
                child = new CCLabel(text, "fonts/arial", 16, CCLabelFormat.SpriteFont);
                AddChild(child);


            }

            void AttachListener()
            {
                // Register Touch Event
                var listener = new CCEventListenerTouchOneByOne();
                listener.IsSwallowTouches = true;

                listener.OnTouchBegan = OnTouchBegan;
                listener.OnTouchEnded = OnTouchEnded;
                listener.OnTouchCancelled = OnTouchCancelled;

                AddEventListener(listener, this);
            }

            bool touchHits(CCTouch  touch)
            {
                var location = touch.Location;
                var area = child.BoundingBox;
                return area.ContainsPoint(child.WorldToParentspace(location));
            }

            bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
            {
                bool hits = touchHits(touch);
                if (hits)
                    scaleButtonTo(0.9f);

                return hits;
            }

            void OnTouchEnded(CCTouch  touch, CCEvent  touchEvent)
            {
                bool hits = touchHits(touch);
                if (hits && Triggered != null)
                    Triggered(this, EventArgs.Empty);
                scaleButtonTo(1);
            }

            void OnTouchCancelled(CCTouch touch, CCEvent  touchEvent)
            {
                scaleButtonTo(1);
            }

            void scaleButtonTo(float scale)
            {
                var action = new CCScaleTo(0.1f, scale);
                action.Tag = 900;
                StopAction(900);
                RunAction(action);
            }
        }

        class AudioSlider : CCNode
        {

            CCControlSlider slider;
            CCLabel lblMinValue, lblMaxValue;
            Direction direction;

            public enum Direction
            {
                Vertical,
                Horizontal
            }

            public AudioSlider(Direction direction = Direction.Horizontal)
            {
                slider = new CCControlSlider("extensions/sliderTrack.png", "extensions/sliderProgress.png", "extensions/sliderThumb.png");
                slider.Scale = 0.5f;
                this.direction = direction;
                if (direction == Direction.Vertical)
                    slider.Rotation = -90.0f;
                AddChild(slider);
                ContentSize = slider.ScaledContentSize;
            }

            public float Value
            {
                get { return slider.Value; }

                set
                {
                    SetValue(slider.MinimumValue, slider.MaximumValue, value);
                }
            }

            public void SetValue(float minValue, float maxValue, float value)
            {
                slider.MinimumValue = minValue;
                slider.MaximumValue = maxValue;
                slider.Value = value;

                var valueText = string.Format("{0,2:f2}", minValue);
                if (lblMinValue == null)
                {
                    lblMinValue = new CCLabel(valueText, "fonts/arial", 8, CCLabelFormat.SpriteFont) { AnchorPoint = CCPoint.AnchorMiddleLeft };
                    AddChild(lblMinValue);

                    if (direction == Direction.Vertical)
                        lblMinValue.Position = new CCPoint(0, slider.ScaledContentSize.Height);
                    else
                        lblMinValue.Position = new CCPoint(0, slider.ScaledContentSize.Height * 1.5f);
                }
                else
                    lblMinValue.Text = valueText;

                valueText = string.Format("{0,2:f2}", maxValue);
                if (lblMaxValue == null)
                {
                    lblMaxValue = new CCLabel(valueText, "fonts/arial", 8, CCLabelFormat.SpriteFont) { AnchorPoint = CCPoint.AnchorMiddleRight };
                    AddChild(lblMaxValue);

                    if (direction == Direction.Vertical)
                    {
                        lblMaxValue.Position = new CCPoint(slider.ScaledContentSize.Height * 1.75f, slider.ScaledContentSize.Width);
                        AnchorPoint = CCPoint.AnchorMiddleLeft;
                    }
                    else
                        lblMaxValue.Position = new CCPoint(slider.ScaledContentSize.Width, slider.ScaledContentSize.Height * 1.5f);
                }
                else
                    lblMaxValue.Text = valueText;
            }

        }

        float MusicVolume { get; set; }
        float EffectsVolume { get; set; }

        #region Constructors

        public CocosDenshionTest()
        {

            MusicVolume = 1;
            EffectsVolume = 1;


            Schedule( (dt) => 

                {
                    var musicVolume = sliderMusicVolume.Value;
                    if ((float)Math.Abs(musicVolume - MusicVolume) > 0.001) {
                        MusicVolume = musicVolume;
                        CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = musicVolume;
                    }

                    var effectsVolume = sliderEffectsVolume.Value;
                    if ((float)Math.Abs(effectsVolume - EffectsVolume) > 0.001) {
                        EffectsVolume = effectsVolume;
                        CCSimpleAudioEngine.SharedEngine.EffectsVolume = effectsVolume;
                    }


                }
            
            
            
            
            );

            // preload background music and effect
            CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic(MUSIC_FILE);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(EFFECT_FILE);

            // set default volume
            CCSimpleAudioEngine.SharedEngine.EffectsVolume = 0.5f;
            CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = 0.5f;



        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 

            AddButtons();
            AddSliders();
        }

        void AddButtons()
        {

            var audio = CCSimpleAudioEngine.SharedEngine;

            var lblMusic = new CCLabel("Control Music", "fonts/arial", 24, CCLabelFormat.SpriteFont);
            AddChildAt(lblMusic, 0.25f, 0.9f);

            var btnPlay = new Button("play");
            btnPlay.Triggered += (sender, e) =>
            {
                audio.BackgroundMusicVolume = sliderMusicVolume.Value;
                audio.PlayBackgroundMusic(MUSIC_FILE, true);
            };
            AddChildAt(btnPlay, 0.1f, 0.75f);

            var btnStop = new Button("stop");
            btnStop.Triggered += (sender, e) =>
            {
                audio.StopBackgroundMusic();
            };
            
            AddChildAt(btnStop, 0.25f, 0.75f);

            var btnRewindMusic = new Button("rewind");
            btnRewindMusic.Triggered += (sender, e) =>
            {
                audio.RewindBackgroundMusic();
            };
            
            AddChildAt(btnRewindMusic, 0.4f, 0.75f);

            var btnPause = new Button("pause");
            btnPause.Triggered += (sender, e) =>
            {
                audio.PauseBackgroundMusic();
            };
            AddChildAt(btnPause, 0.1f, 0.65f);

            var btnResumeMusic = new Button("resume");
            btnResumeMusic.Triggered += (sender, e) =>
            {
                audio.ResumeBackgroundMusic();
            };
            AddChildAt(btnResumeMusic, 0.25f, 0.65f);

            var btnIsPlayingMusic = new Button("is playing");
            btnIsPlayingMusic.Triggered += (sender, e) =>
            {
                if (audio.BackgroundMusicPlaying)
                    CCLog.Log("background music is playing");
                else
                    CCLog.Log("background music is not playing");
            };
            AddChildAt(btnIsPlayingMusic, 0.4f, 0.65f);

            var lblSound = new CCLabel("Control Effects", "fonts/arial", 24, CCLabelFormat.SpriteFont);
            AddChildAt(lblSound, 0.75f, 0.9f);

            var btnPlayEffect = new Button("play");
            btnPlayEffect.Triggered += (sender, e) =>
            {
                var pitch = sliderPitch.Value;
                var pan = sliderPan.Value;
                var gain = sliderGain.Value;
                soundId = audio.PlayEffect(EFFECT_FILE, false);//, pitch, pan, gain);
            };
            AddChildAt(btnPlayEffect, 0.6f, 0.8f);

            var btnPlayEffectInLoop = new Button("play in loop");
            btnPlayEffectInLoop.Triggered += (sender, e) =>
            {
                var pitch = sliderPitch.Value;
                var pan = sliderPan.Value;
                var gain = sliderGain.Value;
                soundId = audio.PlayEffect(EFFECT_FILE, true);//, pitch, pan, gain);
            };
            AddChildAt(btnPlayEffectInLoop, 0.75f, 0.8f);

            var btnStopEffect = new Button("stop");
            btnStopEffect.Triggered += (sender, e) =>
            {
                audio.StopEffect(soundId);
            };
            AddChildAt(btnStopEffect, 0.9f, 0.8f);

            var btnUnloadEffect = new Button("unload");
            btnUnloadEffect.Triggered += (sender, e) =>
            {
                audio.UnloadEffect(EFFECT_FILE);
            };
            AddChildAt(btnUnloadEffect, 0.6f, 0.7f);

            var btnPauseEffect = new Button("pause");
            btnPauseEffect.Triggered += (sender, e) =>
            {
                audio.PauseEffect(soundId);
            };
            AddChildAt(btnPauseEffect, 0.75f, 0.7f);

            var btnResumeEffect = new Button("resume");
            btnResumeEffect.Triggered += (sender, e) =>
            {
                audio.ResumeEffect(soundId);
            };
            AddChildAt(btnResumeEffect, 0.9f, 0.7f);

            var btnPauseAll = new Button("pause all");
            btnPauseAll.Triggered += (sender, e) =>
            {
                audio.PauseAllEffects();
            };
            AddChildAt(btnPauseAll, 0.6f, 0.6f);

            var btnResumeAll = new Button("resume all");
            btnResumeAll.Triggered += (sender, e) =>
            {
                audio.ResumeAllEffects();
            };
            AddChildAt(btnResumeAll, 0.75f, 0.6f);

            var btnStopAll = new Button("stop all");
            btnStopAll.Triggered += (sender, e) =>
            {
                audio.StopAllEffects();
            };
            AddChildAt(btnStopAll, 0.9f, 0.6f);

            var effectsIsPlaying = new Button("Effects playing");
            effectsIsPlaying.Triggered += (sender, e) =>
            {
                if (audio.EffectPlaying(soundId))
                    CCLog.Log("sound effect is playing");
                else
                    CCLog.Log("sound effect is not playing");
            };
            AddChildAt(effectsIsPlaying, 0.7f, 0.4f);
        }

        AudioSlider sliderPitch, sliderMusicVolume, sliderEffectsVolume, sliderPan, sliderGain;

        void AddSliders()
        {
//            var lblPitch = new CCLabel("Pitch", "fonts/arial", 14, CCLabelFormat.SpriteFont);
//            AddChildAt(lblPitch, 0.67f, 0.4f);
//
            sliderPitch = new AudioSlider(AudioSlider.Direction.Horizontal);
            sliderPitch.SetValue(0.5f, 2, 1);
//            AddChildAt(sliderPitch, 0.72f, 0.39f);

//            var lblPan = new CCLabel("Pan", "fonts/arial", 14, CCLabelFormat.SpriteFont);
//            AddChildAt(lblPan, 0.67f, 0.3f);
            sliderPan = new AudioSlider();
            sliderPan.SetValue(-1, 1, 0);
//            AddChildAt(sliderPan, 0.72f, 0.29f);
//
//            var lblGain = new CCLabel("Gain", "fonts/arial", 14, CCLabelFormat.SpriteFont);
//            AddChildAt(lblGain, 0.67f, 0.2f);
            sliderGain = new AudioSlider();
            sliderGain.SetValue(0, 1, 1);
//            AddChildAt(sliderGain, 0.72f, 0.19f);

            var lblEffectsVolume = new CCLabel("Effects Volume", "fonts/arial", 14, CCLabelFormat.SpriteFont);
            AddChildAt(lblEffectsVolume, 0.62f, 0.5f);
            sliderEffectsVolume = new AudioSlider();
            sliderEffectsVolume.SetValue(0, 1, CCSimpleAudioEngine.SharedEngine.EffectsVolume);
            AddChildAt(sliderEffectsVolume, 0.71f, 0.49f);

            var lblMusicVolume = new CCLabel("Music Volume", "fonts/arial", 14, CCLabelFormat.SpriteFont);
            AddChildAt(lblMusicVolume, 0.12f, 0.5f);

            sliderMusicVolume = new AudioSlider();
            sliderMusicVolume.SetValue(0, 1, CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume);
            AddChildAt(sliderMusicVolume, 0.21f, 0.49f);
        }

        void AddChildAt(CCNode node, float percentageX, float percentageY)
        {
            var size = VisibleBoundsWorldspace.Size;
            node.PositionX = percentageX * size.Width;
            node.PositionY = percentageY * size.Height;
            AddChild(node);
        }

        #endregion Setup content

        public override void OnExit()
        {
            base.OnExit();

            CCSimpleAudioEngine.SharedEngine.End();
        }

    }


    public class CocosDenshionTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer layer = new CocosDenshionTest();
            AddChild(layer);

            Director.ReplaceScene(this);
        }
    }
}