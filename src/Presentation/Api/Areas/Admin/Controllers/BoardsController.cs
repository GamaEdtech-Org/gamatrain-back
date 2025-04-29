namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.Board;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Board;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class BoardsController(Lazy<ILogger<BoardsController>> logger, Lazy<IBoardService> boardService)
        : ApiControllerBase<BoardsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<BoardsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<BoardsResponseViewModel>>> GetBoards([NotNull, FromQuery] BoardsRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetBoardsAsync(new ListRequestDto<Board>
                {
                    PagingDto = request.PagingDto,
                });
                return Ok<ListDataSource<BoardsResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new BoardsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Icon = t.Icon,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<BoardsResponseViewModel>>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("{id:int}"), Produces<ApiResponse<BoardResponseViewModel>>()]
        public async Task<IActionResult<BoardResponseViewModel>> GetBoard([FromRoute] int id)
        {
            try
            {
                var result = await boardService.Value.GetBoardAsync(new IdEqualsSpecification<Board, int>(id));
                return Ok<BoardResponseViewModel>(new(result.Errors)
                {
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Description = result.Data.Description,
                        Icon = result.Data.Icon,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<BoardResponseViewModel>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost, Produces<ApiResponse<ManageBoardResponseViewModel>>()]
        public async Task<IActionResult<ManageBoardResponseViewModel>> CreateBoard([NotNull] ManageBoardRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.ManageBoardAsync(new ManageBoardRequestDto
                {
                    Title = request.Title,
                    Description = request.Description,
                    Icon = request.Icon,
                });
                return Ok<ManageBoardResponseViewModel>(new(result.Errors)
                {
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ManageBoardResponseViewModel>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("{id:int}"), Produces<ApiResponse<ManageBoardResponseViewModel>>()]
        public async Task<IActionResult<ManageBoardResponseViewModel>> UpdateBoard([FromRoute] int id, [NotNull, FromBody] ManageBoardRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.ManageBoardAsync(new ManageBoardRequestDto
                {
                    Id = id,
                    Title = request.Title,
                    Description = request.Description,
                    Icon = request.Icon,
                });
                return Ok<ManageBoardResponseViewModel>(new(result.Errors)
                {
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ManageBoardResponseViewModel>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult<bool>> RemoveBoard([FromRoute] int id)
        {
            try
            {
                var result = await boardService.Value.RemoveBoardAsync(new IdEqualsSpecification<Board, int>(id));
                return Ok<bool>(new(result.Errors)
                {
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
