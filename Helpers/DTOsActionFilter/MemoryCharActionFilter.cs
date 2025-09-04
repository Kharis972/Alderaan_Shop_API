using alderaan_shop.DTOs.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace alderaan_shop.Helpers.DTOsActionFilter;

//reception des arguments par les users, automatic model biding par apicontroller
public abstract class MemoryCharActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (object? argument in context.ActionArguments.Values)
        {
            if (argument is IMemoryCharInitializable memoryCharInitializable)
            {
                memoryCharInitializable.InitializeMemoryChar();
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}