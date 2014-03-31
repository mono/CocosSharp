using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    /// <summary>
    /// CCMultipleLayer is a CCLayer with the ability to multiplex it's children.
    /// Features:
    /// - It supports one or more children
    /// - Only one children will be active a time
    /// </summary>
    public class CCLayerMultiplex : CCLayerRGBA
    {
        public const int NoLayer = -1;

        /// Offset used to preserve uniqueness for the layer tags. This is necessary
        /// to allow for SwitchTo(index) and SwitchTo(Layer.Tag) to work properly.
        const int TagOffsetForUniqueness = 5000;

        List<int> layersInOrder = new List<int>();      // The list of layers in their insertion order.


        #region Properties

        public bool ShowFirstLayerOnEnter { get; set; }
        protected int EnabledLayer { get; set; }        // Current index of the active layer.

        public CCAction InAction { get; set; }          // The action to play on the layer that becomes the active layer
        public CCAction OutAction { get; set; }         // The action to play on the active layer when it becomes inactive.

        protected Dictionary<int, CCLayer> Layers { get; set; }

        public virtual CCLayer ActiveLayer
        {
            get
            {
                if (Layers == null || EnabledLayer == NoLayer)
                {
                    return null;
                }
                return Layers[EnabledLayer];
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayerMultiplex() : base()
        {
            ShowFirstLayerOnEnter = true;
            EnabledLayer = NoLayer;
            Layers = new Dictionary<int, CCLayer>();
        }

        public CCLayerMultiplex (params CCLayer[] layers) : this(null, null, layers)
        {
        }

        public CCLayerMultiplex(CCAction inAction, CCAction outAction) : this(inAction, outAction, null)
        {
        }

        public CCLayerMultiplex(CCAction inAction, CCAction outAction, params CCLayer[] layers) : this()
        {
            InAction = inAction;
            OutAction = outAction;

            foreach(CCLayer layer in layers)
            {
                AddLayer(layer);
            }
        }

        #endregion Constructors

        public override void OnEnter()
        {
            if (EnabledLayer == -1 && Layers.Count > 0 && ShowFirstLayerOnEnter)
            {
                SwitchTo(0);
            }

            base.OnEnter();
        }

        /// <summary>
        /// Adds the given layer to the list of layers to multiplex. The CCNode.Tag is used
        /// as thelayer tag, but is offset by the TagOffsetForUniqueness constant.
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayer(CCLayer layer)
        {
            int ix = Layers.Count;
            Layers[ix] = layer;
            layersInOrder.Add(ix);
            if (layer.Tag != CCNode.TagInvalid)
            {
                Layers[layer.Tag + TagOffsetForUniqueness] = layer;
            }
        }


        #region Switching layers

        public CCLayer SwitchToFirstLayer()
        {
            if (layersInOrder.Count == 0)
            {
                return null;
            }
            return SwitchTo(layersInOrder[0]);
        }

        // Calling this method will set ShowFirstLayerOnEnter to false.
        public void SwitchToNone() 
        {
            SwitchTo(NoLayer);
        }

        // Switches to the new layer and removes the old layer from management.
        public CCLayer SwitchToAndRemovePreviousLayer(int n)
        {
            var prevLayer = EnabledLayer;
            CCLayer l = SwitchTo(n);
            Layers[prevLayer] = null;
            return l;
        }

        public CCLayer SwitchToNextLayer()
        {
            if (layersInOrder.Count == 0)
            {
                return null;
            }
            int idx = NoLayer;
            if (EnabledLayer != NoLayer)
            {
                for(int z = 0; z < layersInOrder.Count; z++)
                {
                    int ix = layersInOrder[z];
                    if (Layers[ix] != null)
                    {
                        if ((EnabledLayer > TagOffsetForUniqueness) 
                            && Layers[ix].Tag == (EnabledLayer - TagOffsetForUniqueness))
                        {
                            idx = z;
                            break;
                        }
                        else if(ix == EnabledLayer) 
                        {
                            idx = z;
                            break;
                        }
                    }
                }
                idx = (idx + 1) % layersInOrder.Count;
            }
            else
            {
                idx = 0;
            }
            if (idx == NoLayer)
            {
                idx = 0;
            }
            return SwitchTo(layersInOrder[idx]);
        }

        public CCLayer SwitchToPreviousLayer()
        {
            if (layersInOrder.Count == 0)
            {
                return null;
            }
            int idx = NoLayer;
            if (EnabledLayer != NoLayer)
            {
                for (int z = 0; z < layersInOrder.Count; z++)
                {
                    int ix = layersInOrder[z];
                    if (Layers[ix] != null)
                    {
                        if ((EnabledLayer > TagOffsetForUniqueness) 
                            && Layers[ix].Tag == (EnabledLayer - TagOffsetForUniqueness))
                        {
                            idx = z;
                            break;
                        }
                        else if (ix == EnabledLayer)
                        {
                            idx = z;
                            break;
                        }
                    }
                }
                idx -= 1;
                if (idx < 0)
                {
                    idx = layersInOrder.Count - 1;
                }
            }
            else
            {
                idx = 0;
            }
            return SwitchTo(layersInOrder[idx]);
        }

        /// <summary>
        /// Swtich to the given index layer and use the given action after the layer is
        /// added to the parent. The parameter can be the index or it can be the tag of the layer.
        /// </summary>
        /// <param name="n">Send in NoLayer to hide all multiplexed layers. Otherwise, send in a tag or the logical index of the 
        /// layer to show.</param>
        /// <returns>The layer that is going to be shown. This can return null if the SwitchTo layer is NoLayer</returns>
        public CCLayer SwitchTo(int n)
        {
            if (n != NoLayer)
            {
                if (EnabledLayer == n || EnabledLayer == (n + TagOffsetForUniqueness))
                {
                    if (EnabledLayer == NoLayer)
                    {
                        return null;
                    }
                    return Layers[EnabledLayer];
                }
            }

            if (EnabledLayer != NoLayer)
            {
                CCLayer outLayer = null;
                if (Layers.ContainsKey(EnabledLayer))
                {
                    outLayer = Layers[EnabledLayer];
                    if (OutAction != null)
                    {
                        outLayer.RunAction(
                            new CCSequence(
                                (CCFiniteTimeAction)OutAction,
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
                EnabledLayer = NoLayer;
            }

            // When NoLayer, the multiplexer shows nothing.
            if (n == NoLayer)
            {
                ShowFirstLayerOnEnter = false;
                return null;
            }

            if (!Layers.ContainsKey(n))
            {
                int f = n + TagOffsetForUniqueness;
                if (Layers.ContainsKey(f))
                {
                    n = f;
                }
                else
                {
                    // Invalid index - layer not found
                    return null;
                }
            }

            // Set the active layer
            AddChild(Layers[n]);
            EnabledLayer = n;

            if (InAction != null)
            {
                Layers[n].RunAction(InAction);
            }

            return Layers[EnabledLayer];
        }

        #endregion Switching
    }
}