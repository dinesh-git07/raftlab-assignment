using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RaftLab_Assignment.Configuration;
using RaftLab_Assignment.Interfaces;

namespace RaftLab_Assignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IReqResInterface _reqResClient;
        private readonly ReqResAPISettings _settings;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IReqResInterface reqResClient,
            IOptions<ReqResAPISettings> options,
            ILogger<UsersController> logger)
        {
            _reqResClient = reqResClient ?? throw new ArgumentNullException(nameof(reqResClient));
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _reqResClient.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1)
        {
            try
            {
                var users = await _reqResClient.GetAllUsersAsync(page);
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
