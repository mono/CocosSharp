namespace Cocos2D
{
    public interface ICCBMemberVariableAssigner
    {
        bool OnAssignCCBMemberVariable(object target, string memberVariableName, CCNode node);
    }
}