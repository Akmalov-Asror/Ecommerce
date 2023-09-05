using Microsoft.AspNetCore.Components;
using Models.Dtos;

namespace Ecommerce.Blazor.Pages
{
    public class DisplayProductsBase:ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    
    }
}
