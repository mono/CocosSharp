using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCParallaxNode : CCNode
    {
        // ivars
        CCPoint lastPosition;


        #region Properties

        List<CCPointObject> ParallaxArray { get; set; }

        CCPoint AbsolutePosition
        {
            get 
            {
                CCPoint ret = Position;
                CCNode parent = this;
                while ((parent = parent.Parent) != null) 
                {
                    ret.X += parent.Position.X;
                    ret.Y += parent.Position.Y;
                }
                return ret;
            }
        }

        #endregion Properties


        #region Constructors

        public CCParallaxNode()
        {
            ParallaxArray = new List<CCPointObject>(5);
            lastPosition = new CCPoint(-100, -100);
        }

        #endregion Constructors


        public override void Visit(ref CCAffineTransform parentWorldTransform)
        {
            CCPoint pos = AbsolutePosition;

            if (!pos.Equals(lastPosition))
            {
                foreach(CCPointObject point in ParallaxArray)
                {
                    point.Child.Position = -pos + (pos * point.Ratio) + point.Offset;
                }

                lastPosition = pos;
            }

            base.Visit(ref parentWorldTransform);
        }


        #region Child management

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(false, "ParallaxNode: use addChild:z:parallaxRatio:positionOffset instead");
        }

        public virtual void AddChild(CCNode child, int z, CCPoint ratio, CCPoint offset)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            CCPointObject obj = new CCPointObject(ratio, offset);
            obj.Child = child;

            ParallaxArray.Add(obj);

            CCPoint pos = Position;
            pos *= (ratio + offset); 
            child.Position = pos;

            base.AddChild(child, z, child.Tag);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            foreach(CCPointObject pointObj in ParallaxArray)
            {
                if (pointObj.Child == child)
                {
                    ParallaxArray.Remove(pointObj);
                    break;
                }
            }

            base.RemoveChild(child, cleanup);
        }

        public override void RemoveAllChildren(bool cleanup)
        {
            ParallaxArray.Clear();
            base.RemoveAllChildren(cleanup);
        }

        #endregion Child management
    }
}