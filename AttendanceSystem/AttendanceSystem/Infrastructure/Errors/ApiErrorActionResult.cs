﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class ApiErrorActionResult : IHttpActionResult
    {
        public HttpStatusCode StatusCode { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public String Message { get; private set; }

        public ApiErrorActionResult(HttpRequestMessage request, HttpStatusCode statusCode, String message)
        {
            Request = request;
            StatusCode = statusCode;
            Message = message;
        }

        public Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response =
                             new HttpResponseMessage(StatusCode);
            response.Content = new StringContent(JsonConvert.SerializeObject(new
            {                
                error = new
                {
                    code = 0,
                    message = Message
                }
            }),
            Encoding.UTF8,
            "application/json");
            
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}