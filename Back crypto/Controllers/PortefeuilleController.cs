using AutoMapper;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Backend_Crypto.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using static System.Net.Mime.MediaTypeNames;

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
        private readonly IAuthTokenRepository _tokenRepository;
        private readonly IStockPortefeuilleRepository _stockPortefeuilleRepository;
        private readonly IMapper _mapper;
        public PortefeuilleController(IPorteFeuilleRepository portefeuilleRepository, ITokenValidator tokenValidator , ExternalApiService apiService , IMapper mapper , ITransactionRepository transaction, ICryptoRepository cryptoRepository, UserAnalytique analyse, IStockPortefeuilleRepository stock)
        {
            _portefeuilleRepository = portefeuilleRepository;
            _stockPortefeuilleRepository = stock;
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
                    email = user["idEmail"]["value"].ToString(),
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
                return Ok("PIN de verification envoyer à votre email.");
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
                return Ok("Email de verification envoyer.");
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
        public async Task<IActionResult> Authentification([FromBody] PinDto data)
        {
            int pin = data.pin;
            if (pin == null)
                return BadRequest("PIN Introuvable");

            try
            {
                JObject retour = await _externalApiService.PostDataToApiAsync("pin_verification", data);
                if(retour != null && retour["token"] != null)
                {
                    TokenDto tokenRetour = new TokenDto() 
                    { 
                        token = retour["token"].ToString()
                    };

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
                if (!_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
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
                if (!_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
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

        [HttpPost("achat")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(402)]
        public async Task<IActionResult> SetVente([FromBody] OrdreFormDto data)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                if (!_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas encore de compte.");
                    return StatusCode(406, ModelState);
                }

                if(!_cryptoRepository.CryptoExist(data.idCrypto))
                {
                    ModelState.AddModelError("error", "Le crypto n'existe pas.");
                    return StatusCode(404, ModelState);
                }

                double prixAcheter = data.quantite * _cryptoRepository.GetPrixCrypto(data.idCrypto);

                if (!_portefeuilleRepository.HaveEnoughFond(user["id"].ToObject<int>(), prixAcheter))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas assez de fond pour cette operation.");
                    return StatusCode(402, ModelState);
                }
                var ownPortefeuille = _mapper.Map<Portefeuille>(_portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>()));
                Transaction transac = new Transaction()
                {
                    Type = TypeTransaction.Achat,
                    PortefeuilleOwner = ownPortefeuille,
                    fond = prixAcheter,
                    Ordre = new Ordre()
                    {
                        PrixUnitaire = _cryptoRepository.GetPrixCrypto(data.idCrypto),
                        AmountCrypto = data.quantite,
                        CryptoOrdre = _cryptoRepository.GetCrypto(data.idCrypto)
                    }
                };

                _tokenRepository.CreateToken(transac, user["username"].ToString(), user["idEmail"]["value"].ToString());
                return Ok(new { message = "Achat enregistré avec succès en attente de votre validation par email." });

            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpPost("vente")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(402)]
        public async Task<IActionResult> SetAchat([FromBody] OrdreFormDto data)
        {
            string token = _tokenValidator.GetTokenFromHeader();
            if (token == null)
            {
                return Unauthorized();
            }

            try
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("user", token);
                if (!_portefeuilleRepository.PortefeuilleExiste(user["id"].ToObject<int>()))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas encore de compte.");
                    return StatusCode(406, ModelState);
                }

                if (!_cryptoRepository.CryptoExist(data.idCrypto))
                {
                    ModelState.AddModelError("error", "Le crypto n'existe pas.");
                    return StatusCode(404, ModelState);
                }

                double prixAcheter = data.quantite * _cryptoRepository.GetPrixCrypto(data.idCrypto);

                if (!_portefeuilleRepository.HaveCrypto(user["id"].ToObject<int>(),data.idCrypto))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas assez de cryptomonnaie pour cette operation.");
                    return StatusCode(402, ModelState);

                }
                if (!_portefeuilleRepository.HaveEnoughCrypto(user["id"].ToObject<int>(), data.idCrypto,data.quantite))
                {
                    ModelState.AddModelError("error", "Vous n'avez pas assez de cryptomonnaie pour cette operation.");
                    return StatusCode(402, ModelState);
                }

                var ownPortefeuille = _mapper.Map<Portefeuille>(_portefeuilleRepository.GetPortefeuille(user["id"].ToObject<int>()));

                Transaction transac = new Transaction()
                {
                    Type = TypeTransaction.Vente,
                    PortefeuilleOwner = ownPortefeuille,
                    fond = prixAcheter,
                    Ordre = new Ordre()
                    {
                        PrixUnitaire = _cryptoRepository.GetPrixCrypto(data.idCrypto),
                        AmountCrypto = data.quantite,
                        CryptoOrdre = _cryptoRepository.GetCrypto(data.idCrypto)
                    }
                };

                _tokenRepository.CreateToken(transac, user["username"].ToString(), user["idEmail"]["value"].ToString());
                return Ok(new { message = "Vente enregistré avec succès en attente de votre validation par email." });

            }
            catch (ApiException ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return StatusCode(ex.StatusCode, ModelState);
            }
        }

        [HttpGet("validation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(402)]
        public async Task<IActionResult> ValidateTransac([FromQuery] String token)
        {
            if (!_tokenRepository.isTokenExist(token))
            {
                return RedirectToAction("Error", "Transaction", new { message = "Token de validation inexistante." });
            }

            if (!_tokenRepository.isTokenValid(token))
            {
                return RedirectToAction("Error", "Transaction", new { message = "Token expiré." });
            }

            var transaction = _tokenRepository.GetTransaction(token);
            if(transaction.Type == TypeTransaction.Achat)
            {
                if (!_portefeuilleRepository.HaveEnoughFond(transaction.PortefeuilleOwner.IdUser, transaction.fond))
                {
                    return RedirectToAction("Error", "Transaction", new { message = "Vous avez plus assez de fond pour l'achat." });
                }

                if (!_portefeuilleRepository.HaveCrypto(transaction.PortefeuilleOwner.IdUser, transaction.Ordre.IdCrypto))
                {
                    _portefeuilleRepository.AddStockPortefeuille(transaction.Ordre.CryptoOrdre,transaction.PortefeuilleOwner, transaction.Ordre.AmountCrypto);
                }
                _portefeuilleRepository.ExchangeFond(transaction.PortefeuilleOwner, transaction.fond, true);
                var stockPort = _portefeuilleRepository.getStock(transaction.PortefeuilleOwner.IdUser, transaction.Ordre.IdCrypto);
                stockPort.Stock += transaction.Ordre.AmountCrypto;
                _stockPortefeuilleRepository.updateStock(stockPort);
                return RedirectToAction("Success", "Transaction", new { message = "Votre achat a été validé avec succès !" });
            }
            else if(transaction.Type == TypeTransaction.Vente)
            {
                if (!_portefeuilleRepository.HaveCrypto(transaction.PortefeuilleOwner.IdUser, transaction.Ordre.IdCrypto))
                {
                    return RedirectToAction("Error", "Transaction", new { message = "Vous n'avez pas de cryptomonnaie pour la vente." });
                }

                if (!_portefeuilleRepository.HaveEnoughCrypto(transaction.PortefeuilleOwner.IdUser, transaction.Ordre.IdCrypto, transaction.Ordre.AmountCrypto))
                {
                    return RedirectToAction("Error", "Transaction", new { message = "Vous n'avez pas assez de cryptomonnaie pour la vente." });
                }

                _portefeuilleRepository.ExchangeFond(transaction.PortefeuilleOwner, transaction.fond, false);
                var stockPort = _portefeuilleRepository.getStock(transaction.PortefeuilleOwner.IdUser, transaction.Ordre.IdCrypto);
                stockPort.Stock -= transaction.Ordre.AmountCrypto;
                if(stockPort.Stock <= 0)
                {
                    _stockPortefeuilleRepository.removeStock(stockPort);
                }
                else
                {
                    _stockPortefeuilleRepository.updateStock(stockPort);
                }
                return RedirectToAction("Success", "Transaction", new { message = "Votre vente a été validé avec succès !" });
            }
            return RedirectToAction("Error", "Transaction", new { message = "Transaction invalide ." });
        }

    }
}
