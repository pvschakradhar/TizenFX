﻿/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Runtime.InteropServices;

namespace Tizen.Account.OAuth2
{
    /// <summary>
    /// The response from authroization server containing access token and an optional refresh token.
    /// </summary>
    public class TokenResponse
    {
        private bool _disposed = false;
        private IntPtr _responseHandle;

        internal TokenResponse(IntPtr handle)
        {
            _responseHandle = handle;
        }

        /// <summary>
        /// Destructor of the AuthorizationResponse class.
        /// </summary>
        ~TokenResponse()
        {
            Dispose(false);
        }

        /// <summary>
        /// The access token
        /// </summary>
        public AccessToken AccessToken { get; internal set; }

        /// <summary>
        /// The state parameter present in authorization request.
        /// </summary>
        /// <remarks>
        /// State will be null if user doesn't put credential manually.
        /// </remarks>
        public string State { get; internal set; }

        /// <summary>
        /// The refresh token. The value will be null if authorization server doesn't return a refresh token.
        /// </summary>
        /// <remarks>
        /// Issuing a refresh token is optional at the discretion of the authorization server.
        /// </remarks>
        public RefreshToken RefreshToken { get; internal set; }

        /// <summary>
        /// Custom key-value parameter received from service provider
        /// </summary>
        /// <remarks>
        /// GetCustomValue will return null if user doesn't put credential manually.
        /// </remarks>
        public string GetCustomValue(string key)
        {
            IntPtr value;
            int ret = Interop.Response.GetCustomData(_responseHandle, key, out value);
            if (ret != (int)OAuth2Error.None)
            {
                Log.Error(ErrorFactory.LogTag, "Interop failed");
                throw ErrorFactory.GetException(ret);
            }
            return Marshal.PtrToStringAnsi(value);
        }

        /// <summary>
        /// Releases any unmanaged resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases any unmanaged resources used by this object. Can also dispose any other disposable objects.
        /// </summary>
        /// <param name="disposing">If true, disposes any disposable objects. If false, does not dispose disposable objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free managed objects
            }

            Interop.Response.Destroy(_responseHandle);
            _disposed = true;
        }
    }
}
