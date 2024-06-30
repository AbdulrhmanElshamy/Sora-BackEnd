using Microsoft.AspNetCore.Mvc;

namespace Sofra.Api.Contracts.paymob
{
    public record CallbackSourceDataRequest([FromQuery(Name = "pan")] string pan, [FromQuery(Name = "type")] string type, [FromQuery(Name = "sub_type")] string sub_type);
}
