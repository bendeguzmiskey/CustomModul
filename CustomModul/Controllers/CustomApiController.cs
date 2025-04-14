using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;

namespace CustomModul.CustomModul.Controllers
{
    public class CustomApiController : DnnApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                var values = new string[] { "value1", "value2" };
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "value");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [DnnAuthorize]
        public HttpResponseMessage Post([FromBody] string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Value cannot be empty");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Value received: " + value);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [DnnAuthorize]
        public HttpResponseMessage Put(int id, [FromBody] string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Value cannot be empty");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Value updated: " + value);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [DnnAuthorize]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Item deleted: " + id);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}