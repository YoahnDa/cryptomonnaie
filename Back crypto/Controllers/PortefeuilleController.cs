using AutoMapper;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Backend_Crypto.Services;
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
        private readonly ITokenValidator _tokenValidator;
        private readonly ExternalApiService _externalApiService;
        private readonly IMapper _mapper;
        public PortefeuilleController(IPorteFeuilleRepository portefeuilleRepository, ITokenValidator tokenValidator , ExternalApiService apiService , IMapper mapper)
        {
            _portefeuilleRepository = portefeuilleRepository;
            _tokenValidator = tokenValidator;
            _externalApiService = apiService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(UserDto))]
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
                PortefeuilleDto portefeuille = _portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>());
                UserDto userRet = new UserDto() 
                { 
                    id = user["id"].ToObject<int>(),
                    username = user["username"].ToString(),
                    email = user["idEmail"]["value"].ToString(),
                    portefeuille = portefeuille
                };
                return Ok(userRet);
            }catch(ApiException ex)
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


        [HttpPost]
        [ProducesResponseType(401)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreationCompte()
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
                    ModelState.AddModelError("error", "L'utilisateur a déjà un compte");
                    return StatusCode(406, ModelState);
                }

                if (!_portefeuilleRepository.CreatePortefeuille(user["id"].ToObject<int>()))
                {
                    ModelState.AddModelError("error", "Erreur lors du sauvegarde");
                    return StatusCode(500, ModelState);
                }

                return Created();
                        
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
        public async Task<IActionResult> SetRetrait([FromBody] dynamic data)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }
            if (data["fond"] == null)
            {
                return BadRequest();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                if (_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas encore de compte.");
                    return StatusCode(406, ModelState);
                }

                if (!_portefeuilleRepository.HaveEnoughFond(user["id"].ToObject<int>() , data["fond"].ToObject<double>()))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas assez de fond.");
                    return StatusCode(402, ModelState);
                }

                var ownPortefeuille = _mapper.Map<Portefeuille>(_portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>()));
                //_portefeuilleRepository.ExchangeFond()
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("depot")]
        public async Task<IActionResult> SetDepot([FromBody] dynamic data)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }
            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                

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
