
using Microsoft.AspNetCore.Mvc;
using System;

namespace Adventure4YouAPI.Controllers
{
    public interface ICrudController<GetViewModel, ViewModelType> : ICudController<ViewModelType>
    {
        IActionResult Get();

        IActionResult GetById(Guid entityId);
    }
}
