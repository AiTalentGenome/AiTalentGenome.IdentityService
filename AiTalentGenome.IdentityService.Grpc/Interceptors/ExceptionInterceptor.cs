// AiTalentGenome.IdentityService.Grpc/Interceptors/ExceptionInterceptor.cs

using Grpc.AspNetCore.Server.Model;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AiTalentGenome.IdentityService.Grpc.Interceptors;

public class ExceptionInterceptor(ILogger<ExceptionInterceptor> logger) : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger = logger;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выполнении gRPC метода {Method}", context.Method);

            // Маппим наши ошибки на gRPC статусы
            var status = ex switch
            {
                ArgumentException => new Status(StatusCode.InvalidArgument, ex.Message),
                TimeoutException => new Status(StatusCode.DeadlineExceeded, "Превышено время ожидания"),
                // Специальная обработка для ошибок HH
                HttpRequestException httpEx when httpEx.Message.Contains("invalid_grant") 
                    => new Status(StatusCode.InvalidArgument, "Код авторизации недействителен или уже использован"),
                _ => new Status(StatusCode.Internal, "Внутренняя ошибка сервера")
            };

            throw new RpcException(status);
        }
    }
}