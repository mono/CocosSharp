using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
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

        public new static CCParallaxNode Create()
        {
            var pRet = new CCParallaxNode();
            return pRet;
        }

        // super methods
        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(false, "ParallaxNode: use addChild:z:parallaxRatio:positionOffset instead");
        }

        public virtual void AddChild(CCNode child, int z, CCPoint ratio, CCPoint offset)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            CCPointObject obj = CCPointObject.Create(ratio, offset);
            obj.Child = child;

            m_pParallaxArray.Add(obj);

            CCPoint pos = m_tPosition;
            pos.x = pos.x * ratio.x + offset.x;
            pos.y = pos.y * ratio.y + offset.y;
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
            CCPoint ret = m_tPosition;
            CCNode cn = this;
            while (cn.Parent != null)
            {
                cn = cn.Parent;
                ret = new CCPoint(ret.x + cn.Position.x, ret.y + cn.Position.y);
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
                    float x = -pos.x + pos.x * point.Ratio.x + point.Offset.x;
                    float y = -pos.y + pos.y * point.Ratio.y + point.Offset.y;
                    point.Child.Position = new CCPoint(x, y);
                }
                
                m_tLastPosition = pos;
            }
            
            base.Visit();
        }
    }
}