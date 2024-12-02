using MediatR;
using Microsoft.AspNetCore.Mvc;
using ClaudeSearch.Application.Queries;

namespace ClaudeSearch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchResultDto>>> Search(
            [FromQuery] string query,
            CancellationToken cancellationToken)
        {
            var results = await _mediator.Send(
                new SearchQuery(query),
                cancellationToken);

            return Ok(results);
        }
    }
}