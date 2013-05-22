namespace Cocos2D
{
    internal interface ICCBMemberVariableAssigner
    {
        bool OnAssignCcbMemberVariable(object target, string memberVariableName, CCNode node);
    }
}