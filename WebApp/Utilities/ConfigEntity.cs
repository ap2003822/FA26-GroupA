namespace Webapp.Utility
{
    public class ConfigEntity
    {
#if (DEBUG)
        public static string BaseURL = "https://localhost:7170/";
        /*#elif (RELEASE) Published version, need to know where it goes.
            public static string BaseURL =*/
#endif    
    }
}