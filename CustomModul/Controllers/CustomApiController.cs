using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using DotNetNuke.Web.Api;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json;

namespace CustomModul.CustomModul.Controllers
{
    public class CustomApiController : DnnApiController
    {
        private static readonly Dictionary<string, string> Categories = new Dictionary<string, string>
        {
            { "Dog", "Kutya" },
            { "Cat", "Macska" },
            { "Rabbit", "Kisemlősök" },
            { "Hamster", "Kisemlősök" },
            { "Guinea pig", "Kisemlősök" },
            { "Ferret", "Kisemlősök" },
            { "Mouse", "Kisemlősök" },
            { "Rat", "Kisemlősök" },
            { "Gerbil", "Kisemlősök" },
            { "Fish", "Halak, hüllők" },
            { "Reptile", "Halak, hüllők" },
            { "Lizard", "Halak, hüllők" },
            { "Turtle", "Halak, hüllők" },
            { "Snake", "Halak, hüllők" },
            { "Frog", "Halak, hüllők" }
        };

        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> AnalyzeDummyImage()
        {
            var debugSteps = new List<string>();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                debugSteps.Add("Debug: Entered AnalyzeDummyImage");

                // Small black square PNG (1x1 pixel)
                string base64Image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVQYV2P4DwQACfsD/Qp3Pz4AAAAASUVORK5CYII=";
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                debugSteps.Add($"Debug: Dummy image created ({stopwatch.ElapsedMilliseconds} ms)");

                var modulePath = HostingEnvironment.MapPath("~/DesktopModules/CustomModul");
                var credentialsPath = Path.Combine(modulePath, "CustomModul", "apikey", "superb-webbing-455211-d7-2e140293e808.json");

                if (!File.Exists(credentialsPath))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: Google credentials file not found at: " + credentialsPath, steps = debugSteps });
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
                debugSteps.Add($"Debug: Credentials set ({stopwatch.ElapsedMilliseconds} ms)");

                var client = await ImageAnnotatorClient.CreateAsync();
                debugSteps.Add($"Debug: Vision client initialized ({stopwatch.ElapsedMilliseconds} ms)");

                var image = Image.FromBytes(imageBytes);

                var response = await client.DetectLabelsAsync(image);
                debugSteps.Add($"Debug: Labels detected ({stopwatch.ElapsedMilliseconds} ms)");

                var labels = response.Select(label => label.Description).ToList();

                var detectedCategories = labels
                    .Where(label => Categories.ContainsKey(label))
                    .Select(label => Categories[label])
                    .Distinct()
                    .ToList();

                var result = new
                {
                    categories = detectedCategories,
                    detectedLabels = labels,
                    debugSteps = debugSteps
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                debugSteps.Add("Debug: Exception occurred: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    error = "Debug: Exception: " + ex.Message,
                    stackTrace = ex.StackTrace,
                    steps = debugSteps
                });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage TestCredentials()
        {
            try
            {
                var modulePath = HostingEnvironment.MapPath("~/DesktopModules/CustomModul");
                var credentialsPath = Path.Combine(modulePath, "CustomModul", "apikey", "superb-webbing-455211-d7-2e140293e808.json");
                var exists = File.Exists(credentialsPath);
                if (!exists)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: Credentials file not found at: " + credentialsPath });
                }
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
                var client = ImageAnnotatorClient.Create();
                return Request.CreateResponse(HttpStatusCode.OK, "Debug: Credentials file found and Vision client initialized.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: TestCredentials failed: " + ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                var values = new string[] { "value1", "value2" };
                var response = Request.CreateResponse(HttpStatusCode.OK, values);
                response.Content = new StringContent(JsonConvert.SerializeObject(values), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: " + ex.Message });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: " + ex.Message });
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Debug: Value cannot be empty" });
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Value received: " + value);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: " + ex.Message });
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Debug: Value cannot be empty" });
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Value updated: " + value);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: " + ex.Message });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: " + ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> AnalyzeAnimal()
        {
            var debugSteps = new List<string>();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                debugSteps.Add("Debug: Entered AnalyzeAnimal");

                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Debug: Invalid request format. Expected multipart/form-data.", steps = debugSteps });
                }

                debugSteps.Add("Debug: Reading multipart data");
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                debugSteps.Add($"Debug: Multipart parsed ({stopwatch.ElapsedMilliseconds} ms)");

                var file = provider.Contents.FirstOrDefault(c => c.Headers.ContentDisposition.FileName != null);
                if (file == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Debug: No image file provided.", steps = debugSteps });
                }

                var imageBytes = await file.ReadAsByteArrayAsync();
                debugSteps.Add($"Debug: Image read ({stopwatch.ElapsedMilliseconds} ms)");

                if (imageBytes.Length == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Debug: Image file is empty.", steps = debugSteps });
                }

                if (imageBytes.Length > 10 * 1024 * 1024)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "Debug: Image size exceeds 10MB limit.", steps = debugSteps });
                }

                var modulePath = HostingEnvironment.MapPath("~/DesktopModules/CustomModul");
                var credentialsPath = Path.Combine(modulePath, "CustomModul", "apikey", "superb-webbing-455211-d7-2e140293e808.json");

                if (!File.Exists(credentialsPath))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Debug: Google credentials file not found at: " + credentialsPath, steps = debugSteps });
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
                debugSteps.Add($"Debug: Credentials set ({stopwatch.ElapsedMilliseconds} ms)");

                var client = await ImageAnnotatorClient.CreateAsync();
                debugSteps.Add($"Debug: Vision client initialized ({stopwatch.ElapsedMilliseconds} ms)");

                var image = Image.FromBytes(imageBytes);

                var response = await client.DetectLabelsAsync(image);
                debugSteps.Add($"Debug: Labels detected ({stopwatch.ElapsedMilliseconds} ms)");

                var labels = response.Select(label => label.Description).ToList();

                var detectedCategories = labels
                    .Where(label => Categories.ContainsKey(label))
                    .Select(label => Categories[label])
                    .Distinct()
                    .ToList();

                var result = new
                {
                    categories = detectedCategories,
                    detectedLabels = labels,
                    debugSteps = debugSteps
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                debugSteps.Add("Debug: Exception occurred: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    error = "Debug: Exception: " + ex.Message,
                    stackTrace = ex.StackTrace,
                    steps = debugSteps
                });
            }
        }
    }
}
