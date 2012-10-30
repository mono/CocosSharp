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
                absPt.x = pt.x;
                absPt.y = containerSize.height - pt.y;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeRelativeTopRight)
            {
                absPt.x = containerSize.width - pt.x;
                absPt.y = containerSize.height - pt.y;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeRelativeBottomRight)
            {
                absPt.x = containerSize.width - pt.x;
                absPt.y = pt.y;
            }
            else if (nType == kCCBPositionType.kCCBPositionTypePercent)
            {
                absPt.x = (int) (containerSize.width * pt.x / 100.0f);
                absPt.y = (int) (containerSize.height * pt.y / 100.0f);
            }
            else if (nType == kCCBPositionType.kCCBPositionTypeMultiplyResolution)
            {
                float resolutionScale = CCBReader.ResolutionScale;

                absPt.x = pt.x * resolutionScale;
                absPt.y = pt.y * resolutionScale;
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