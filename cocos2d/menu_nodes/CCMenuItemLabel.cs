using System;

namespace cocos2d
{
    public class CCMenuItemLabel : CCMenuItem, ICCRGBAProtocol
    {
        protected float m_fOriginalScale;
        protected CCNode m_pLabel;
        protected CCColor3B m_tColorBackup;
        protected CCColor3B m_tDisabledColor;

        public CCColor3B DisabledColor
        {
            get { return m_tDisabledColor; }
            set { m_tDisabledColor = value; }
        }

        public CCNode Label
        {
            get { return m_pLabel; }
            set
            {
                if (value != null)
                {
                    AddChild(value);
                    value.AnchorPoint = new CCPoint(0, 0);
                    ContentSize = value.ContentSize;
                }

                if (m_pLabel != null)
                {
                    RemoveChild(m_pLabel, true);
                }

                m_pLabel = value;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if (m_bIsEnabled != value)
                {
                    if (!value)
                    {
                        m_tColorBackup = (m_pLabel as ICCRGBAProtocol).Color;
                        (m_pLabel as ICCRGBAProtocol).Color = m_tDisabledColor;
                    }
                    else
                    {
                        (m_pLabel as ICCRGBAProtocol).Color = m_tColorBackup;
                    }
                }
                base.Enabled = value;
            }
        }

        public static CCMenuItemLabel Create(CCNode label, SEL_MenuHandler selector)
        {
            var pRet = new CCMenuItemLabel();
            pRet.InitWithLabel(label, selector);
            return pRet;
        }

        public static CCMenuItemLabel Create(CCNode label)
        {
            var pRet = new CCMenuItemLabel();
            pRet.InitWithLabel(label, null);
            return pRet;
        }

        protected bool InitWithLabel(CCNode label, SEL_MenuHandler selector)
        {
            base.InitWithTarget(selector);
            m_fOriginalScale = 1.0f;
            m_tColorBackup = CCTypes.CCWhite;
            DisabledColor = new CCColor3B(126, 126, 126);
            Label = label;
            return true;
        }

        public void SetString(string label)
        {
            (m_pLabel as ICCLabelProtocol).Label = (label);
            ContentSize = m_pLabel.ContentSize;
        }

        public override void Activate()
        {
            if (m_bIsEnabled)
            {
                StopAllActions();
                Scale = m_fOriginalScale;
                base.Activate();
            }
        }

        public override void Selected()
        {
            // subclass to change the default action
            if (m_bIsEnabled)
            {
                base.Selected();

                CCAction action = GetActionByTag(unchecked((int) kZoomActionTag));
                if (action != null)
                {
                    StopAction(action);
                }
                else
                {
                    m_fOriginalScale = Scale;
                }

                CCAction zoomAction = CCScaleTo.Create(0.1f, m_fOriginalScale * 1.2f);
                zoomAction.Tag = unchecked((int) kZoomActionTag);
                RunAction(zoomAction);
            }
        }

        public override void Unselected()
        {
            // subclass to change the default action
            if (m_bIsEnabled)
            {
                base.Unselected();
                StopActionByTag(unchecked((int) kZoomActionTag));
                CCAction zoomAction = CCScaleTo.Create(0.1f, m_fOriginalScale);
                zoomAction.Tag = unchecked((int) kZoomActionTag);
                RunAction(zoomAction);
            }
        }

        #region CCRGBAProtocol interface

        public CCColor3B Color
        {
            get { return (m_pLabel as ICCRGBAProtocol).Color; }
            set { (m_pLabel as ICCRGBAProtocol).Color = value; }
        }

        public byte Opacity
        {
            get { return (m_pLabel as ICCRGBAProtocol).Opacity; }
            set { (m_pLabel as ICCRGBAProtocol).Opacity = value; }
        }

        public bool IsOpacityModifyRGB
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}