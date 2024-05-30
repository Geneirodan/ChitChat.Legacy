using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Messages.Commands.IntegrationTests.Extensions;

public static class ObjectExtensions
{
    public static JsonContent ToJson(this object @object) => JsonContent.Create(@object);

    public static async Task<T?> AsContent<T>(this HttpResponseMessage @this) =>
        JsonConvert.DeserializeObject<T>(await @this.Content.ReadAsStringAsync().ConfigureAwait(false));
    
    public static async Task<ValidationProblemDetails?> AsProblemDetails(this HttpResponseMessage @this) =>
        JsonConvert.DeserializeObject<ValidationProblemDetails>(await @this.Content.ReadAsStringAsync().ConfigureAwait(false));
}
