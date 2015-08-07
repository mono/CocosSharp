using System;

namespace CocosSharp
{
    public class CCIMEKeybardEventArgs : System.EventArgs
    {
        public CCIMEKeybardEventArgs(string textToInsert, int length)
        {
            Text = textToInsert;
            Length = length;
            Cancel = false;
        }

        public string Text { get; set; }
        public int Length { get; set; }
        public bool Cancel { get; set; }
    }
}

