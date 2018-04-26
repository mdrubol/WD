﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WD.WebApi.Handlers
{
    public class SessionHandler : DelegatingHandler
    {
        static public string SessionIdToken = "token-id";
        async protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string sessionId;

            // Try to get the session ID from the request; otherwise create a new ID.
            var cookie = request.Headers.GetCookies(SessionIdToken).FirstOrDefault();
            if (cookie == null)
            {
                sessionId = Guid.NewGuid().ToString();
            }
            else
            {
                sessionId = cookie[SessionIdToken].Value;
                try
                {
                     Guid.Parse(sessionId);
                }
                catch (FormatException)
                {
                    // Bad session ID. Create a new one.
                    sessionId = Guid.NewGuid().ToString();
                }
            }

            // Store the session ID in the request property bag.
            request.Properties[SessionIdToken] = sessionId;

            // Continue processing the HTTP request.
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Set the session ID as a cookie in the response message.
            response.Headers.AddCookies(new CookieHeaderValue[] {
            new CookieHeaderValue(SessionIdToken, sessionId) 
        });

            return response;
        }
    }
}