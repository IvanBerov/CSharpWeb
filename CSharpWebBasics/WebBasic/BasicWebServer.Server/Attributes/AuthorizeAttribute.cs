using System;

namespace BasicWebServer.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    internal class AuthorizeAttribute : Attribute
    {
    }
}
