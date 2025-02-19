﻿using Microsoft.AspNetCore.Mvc;

namespace NestAPIGateway.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}