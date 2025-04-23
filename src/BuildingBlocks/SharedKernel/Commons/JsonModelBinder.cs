using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace SharedKernel.Commons;

public class JsonModelBinder<T> : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.FieldName, valueProviderResult);

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.CompletedTask;
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<T>(value, options);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.FieldName, ex.Message);
        }

        return Task.CompletedTask;
    }
}