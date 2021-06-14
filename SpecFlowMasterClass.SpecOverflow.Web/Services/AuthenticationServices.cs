using System;
using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.Http;
using SpecFlowMasterClass.SpecOverflow.Web.Utils;

namespace SpecFlowMasterClass.SpecOverflow.Web.Services
{
    public static class AuthenticationServices
    {
        private static readonly ConcurrentDictionary<string, string> LoggedInUsersByToken = new ConcurrentDictionary<string, string>();

        public static string SetCurrentUser(string userName)
        {
            var token = Guid.NewGuid().ToString("N");
            if (LoggedInUsersByToken.TryAdd(token, userName))
                return token;
            return null;
        }

        private static string GetTokenFromCookie(HttpContext httpContext)
        {
            var authTokenCookie = httpContext?.Request.Cookies["auth-token"];
            return authTokenCookie;
        }

        private static string EnsureToken(HttpContext httpContext, string token)
        {
            return token ?? GetTokenFromCookie(httpContext); // check cookies if not provided
        }

        public static string GetCurrentUserName(HttpContext httpContext, string requestToken = null)
        {
            var token = EnsureToken(httpContext, requestToken);
            if (token == null || !LoggedInUsersByToken.TryGetValue(token, out var userName))
                return null;
            return userName;
        }

        public static bool IsAdmin(HttpContext httpContext, string requestToken = null)
            => GetCurrentUserName(httpContext, requestToken) == "admin";

        public static bool IsLoggedIn(HttpContext httpContext, string requestToken = null)
            => GetCurrentUserName(httpContext, requestToken) != null;

        public static void ClearLoggedInUser(HttpContext httpContext, string requestToken = null)
        {
            var token = EnsureToken(httpContext, requestToken);
            if (token != null)
                LoggedInUsersByToken.TryRemove(token, out _);
        }

        public static string EnsureAuthenticated(HttpContext httpContext, string requestToken)
        {
            var currentUserName = GetCurrentUserName(httpContext, requestToken);
            if (currentUserName == null)
                throw new HttpResponseException(HttpStatusCode.Forbidden, "Not logged in");
            return currentUserName;
        }

        public static void EnsureAdminAuthenticated(HttpContext httpContext, string requestToken)
        {
            var currentUserName = EnsureAuthenticated(httpContext, requestToken);
            if (currentUserName != DefaultDataServices.AdminUserName)
                throw new HttpResponseException(HttpStatusCode.Forbidden, "Not admin");
        }

        public static void AddAuthCookie(HttpResponse response, string token)
        {
            if (response == null)
                return; // supports stub environment

            var option = new CookieOptions
            {
                Path = "/",
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                IsEssential = true
            };
            response.Cookies.Append("auth-token", token, option);
        }
    }
}