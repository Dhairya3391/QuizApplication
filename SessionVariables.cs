namespace QuizApplication
{
    public static class SessionVariables
    {
        //Provides access to the current Microsoft.AspNetCore.Http.IHttpContextAccessor.HttpContext
        private static IHttpContextAccessor _httpContextAccessor;

        static SessionVariables()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public static int? UserID()
        {
            //Initialize the UserID with null
            int? UserID = null;

            //check if session contains specified key?
            //if it contains then return the value contained by the key.
            if (_httpContextAccessor.HttpContext.Session.GetString("UserId") != null)
            {
                UserID = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetInt32("UserId").ToString());
            }
            return UserID;
        }

        public static string? UserName()
        {
            string? UserName = null;

            if (_httpContextAccessor.HttpContext.Session.GetString("Username") != null)
            {
                UserName = _httpContextAccessor.HttpContext.Session.GetString("Username").ToString();
            }
            return UserName;
        }
    }
}
