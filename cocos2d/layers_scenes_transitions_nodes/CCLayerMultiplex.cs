using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    /// <summary>
    /// CCMultipleLayer is a CCLayer with the ability to multiplex it's children.
    /// Features:
    /// - It supports one or more children
    /// - Only one children will be active a time
    /// </summary>
    public class CCLayerMultiplex : CCLayerRGBA
    {
        private const int kTagOffsetForUniqueness = 5000;
        protected int m_nEnabledLayer=-1;
//        protected List<CCLayer> m_pLayers;
        protected Dictionary<int, CCLayer> m_pLayers = new Dictionary<int,CCLayer>();
        private CCAction m_InAction, m_OutAction;

        /// <summary>
        ///  creates a CCLayerMultiplex with one or more layers using a variable argument list. 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public CCLayerMultiplex (params CCLayer[] layer)
        {
            InitWithLayers(layer);
        }

        public CCLayerMultiplex(CCAction inAction, CCAction outAction, params CCLayer[] layer)
        {
            InitWithLayers(layer);
            m_InAction = inAction;
            m_OutAction = outAction;
        }

        public CCLayerMultiplex()
        {
        }

        public CCLayerMultiplex(CCAction inAction, CCAction outAction)
        {
            m_InAction = inAction;
            m_OutAction = outAction;
        }

        private bool InitWithLayer(CCLayer layer)
        {
            m_pLayers = new Dictionary<int,CCLayer>();
            m_pLayers[m_pLayers.Count] = layer;
            if (layer.Tag != CCNode.kCCNodeTagInvalid)
            {
                m_pLayers[layer.Tag + kTagOffsetForUniqueness] = layer;
            }
            return true;
        }

        /** initializes a MultiplexLayer with one or more layers using a variable argument list. */

        private bool InitWithLayers(params CCLayer[] layer)
        {
            m_pLayers = new Dictionary<int, CCLayer>();
            for (int i = 0; i < layer.Length; i++)
            {
                m_pLayers[i] = layer[i];
                if (layer[i].Tag != CCNode.kCCNodeTagInvalid)
                {
                    m_pLayers[layer[i].Tag + kTagOffsetForUniqueness] = layer[i];
                }
            }
            return true;
        }

        /// <summary>
        /// The action to play on the layer that becomes the active layer
        /// </summary>
        public CCAction InAction
        {
            get { return (m_InAction); }
            set { m_InAction = value; }
        }

        /// <summary>
        /// The action to play on the active layer when it becomes inactive.
        /// </summary>
        public CCAction OutAction
        {
            get { return (m_OutAction); }
            set { m_OutAction = value; }
        }
        /// <summary>
        ///  * lua script can not init with undetermined number of variables
        /// * so add these functinons to be used with lua.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public CCLayerMultiplex(CCAction inAction, CCAction outAction, CCLayer layer)
        {
            InitWithLayer(layer);
            m_InAction = inAction;
            m_OutAction = outAction;
        }

        public void AddLayer(CCLayer layer)
        {
            if (m_pLayers == null)
            {
                InitWithLayer(layer);
            }
            else
            {
                m_pLayers[m_pLayers.Count] = layer;
                if (layer.Tag != CCNode.kCCNodeTagInvalid)
                {
                    m_pLayers[layer.Tag + kTagOffsetForUniqueness] = layer;
                }
            }
        }

        /// <summary>
        /// Returns the active layer that was last selected. This method will return null
        /// if no layer has been selected.
        /// </summary>
        public virtual CCLayer ActiveLayer
        {
            get
            {
                if (m_pLayers == null || m_nEnabledLayer == -1)
                {
                    return (null);
                }
                return (m_pLayers[m_nEnabledLayer]);
            }
        }

        /** switches to a certain layer indexed by n. 
        The current (old) layer will be removed from it's parent with 'cleanup:YES'.
        */

        public override void OnEnter()
        {
            if (m_nEnabledLayer == -1 && m_pLayers.Count > 0)
            {
                SwitchTo(0);
            }
            else if(m_nEnabledLayer != -1)
            {
                if(m_InAction != null) {
                    m_pLayers[m_nEnabledLayer].RunAction(m_InAction.Copy());
                }
            }
            base.OnEnter();
        }

        public void SwitchToNone() 
        {
            if (m_nEnabledLayer != -1)
            {
                CCLayer outLayer = null;
                if (m_pLayers.ContainsKey(m_nEnabledLayer))
                {
                    outLayer = m_pLayers[m_nEnabledLayer];
                    if (m_OutAction != null)
                    {
                        outLayer.RunAction(
                            new CCSequence(
                                (CCFiniteTimeAction)m_OutAction.Copy(),
                                new CCCallFunc(() => RemoveChild(outLayer, true))
                                )
                            );
                    }
                    else
                    {
                        RemoveChild(outLayer, true);
                    }
                }
            }
            // We have no enabled layer at this point
            m_nEnabledLayer = -1;
        }

        /// <summary>
        /// Swtich to the given index layer and use the given action after the layer is
        /// added to the parent. The parameter can be the index or it can be the tag of the layer.
        /// </summary>
        /// <param name="n">Send in -1 to hide all multiplexed layers. Otherwise, send in a tag or the logical index of the 
        /// layer to show.</param>
        public CCLayer SwitchTo(int n)
        {
            if (n != -1)
            {
                if (m_nEnabledLayer == n || m_nEnabledLayer == (n + kTagOffsetForUniqueness))
                {
                    // no-op
                    if (m_nEnabledLayer == -1)
                    {
                        return (null);
                    }
                    return (m_pLayers[m_nEnabledLayer]);
                }
            }
            if (m_nEnabledLayer != -1)
            {
                CCLayer outLayer = null;
                if (m_pLayers.ContainsKey(m_nEnabledLayer))
                {
                    outLayer = m_pLayers[m_nEnabledLayer];
                    if (m_OutAction != null)
                    {
                        outLayer.RunAction(
                            new CCSequence(
                                (CCFiniteTimeAction)m_OutAction.Copy(),
                                new CCCallFunc(() => RemoveChild(outLayer, true))
                                )
                            );
                    }
                    else
                    {
                        RemoveChild(outLayer, true);
                    }
                }
                // We have no enabled layer at this point
                m_nEnabledLayer = -1;
            }
            // When -1, the multiplexer shows nothing.
            if (n == -1)
            {
                return (null);
            }
            if (!m_pLayers.ContainsKey(n))
            {
                int f = n + kTagOffsetForUniqueness;
                if (m_pLayers.ContainsKey(f))
                {
                    n = f;
                }
                else
                {
                    // Invalid index - layer not found
                    return(null);
                }
            }
            // Set the active layer
            AddChild(m_pLayers[n]);
            m_nEnabledLayer = n;
            // Run the in-action on the new layer
            if (m_InAction != null)
            {
                m_pLayers[n].RunAction(m_InAction.Copy());
            }
            return (m_pLayers[m_nEnabledLayer]);
        }


        /// <summary>
        /// Switches to the new layer and removes the old layer from management. 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public CCLayer SwitchToAndReleaseMe(int n)
        {
            var prevLayer = m_nEnabledLayer;
            CCLayer l = SwitchTo(n);
            m_pLayers[prevLayer] = null;
            return (l);
        }
    }
}