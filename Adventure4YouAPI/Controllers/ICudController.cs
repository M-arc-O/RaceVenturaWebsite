using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Adventure4YouAPI.Controllers
{
    public interface ICudController<ViewModelType>
    {
        HttpContext HttpContext { get; }

        ControllerContext ControllerContext { get; set; }

        ActionResult<ViewModelType> Add([FromBody]ViewModelType viewModel);

        ActionResult<ViewModelType> Edit([FromBody]ViewModelType viewModel);

        ActionResult<Guid> Delete(Guid id);
    }
}
