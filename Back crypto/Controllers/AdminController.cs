using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Backend_Crypto.Repository;
using Backend_Crypto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend_Crypto.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPorteFeuilleRepository _porteFeuilleRepository;
        private readonly ExternalApiService _externalApiService;
        private readonly TokenValidator _tokenValidator;
        private readonly UserAnalytique _analytique;
        public AdminController (ITransactionRepository transactionRepository, UserAnalytique userAnalytique, IPorteFeuilleRepository portefeuille)
        {
            _transactionRepository = transactionRepository;
            _porteFeuilleRepository = portefeuille;
            _analytique = userAnalytique;
        }

        [HttpGet("transaction")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOperationInfo()
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                JArray roles = (JArray)user["roles"];
                if(!(roles != null && roles.Contains("ROLE_ADMIN")))
                {
                    return Unauthorized("Vous êtes pas admin.");
                }
                var listType = new List<TypeTransaction> { TypeTransaction.Vente, TypeTransaction.Achat, TypeTransaction.Depot, TypeTransaction.Retrait };
                var listRetour = _analytique.getTransac(listType, token);
                return Ok(listRetour);
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpGet("transaction/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOperationUser(int id)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            var listType = new List<TypeTransaction> { TypeTransaction.Vente, TypeTransaction.Achat, TypeTransaction.Depot,TypeTransaction.Achat};
            if (_porteFeuilleRepository.PortefeuilleExiste(id))
            {
                return NotFound("Portefeuille introuvable.");
            }
            var portefeuille = _porteFeuilleRepository.GetPortefeuille(id);

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                JArray roles = (JArray)user["roles"];
                if (!(roles != null && roles.Contains("ROLE_ADMIN")))
                {
                    return Unauthorized("Vous êtes pas admin.");
                }
                
                var listRetour = _analytique.getTransacUser(listType, portefeuille.IdPortefeuille, token);
                return Ok(listRetour);
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpGet("transaction/validation/exchangefond/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ValidateTransac(int id)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                JArray roles = (JArray)user["roles"];
                if (!(roles != null && roles.Contains("ROLE_ADMIN")))
                {
                    return Unauthorized("Vous êtes pas admin.");
                }
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }

            var transaction = _transactionRepository.GetTransaction(id);
            if (transaction == null)
            {
                return NotFound("Transaction introuvable.");
            }else if (!_porteFeuilleRepository.PortefeuilleExiste(transaction.PortefeuilleOwner.IdUser))
            {
                return NotFound("Portefeuille introuvable.");
            }

            var portefeuille = transaction.PortefeuilleOwner;

            if(transaction.Type == TypeTransaction.Depot)
            {
                portefeuille.Fond += transaction.fond;
                _porteFeuilleRepository.UpdatePortefeuille(portefeuille);
            }else if(transaction.Type == TypeTransaction.Retrait)
            {
                if (!_porteFeuilleRepository.HaveEnoughFond(portefeuille.IdUser, transaction.fond))
                {
                    return Unauthorized("Pas assez de fond.");
                }
                portefeuille.Fond -= transaction.fond;
            }
            _porteFeuilleRepository.UpdatePortefeuille(portefeuille);
            _transactionRepository.ChangeEtat(transaction, Status.Valid);
            return Ok("Transaction valider.");
        }

        [HttpGet("transaction/annulation/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public async  Task<IActionResult> AnnulationTransac(int id)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                JArray roles = (JArray)user["roles"];
                if (!(roles != null && roles.Contains("ROLE_ADMIN")))
                {
                    return Unauthorized("Vous êtes pas admin.");
                }
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }

            var transaction = _transactionRepository.GetTransaction(id);
            if (transaction == null)
            {
                return NotFound("Transaction introuvable.");
            }
            else if (!_porteFeuilleRepository.PortefeuilleExiste(transaction.PortefeuilleOwner.IdUser))
            {
                return NotFound("Portefeuille introuvable.");
            }
            _transactionRepository.ChangeEtat(transaction,Status.Annuler);
            return Ok("Transaction annuler.");
        }

        [HttpGet("transaction/recherche")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDtoAdmin>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Search([FromQuery] List<int>? idUtilisateurs,[FromQuery] List<int>? idCryptos)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                JArray roles = (JArray)user["roles"];
                if (!(roles != null && roles.Contains("ROLE_ADMIN")))
                {
                    return Unauthorized("Vous êtes pas admin.");
                }

                var filter = new TransactionFilterDto
                {
                    IdUtilisateurs = idUtilisateurs ?? new List<int>(),
                    IdCryptos = idCryptos ?? new List<int>(),
                    Types = new List<TypeTransaction>() { TypeTransaction.Retrait, TypeTransaction.Depot, TypeTransaction.Vente, TypeTransaction.Achat },
                };
                return Ok(_analytique.GetTransactionsByFilter(filter,token));
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }
    }
}
