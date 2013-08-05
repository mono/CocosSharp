using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace Cocos2D
{
    public static class CCBHelper
    {
        public static CCPoint GetAbsolutePosition(CCPoint pt, CCBPositionType nType, CCSize containerSize, string pPropName)
        {
            CCPoint absPt = new CCPoint(0, 0);
            
            if (nType == CCBPositionType.RelativeBottomLeft)
            {
                absPt = pt;
            }
            else if (nType == CCBPositionType.RelativeTopLeft)
            {
                absPt.X = pt.X;
                absPt.Y = containerSize.Height - pt.Y;
            }
            else if (nType == CCBPositionType.RelativeTopRight)
            {
                absPt.X = containerSize.Width - pt.X;
                absPt.Y = containerSize.Height - pt.Y;
            }
            else if (nType == CCBPositionType.RelativeBottomRight)
            {
                absPt.X = containerSize.Width - pt.X;
                absPt.Y = pt.Y;
            }
            else if (nType == CCBPositionType.Percent)
            {
                absPt.X = (int) (containerSize.Width * pt.X / 100.0f);
                absPt.Y = (int) (containerSize.Height * pt.Y / 100.0f);
            }
            else if (nType == CCBPositionType.MultiplyResolution)
            {
                float resolutionScale = CCBReader.ResolutionScale;

                absPt.X = pt.X * resolutionScale;
                absPt.Y = pt.Y * resolutionScale;
            }

            return absPt;
        }

        public static void SetRelativeScale(CCNode node, float fScaleX, float fScaleY, CCBScaleType nType, string pPropName)
        {
            Debug.Assert(node != null, "node should not be null");

            if (nType == CCBScaleType.MultiplyResolution)
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