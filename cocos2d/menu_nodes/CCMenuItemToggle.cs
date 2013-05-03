using System;
using System.Collections.Generic;

namespace cocos2d
{
    public class CCMenuItemToggle : CCMenuItem, ICCRGBAProtocol
    {
        private byte m_cOpacity;
        public List<CCMenuItem> m_pSubItems;
        private CCColor3B m_tColor;
        private int m_uSelectedIndex;

        public CCMenuItemToggle ()
        {
            InitWithTarget(null);
        }
        
        public CCMenuItemToggle (SEL_MenuHandler selector, params CCMenuItem[] items)
        {
            InitWithTarget(selector, items);
        }

        public int SelectedIndex
        {
            get { return m_uSelectedIndex; }
            set
            {
                if (value != m_uSelectedIndex && m_pSubItems.Count > 0)
                {
                    m_uSelectedIndex = value;
                    var currentItem = (CCMenuItem) GetChildByTag(kCurrentItem);
                    if (currentItem != null)
                    {
                        currentItem.RemoveFromParentAndCleanup(false);
                    }

                    CCMenuItem item = m_pSubItems[m_uSelectedIndex];
                    AddChild(item, 0, kCurrentItem);
                    CCSize s = item.ContentSize;
                    ContentSize = s;
                    item.Position = new CCPoint(s.Width / 2, s.Height / 2);
                }
            }
        }

        public List<CCMenuItem> SubItems
        {
            get { return m_pSubItems; }
            set { m_pSubItems = value; }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                foreach (CCMenuItem item in m_pSubItems)
                {
                    item.Enabled = value;
                }
            }
        }

        #region ICCRGBAProtocol Members

        public byte Opacity
        {
            get { return m_cOpacity; }
            set
            {
                m_cOpacity = value;
                if (m_pSubItems != null && m_pSubItems.Count > 0)
                {
                    for (int i = 0; i < m_pSubItems.Count; i++)
                    {
                        var rgba = m_pSubItems[i] as ICCRGBAProtocol;
                        if (rgba != null)
                        {
                            rgba.Opacity = value;
                        }
                    }
                }
            }
        }

        public CCColor3B Color
        {
            get { return m_tColor; }
            set
            {
                m_tColor = value;
                if (m_pSubItems != null && m_pSubItems.Count > 0)
                {
                    for (int i = 0; i < m_pSubItems.Count; i++)
                    {
                        var rgba = m_pSubItems[i] as ICCRGBAProtocol;
                        if (rgba != null)
                        {
                            rgba.Color = value;
                        }
                    }
                }
            }
        }

        public bool IsOpacityModifyRGB
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        public bool InitWithTarget(SEL_MenuHandler selector, CCMenuItem[] items)
        {
            base.InitWithTarget(selector);
            m_pSubItems = new List<CCMenuItem>();
            foreach (CCMenuItem item in items)
            {
                m_pSubItems.Add(item);
            }
            m_uSelectedIndex = int.MaxValue;
            SelectedIndex = 0;
            return true;
        }

        public static CCMenuItemToggle Create(CCMenuItem item)
        {
            var pRet = new CCMenuItemToggle();
            pRet.InitWithItem(item);
            return pRet;
        }

        public bool InitWithItem(CCMenuItem item)
        {
            base.InitWithTarget(null);
            m_pSubItems = new List<CCMenuItem>();
            m_pSubItems.Add(item);
            m_uSelectedIndex = int.MaxValue;
            SelectedIndex = 0;
            return true;
        }

        public void AddSubItem(CCMenuItem item)
        {
            m_pSubItems.Add(item);
        }

        public CCMenuItem SelectedItem
        {
            get { return m_pSubItems[m_uSelectedIndex]; }
        }

        public override void Activate()
        {
            // update index
            if (m_bIsEnabled)
            {
                int newIndex = (m_uSelectedIndex + 1) % m_pSubItems.Count;
                SelectedIndex = newIndex;
            }
            base.Activate();
        }

        public override void Selected()
        {
            base.Selected();
            m_pSubItems[m_uSelectedIndex].Selected();
        }

        public override void Unselected()
        {
            base.Unselected();
            m_pSubItems[m_uSelectedIndex].Unselected();
        }
    }
}