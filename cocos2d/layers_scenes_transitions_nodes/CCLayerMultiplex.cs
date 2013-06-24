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
        protected int m_nEnabledLayer=-1;
        protected List<CCLayer> m_pLayers;
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

        public CCAction InAction
        {
            get { return (m_InAction); }
            set { m_InAction = value; }
        }

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
                m_pLayers.Add(layer);
            }
        }

        private bool InitWithLayer(CCLayer layer)
        {
            m_pLayers = new List<CCLayer>();
            m_pLayers.Add(layer);
            return true;
        }

        /** initializes a MultiplexLayer with one or more layers using a variable argument list. */

        private bool InitWithLayers(params CCLayer[] layer)
        {
            m_pLayers = new List<CCLayer>();
            m_pLayers.AddRange(layer);
            return true;
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
                    m_pLayers[m_nEnabledLayer].RunAction(m_InAction);
                }
            }
            base.OnEnter();
        }

        /// <summary>
        /// Swtich to the given index layer and use the given action after the layer is
        /// added to the scene.
        /// </summary>
        /// <param name="n"></param>
        public CCLayer SwitchTo(int n)
        {
            if (m_nEnabledLayer == n)
            {
                // no-op
                if (m_nEnabledLayer == -1)
                {
                    return (null);
                }
                return (m_pLayers[m_nEnabledLayer]);
            }

            Debug.Assert(n < m_pLayers.Count, "Invalid index in MultiplexLayer SwitchTo");

            if (m_nEnabledLayer != -1)
            {
                CCLayer outLayer = m_pLayers[m_nEnabledLayer];

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

            m_nEnabledLayer = n;

            AddChild(m_pLayers[n]);
            
            if (m_InAction != null)
            {
                m_pLayers[n].RunAction(m_InAction.Copy());
            }
            return (m_pLayers[m_nEnabledLayer]);
        }

        /** release the current layer and switches to another layer indexed by n.
        The current (old) layer will be removed from it's parent with 'cleanup:YES'.
        */
        public CCLayer SwitchToAndReleaseMe(int n)
        {
            Debug.Assert(n < m_pLayers.Count, "Invalid index in MultiplexLayer SwitchToAndReleaseMe");

            var prevLayer = m_nEnabledLayer;
            CCLayer l = SwitchTo(n);
            m_pLayers[prevLayer] = null;
            return (l);
        }
    }
}