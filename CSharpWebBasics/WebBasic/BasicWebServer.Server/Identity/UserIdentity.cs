﻿namespace BasicWebServer.Server.Identity
{
    public class UserIdentity
    {
        public string Id { get; init; }

        public bool IsAuthenticated => Id != null;
    }
}