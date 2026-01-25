namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Data.Dto.VotingPower;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Specification.VotingPower;
    using GamaEdtech.Presentation.ViewModel.VotingPower;

    using Microsoft.AspNetCore.Mvc;

    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class VotingPowersController(Lazy<ILogger<VotingPowersController>> logger, Lazy<IVotingPowerService> votingPowerService)
        : ApiControllerBase<VotingPowersController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<VotingPowersResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<VotingPowersResponseViewModel>>> GetVotingPowers([NotNull, FromQuery] VotingPowersRequestViewModel request)
        {
            try
            {
                ISpecification<VotingPower>? specification = null;

                if (!string.IsNullOrEmpty(request.WalletAddress))
                {
                    specification = new WalletAddressContainsSpecification(request.WalletAddress);
                }

                if (!string.IsNullOrEmpty(request.ProposalId))
                {
                    var proposalSpecification = new ProposalIdContainsSpecification(request.ProposalId);
                    specification = specification is null ? proposalSpecification : specification.And(proposalSpecification);
                }

                var result = await votingPowerService.Value.GetVotingPowersAsync(new ListRequestDto<VotingPower>
                {
                    PagingDto = request.PagingDto,
                    Specification = specification,
                });
                return Ok<ListDataSource<VotingPowersResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new VotingPowersResponseViewModel
                        {
                            Id = t.Id,
                            Amount = t.Amount,
                            ProposalId = t.ProposalId,
                            TokenAccount = t.TokenAccount,
                            WalletAddress = t.WalletAddress,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<VotingPowersResponseViewModel>>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost, Produces<ApiResponse<Void>>()]
        public async Task<IActionResult<Void>> CreateVotingPower([NotNull, FromBody] CreateVotingPowerRequestViewModel request)
        {
            try
            {
                var signer = SignerUtilities.GetSigner("Ed25519");

                Ed25519PublicKeyParameters? publicKey = null;
                try
                {
                    publicKey = new Ed25519PublicKeyParameters(Convert.FromHexString(request.PublicKey!));
                }
                catch (Exception)
                {
                    return Ok<Void>(new() { Errors = [new() { Message = "Invalid PublicKey" }] });
                }

                signer.Init(false, publicKey);
                signer.BlockUpdate(System.Text.Encoding.ASCII.GetBytes(request.Message!), 0, request.Message!.Length);

                var valid = signer.VerifySignature(Convert.FromHexString(request.SignedMessage!));
                if (!valid)
                {
                    return Ok<Void>(new() { Errors = [new() { Message = "Invalid Signature" }] });
                }

                var lst = request.Data.Select(t => new ManageVotingPowerRequestDto
                {
                    Amount = t.Amount.GetValueOrDefault(),
                    WalletAddress = t.WalletAddress,
                    TokenAccount = t.TokenAccount,
                    ProposalId = t.ProposalId,
                    CreationDate = DateTimeOffset.UtcNow,
                });
                var result = await votingPowerService.Value.BulkImportVotingPowersAsync(lst);

                return Ok<Void>(new(result.Errors)
                {
                    Data = new(),
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
