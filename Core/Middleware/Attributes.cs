namespace RentMaster.Core.Middleware;

public class Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminScopeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserScopeAttribute : Attribute { }
}