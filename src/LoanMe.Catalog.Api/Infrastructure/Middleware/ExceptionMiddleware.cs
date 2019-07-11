//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Net;
//using System.Security.Authentication;
//using System.Threading.Tasks;

//namespace LoanMe.Customers.Api.Infrastructure.Middleware
//{
//	public class ExceptionMiddleware
//	{
//		private const string CONTENT_TYPE_JSON = "application/json";

//		private readonly RequestDelegate _next;
//		private readonly IHostingEnvironment _env;
//		private readonly ILogger _logger;

//		public ExceptionMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<ExceptionMiddleware> logger)
//		{
//			_next = next ?? throw new ArgumentNullException(nameof(next));
//			_env = env ?? throw new ArgumentException(nameof(env));
//			_logger = logger;
//		}

//		public async Task Invoke(HttpContext context)
//		{
//			try
//			{
//				await _next(context);
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, ex.Message);
//				await HandleExceptionAsync(context, ex, _env);
//			}
//		}

//		private static async Task HandleExceptionAsync(
//			HttpContext context,
//			Exception exception,
//			IHostingEnvironment env)
//		{
//			HttpStatusCode httpCode;
//			string errorMessage;
//			int errorCode;

//			switch (exception)
//			{
//				//case ArgumentException ex:
//				//	httpCode = HttpStatusCode.BadRequest;
//				//	errorCode = ExceptionCodes.ARGUMENT_NULL_EXCEPTION;
//				//	errorMessage = ex.Message;
//				//	break;
//				case AuthenticationException ex:
//					httpCode = HttpStatusCode.Unauthorized;
//					// errorCode = ExceptionCodes.UNAUTHORIZED_EXCEPTION;
//					errorMessage = GetErrorMessage(ex, env);
//					break;
//				case ArgumentNullException ex:
//					httpCode = HttpStatusCode.BadRequest;
//					// errorCode = ExceptionCodes.ARGUMENT_NULL_EXCEPTION;
//					errorMessage = ex.Message;
//					break;			
//				default:
//					httpCode = HttpStatusCode.InternalServerError;
//					// errorCode = ExceptionCodes.GeneralException;
//					errorMessage = GetErrorMessage(exception, env);
//					break;
//			}

//			context.Response.StatusCode = (int)httpCode;
//			context.Response.ContentType = CONTENT_TYPE_JSON;
//			await context.Response.WriteAsync(JsonConvert.SerializeObject(new {  }));
//		}

//		private static string GetErrorMessage(Exception exception, IHostingEnvironment env)
//		{
//			return $"Unexpected error.{ (env.IsDevelopment() ? exception.Message + ". " + exception.StackTrace : string.Empty) }";
//		}
//	}

//	public static class ExceptionRestMiddlewareExtensions
//	{
//		public static IApplicationBuilder UseRestExceptionHandler(this IApplicationBuilder builder)
//		{
//			return builder.UseMiddleware<ExceptionMiddleware>();
//		}
//	}
//}
