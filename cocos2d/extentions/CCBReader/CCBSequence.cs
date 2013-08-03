using System;

namespace Cocos2D
{
    public class CCBSequence 
    {
        public CCBSequence()
        {
            _name = "";
        }

        private float _duration;
        private string _name;
        private int _sequenceId;
        private int _chainedSequenceId;
        private CCBSequenceProperty _callBackChannel;
        private CCBSequenceProperty _soundChannel;

        public float Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int SequenceId
        {
            get { return _sequenceId; }
            set { _sequenceId = value; }
        }

        public int ChainedSequenceId
        {
            get { return _chainedSequenceId; }
            set { _chainedSequenceId = value; }
        }

        public CCBSequenceProperty CallBackChannel
        {
            get { return _callBackChannel; }
            set { _callBackChannel = value; }
        }

        public CCBSequenceProperty SoundChannel
        {
            get { return _soundChannel; }
            set { _soundChannel = value; }
        }
    }
}