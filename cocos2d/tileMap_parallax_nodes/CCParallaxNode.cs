using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCParallaxNode : CCNode
    {
        protected List<CCPointObject> m_pParallaxArray;
        protected CCPoint m_tLastPosition;

        public CCParallaxNode()
        {
            m_pParallaxArray = new List<CCPointObject>(5);
            m_tLastPosition = new CCPoint(-100, -100);
        }

        public List<CCPointObject> ParallaxArray
        {
            get { return m_pParallaxArray; }
            set { m_pParallaxArray = value; }
        }

        // super methods
        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(false, "ParallaxNode: use addChild:z:parallaxRatio:positionOffset instead");
        }

        public virtual void AddChild(CCNode child, int z, CCPoint ratio, CCPoint offset)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            CCPointObject obj = new CCPointObject(ratio, offset);
            obj.Child = child;

            m_pParallaxArray.Add(obj);

            CCPoint pos = m_obPosition;
            pos.X = pos.X * ratio.X + offset.X;
            pos.Y = pos.Y * ratio.Y + offset.Y;
            child.Position = pos;

            base.AddChild(child, z, child.Tag);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            for (int i = 0; i < m_pParallaxArray.Count; i++)
            {
                if (m_pParallaxArray[i].Child == child)
                {
                    m_pParallaxArray.RemoveAt(i);
                    break;
                }
            }
            base.RemoveChild(child, cleanup);
        }

        public override void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            m_pParallaxArray.Clear();
            base.RemoveAllChildrenWithCleanup(cleanup);
        }

        private CCPoint AbsolutePosition()
        {
            CCPoint ret = m_obPosition;
            CCNode cn = this;
            while (cn.Parent != null)
            {
                cn = cn.Parent;
                ret = new CCPoint(ret.X + cn.Position.X, ret.Y + cn.Position.Y);
            }
            return ret;
        }

        public override void Visit()
        {
            CCPoint pos = AbsolutePosition();
            
            if (!pos.Equals(m_tLastPosition))
            {
                for (int i = 0; i < m_pParallaxArray.Count; i++)
                {
                    var point = m_pParallaxArray[i];
                    float x = -pos.X + pos.X * point.Ratio.X + point.Offset.X;
                    float y = -pos.Y + pos.Y * point.Ratio.Y + point.Offset.Y;
                    point.Child.Position = new CCPoint(x, y);
                }
                
                m_tLastPosition = pos;
            }
            
            base.Visit();
        }
    }
}