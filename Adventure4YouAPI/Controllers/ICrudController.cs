
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Adventure4YouAPI.Controllers
{
    public interface ICrudController<GetViewModel, ViewModelType> : ICudController<ViewModelType>
    {
        ActionResult<IEnumerable<GetViewModel>> Get();

        ActionResult<ViewModelType> GetById(Guid entityId);
    }
}
