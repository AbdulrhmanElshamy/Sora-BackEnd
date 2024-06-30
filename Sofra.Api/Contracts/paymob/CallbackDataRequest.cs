using Microsoft.AspNetCore.Mvc;

namespace Sofra.Api.Contracts.paymob
{
    public record CallbackDataRequest([FromQuery(Name = "message")] string message);
    
}
