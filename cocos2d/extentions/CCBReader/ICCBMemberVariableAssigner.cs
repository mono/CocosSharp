namespace cocos2d
{
    internal interface ICCBMemberVariableAssigner
    {
        bool OnAssignCcbMemberVariable(CCObject target, string memberVariableName, CCNode node);
    }
}