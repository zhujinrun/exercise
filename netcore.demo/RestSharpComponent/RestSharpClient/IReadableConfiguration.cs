﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpComponent
{
    public interface IReadableConfiguration
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>Access token.</value>
        string AccessToken { get; }

        /// <summary>
        /// Gets the base path.
        /// </summary>
        /// <value>Base path.</value>
        string BasePath { get; }

        /// <summary>
        /// Gets the date time format.
        /// </summary>
        /// <value>Date time foramt.</value>
        string DateTimeFormat { get; }

        /// <summary>
        /// Gets the default headers.
        /// </summary>
        /// <value>Default headers.</value>
        IDictionary<string, string> DefaultHeaders { get; }

        /// <summary>
        /// Gets the temp folder path.
        /// </summary>
        /// <value>Temp folder path.</value>
        string TempFolderPath { get; }

        /// <summary>
        /// Gets the HTTP connection timeout (in milliseconds)
        /// </summary>
        /// <value>HTTP connection timeout.</value>
        int Timeout { get; }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>User agent.</value>
        string UserAgent { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>Username.</value>
        string Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>Password.</value>
        string Password { get; }

        /// <summary>
        /// Gets the Gate APIv4 key.
        /// </summary>
        /// <value>APIv4 key.</value>
        string ApiV4Key { get; }

        /// <summary>
        /// Gets the Gate APIv4 secret.
        /// </summary>
        /// <value>APIv4 secret.</value>
        string ApiV4Secret { get; }

        /// <summary>
        /// helper method to update Gate APIv4 key and secret
        /// </summary>
        /// <param name="key">APIv4 key</param>
        /// <param name="secret">APIv4 secret</param>
        void SetGateApiV4KeyPair(string key, string secret);

        /// <summary>
        /// Gets certificate collection to be sent with requests.
        /// </summary>
        /// <value>X509 Certificate collection.</value>
        X509CertificateCollection ClientCertificates { get; }

        /// <summary>
        /// Get WebProxy used to send HTTP request
        /// </summary>
        /// <value>WebProxy.</value>
        IWebProxy Proxy { get; }
    }
}
