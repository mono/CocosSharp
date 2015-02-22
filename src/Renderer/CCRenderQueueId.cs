using System;

namespace CocosSharp
{

    /// <summary>
    /// Implementation of a render queue id used for sorting the priority queue.
    /// 
    /// Original Idea from Implementing a Render Queue for Games -> http://ploobs.com.br/?p=2378 
    ///   and Order your graphics draw calls around -> http://realtimecollisiondetection.net/blog/?p=86
    /// 
    /// </summary>
    internal class CCRenderQueueId : IComparable<CCRenderQueueId>
    {
        public override string ToString()
        {
            return string.Concat("G: ", group 
                ," QG: ", (QUEUE_GROUP)queueGroup
                , " D: " , depthSorting);
        }

        #region IComparable<Employee> Members

        public int CompareTo(CCRenderQueueId other)
        {
            //// First compare grouped commands
            if (this.group < other.group) return 1;
            else if (this.group > other.group) return -1;
            else // If equal then we need to compare queue group
            {
                //return 0;
                if (this.queueGroup > other.queueGroup) return 1;
                else if (this.queueGroup < other.queueGroup) return -1;
                else
                {
                    //return 0;
                    if (this.depthSorting > other.depthSorting) return 1;
                    else if (this.depthSorting < other.depthSorting) return -1;
                    else return 0;
                }
            }
        }

        #endregion

        /*
         * 2 Group. Used for grouping commands
           1 QueueGroup.  Determines sort order within the group
           13 Extra
           24 Depth sorting. We want to sort translucent geometry back-to-front for proper draw ordering and perhaps opaque geometry front-to-back to aid z-culling.
           24 Material. We want to sort by material to minimize state settings (textures, shaders, constants). A material might have multiple passes.
         * =64
         * */
        long id;

        public readonly long group;
        public readonly long queueGroup;
        public readonly long depthSorting;
        public readonly long materialid;
        public readonly long extra;
        bool flipMaterialWithSorting = false;

        public CCRenderQueueId(int group,
            int extra, 
            int depthSorting, 
            int material)
        {

            this.group = group;

            if (depthSorting == 0)
                this.queueGroup = (int)QUEUE_GROUP.GLOBALZ_ZERO;
            else if (depthSorting < 0)
                this.queueGroup = (int)QUEUE_GROUP.GLOBALZ_NEG;
            else
                this.queueGroup = (int)QUEUE_GROUP.GLOBALZ_POS;

            this.depthSorting = depthSorting;
            this.materialid = material;
            this.extra = extra;
        }

        public long CachedId
        {
            get
            {
                return id;
            }
        }

        public long GenerateId(bool flipMaterialWithSorting = false)
        {
            this.flipMaterialWithSorting = flipMaterialWithSorting;

            if (flipMaterialWithSorting)
            {
                id = group << 62
                    | queueGroup << 61
                    | extra << 48
                    | materialid << 24
                    | depthSorting
                    ;
            }
            else
            {
                id = group << 62
                    | queueGroup << 61
                    | extra << 48
                    | depthSorting << 24
                    | materialid
                    ;
            }
            return id;


        }
    }

}

