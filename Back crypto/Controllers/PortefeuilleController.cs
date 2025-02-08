using AutoMapper;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Backend_Crypto.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend_Crypto.Controllers
{
    [Route("api/compte")]
    [ApiController]
    public class PortefeuilleController : ControllerBase
    {
        private readonly IPorteFeuilleRepository _portefeuilleRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITokenValidator _tokenValidator;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly UserAnalytique _analyser;
        private readonly ExternalApiService _externalApiService;
        private readonly IMapper _mapper;
        public PortefeuilleController(IPorteFeuilleRepository portefeuilleRepository, ITokenValidator tokenValidator , ExternalApiService apiService , IMapper mapper , ITransactionRepository transaction, ICryptoRepository cryptoRepository, UserAnalytique analyse)
        {
            _portefeuilleRepository = portefeuilleRepository;
            _cryptoRepository =  cryptoRepository;
            _transactionRepository = transaction;
            _tokenValidator = tokenValidator;
            _externalApiService = apiService;
            _analyser = analyse;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(AnalysePortefeuilleDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCompte()
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if(token == null)
            {
                return Unauthorized();
            }
            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                Portefeuille portefeuille = _portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>());
                AnalysePortefeuilleDto userRet = await _analyser.getInfoPortefeuille(user["id"].ToObject<int>(),token);
                return Ok(userRet);
            }catch(ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpGet("info")]
        [ProducesResponseType(200, Type = typeof(UserInfoDto))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetInfoUser()
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }
            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                JArray roles = (JArray)user["roles"];  // Accéder à la propriété "roles" (qui est un tableau)

                UserInfoDto userRet = new UserInfoDto()
                {
                    id = user["id"].ToObject<int>(),
                    username = user["username"].ToString(),
                    email = user["email"].ToString(),
                    isAdmin = roles != null && roles.Contains("ROLE_ADMIN")
                };
                return Ok(userRet);
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("connection")]
        [ProducesResponseType(406)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Connection([FromBody] UserConnectDto info)
        {
            if (info == null)
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                JObject retour = await _externalApiService.PostDataToApiAsync("login", info);
                return Ok(retour != null && retour["message"] != null ? retour["message"] : "Email envoyer");
            }catch(ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("inscription")]
        [ProducesResponseType(406)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Inscription([FromBody] UserInscriptionDto info)
        {
            if (info == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                JObject retour = await _externalApiService.PostDataToApiAsync("inscription", info);
                return Ok(retour != null && retour["message"] != null ? retour["message"] : "Email envoyer");
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("favoris/{id}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddFavoris(int id)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            if (!_cryptoRepository.CryptoExist(id))
            {
                return NotFound("Cryptomonnaie introuvable.");
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                _analyser.updateFavs(user["id"].ToObject<int>(), id);
                return Ok("Favoris modifier.");

            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("authpin")]
        [ProducesResponseType(406)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Authentification([FromBody] dynamic data)
        {
            int pin = data.pin;
            if (pin == null)
                return BadRequest("PIN Introuvable");

            try
            {
                JObject pinObj = new JObject();
                pinObj["pin"] = data.pin;
                JObject retour = await _externalApiService.PostDataToApiAsync("pin_verification", pinObj);
                if(retour != null && retour["token"] != null)
                {
                    JObject tokenRetour = new JObject();
                    tokenRetour["token"] = retour["token"];

                    JObject user = await _externalApiService.GetDataFromApiAsync("user", retour["token"].ToString());
                    if (!_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
                    {
                        _portefeuilleRepository.CreatePortefeuille(user["id"].ToObject<int>());
                    }
                    return StatusCode(200, tokenRetour);
                }
                ModelState.AddModelError("error", "Erreur d'authentification.");
                return StatusCode(500, ModelState);
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }


        [HttpPost("retrait")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(402)]
        public async Task<IActionResult> SetRetrait([FromBody] ExchangeFondDto data)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                if (_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas encore de compte.");
                    return StatusCode(406, ModelState);
                }

                if (!_portefeuilleRepository.HaveEnoughFond(user["id"].ToObject<int>() ,(double)data.fond))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas assez de fond.");
                    return StatusCode(402, ModelState);
                }

                var ownPortefeuille = _mapper.Map<Portefeuille>(_portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>()));
                Transaction transac = new Transaction() 
                { 
                    PortefeuilleOwner = ownPortefeuille,
                    fond = data.fond
                };
                _transactionRepository.CreateTransaction(TypeTransaction.Retrait,ownPortefeuille,data.fond);
                return Ok(new { message = "Dépôt enregistré avec succès et en attente du validation de l'admin !!" });
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("depot")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(402)]
        public async Task<IActionResult> SetDepot([FromBody] ExchangeFondDto data)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }
            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                if (_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas encore de compte.");
                    return StatusCode(406, ModelState);
                }

                if (!_portefeuilleRepository.HaveEnoughFond(user["id"].ToObject<int>(), (double)data.fond))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas assez de fond.");
                    return StatusCode(402, ModelState);
                }
                var ownPortefeuille = _mapper.Map<Portefeuille>(_portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>()));
                _transactionRepository.CreateTransaction(TypeTransaction.Depot, ownPortefeuille, data.fond);
                return Ok(new { message = "Dépôt enregistré avec succès en attente du validation de l'admin." });

            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        // DELETE api/<PortefeuilleController>/5
        [HttpDelete("{id}")]
        public void Delete()
        {
        }
    }
}
