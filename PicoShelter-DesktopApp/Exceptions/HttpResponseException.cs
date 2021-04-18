using PicoShelter_DesktopApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.Exceptions
{

    [Serializable]
    public class HttpResponseException : Exception
    {
        public HttpStatusCode? StatusCode { get; set; }
        public ErrorDetailsDto Details { get; set; }
        public List<ModelStateErrorDto> ModelStateErrors { get; set; }

        public HttpResponseException() { }
        public HttpResponseException(HttpStatusCode? statusCode = null, ErrorDetailsDto details = null, List<ModelStateErrorDto> modelStateErrors = null) 
            : base(BuildMessage(statusCode, details, modelStateErrors)) 
        {
            StatusCode = statusCode;
            Details = details;
            ModelStateErrors = modelStateErrors;
        }
        protected HttpResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string BuildMessage(HttpStatusCode? statusCode = null, ErrorDetailsDto details = null, List<ModelStateErrorDto> modelStateErrors = null)
        {
            var r = "The request has returned unsuccess response.";
            if (statusCode != null)
                r += " Status code is " + ((int)statusCode).ToString() + " - " + statusCode.ToString() + '.';

            if (details != null)
                r += " Error Details: " + JsonSerializer.Serialize(details) + '.';

            if (modelStateErrors != null)
                r += " ModelState Errors: " + JsonSerializer.Serialize(modelStateErrors) + '.';

            return r;
        }
    }
}
