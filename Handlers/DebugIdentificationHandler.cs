namespace RyeBot.Handlers
{
    public static class DebugIdentificationHandler
    {
        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
