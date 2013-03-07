using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class RenderTextureTestDepthStencil : RenderTextureTestDemo
    {
        public RenderTextureTestDepthStencil()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCSprite sprite = CCSprite.Create("Images/fire");
            sprite.Position = new CCPoint(s.Width * 0.25f, 0);
            sprite.Scale = 10;
#if IOS
            CCRenderTexture rend = CCRenderTexture.Create((int)s.Width, (int)s.Height, SurfaceFormat.Color, DepthFormat.Depth16, RenderTargetUsage.DiscardContents);
#else
            CCRenderTexture rend = CCRenderTexture.Create((int)s.Width, (int)s.Height, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, RenderTargetUsage.DiscardContents);
#endif

            rend.BeginWithClear(0, 0, 0, 0, 0);

            var save = DrawManager.DepthStencilState;

            DrawManager.DepthStencilState = new DepthStencilState()
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

            DrawManager.DepthStencilState = new DepthStencilState()
            {
                DepthBufferEnable = false,
                StencilEnable = true,
                StencilFunction = CompareFunction.NotEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1
            };
            // GL_SRC_ALPHA
#if IOS
            DrawManager.BlendFunc(new CCBlendFunc(ccMacros.CC_BLEND_SRC, ccMacros.CC_BLEND_DST));
            // OGLES.GL_SRC_ALPHA, OGLES.GL_ONE_MINUS_SRC_ALPHA));
#else
            DrawManager.BlendFunc(new CCBlendFunc(OGLES.GL_ONE, OGLES.GL_ONE_MINUS_SRC_ALPHA));
#endif
            
            //! move sprite half width and height, and draw only where not marked
            sprite.Position = sprite.Position + new CCPoint(sprite.ContentSize.Width * sprite.Scale, sprite.ContentSize.Height * sprite.Scale) * 0.5f;

            sprite.Visit();

            DrawManager.DepthStencilState = save;

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
