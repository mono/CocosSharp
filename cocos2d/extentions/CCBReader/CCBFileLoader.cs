namespace cocos2d
{
    internal class CCBFileLoader : CCNodeLoader
    {
        private const string PROPERTY_CCBFILE = "ccbFile";

        public override CCNode CreateCCNode()
        {
            return CCBFile.Create();
        }

        protected override void OnHandlePropTypeCCBFile(CCNode node, CCNode parent, string propertyName, CCNode fileNode, CCBReader reader)
        {
            if (propertyName == PROPERTY_CCBFILE)
            {
                ((CCBFile) node).FileNode = fileNode;
            }
            else
            {
                base.OnHandlePropTypeCCBFile(node, parent, propertyName, fileNode, reader);
            }
        }
    }
}