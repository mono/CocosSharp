using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace cocos2d
{
    public static class CCBHelper
    {
        public static CCPoint GetAbsolutePosition(CCPoint pt, kCCBPositionType nType, CCSize containerSize, string pPropName)
        {
            CCPoint absPt = new CCPoint(0, 0);
            
            if (nType == kCCBPositionType.kCCBPositionTypeRelativeBottomLeft)
            {
                absPt = pt;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeRelativeTopLeft)
            {
                absPt.X = pt.X;
                absPt.Y = containerSize.Height - pt.Y;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeRelativeTopRight)
            {
                absPt.X = containerSize.Width - pt.X;
                absPt.Y = containerSize.Height - pt.Y;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeRelativeBottomRight)
            {
                absPt.X = containerSize.Width - pt.X;
                absPt.Y = pt.Y;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypePercent)
            {
                absPt.X = (int) (containerSize.Width * pt.X / 100.0f);
                absPt.Y = (int) (containerSize.Height * pt.Y / 100.0f);
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeMultiplyResolution)
            {
                float resolutionScale = CCBReader.ResolutionScale;

                absPt.X = pt.X * resolutionScale;
                absPt.Y = pt.Y * resolutionScale;
            }

            return absPt;
        }

        public static void SetRelativeScale(CCNode node, float fScaleX, float fScaleY, kCCBScaleType nType, string pPropName)
        {
            Debug.Assert(node != null, "node should not be null");

            if (nType == kCCBScaleType.kCCBScaleTypeMultiplyResolution)
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