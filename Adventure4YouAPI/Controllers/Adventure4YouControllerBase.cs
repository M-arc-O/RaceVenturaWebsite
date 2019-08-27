using Microsoft.AspNetCore.Mvc;
using System;

namespace Adventure4YouAPI.Controllers
{
    public abstract class Adventure4YouControllerBase: ControllerBase
    {
        protected Guid GetUserId()
        {
            return new Guid(User.FindFirst("id")?.Value);
        }
    }
}
