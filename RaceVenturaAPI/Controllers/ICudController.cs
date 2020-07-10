using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace RaceVenturaAPI.Controllers
{
    public interface ICudController<ViewModelType>
    {
        HttpContext HttpContext { get; }

        ControllerContext ControllerContext { get; set; }

        ModelStateDictionary ModelState { get; }

        IActionResult Add([FromBody]ViewModelType viewModel);

        IActionResult Edit([FromBody]ViewModelType viewModel);

        IActionResult Delete(Guid id);
    }
}
