using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class RenderTextureTestDepthStencil : RenderTextureTestDemo
    {
        #region Properties

        public override string Title
        {
            get { return "Testing depthStencil attachment"; }
        }

        public override string Subtitle
        {
            get { return "Circle should be missing 1/4 of its region"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CCSprite sprite = new CCSprite("Images/fire");
            sprite.Position = new CCPoint(windowSize.Width * 0.25f, 0);
            sprite.Scale = 10;

            CCRenderTexture rend = new CCRenderTexture(windowSize, windowSize,
                CCSurfaceFormat.Color, 
                CCDepthFormat.Depth24Stencil8, CCRenderTargetUsage.DiscardContents);

            rend.BeginWithClear(0, 0, 0, 0, 0);

//            var save = CCDrawManager.SharedDrawManager.DepthStencilState;
//
//            CCDrawManager.SharedDrawManager.DepthStencilState = new DepthStencilState()
//                {
//                    ReferenceStencil = 1,
//
//                    DepthBufferEnable = false,
//                    StencilEnable = true,
//                    StencilFunction = CompareFunction.Always,
//                    StencilPass = StencilOperation.Replace,
//                    
//                    TwoSidedStencilMode = true,
//                    CounterClockwiseStencilFunction = CompareFunction.Always,
//                    CounterClockwiseStencilPass = StencilOperation.Replace,
//                };

            sprite.Visit();

//            CCDrawManager.SharedDrawManager.DepthStencilState = new DepthStencilState()
//            {
//                DepthBufferEnable = false,
//                StencilEnable = true,
//                StencilFunction = CompareFunction.NotEqual,
//                StencilPass = StencilOperation.Keep,
//                ReferenceStencil = 1
//            };

//            CCDrawManager.SharedDrawManager.BlendFunc(new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ONE_MINUS_SRC_ALPHA));

            sprite.Position = sprite.Position 
                + new CCPoint(sprite.ContentSize.Width * sprite.ScaleX, sprite.ContentSize.Height * sprite.ScaleY) * 0.5f;

            sprite.Visit();

//            CCDrawManager.SharedDrawManager.DepthStencilState = save;

            rend.End();

            rend.Position = new CCPoint(windowSize.Width * 0.5f, windowSize.Height * 0.5f);

            AddChild(rend);
        }

        #endregion Setup content
    }
}
