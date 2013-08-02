using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace Cocos2D.CCBReader
{
    public static class CCBHelper
    {
        public static CCPoint GetAbsolutePosition(CCPoint pt, PositionType nType, CCSize containerSize, string pPropName)
        {
            CCPoint absPt = new CCPoint(0, 0);
            
            if (nType == PositionType.RelativeBottomLeft)
            {
                absPt = pt;
            }
            else if (nType == PositionType.RelativeTopLeft)
            {
                absPt.X = pt.X;
                absPt.Y = containerSize.Height - pt.Y;
            }
            else if (nType == PositionType.RelativeTopRight)
            {
                absPt.X = containerSize.Width - pt.X;
                absPt.Y = containerSize.Height - pt.Y;
            }
            else if (nType == PositionType.RelativeBottomRight)
            {
                absPt.X = containerSize.Width - pt.X;
                absPt.Y = pt.Y;
            }
            else if (nType == PositionType.Percent)
            {
                absPt.X = (int) (containerSize.Width * pt.X / 100.0f);
                absPt.Y = (int) (containerSize.Height * pt.Y / 100.0f);
            }
            else if (nType == PositionType.MultiplyResolution)
            {
                float resolutionScale = CCBReader.ResolutionScale;

                absPt.X = pt.X * resolutionScale;
                absPt.Y = pt.Y * resolutionScale;
            }

            return absPt;
        }

        public static void SetRelativeScale(CCNode node, float fScaleX, float fScaleY, ScaleType nType, string pPropName)
        {
            Debug.Assert(node != null, "node should not be null");

            if (nType == ScaleType.MultiplyResolution)
            {
                float resolutionScale = CCBReader.ResolutionScale;

                fScaleX *= resolutionScale;
                fScaleY *= resolutionScale;
            }

            node.ScaleX = fScaleX;
            node.ScaleY = fScaleY;
        }
    }
}