﻿using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public abstract class ApiControllerBase : ControllerBase
	{
		private ISender? _mediator;
		protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
	}

}
