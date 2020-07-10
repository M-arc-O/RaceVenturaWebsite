
using Microsoft.AspNetCore.Mvc;
using System;

namespace RaceVenturaAPI.Controllers
{
    public interface ICrudController<GetViewModel, ViewModelType> : ICudController<ViewModelType>
    {
        IActionResult Get();

        IActionResult GetById(Guid entityId);
    }
}
