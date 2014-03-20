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
        public RenderTextureTestDepthStencil()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite = new CCSprite("Images/fire");
            sprite.Position = new CCPoint(s.Width * 0.25f, 0);
            sprite.Scale = 10;

			CCRenderTexture rend = new CCRenderTexture((int)s.Width, (int)s.Height, CCSurfaceFormat.Color, DepthFormat.Depth24Stencil8, RenderTargetUsage.DiscardContents);

            rend.BeginWithClear(0, 0, 0, 0, 0);

            var save = CCDrawManager.DepthStencilState;

            CCDrawManager.DepthStencilState = new DepthStencilState()
                {
                    ReferenceStencil = 1,

                    DepthBufferEnable = false,
                    StencilEnable = true,
                    StencilFunction = CompareFunction.Always,
                    StencilPass = StencilOperation.Replace,
                    
                    TwoSidedStencilMode = true,
                    CounterClockwiseStencilFunction = CompareFunction.Always,
                    CounterClockwiseStencilPass = StencilOperation.Replace,
                };

            sprite.Visit();

            CCDrawManager.DepthStencilState = new DepthStencilState()
            {
                DepthBufferEnable = false,
                StencilEnable = true,
                StencilFunction = CompareFunction.NotEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1
            };
            // GL_SRC_ALPHA

            CCDrawManager.BlendFunc(new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ONE_MINUS_SRC_ALPHA));

            //! move sprite half width and height, and draw only where not marked
            sprite.Position = sprite.Position + new CCPoint(sprite.ContentSize.Width * sprite.Scale, sprite.ContentSize.Height * sprite.Scale) * 0.5f;

            sprite.Visit();

            CCDrawManager.DepthStencilState = save;

            rend.End();


            rend.Position = new CCPoint(s.Width * 0.5f, s.Height * 0.5f);

            AddChild(rend);
        }

        public override string title()
        {
            return "Testing depthStencil attachment";
        }

        public override string subtitle()
        {
            return "Circle should be missing 1/4 of its region";
        }
    }
}
