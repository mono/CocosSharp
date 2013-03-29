namespace cocos2d
{
    internal interface ICCBMemberVariableAssigner
    {
        bool OnAssignCcbMemberVariable(object target, string memberVariableName, CCNode node);
    }
}