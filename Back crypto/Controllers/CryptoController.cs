using Microsoft.AspNetCore.Mvc;
using Backend_Crypto.Models;
using AutoMapper;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Data;
using Backend_Crypto.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend_Crypto.Controllers
{
    [Route("api/crypto")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IHistoriqueRepository _historiqueRepository;
        private readonly IMapper _mapper;
        private readonly CrudCryptoFirebase _cryptoFirebase;
        private readonly CrudHistoriquePrixFirebase _historique;
        private readonly AnalytiqueCryptoService _analyzer;

        public CryptoController(ICryptoRepository cryptoRepository, IMapper mapper,CrudHistoriquePrixFirebase histo,IHistoriqueRepository historique, AnalytiqueCryptoService analytiqueCryptoService, CrudCryptoFirebase cryptoFirebase)
        {
            _cryptoRepository = cryptoRepository;
            _analyzer = analytiqueCryptoService;
            _historiqueRepository = historique;
            _mapper = mapper;
            _historique = histo;
            _cryptoFirebase = cryptoFirebase;
        }

        // GET: api/<Crypto>
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<CryptoDtoAnalytique>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllCrypto()
        {
            var cryptos = _analyzer.getCrypto();
            if (!ModelState.IsValid)
            {
                // Créer une liste d'erreurs à partir de ModelState
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                // Retourner les erreurs avec un statut BadRequest
                return BadRequest(new { errors });
            }
            return Ok(cryptos);
        }

        // GET api/<Crypto>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CryptoDto))]
        [ProducesResponseType(404)]
        public IActionResult GetOneCrypto(int id)
        {
            if (!_cryptoRepository.CryptoExist(id))
                return NotFound();

            var crypto = _mapper.Map<CryptoDto>(_cryptoRepository.GetCrypto(id));
            if (!ModelState.IsValid)
            {
                // Créer une liste d'erreurs à partir de ModelState
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                // Retourner les erreurs avec un statut BadRequest
                return BadRequest(new { errors });
            }
            return Ok(crypto);
        }

        // POST api/<Crypto>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCrypto([FromBody] CryptoCreateData value)
        {
            if(value == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                // Créer une liste d'erreurs à partir de ModelState
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                // Retourner les erreurs avec un statut BadRequest
                return BadRequest(new { errors });
            }

            if (_cryptoRepository.findByName(value.name) != null || _cryptoRepository.findBySymbole(value.symbole) != null)
            {
                ModelState.AddModelError("error", "Le symbole ou le nom existe déjà");
                return StatusCode(422, ModelState);
            }

            if(!_cryptoRepository.CreateCrypto(value))
            {
                ModelState.AddModelError("error", "Erreur lors du sauvegarde");
                return StatusCode(500, ModelState);
            }
            await _cryptoFirebase.CreateCryptoAsync(_mapper.Map<CryptoFirebaseDto>(_cryptoRepository.findByName(value.name)));
            var histo = _cryptoRepository.GetFirstHisto(_cryptoRepository.findByName(value.name).IdCrypto);
            await _historique.CreateHistoriqueAsync(_mapper.Map<HistoriquePrixFirebaseDto>(histo));
            return Ok("Nouveau crypto créer.");
        }

        
        [HttpPut]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCrypto([FromBody] CryptoUpdateDto value)
        {
            if(value==null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_cryptoRepository.CryptoExist(value.IdCrypto))
                return NotFound();

            if (!ModelState.IsValid)
            {
                // Créer une liste d'erreurs à partir de ModelState
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                // Retourner les erreurs avec un statut BadRequest
                return BadRequest(new { errors });
            }

            var cryptoMap = _mapper.Map<Crypto>(value);

            if (!_cryptoRepository.updateCrypto(cryptoMap))
            {
                ModelState.AddModelError("error", "Erreur lors du sauvegarde");
                return StatusCode(500, ModelState);
            }
            await _cryptoFirebase.UpdateCryptoAsync(_mapper.Map<CryptoFirebaseDto>(_cryptoRepository.GetCrypto(value.IdCrypto)));
            return Ok("Update du crypto fini.");
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> deleteCrypto(int id)
        {
            if(!_cryptoRepository.CryptoExist(id))
                return NotFound();

            var cryptoDelete = _cryptoRepository.GetCrypto(id);

            if (!ModelState.IsValid)
            {
                // Créer une liste d'erreurs à partir de ModelState
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                // Retourner les erreurs avec un statut BadRequest
                return BadRequest(new { errors });
            }

            if (!_cryptoRepository.removeCrypto(cryptoDelete))
            {
                ModelState.AddModelError("error", "Erreur lors du sauvegarde");
                return StatusCode(500, ModelState);
            }
            await _cryptoFirebase.DeleteCryptoAsync(id.ToString());
            return Ok("Suppression effectué.");
        }

        // GET api/<Crypto>/5
        [HttpGet("historique/{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AnalytiqueHistoriqueDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetHistoriquePrix(int id)
        {
            if (!_cryptoRepository.CryptoExist(id))
                return NotFound();

            var historique = _analyzer.getPrixCrypto(id);
            if (!ModelState.IsValid)
            {
                // Créer une liste d'erreurs à partir de ModelState
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                // Retourner les erreurs avec un statut BadRequest
                return BadRequest(new { errors });
            }
            return Ok(historique);
        }
    }
}
